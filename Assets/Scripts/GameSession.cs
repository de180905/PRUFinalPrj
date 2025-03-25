using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;

    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;
    
    void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start() 
    {
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();    
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    public void AddToScore(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = score.ToString(); 
    }

    void TakeLife()
    {
        playerLives--;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text = playerLives.ToString();
    }
    public void SaveScore(int score)
    {
        // Save last score
        PlayerPrefs.SetInt("LastScore", score);
        // Save highest score if the new score is greater
        if (score > PlayerPrefs.GetInt("HighestScore", 0))
        {
            PlayerPrefs.SetInt("HighestScore", score);
        }
        // Save changes
        PlayerPrefs.Save();
    }
    void ResetGameSession()
    {
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SaveScore(score);
        SceneManager.LoadScene("Start");
        Destroy(gameObject);
    }
}
