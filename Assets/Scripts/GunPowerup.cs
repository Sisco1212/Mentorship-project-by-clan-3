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
    private Button gunPowerupButton;

    void Start ()
    {
        gunPowerupButton = GetComponent<Button>();
        enemyAIScript = FindObjectOfType<EnemyAI>();
    }

    public void StartGun() {
        playerAnimator.SetTrigger("Shoot");
        enemyAIScript.powerupCharge = 0;
        enemyAIScript.comboBar.value = 0;
        // gunPowerupButton.interactable = false;
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
}
