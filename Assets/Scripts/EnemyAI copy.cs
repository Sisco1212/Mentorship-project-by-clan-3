/* using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Animator animator;
    private Transform player;

    // AI properties
    public float attackRange = 2.2f;
    public float retreatRange = 1.0f;
    public float actionCooldown = 1.0f;

    private float actionTimer;
    private bool isPlayerAttacking;

    private enum AIState { Idle, Move, Attack, Defend }
    private AIState currentState;

    private float speed = 0f;
    private float attacking = -1f;
    private float distanceToPlayer;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        currentState = AIState.Idle;
        actionTimer = actionCooldown;
    }

    void Update()
    {
        actionTimer -= Time.deltaTime;
        distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (actionTimer <= 0)
        {
            DecideAction();
            actionTimer = Random.Range(1.0f, 3.0f);
        }

        // Update Animator parameters to control blend trees
        animator.SetFloat("Speed", speed);
        animator.SetFloat("Attacking", attacking);
    }

    private void DecideAction()
    {
        // Determine the AI's action based on distance and player actions
        CheckIfPlayerIsAttacking();

        if (distanceToPlayer < attackRange)
        {
            if (isPlayerAttacking)
            {
                Defend();
            }
            else
            {
                Attack();
            }
        }
        else if (distanceToPlayer < retreatRange)
        {
            if (Random.Range(0, 100) < 30)
                Retreat();
            else
                ApproachPlayer();
        }
        else
        {
            Idle();
        }
    }

    private void CheckIfPlayerIsAttacking()
    {
        // Implement logic to detect if player is attacking (e.g., using collision detection)
        isPlayerAttacking = false;
    }

    private void Attack()
    {
        currentState = AIState.Attack;
        speed = 0;  // Set speed to 0 since we're attacking (no movement)
        attacking = (float)Random.Range(0, 5);
        Debug.Log("Enemy attacks!");
    }

    private void Defend()
    {
        currentState = AIState.Defend;
        speed = 0;  // Set speed to 0 since we're defending
        Debug.Log("Enemy defends!");
    }

    private void ApproachPlayer()
    {
        currentState = AIState.Move;
        speed = 1;  // Set speed > 0 to indicate approaching
        attacking = -1f;
        Debug.Log("Enemy approaches player.");
    }

    private void Retreat()
    {
        currentState = AIState.Move;
        speed = -1;  // Set speed < 0 to indicate retreating
        attacking = -1f;
        Debug.Log("Enemy retreats.");
    }

    private void Idle()
    {
        currentState = AIState.Idle;
        speed = 0;  // Set speed to 0 for idle
        attacking = -1f;
        Debug.Log("Enemy is idling.");
    }
}
 */