using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string levelToLoad;
    [SerializeField] private GameObject settingsWindow;
    [SerializeField] private GameObject inputsSettingsWindow;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void StartGame()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public void SettingsButton()
    {
        settingsWindow.SetActive(true);
    }

    public void CloseSettingsWindow()
    {
        settingsWindow.SetActive(false);
    }

    public void InputsSettingsButton()
    {
        inputsSettingsWindow.SetActive(true);
    }

    public void CloseInputsSettingsWindow()
    {
        inputsSettingsWindow.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}