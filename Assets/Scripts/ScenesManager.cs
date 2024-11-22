using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager InstanceTwo { get; private set; }

    private bool isScreenTapped = false;
    public Animator tapAnimation;    

    private void Awake()
    {
        if (InstanceTwo == null)
        {
            InstanceTwo = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // tapAnimation = gameObject.Find("TapText").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isScreenTapped)
        {

        isScreenTapped = true;
        tapAnimation.SetBool("ScreenTapped", true);
        // LoadLevelSelection();
        StartCoroutine(Loading());
}
}

    IEnumerator Loading() {
        yield return new WaitForSeconds(2.0f);
        LoadLevelSelection();
    }

   	public void LoadLevelSelection() {
		SceneManager.LoadScene("LevelSelection");
	}

    public void LoadLevel1() {
		SceneManager.LoadScene("FightingScene");
	}

}
