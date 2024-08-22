using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Script's References")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private ODMGearController ODMGearControllerRef;
    [SerializeField] private PlayerAttack playerAttack;

    [Header("UI")]
    [SerializeField] private static bool gameIsPaused = false;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject settingsWindow;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Paused();
            }
        }
    }

    void Paused()
    {
        playerController.enabled = false;
        ODMGearControllerRef.StopODMGear();
        ODMGearControllerRef.enabled = false;
        playerAttack.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        gameIsPaused = true;
    }

    public void Resume()
    {
        playerController.enabled = true;
        ODMGearControllerRef.enabled = true;
        playerAttack.enabled = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        gameIsPaused = false;
    }

    public void OpenSettingsWindow()
    {
        settingsWindow.SetActive(true);
    }

    public void CloseSettingsWindow()
    {
        settingsWindow.SetActive(false);
    }

    public void LoadMainMenu()
    {
        Resume();
        SceneManager.LoadScene("MainMenu");
    }
}