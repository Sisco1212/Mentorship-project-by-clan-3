using UnityEngine;
using UnityEngine.UI;

public class Fighter : MonoBehaviour
{
    private Animator animator;
    private CharacterController controller;
    public Transform leftFootTransform;
    public Transform rightFootTransform;
    public Transform leftHandTransform;
    public Transform rightHandTransform;

    public Slider healthSlider;
    public float maxHealth = 100f;
    private float currentHealth;
    public bool isDead = false;
    public float gravity = 9.8f;
    private Vector3 velocity;

    private float jumpHeight = 1f;
    private float jumpVelocity;

    private void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        currentHealth = maxHealth;
        jumpVelocity = Mathf.Sqrt(jumpHeight * 2f * gravity);
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthSlider.value = currentHealth;

        if(!isDead && currentHealth<=0){
            isDead = true;
            if(gameObject.CompareTag("Player")){
                GameManager.Instance.LoseFight();
            }
            else{
                GameManager.Instance.WinFight();
            }
        }
    }

    void LateUpdate(){
        animator.SetBool("dead", isDead);
        Vector3 position = transform.position;
        position.z = 0;
        transform.position = position;
        

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Apply gravity over time
        velocity.y -= gravity * Time.deltaTime;

        // Apply velocity to move character downwards
        controller.Move(velocity * Time.deltaTime);
    }

    public void ApplyJumpForce(){
        velocity.y = jumpVelocity;
    }

    public void Jump()
    {
        if (controller.isGrounded)
        {
            animator.Play("Jump");
        }
    }
}



