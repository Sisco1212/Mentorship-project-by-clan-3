using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20.0f;
    public Rigidbody rb;
    public GameObject gunImpactEffect;
    public AudioSource audioSource;
    // public AudioClip explosionSound;


    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    void OnTriggerEnter(Collider collider) {

    EnemyAI enemy = collider.GetComponent<EnemyAI>();
    
    if (enemy != null)
    {
        enemy.PerformHurt(30.0f, "hurt");
        GameObject gunImpactEffectCopy = Instantiate(gunImpactEffect, transform.position, transform.rotation);
        Destroy(gunImpactEffectCopy, 2f);
        FindObjectOfType<PlayerController>().PlayExplosionSound();
    }
        Debug.Log(collider.name);
        Destroy(gameObject);
    }
}
