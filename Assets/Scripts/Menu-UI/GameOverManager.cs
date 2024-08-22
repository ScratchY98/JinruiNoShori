using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("Script's References")]
    [SerializeField] private PlayerController playerControllerRef;
    [SerializeField] private ODMGearController ODMGearControllerRef;
    [SerializeField] private PlayerAttack playerAttackRef;

    [Header("UI")]
    [SerializeField] private GameObject gameOverUI;

    public void OnPlayerDeath()
    {
        gameOverUI.SetActive(true);
        playerControllerRef.enabled = false;
        ODMGearControllerRef.enabled = false;
        playerAttackRef.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
    }

    public void RetryButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        gameOverUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}