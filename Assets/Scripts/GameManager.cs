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
    public AudioClip fightSound;
    public AudioClip ko;
    private AudioSource audioSource;
    
    public GameObject winText;
    public GameObject lostText;

    public int nextSceneLoad;
   
    private void Awake()
    {
        Instance = this; // Assign the singleton instance
        dialogueTrigger = FindObjectOfType<DialogueTrigger>();
        audioSource = GetComponent<AudioSource>();
        nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1;
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
        if (!isGameWon && !isGameLost)
        {
            isGameWon = true;
            Debug.Log("You Won the Fight!");
        }
        winText.SetActive(true);
        Invoke("WinAnimation", 3f);
        PlayKOSound();
    }

    private void LevelSelection(){
        SceneManager.LoadScene("LevelSelection");

        if (nextSceneLoad > PlayerPrefs.GetInt("levelAt"))
        {
            PlayerPrefs.SetInt("levelAt", nextSceneLoad);
        }
    }

    public void LoseFight()
    {
        fightStarted = false;
        if (!isGameLost && !isGameWon)
        {
            isGameLost = true;
            Debug.Log("You Lost the Fight!");
        }
        lostText.SetActive(true);
        Invoke("LostAnimation", 3f);
        PlayKOSound();
    }

    private void WinAnimation(){
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().PerformWin();
        GameObject.FindWithTag("Enemy").SetActive(false);
        Camera.main.gameObject.GetComponent<CameraController>().StartEllipseRotation();
        Invoke("LevelSelection", 8f);
    }

    private void LostAnimation(){
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().PerformLost();
        GameObject.FindWithTag("Enemy").SetActive(false);
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
        ResetGameStates();
		SceneManager.LoadScene("Menu");
	}
    public void LoadLevel1() {
        ResetGameStates();
		SceneManager.LoadScene("Level1");
	}

    public void PlayHitSound(){
        if(audioSource != null && hitSounds.Length>0){
        audioSource.clip = hitSounds[Random.Range(0, hitSounds.Length)];
        audioSource.Play();
        }
    }

    public void PlayFightSound() {
        if(audioSource != null){
        audioSource.clip = fightSound;
        audioSource.PlayOneShot(fightSound);
        }
    }
   
    public void PlayKOSound() {
        if(audioSource != null){
        audioSource.clip = ko;
        audioSource.PlayOneShot(ko);
        }
    }

    public void PlayBlockSound(){
        if(audioSource != null && blockSounds.Length>0){
            audioSource.clip = blockSounds[Random.Range(0, blockSounds.Length)];
            audioSource.Play();
        }
    }
    public void Retry() {
        ResetGameStates();
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
