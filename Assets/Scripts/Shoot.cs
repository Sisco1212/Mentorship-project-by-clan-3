using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Transform gunTransform;
    public GameObject bulletPrefab;
    public AudioSource gunAudiosource;
    public AudioClip gunshotSound;

    // private void Start() 
    // {
    //     gunTransform = GetComponent<Transform>();
    // } 

    public void Shooting()
    {
        Instantiate(bulletPrefab, gunTransform.position, bulletPrefab.transform.rotation);
        PlayGunshotSound();
    }

       public void PlayGunshotSound()
    {
        if (gunAudiosource != null)
        {
            gunAudiosource.clip = gunshotSound;
            gunAudiosource.Play();
        }
    }

}
