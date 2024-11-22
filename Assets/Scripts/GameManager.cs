using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool isGamePaused = false;
    private bool isGameWon = false;
    private bool isGameLost = false;
    public bool fightStarted = false;
    DialogueTrigger dialogueTrigger;
    
    public GameObject winText;
    public GameObject lostText;
    private ScenesManager scenes;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        dialogueTrigger = FindObjectOfType<DialogueTrigger>();
        scenes = FindObjectOfType<ScenesManager>();
    }

    public void PauseGame()
    {
        if (!isGamePaused)
        {
            isGamePaused = true;
            Time.timeScale = 0f;
        }
    }

    public void ResumeGame()
    {
        if (isGamePaused)
        {
            isGamePaused = false;
            Time.timeScale = 1f; 
        }
    }

    public void WinFight()
    {
        if (!isGameWon && !isGameLost)
        {
            isGameWon = true;
            Debug.Log("You Won the Fight!");
        }

        // dialogueTrigger.TriggerDialogue();
    winText.SetActive(true);
    // StartCoroutine(Loading());
    scenes.LoadLevelSelection();
    }

    public void LoseFight()
    {
        if (!isGameLost && !isGameWon)
        {
            isGameLost = true;
            Debug.Log("You Lost the Fight!");
        }

        lostText.SetActive(true);
    }

    public void ResetGameStates()
    {
        isGamePaused = false;
        isGameWon = false;
        isGameLost = false;
        Time.timeScale = 1f;  // Ensure time scale is reset
    }

    public void ShakeCamera(float magnitude=0.08f, float duration=0.2f){
        Camera.main.gameObject.GetComponent<CameraController>().StartShake(magnitude, duration);
    }

    	public void LoadMenu() {
		SceneManager.LoadScene("Menu");
	}
        public void LoadLevel1() {
		SceneManager.LoadScene("FightingScene");
	}
        public void Retry() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

    //     public void LoadLevelSelection() {
	// 	SceneManager.LoadScene("LevelSelection");
	// }

//  IEnumerator Loading() {
//         yield return new WaitForSeconds(2.0f);
//         LoadLevelSelection();
//     }
}
