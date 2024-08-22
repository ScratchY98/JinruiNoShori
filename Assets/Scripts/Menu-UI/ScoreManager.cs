using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int score;
    [SerializeField] private Text scoreText;

    public static ScoreManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        score = 0;
    }

    public void AddScore(int scoreAmount)
    {
        score += scoreAmount;
        scoreText.text = score.ToString();
    }
}
