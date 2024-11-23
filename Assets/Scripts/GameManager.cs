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
    public AudioClip Background ;

    private AudioSource audioSource;
    [SerializeField] AudioSource Sfx;
    
    public GameObject winText;
    public GameObject lostText;
    private ScenesManager scenes;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Assign the singleton instance
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
        dialogueTrigger = FindObjectOfType<DialogueTrigger>();
        audioSource = GetComponent<AudioSource>();
        scenes = FindObjectOfType<ScenesManager>();
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
        if (!isGameWon && !isGameLost)
        {
            isGameWon = true;
            Debug.Log("You Won the Fight!");
        }

        // dialogueTrigger.TriggerDialogue();
    winText.SetActive(true);
    // StartCoroutine(Loading());
        Invoke("LevelSelection", 3.5f);
    }

    private void LevelSelection(){
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

    private void Start()
    {
        Sfx.clip = Background;
        Sfx.Play(); 

    }
    //     public void LoadLevelSelection() {
    // 	SceneManager.LoadScene("LevelSelection");
    // }

    //  IEnumerator Loading() {
    //         yield return new WaitForSeconds(2.0f);
    //         LoadLevelSelection();
    //     }
}
