using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    
    private bool gameOver = false;
    private AudioSource audioSource;
    private AudioSource audioBGM;
    private float gameOverStartTime;

    public KinectManager kinectManager;
    public EnemyController enemyController;
    public GameObject hideScenePanel;
    public Text textResult;
    public float gameDuration = 20.0f;
    public AudioClip gameOverAudio;
    public AudioClip startGameAudio;
    public AudioClip gameplayAudio;
    public bool gameModeTime = true;
    public int maxLives = 0;
    public static int lives = 0;
    public float gameOverDuration = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponents<AudioSource>()[1];
        audioSource.clip = startGameAudio;
        audioSource.Play();

        audioBGM = this.GetComponents<AudioSource>()[0];
        audioBGM.clip = gameplayAudio;
        audioBGM.Play();

        lives = maxLives;
    }

    // Update is called once per frame
    void Update()
    {

        if (gameModeTime)
        {
            //Spawn of enemies
            if (Time.timeSinceLevelLoad > gameDuration && !gameOver)
            {
                StartGameOver();
            }
            else if (Time.timeSinceLevelLoad > gameDuration + gameOverDuration && gameOver)
            {
                ReturnToMenu();
            }
        }
        else
        {
            if (lives == 0 && !gameOver)
            {
                StartGameOver();
            }

            if((Time.time > gameOverStartTime + gameOverDuration) && gameOver)
            {
                ReturnToMenu();
            }
        }
        
    }

    private void StartGameOver()
    {
        gameOver = true;

        //Stops sound
        audioBGM.Stop();

        audioSource.clip = gameOverAudio;
        audioSource.loop = false;
        audioSource.Play();


        //Disable kinect
        if (kinectManager != null)
            kinectManager.enabled = false;

        //Disable enemies
        enemyController.DestroyAllEnemies();
        enemyController.enabled = false;

        //Show results
        hideScenePanel.SetActive(true);
        textResult.gameObject.SetActive(true);

        textResult.text = "Player 1: " + ScoreManager.P1Score + "\n" +
                        "Player 2: " + ScoreManager.P2Score + "\n" +
                        "Player 3: " + ScoreManager.P3Score + "\n" +
                        "Player 4: " + ScoreManager.P4Score + "\n";


        gameOverStartTime = Time.time;

    }

    private void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}
