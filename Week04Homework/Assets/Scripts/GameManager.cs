using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TextMeshProUGUI display;
    
    public int score;
    public int CurrentHighScore;
    public string playerName = "";
    
    //creating file directory to place saved information
    const string FILE_DIR = "/DATA/";
    const string DATA_FILE = "highScores.txt";
    string FILE_FULL_PATH;
    
    //establishing variable Score, that gets the current score variable, and sets the score as Score to be referenced in saved data.
    public int Score
    {
        get
        {
            return score;
        }

        set { score = value; }

    }
    
    public string PlayerName
    {
        get
        {
            return playerName;
        }

        set { playerName = value; }

    }

    // Creating the string of high scores & a list to be placed into. 
    string highScoresString = "";
    
    List<int> highScores;

    public List<int> HighScores
    {
        get
        {
            //if high Scores list exists and the file exists, we pull from the file
            if (highScores == null && File.Exists(FILE_FULL_PATH))
            {
                Debug.Log("got from file");
                //in the high scores list, we make a new list for the current list can exist into.
                highScores = new List<int>();
                
                //reading all the text inside the file
                highScoresString = File.ReadAllText(FILE_FULL_PATH);
                //trimming at every space
                highScoresString = highScoresString.Trim();
                //new string array, that is auto split after input with a line break
                string[] highScoreArray = highScoresString.Split("\n");
                //only add to the array if the high score is higher than the current score on the list
                for (int i = 0; i < highScoreArray.Length; i++)
                {
                    int currentHighScore = Int32.Parse(highScoreArray[i]);
                    CurrentHighScore = currentHighScore;
                    highScores.Add(currentHighScore);
                }
            }
            else if(highScores == null)
            {
                Debug.Log("NOPE");
                highScores = new List<int>();
                highScores.Add(3);
                highScores.Add(2);
                highScores.Add(1);
                highScores.Add(0);
            }

            return highScores;
        }
    }
    
    
    

    float timer = 0;

    public int maxTime = 10;
    //are you in the game? This is easier to set level change more reliably than the timer
    bool isInGame = true;
    
    void Awake()
    {
        //singleton so that it can transfer into a new level with the same settings saved
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        //the file path is created & accessed
        FILE_FULL_PATH = Application.dataPath + FILE_DIR + DATA_FILE;
    }
    void Update()
    {
        //if you're in the game level/s you will see the current score & timer only
        if (isInGame)
        {
            display.text = "Score: " + score + "\nTime:" + (maxTime - (int)timer);
        }
        //otherwise you will have ended the game & now you are shown the end game text
        else
        {
            display.text = "GAME OVER\nFINAL SCORE: " + score + PlayerName +
                           "\nHigh Scores:\n" + highScoresString;
        }

        //add the fraction of a second between frames to timer
        timer += Time.deltaTime;
        
        //if timer is >= maxTime. We change scenes to the endgame, & make sure high scores are set & saved
        if (timer >= maxTime && isInGame)
        {
            isInGame = false;
            SceneManager.LoadScene("EndScene");
            SetHighScore();
        }
    }

    //checking if the current score can be a high score
    bool IsHighScore(int score)
    {
        //if the score is greater than any current high score, then this is a new high score
        for (int i = 0; i < HighScores.Count; i++)
        {
            if (CurrentHighScore < score)
            {
                return true;
            }
        }

        return false;
    }

    void SetHighScore()
    {   //if there is a new high score, we remove a score from the top slot & replace it with the new score
        //this starts at place 0 in the array 
        if (IsHighScore(score))
        {
            int highScoreSlot = -1;

            for (int i = 0; i < HighScores.Count; i++)
            {
                if (score > CurrentHighScore)
                {
                    highScoreSlot = i;
                    break;
                }
            }
            
            //inserting the new high score by slotting it into the array
            highScores.Insert(highScoreSlot, score);
            //the array range is limited by 5, so lower scores get booted out from display
            highScores = highScores.GetRange(0, 5);
            //creating a new variable to be referenced in the scoreboard
            //by establishing that this variable contains the array of High Scores
            string scoreBoardText = "";

            foreach (var highScore in highScores)
            {   
                scoreBoardText += highScore + PlayerName + "\n";
            }

            highScoresString = scoreBoardText;
            //on game over we log the newest score list to rewrite any new scores    
            File.WriteAllText(FILE_FULL_PATH, highScoresString);
        }
    }
}
