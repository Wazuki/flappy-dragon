using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour {

    private Text gameOverScore;
    private InputField nameInputField;
    private GameManager gameManagerScript;
    private LeaderboardManager leaderboardManagerScript;
	// Use this for initialization
	void Start ()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        leaderboardManagerScript = GameObject.Find("Leaderboard Panel").GetComponent<LeaderboardManager>();

        gameOverScore = GameObject.Find("Game Over Score").GetComponent<Text>();

        nameInputField = GameObject.Find("Name Input Field").GetComponent<InputField>();
        gameObject.SetActive(false);
	}
	
	public void ShowGameOverPanel()
    {
        gameObject.SetActive(true);
        nameInputField.textComponent.text = "";
        nameInputField.placeholder.GetComponent<Text>().text = "Click me";
        gameOverScore.text = "SCORE " + gameManagerScript.gameScore.ToString();        
    }

    public void CloseGameOverPanel()
    {
        gameObject.SetActive(false);
        gameManagerScript.ResetGameVariables();
    }

    public void SubmitScoreInput()
    {
        Debug.Log("Attempting to submit score.");
        if (nameInputField.text.Length == 0)
        {
            Debug.Log("Error: name not long enough");
            nameInputField.placeholder.GetComponent<Text>().text = "Please enter a name.";
            return;
        }
        Debug.Log("Sending score: " + nameInputField.text + ": " + gameManagerScript.gameScore);
        leaderboardManagerScript.SubmitNewScore(nameInputField.text, gameManagerScript.gameScore);
        Debug.Log("Score sent");

        CloseGameOverPanel();
    }
}
