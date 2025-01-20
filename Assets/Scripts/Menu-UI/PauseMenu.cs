using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Script's References")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private ODMGearController[] ODMGearControllerRefs;

    [SerializeField] private Behaviour[] scriptsToDisable;

    [Header("UI")]
    [SerializeField] private static bool gameIsPaused = false;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject settingsWindow;

    private PlayerInput playerInput;

    private void Start()
    {
        playerInput = LoadData.instance.playerInput;
    }
    void Update()
    {
        if (playerInput.actions["PauseMenu"].WasPerformedThisFrame())
        {
            if (gameIsPaused)
                Resume(); // Close the pause menu.
            else
                Paused(); // Open the pause menu.
        }
    }

    // Paused the game.
    void Paused()
    {
        // Disables ODM Gear.
        foreach (ODMGearController odmGear in ODMGearControllerRefs) {
            odmGear.StopODMGear(); }

        // Disable the player's scripts.
        foreach (Behaviour script in scriptsToDisable) {
            script.enabled = false; }


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
        // Enable the player's scripts
        foreach (Behaviour script in scriptsToDisable){
            script.enabled = true; }

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