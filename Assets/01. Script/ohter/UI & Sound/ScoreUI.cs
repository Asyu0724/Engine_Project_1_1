using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public int currentScore { get; set; }
    private void Awake()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        scoreText.text = "Score     : 0";
        highScoreText.text = $"HiScore : {PlayerPrefs.GetInt("BestScore", 0)}";
    }
    public void UpdateScore(int score)
    {
        currentScore += score;
        scoreText.text = "Score     : " + currentScore;
    }

    public void UpdateHighScore(int score)
    {
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        if (score > bestScore)
        {
            PlayerPrefs.SetInt("BestScore", score);
            PlayerPrefs.Save();
            highScoreText.text = $"HiScore : {score}";
        }
    }
}
