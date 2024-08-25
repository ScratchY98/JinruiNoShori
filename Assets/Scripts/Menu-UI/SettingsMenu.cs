using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Linq;

public class SettingsMenu : MonoBehaviour
{
    [Header ("Audio")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider soundSlider;

    [Header ("Resolutions")]
    [SerializeField] private Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    [Header ("Toggle")]
    [SerializeField] private Toggle fullScreenToggle;
    [SerializeField] private Toggle titanSmokeToggle;

    [Header("Other")]
    [SerializeField] private bool isMainScene;

    public void Start()
    {
        // Load All Data.
        LoadData();

        // Set soundSlider value.
        audioMixer.GetFloat("Sound", out float soundValueForSlider);
        soundSlider.value = soundValueForSlider;

        // Set ResolutionDropdown options.
        SetResolutionDropdownOptions();

        Screen.fullScreen = true;
    }

    // Load all Data
    private void LoadData()
    {
        LoadResolutionData();
        LoadFullScreenData();
        LoadSoundData();

        if (isMainScene)
            LoadTitanSmokeToogleData();
    }

    // Set ResolutionDropdown options.
    private void SetResolutionDropdownOptions()
    {
        resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    // Set Resolution.
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        int width = resolution.width;
        int height = resolution.height;

        PlayerPrefs.SetInt("ResolutionW", width);
        PlayerPrefs.SetInt("ResolutionH", height);
    }

    // Load Resolution Data
    private void LoadResolutionData()
    {
        int width = PlayerPrefs.GetInt("ResolutionW", 1920);
        int height = PlayerPrefs.GetInt("ResolutionH", 1080);

        Screen.SetResolution(width, height, Screen.fullScreen);
    }

    // Set Fullscreen
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;

        int isFullScreenInt = isFullScreen ? 1 : 0;

        PlayerPrefs.SetInt("isFullScreen", isFullScreenInt);
    }

    // Load Fullscreen Data
    private void LoadFullScreenData()
    {
        int isFullScreenInt = PlayerPrefs.GetInt("isFullScreen", 1);

        bool isFullScreen = isFullScreenInt == 1 ? true : false;

        fullScreenToggle.isOn = isFullScreen;
        Screen.fullScreen = isFullScreen;
    }

    // Set Sound Volume
    public void SetSoundVolume(float volume)
    {
        audioMixer.SetFloat("Sound", volume);

        PlayerPrefs.SetFloat("Sound", volume);
    }

    // Load Sound Volume Data
    private void LoadSoundData()
    {
       
        soundSlider.value = PlayerPrefs.GetFloat("Sound", 0);
    }

    // Set Titan Smoke
    public void SetTitanSmoke(bool isTitanSmoke)
    {
        // Set if we can to use the smoke of titan. We use a int for save a bool in PlayerPref. If isTitanSmokeInt = 1, we can, else we can't.
        int isTitanSmokeInt = isTitanSmoke ? 1 : 0;

        PlayerPrefs.SetInt("isTitanSmoke", isTitanSmokeInt);
    }

    // Load Titan Smoke Data.
    private void LoadTitanSmokeToogleData()
    {
        titanSmokeToggle.isOn = PlayerPrefs.GetInt("isTitanSmoke", 1) == 1 ? true : false;
    }
}