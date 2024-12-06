using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform gunTransform;
    public GameObject bulletPrefab;
    public AudioSource gunAudiosource;
    public AudioClip gunshotSound;

    public void Shoot()
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
