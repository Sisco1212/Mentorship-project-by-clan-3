using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDamage : MonoBehaviour
{ 
    public float damage = 10.0f;
    private void OnTriggerEnter(Collider col)
    {
        // Destroy(col.gameObject);
        // Debug.Log("Collided");

       Hitimpact target = col.gameObject.transform.GetComponent<Hitimpact>();
        if (target != null)
        {
            target.TakeDamage(damage);
            Debug.Log("Collided");
        }
    }
}
