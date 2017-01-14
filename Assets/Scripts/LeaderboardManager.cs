using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard
{
    private string name;
    private int score;

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public int Score
    {
        get { return score; }
        set { score = value; }
    }
}

public class LeaderboardManager : MonoBehaviour {

    public GameObject leaderboardScore;
    public GameObject retrievingScores;

    public List<Leaderboard> leaderboard;

    private GameObject leaderboardScoreContainer;

    private int leaderboardSpacer;

	// Use this for initialization
	void Start ()
    {
        leaderboardSpacer = Screen.height / 12;
        leaderboard = new List<Leaderboard>();

        leaderboardScoreContainer = GameObject.Find("Leaderboard Score Container");

        gameObject.SetActive(false);
    }
	
    public void ShowLeaderboardPanel()
    {
        gameObject.SetActive(true);
        //Destroy all old score objects before refreshing the leaderboard
        GameObject[] scores = GameObject.FindGameObjectsWithTag("LeaderboardScore");
        foreach (GameObject s in scores) Destroy(s);

        RefreshLeaderboard();
    }

    public void HideLeaderboardPanel()
    {
        gameObject.SetActive(false);
    }

    public void RefreshLeaderboard()
    {
        retrievingScores.SetActive(true);
        //Get the initial top 10 scores for the scoreboard
        string url = "flappydragon.wazuki.me/leaderboard.php";

        WWW www = new WWW("https://" + url);
        Debug.Log("Attempting WWW request");
        StartCoroutine(RetrieveLeaderboard(www));

    }

    public void DisplayScores()
    {
        retrievingScores.SetActive(false);
        int lbCt = 1;
        foreach (Leaderboard lb in leaderboard)
        {
            var scoreObject = Instantiate(leaderboardScore, leaderboardScoreContainer.transform);
            scoreObject.GetComponent<Text>().text = lb.Name + " " + lb.Score.ToString();

            var leaderboardHeader = GameObject.Find("Leaderboard Header");
            scoreObject.transform.SetParent(leaderboardScoreContainer.transform);
            scoreObject.transform.position = new Vector2(leaderboardHeader.transform.position.x, leaderboardHeader.transform.position.y - (leaderboardSpacer * lbCt));
            lbCt++;
        }
    }

    IEnumerator RetrieveLeaderboard(WWW www)
    {
        yield return www;

        //Check for errors
        if(www.error == null)
        {
            Debug.Log("WWW okay!: " + www.text);
            
            string[] lines = www.text.Split('\n');

            //Empty the Leaderboard list before pulling in the new data
            leaderboard.Clear();
            int ct = 0;
            foreach(string l in lines)
            {
                ct++;
                string[] splitLine = l.Split('|');
                Debug.Log(l);

                if (splitLine[0] == "") break; //Break out of the loop if splitline is empty.

                Leaderboard lb = new Leaderboard();
                lb.Name = splitLine[0];
                lb.Score = int.Parse(splitLine[1]);
                Debug.Log(lb.Score);
                leaderboard.Add(lb);
                if (ct == 10) break;
            }

            foreach (Leaderboard lb in leaderboard)
            {
                Debug.Log(lb.Name + " scored a " + lb.Score);
            }

            DisplayScores();
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    public void SubmitNewScore(string name, int score)
    {
        
        string url = "flappydragon.wazuki.me/leaderboard.php?name=" + WWW.EscapeURL(name) + "&score=" + score.ToString();
        WWW www = new WWW("https://" + url);
        Debug.Log("Sending new score");
    }
}
