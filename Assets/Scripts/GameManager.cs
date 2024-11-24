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

    public AudioClip[] hitSounds = {};
    public AudioClip[] blockSounds = {};
    private AudioSource audioSource;
    
    public GameObject winText;
    public GameObject lostText;
    // private ScenesManager scenes;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Assign the singleton instance
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
        dialogueTrigger = FindObjectOfType<DialogueTrigger>();
        audioSource = GetComponent<AudioSource>();
        // scenes = FindObjectOfType<ScenesManager>();
        DontDestroyOnLoad(winText);
        DontDestroyOnLoad(lostText);
    }



     private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null; // Reset Instance when destroyed to allow fresh instance in new scene
        }
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
        fightStarted = false;
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().PerformWin();
        Camera.main.gameObject.GetComponent<CameraController>().StartEllipseRotation();
        if (!isGameWon && !isGameLost)
        {
            isGameWon = true;
            Debug.Log("You Won the Fight!");
    winText.SetActive(true);
        Invoke("LevelSelection", 3.5f);
        }

        // dialogueTrigger.TriggerDialogue();
        winText.SetActive(true);
        // StartCoroutine(Loading());
        Invoke("WinAnimation", 3f);
    }

    private void LevelSelection(){
        // FindObjectOfType<ScenesManager>().LoadLevelSelection();
        SceneManager.LoadScene("LevelSelection");
    }

    public void LoseFight()
    {
        fightStarted = false;
        if (!isGameLost && !isGameWon)
        {
            isGameLost = true;
            Debug.Log("You Lost the Fight!");
        lostText.SetActive(true);
        Invoke("LostAnimation", 3f);
    }

    private void WinAnimation(){
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().PerformWin();
        Camera.main.gameObject.GetComponent<CameraController>().StartEllipseRotation();
    }

    private void LostAnimation(){
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().PerformLost();
        Camera.main.gameObject.GetComponent<CameraController>().StartEllipseRotation();
    }

    public void ResetGameStates()
    {
        isGamePaused = false;
        isGameWon = false;
        isGameLost = false;
        fightStarted = false;
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

    public void PlayHitSound(){
        if(audioSource != null && hitSounds.Length>0){
            audioSource.clip = hitSounds[Random.Range(0, hitSounds.Length)];
            audioSource.Play();
        }
    }

    public void PlayBlockSound(){
        if(audioSource != null && blockSounds.Length>0){
            audioSource.clip = blockSounds[Random.Range(0, blockSounds.Length)];
            audioSource.Play();
        }
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
