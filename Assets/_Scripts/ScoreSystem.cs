using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem instance;
    private int score = 0;
    TMP_Text scoreText;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreText = GetComponent<TMP_Text>();
        UpdateScore(0);
    }
    public void UpdateScore(int pointValue)
    {
        score += pointValue;
        scoreText.text = "Score: " + score.ToString();
    }
}