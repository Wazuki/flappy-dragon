using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject gameOverPanel;

    public int gameScore;
    public Text scoreUI, highScoreUI;


    public Transform bottomObstacle, topObstacle;

    private AudioSource scoreSound;

    public bool gameHasStarted;

    private DragonScript dragonScript;

    public GameObject menuPanel;

    private AudioSource buttonClickSound;

    private GameOverPanel gameOverPanelScript;

    //Init the game by pausing it
    void Awake()
    {
        Time.timeScale = 0f;

        //Set up the keys for PlayerPrefs if they don't exist
        if(!PlayerPrefs.HasKey("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", 0);
            PlayerPrefs.SetFloat("MusicVol", 1.0f);
            PlayerPrefs.SetFloat("SoundVol", 1.0f);

            PlayerPrefs.Save();
        }

    }

	// Use this for initialization
	void Start ()
    {
        gameOverPanelScript = gameOverPanel.GetComponent<GameOverPanel>();

        //scoreUI = GameObject.Find("Score Text").GetComponent<Text>();
        highScoreUI.text = PlayerPrefs.GetInt("HighScore").ToString();
        scoreSound = gameObject.GetComponent<AudioSource>();

        dragonScript = GameObject.Find("Dragon").GetComponent<DragonScript>();

        menuPanel = GameObject.Find("Menu Panel");

        buttonClickSound = GameObject.Find("Button Click Audio Source").GetComponent<AudioSource>();

        Button[] buttons = GameObject.Find("Menu Panel").GetComponentsInChildren<Button>();

        foreach(Button b in buttons)
        {
            b.onClick.AddListener(PlaySoundOnClick);
        }

	}

    public void PlaySoundOnClick()
    {
        if (!buttonClickSound.isPlaying)
        {
            buttonClickSound.Play();
        }
    }


    void Update()
    {
        if(gameHasStarted && !dragonScript.isAlive)
        {
            ShowMenuPanel();
            if (gameScore > PlayerPrefs.GetInt("HighScore"))
            {
                PlayerPrefs.SetInt("HighScore", gameScore);
                PlayerPrefs.Save();
                GameObject.Find("HighScoreLabel").GetComponent<Text>().text = "NEW HIGH SCORE!";
            }
            else
            {
                GameObject.Find("HighScoreLabel").GetComponent<Text>().text = "HIGH SCORE";
            }

            highScoreUI.text = PlayerPrefs.GetInt("HighScore").ToString();

            Time.timeScale = 0f;
            gameOverPanelScript.ShowGameOverPanel();

        }
        

        //Only allow the escape key to quit if this is a UNITY_STANDALONE build
        #if UNITY_STANDALONE

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

        #endif
    }


    public void GmAddScore()
    {
        this.gameScore++;
        scoreUI.text = gameScore.ToString();
        scoreSound.Play();
    }

    void ObstacleSpawner()
    {
        int randObstacleMax = 2;
        if (gameScore > 10) randObstacleMax++;

        int rand = Random.Range(2, randObstacleMax); //Pick which obstacle to spawn based on switch below (0 bottom, 1 top)
        float topObstacleMinY = 1f,
            topObstacleMaxY = 6f,
            bottomObstacleMinY = -6f,
            bottomObstacleMaxY = -1f;

        switch(rand)
        {
            case 0:
                Instantiate(bottomObstacle, new Vector2(9f, Random.Range(bottomObstacleMinY, bottomObstacleMaxY)), Quaternion.identity);
                break;

            case 1:
                Instantiate(topObstacle, new Vector2(9f, Random.Range(topObstacleMinY, topObstacleMaxY)), Quaternion.identity);
                break;
            case 2:
                float spacer = Random.Range(10f, 11.5f);
                float spawnLoc = Random.Range(topObstacleMinY, topObstacleMaxY);
                Instantiate(topObstacle, new Vector2(9f, spawnLoc), Quaternion.identity);
                Instantiate(bottomObstacle, new Vector2(9f, spawnLoc - spacer), Quaternion.identity);
                break;
        }
    }
    
    //Start the game.
    public void PlayButton()
    {
        if(!gameHasStarted)
        {
            //Restore Time.timeScale, then call the repeater.
            Time.timeScale = 1f;
            InvokeRepeating("ObstacleSpawner", 0.5f, 1.5f);

            //Hide the UI menu panel.
            menuPanel.SetActive(false);
            gameHasStarted = true;
        }
    }

    public void ResetGameVariables()
    {
        CancelInvoke();
        gameHasStarted = false;
        gameScore = 0;
        scoreUI.text = gameScore.ToString();
        //Destroy all obstacles and reset the dragon's position.
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacles");

        for (var i = 0; i < obstacles.Length; i++)
        {
            Destroy(obstacles[i]);
        }

        GameObject dragon = GameObject.Find("Dragon");
        dragon.transform.position = new Vector2(-2.61f, 3.98f);
        dragonScript.isAlive = true;
    }

    void ShowMenuPanel()
    {
        menuPanel.SetActive(true);
    }
}
