using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NarrationManager : MonoBehaviour
{

    private Queue<string> sentences;
    private Queue<string> names;
    // private Queue<Sprite> images;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    // public Image characterImage;
    
    NarrationTrigger narrationTrigger;

    public Animator animator;
    private AsyncLoader loader;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        names = new Queue<string>();
        // images = new Queue<Sprite>();
        narrationTrigger = FindObjectOfType<NarrationTrigger>();
        narrationTrigger.TriggerNarration();
        loader = FindObjectOfType<AsyncLoader>();
    }

    public void StartDialogue(Narration dialogue)
    {
        animator.SetBool("IsOpen", true);
        // nameText.text = dialogue.name;
        sentences.Clear();
        names.Clear();
        // images.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        
        foreach (string name in dialogue.names)
        {
            names.Enqueue(name);
        }

        // foreach (Sprite image in dialogue.images)
        // {
            // images.Enqueue(image);
        // }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

       string sentence = sentences.Dequeue();
       string name = names.Dequeue();
    //    Sprite image = images.Dequeue();
       dialogueText.text = sentence;
       nameText.text = name;
    //    characterImage.GetComponent<Image>().sprite = image;
        StopAllCoroutines();
       StartCoroutine(TypeSentence(sentence));
    }

    public IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.035f); // Wait for the set time between each letter
        }
    }

    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        Debug.Log("End of conversation");
        StartCoroutine(loader.LoadLevelASync("Level1"));
    }

    
}
