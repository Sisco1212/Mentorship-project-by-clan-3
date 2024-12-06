using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunPowerup : MonoBehaviour
{
    public GameObject gunInThigh;
    public GameObject gunInHand;
    public Animator playerAnimator;
    private EnemyAI enemyAIScript;
    public Button gunPowerupButton;
    AudioSource gAudiosource;
    public AudioClip loadGunSound;

    void Start ()
    {
        enemyAIScript = FindObjectOfType<EnemyAI>();
        gAudiosource = GetComponent<AudioSource>();
    }

    public void StartGun() {
        playerAnimator.SetTrigger("Shoot");
        enemyAIScript.powerupCharge = 0;
        enemyAIScript.comboBar.value = 0;
        gunPowerupButton.interactable = false;
        enemyAIScript.BeInactiveFor(3.2f);
    }

     public void StartGunPowerup()
    {
        gunInThigh.SetActive(false);
        gunInHand.SetActive(true);
    }

    public void EndGunPowerup()
    {
        gunInThigh.SetActive(true);
        gunInHand.SetActive(false);
    }

     public void PlayLoadGunSound()
    {
        if (gAudiosource != null)
        {
            gAudiosource.clip = loadGunSound;
            gAudiosource.Play();
        }
    }
}
