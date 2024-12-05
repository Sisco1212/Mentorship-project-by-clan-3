using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrationTrigger : MonoBehaviour
{
    public Narration narration;

    // void Start()
    // {
    //     TriggerDialogue();
    // }

    public void TriggerNarration()
    {
        FindObjectOfType<NarrationManager>().StartDialogue(narration);
        // gameObject.SetActive(false);
    }
}
