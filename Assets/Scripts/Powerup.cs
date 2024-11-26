using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public Animator playerAnimator;
    public GameObject sheathedSword;
    public GameObject unSheathedSword;

    public void SwordPowerSlash(){
        playerAnimator.SetTrigger("SwordPowerUp");
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
