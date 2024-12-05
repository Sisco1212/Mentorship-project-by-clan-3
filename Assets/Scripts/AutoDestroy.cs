using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float lifeTime = 1f;
    void Start()
    {
        Invoke("DestroyThis", lifeTime);
    }

    // Update is called once per frame
    void DestroyThis()
    {
        Destroy(gameObject);
    }
}
