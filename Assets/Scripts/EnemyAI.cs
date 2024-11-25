using UnityEngine;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    private Transform player;
    
    // AI properties
    public float movementSpeed = 2f;
    public float rotationSpeed = 30f;
    public float attackRange = 1.5f;
    public float retreatRange = 2.5f;
    public float actionCooldown = 1.0f;
    public float walkSpeed = 2.0f;

    private CharacterController characterController;
    
    private float actionTimer;
    private bool isPlayerAttacking;

    private enum AIState { Idle, Attack, Defend, Dodge, Retreat, Approach }
    private AIState currentState;
    private float distanceToPlayer = 0f;

    [SerializeField]
    private List<string> attackTriggers = new List<string>();
    [SerializeField]
    private ParticleSystem[] attackEffects = {};
    private Fighter fighter;
    private Fighter opponentFighter;

    public AudioClip[] hitVoices = {};
    private AudioSource audioSource;
    

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        fighter = GetComponent<Fighter>();
        characterController = GetComponent<CharacterController>();
        opponentFighter = player.gameObject.GetComponent<Fighter>();
        currentState = AIState.Idle;
        actionTimer = actionCooldown;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(fighter.isDead || opponentFighter.isDead || !GameManager.Instance.fightStarted) return;
        distanceToPlayer = Mathf.Abs(transform.position.x - player.position.x);
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3((player.position-transform.position).x, 0f, 0f));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        actionTimer -= Time.deltaTime;

        switch(currentState){
            case AIState.Approach:
                characterController.Move((new Vector3((player.position-transform.position).x, 0f, 0f).normalized) * movementSpeed * Time.deltaTime);
                if(distanceToPlayer <= attackRange)
                    Idle();
                break;
            case AIState.Retreat:
                characterController.Move((new Vector3((transform.position-player.position).x, 0f, 0f).normalized) * movementSpeed * Time.deltaTime);
                break;
            default:
                break;
        }

        if (actionTimer <= 0)
        {
            MakeDecision();
            actionTimer = Random.Range(currentState==AIState.Attack ? 0.5f : 0.7f, currentState==AIState.Attack ? 1.2f : 2.5f);
        }
    }

    private void MakeDecision()
    {
        // Determine AI action based on distance, energy, and player attack
        if (distanceToPlayer < retreatRange)
        {
            if (Random.Range(0,100) < 70)
                Retreat();
            else
                Idle();
        }
        else if (distanceToPlayer < attackRange)
        {
            if (player.gameObject.GetComponent<PlayerController>().isPlayerAttacking)
            {
                DefendOrRetreat();
            }
            else
            {
                Attack();
            }
        }
        else
        {
            float rad = Random.Range(0,100);
            if (rad < 20)
                Idle();
            else if(rad<=92)
                ApproachPlayer();
            else
                Retreat();
        }
    }

    private void Attack()
    {
        if(attackTriggers.Count == 0) return;
        currentState = AIState.Attack;
        animator.SetTrigger(attackTriggers[Random.Range(0, attackTriggers.Count)]);
        Debug.Log("Enemy attacks!");
        if(audioSource != null && hitVoices.Length>0){
        audioSource.clip = hitVoices[Random.Range(0, hitVoices.Length)];
        audioSource.Play();
        }
    }

    private void DefendOrRetreat()
    {
        int actionChoice = Random.Range(0, 10);
        
        if (actionChoice < 8)
        {
            currentState = AIState.Defend;
            animator.SetTrigger("block");
            Debug.Log("Enemy defends!");
        }
        else
        {
            Retreat();
        }
    }

    private void PushBackEffect(){
        Vector3 slideVal = (transform.position-player.position).normalized * 0.25f;
        slideVal.y = 0f;
        slideVal.z = 0f;
        characterController.Move(slideVal);
    }

    public void PerformHurt(float damageAmount){
        if(currentState != AIState.Defend){
            animator.SetTrigger("hurt");
            GameManager.Instance.PlayHitSound();
            GameManager.Instance.ShakeCamera();
            fighter.TakeDamage(damageAmount);
        }
        else{
            GameManager.Instance.PlayBlockSound();
        }
        //PushBackEffect();
    }

    private void ApproachPlayer()
    {
        currentState = AIState.Approach;
        animator.SetTrigger("mf");
        Debug.Log("Enemy approaches player.");
    }

    private void Retreat()
    {
        currentState = AIState.Retreat;
        animator.SetTrigger("mb");
        Debug.Log("Enemy retreats.");
    }

    private void Idle()
    {
        currentState = AIState.Idle;
        animator.SetTrigger("idle");
        Debug.Log("Enemy is idling."); 
    }

    public void AttackEffect(string location){
        Transform hitLocation = null;
        if(distanceToPlayer <= attackRange){
            player.gameObject.GetComponent<PlayerController>().PerformHurt(7.0f);
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
