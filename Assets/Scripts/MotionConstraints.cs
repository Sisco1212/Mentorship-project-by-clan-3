using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionConstraints : MonoBehaviour
{

    public bool canMoveLeft = true, canMoveRight = true;
    public float leftPos = -5f;
    public float rightPos = 5f;
    void Update()
    {
        if(transform.position.x <= leftPos){
            canMoveLeft = false;
        }
        else{
            canMoveLeft = true;
        }
        if(transform.position.x >= rightPos){
            canMoveRight = false;
        }
        else{
            canMoveRight = true;
        }
    }
}
