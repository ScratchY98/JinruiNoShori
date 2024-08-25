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
                // Close the pause menu.
                Resume();
            }
            else
            {
                // Open the pause menu.
                Paused();
            }
        }
    }

    // Paused the game.
    void Paused()
    {
        // Disable the playerController script.
        playerController.enabled = false;

        // Disables the player's attack.
        playerAttack.enabled = false;

        // Disables ODM Gear.
        ODMGearControllerRef.StopODMGear();
        ODMGearControllerRef.enabled = false;

        // Unlock the cursor and make it visible.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Activate the menu.
        pauseMenuUI.SetActive(true);

        // Stop the time.
        Time.timeScale = 0;
        gameIsPaused = true;
    }

    // Resume the Game.
    public void Resume()
    {
        // Enable player scripts.
        playerController.enabled = true;
        ODMGearControllerRef.enabled = true;
        playerAttack.enabled = true;

        // Confines the cursor and makes it invisible.
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        // Disable the menu.
        pauseMenuUI.SetActive(false);

        // Sets the time to 1 (normal value).
        Time.timeScale = 1;
        gameIsPaused = false;
    }

    // Open the Settings Windows.
    public void OpenSettingsWindow()
    {
        settingsWindow.SetActive(true);
    }

    // Close the Settings Windows.
    public void CloseSettingsWindow()
    {
        settingsWindow.SetActive(false);
    }

    // Open the Main Menu.
    public void LoadMainMenu()
    {
        Resume();
        SceneManager.LoadScene("MainMenu");
    }
}