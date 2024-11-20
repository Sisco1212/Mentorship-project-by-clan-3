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
    public float walkSpeed = 1.0f;

    public CharacterController characterController;
    
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
    

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        fighter = GetComponent<Fighter>();
        opponentFighter = player.gameObject.GetComponent<Fighter>();
        currentState = AIState.Idle;
        actionTimer = actionCooldown;
    }

    void Update()
    {
        if(fighter.isDead || opponentFighter.isDead) return;
        distanceToPlayer = Mathf.Abs(transform.position.x - player.position.x);
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3((player.position-transform.position).x, 0f, 0f));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        actionTimer -= Time.deltaTime;

        switch(currentState){
            case AIState.Approach:
                characterController.Move((new Vector3((player.position-transform.position).x, 0f, 0f).normalized) * walkSpeed * Time.deltaTime);
                if(distanceToPlayer <= attackRange)
                    Idle();
                break;
            case AIState.Retreat:
                characterController.Move((new Vector3((transform.position-player.position).x, 0f, 0f).normalized) * walkSpeed * Time.deltaTime);
                break;
            default:
                break;
        }

        if (actionTimer <= 0)
        {
            MakeDecision();
            actionTimer = Random.Range(0.7f, 2.5f);
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
                DefendOrDodge();
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
    }

    private void DefendOrDodge()
    {
        int actionChoice = Random.Range(0, 2);
        
        if (actionChoice == 0)
        {
            currentState = AIState.Defend;
            animator.SetTrigger("block");
            Debug.Log("Enemy defends!");
        }
        else
        {
            /* currentState = AIState.Dodge;
            animator.SetTrigger("Dodge");
            Debug.Log("Enemy dodges!"); */
        }
    }

    public void PerformHurt(float damageAmount){
        if(currentState != AIState.Defend){
            animator.SetTrigger("hurt");
            fighter.TakeDamage(damageAmount);
        }
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
