using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueTrigger : MonoBehaviour
{
    public GameObject narrationPanel;
    NarrationTrigger narrationTrigger;
    public GameObject chapterPanel;
    DialogueTrigger dialogueTrigger;

    // Start is called before the first frame update
    void Start()
    {
        narrationTrigger = FindObjectOfType<NarrationTrigger>();
        // narrationTrigger.TriggerNarration();
        dialogueTrigger = FindObjectOfType<DialogueTrigger>();
    }

    public void StartPrologue() {
       if (narrationPanel != null)
       {
        chapterPanel.SetActive(false);
        narrationPanel.SetActive(true);
        narrationTrigger.TriggerNarration();
       }

       else {
         chapterPanel.SetActive(false);
        GameManager.Instance.PlayBgMusic();
        dialogueTrigger.TriggerDialogue();
       }
    }

}
