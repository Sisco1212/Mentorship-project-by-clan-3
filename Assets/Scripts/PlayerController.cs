using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    public float movementSpeed = 2f;
    public float rotationSpeed = 30f;
    public CharacterController characterController;
    public Animator animator;

    [Header("Player Fight")]
    public float attackCooldown = 0.25f;
    public int attackDamages = 5;
    public string[] attackAnimations = {"att1", "att2", "att3", "att4"};
    private float lastAttackTime;
    private Fighter fighter;
    private Fighter opponentFighter;

    [Header("Other")]
    private Transform enemy;
    public float attackRange = 1.5f;
    public float canApproachLimit = 1.1f;
    public ParticleSystem[] attackEffects = {};
    public bool isPlayerAttacking = false;
    public bool isPlayerBlocking = false;
    private float distanceToEnemy = 0f;

    void Awake(){
        enemy = GameObject.FindWithTag("Enemy").transform;
        fighter = GetComponent<Fighter>();
        opponentFighter = enemy.gameObject.GetComponent<Fighter>();
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

    public void PerformHurt(float damageAmount){
        if(!isPlayerBlocking){
            animator.Play("Hurt");
            GameManager.Instance.PlayHitSound(); 
            GameManager.Instance.ShakeCamera();
            fighter.TakeDamage(damageAmount);
        }
        else{
            GameManager.Instance.PlayBlockSound();
        }
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

        characterController.Move(movement * movementSpeed * Time.deltaTime); 

    }

    void PerformAttack(int attackIndex){
        if(Time.time - lastAttackTime > attackCooldown){
            animator.Play(attackAnimations[attackIndex]);
            int damage = attackDamages;
            isPlayerAttacking = true;
            lastAttackTime = Time.time;
        }
    }

    public void AttackEffect(string location){
        Transform hitLocation = null;
        if(distanceToEnemy <= attackRange){
            enemy.gameObject.GetComponent<EnemyAI>().PerformHurt(7.0f);
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
}