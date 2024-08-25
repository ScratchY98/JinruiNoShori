using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int score;      
    [SerializeField] private Text scoreText;

    public static ScoreManager instance;

    // Create an instance of ScoreManager.
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There is more than one instance of ScoreManager in the scene.");
            return;
        }

        instance = this;
    }

    // Initializes the score to 0 at Start.
    private void Start()
    {
        score = 0;
    }

    // Add score.
    public void AddScore(int scoreAmount)
    {
        score += scoreAmount;
        scoreText.text = score.ToString();
    }
}
