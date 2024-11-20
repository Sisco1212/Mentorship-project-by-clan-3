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
    private float gravity = 10f;
    private Vector3 velocity;

    private void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        currentHealth = maxHealth;
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
}



