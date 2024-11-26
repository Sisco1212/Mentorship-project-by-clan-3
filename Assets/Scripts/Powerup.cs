using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public Animator playerAnimator;
    public GameObject sheathedSword;
    public GameObject unSheathedSword;

    // Transform hitLocation = null;
        
    //     if(distanceToEnemy <= attackRange){
    //         enemy.gameObject.GetComponent<EnemyAI>().PerformHurt(3.5f);
    //     }


    public void StartSwordPowerup()
    {
        playerAnimator.SetBool("SwordPowerup", true);
        sheathedSword.SetActive(false);
        unSheathedSword.SetActive(true);
        Invoke("EndSwordPowerup", 2f);
    }

    public void EndSwordPowerup()
    {
        playerAnimator.SetBool("SwordPowerup", false);
        sheathedSword.SetActive(true);
        unSheathedSword.SetActive(false);
    }
}
