using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("Script's References")]
    [SerializeField] private ODMGearController ODMGearControllerRef;
    [SerializeField] private PlayerAttack playerAttackRef;

    [Header("UI")]
    [SerializeField] private GameObject gameOverUI;


    public void OnPlayerDeath()
    {
        // // Active l'écran de GameOver.
        gameOverUI.SetActive(true);

        // Unlock the cursor and make it visible
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Stop the time - The world !
        Time.timeScale = 0;
    }

    // Button to restart
    public void RetryButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        gameOverUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    // Button to return to the main menu
    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Button to exit
    public void QuitButton()
    {
        Application.Quit();
    }
}