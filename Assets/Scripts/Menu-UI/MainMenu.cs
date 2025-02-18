using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    private string mapToLoad;
    [SerializeField] private GameObject settingsWindow;
    [SerializeField] private GameObject creditWindow;
    [SerializeField] private GameObject changeAvatarWindow;
    [SerializeField] private Dropdown mapDropdown;
    [SerializeField] private List<string> maps = new List<string>();
    [SerializeField] private ChangeAvatar changeAvatar;

    public void Start()
    {
        // Unlock the cursor and make it visible
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        mapDropdown.ClearOptions();
        mapDropdown.AddOptions(maps);

        mapToLoad = maps[0];
        changeAvatar.LoadAllColor();
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

    public void OpenChangeAvatarWindow()
    {
        changeAvatarWindow.SetActive(true);
    }

    public void CloseChangeAvatarWindow()
    {
        changeAvatarWindow.SetActive(false);
    }

    public void ChangeMap(int map)
    {
        mapToLoad = maps[map];
    }

    // Button to Close the Settings Window.
    public void CloseSettingsWindow()
    {
        settingsWindow.SetActive(false);
        changeAvatar.SaveAllColor();
    }

    public void OpenCreditWindow()
    {
        creditWindow.SetActive(true);
    }

    public void CloseCreditWindow()
    {
        creditWindow.SetActive(false);
    }

    // Button to exit
    public void QuitGame()
    {
        Application.Quit();
    }
}