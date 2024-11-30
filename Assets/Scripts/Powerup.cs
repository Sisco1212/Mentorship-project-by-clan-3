using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Powerup : MonoBehaviour
{
    public Animator playerAnimator;
    public GameObject sheathedSword;
    public GameObject unSheathedSword;
    private Button powerupButton;
    private EnemyAI enemyAIScript;

    void Start ()
    {
        powerupButton = GetComponent<Button>();
        powerupButton.interactable = false;
        enemyAIScript = FindObjectOfType<EnemyAI>();
    }


    public void SwordPowerSlash(){
        playerAnimator.SetTrigger("SwordPowerUp");
        enemyAIScript.powerupCharge = 0;
        enemyAIScript.Idle();
        powerupButton.interactable = false;
    }

    public void StartSwordPowerup()
    {
        sheathedSword.SetActive(false);
        unSheathedSword.SetActive(true);
    }

    public void EndSwordPowerup()
    {
        sheathedSword.SetActive(true);
        unSheathedSword.SetActive(false);
    }

}
