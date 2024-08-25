using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string levelToLoad;
    [SerializeField] private GameObject settingsWindow;

    public void Start()
    {
        // Unlock the cursor and make it visible
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Button to start the game
    public void StartGame()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    // Button to open the Setting Window.
    public void SettingsButton()
    {
        settingsWindow.SetActive(true);
    }

    // Button to Close the Settings Window.
    public void CloseSettingsWindow()
    {
        settingsWindow.SetActive(false);
    }

    // Button to exit
    public void QuitGame()
    {
        Application.Quit();
    }
}