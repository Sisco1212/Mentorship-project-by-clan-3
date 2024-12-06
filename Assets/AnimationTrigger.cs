using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    public GameObject chapterName;

    public void ShowChapterName() {
        chapterName.SetActive(true);
    }
}
