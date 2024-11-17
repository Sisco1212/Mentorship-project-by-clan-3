using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitimpact : MonoBehaviour
{
    public float health = 30.0f;

    public void TakeDamage(float amount)
    {
        health -=amount;

        if(health <= 0f)
        Die();
    }

    void Die() {
        Destroy(gameObject);
    }
}
