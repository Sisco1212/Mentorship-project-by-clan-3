using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    public float movementSpeed = 2f;
    public float rotationSpeed = 30f;
    private CharacterController characterController;
    public Animator animator;
    public RuntimeAnimatorController winController;
    public RuntimeAnimatorController loseController;

    [Header("Player Fight")]
    public float attackCooldown = 0.25f;
    public int attackDamages = 5;
    public string[] attackAnimations = {"att1", "att2", "att3", "att4"};
    private float lastAttackTime;
    private Fighter fighter;
    private Fighter opponentFighter;

    private MotionConstraints motionConstraints;

    [Header("Other")]
    private Transform enemy;
    public float attackRange = 1.5f;
    public float canApproachLimit = 1.1f;
    public ParticleSystem[] attackEffects = {};
    public bool isPlayerAttacking = false;
    public bool isPlayerBlocking = false;
    private float distanceToEnemy = 0f;

    public AudioClip[] hitVoices = {};
    public AudioClip unSheatheSwordSound;
    public AudioClip sheatheSwordSound;
    private AudioSource audioSource;
    
    public AudioClip[] swordSounds;
    public AudioClip emptySwordSound;
    public AudioSource swordAudioSource;

    public ParticleSystem bloodEffects;

     public AudioClip explosionSound;

    // public AudioSource gunSounds;
    // public AudioClip loadGunSound;

    void Awake(){
        enemy = GameObject.FindWithTag("Enemy").transform;
        fighter = GetComponent<Fighter>();
        motionConstraints = GetComponent<MotionConstraints>();
        characterController = GetComponent<CharacterController>();
        opponentFighter = enemy.gameObject.GetComponent<Fighter>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        if(fighter.isDead || opponentFighter.isDead || !GameManager.Instance.fightStarted) return;
        distanceToEnemy = Mathf.Abs(transform.position.x - enemy.position.x);
        PerformMovement();
        if(Time.time - lastAttackTime > attackCooldown){
            isPlayerAttacking = false;
        }
        if(GestureDetector.Instance.IsSingleTapRight()){
            PerformAttack(Random.Range(0, attackAnimations.Length));
        }
        else if(GestureDetector.Instance.IsHoldLeft() && !GestureDetector.Instance.IsSwiping()){
            Debug.Log(Random.Range(0, 100));
            isPlayerBlocking = true;
        }
        if(!GestureDetector.Instance.IsHoldLeft()){
            isPlayerBlocking = false;
        }
    }

    private void PushBackEffect(){
        Vector3 slideVal = (transform.position-enemy.position).normalized * 0.25f;
        slideVal.y = 0f;
        slideVal.z = 0f;
        characterController.Move(slideVal);
    }

    public void PerformHurt(float damageAmount){
        if(!isPlayerBlocking){
            animator.Play("Hurt");
            GameManager.Instance.PlayHitSound(); 
            GameManager.Instance.ShakeCamera();
            fighter.TakeDamage(damageAmount);
            // GetComponent<Animator>().SetBool("SwordPowerup", false);
            FindObjectOfType<Powerup>().EndSwordPowerup();
            FindObjectOfType<GunPowerup>().EndGunPowerup();
        }
        else{
            if(GameManager.Instance.blockEffects.Length>0){
                Instantiate(GameManager.Instance.blockEffects[Random.Range(0, GameManager.Instance.blockEffects.Length)], fighter.transform);
            }
            GameManager.Instance.PlayBlockSound();
        }
        //PushBackEffect();
    }

    public void PerformWin(){
        animator.runtimeAnimatorController = winController;
    }

    public void PerformLost(){
        animator.runtimeAnimatorController = loseController;
    }

    void PerformMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        animator.SetBool("Block", isPlayerBlocking);

        Quaternion targetRotation = Quaternion.LookRotation(new Vector3((enemy.position-transform.position).x, 0f, 0f));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        Vector3 movement = Vector3.zero;

        if(GestureDetector.Instance.IsSwipeUp()){
            fighter.Jump();
        }

        if(GestureDetector.Instance.IsSwipingRight()){
            if(distanceToEnemy>=canApproachLimit){
                movement = new Vector3(1f, 0f, 0f);
            }
            animator.SetBool("Walking", true);
        }
        else if(GestureDetector.Instance.IsSwipingLeft()){
            movement = new Vector3(-1f, 0f, 0f);
            animator.SetBool("WalkingBack", true);
        }
        else
        {
            movement = Vector3.zero;
            animator.SetBool("Walking", false);
            animator.SetBool("WalkingBack", false);
        }

        if((movement.x<0 && motionConstraints.canMoveLeft) || (movement.x>0 && motionConstraints.canMoveRight)){
            characterController.Move(movement * movementSpeed * Time.deltaTime);
        }

    }

    void PerformAttack(int attackIndex){
        if(Time.time - lastAttackTime > attackCooldown){
            animator.Play(attackAnimations[attackIndex]);
            int damage = attackDamages;
            isPlayerAttacking = true;
            lastAttackTime = Time.time;
        if(audioSource != null && hitVoices.Length>0){
        audioSource.clip = hitVoices[Random.Range(0, hitVoices.Length)];
        audioSource.Play();
        }
        }
    }

    public void AttackEffect(string location){
        Transform hitLocation = null;
        if(distanceToEnemy <= attackRange){
            enemy.gameObject.GetComponent<EnemyAI>().PerformHurt(3.5f, "hurt");
        }
        if(fighter){
            switch(location){
                case "LF":
                    hitLocation = fighter.leftFootTransform;
                    break;
                case "RF":
                    hitLocation = fighter.rightFootTransform;
                    break;
                case "LH":
                    hitLocation = fighter.leftHandTransform;
                    break;
                case "RH":
                    hitLocation = fighter.rightHandTransform;
                    break;
                default:
                    break;
            }
        }
        if(hitLocation != null){
            Instantiate(attackEffects[Random.Range(0, attackEffects.Length)], hitLocation);
        }
    }
    
    public void SwordAttackEffect(){
        Transform hitLocation = null;
        if(distanceToEnemy <= attackRange){
            enemy.gameObject.GetComponent<EnemyAI>().PerformHurt(5.5f, "SwordHurt");
            enemy.gameObject.GetComponent<EnemyAI>().powerupCharge = 0;
            bloodEffects.Play();
            // enemy.gameObject.GetComponent<EnemyAI>().powerupButton.interactable = false;
            PlaySwordSounds();
        }

        else {
            PlayEmptySwordSound();
        }

        if(fighter){
        hitLocation = fighter.swordTransform;            
        if(hitLocation != null){
            Instantiate(attackEffects[Random.Range(0, attackEffects.Length)], hitLocation);
        }    
        }
        if(audioSource != null && hitVoices.Length>0){
        audioSource.clip = hitVoices[Random.Range(0, hitVoices.Length)];
        audioSource.Play();
        }       
    }

    public void PlayUnsheatheSwordSound() {
        if(swordAudioSource != null){
        swordAudioSource.clip = unSheatheSwordSound;
        swordAudioSource.Play();
        }
    }

    public void PlaySheatheSwordSound() {
        if(swordAudioSource != null){
        swordAudioSource.clip = sheatheSwordSound;
        swordAudioSource.Play();
        }
    }

    public void PlaySwordSounds() {
        if (swordAudioSource != null && swordSounds.Length > 0)
        {
            swordAudioSource.clip = swordSounds[Random.Range(0, swordSounds.Length)];
            swordAudioSource.Play();
        }
    }
    public void PlayEmptySwordSound() {
        if (swordAudioSource != null && swordSounds.Length > 0)
        {
            swordAudioSource.clip = emptySwordSound;
            swordAudioSource.Play();
        }
    }

    public void PlayExplosionSound() 
    {
        if (audioSource != null)
        {
            audioSource.clip = explosionSound;
            audioSource.Play();
        }
    }
}