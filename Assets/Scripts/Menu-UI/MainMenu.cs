using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    private string mapToLoad;
    [SerializeField] private GameObject settingsWindow;
    [SerializeField] private Dropdown mapDropdown;
    [SerializeField] private List<string> maps = new List<string>();

    public void Start()
    {
        // Unlock the cursor and make it visible
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        mapDropdown.ClearOptions();
        mapDropdown.AddOptions(maps);

        mapToLoad = maps[0];
    }

    // Button to start the game
    public void StartGame()
    {
        SceneManager.LoadScene(mapToLoad);
    }

    // Button to open the Setting Window.
    public void SettingsButton()
    {
        settingsWindow.SetActive(true);
    }

    public void ChangeMap(int map)
    {
        mapToLoad = maps[map];
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