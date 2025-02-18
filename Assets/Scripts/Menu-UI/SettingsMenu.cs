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
    [SerializeField] private Toggle cloudToggle;
    [SerializeField] private Toggle viewFPSToggle;

    [Header("Slider")]
    [SerializeField] private Slider gamepadSensibility;
    [SerializeField] private Slider mouseSensibility;

    [Header("Other")]
    [SerializeField] private bool isMainScene;
    [SerializeField] private GameObject viewFPSText;
    [SerializeField] private ThirdPersonCameraController thirdPersonCameraController;

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


        LoadGamepadSensibilityData();
        LoadMouseSensibilityData();
        LoadViewFPSToogleData();

        if (!isMainScene)
            return;

        LoadCloudToogleData();
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
                currentResolutionIndex = i;
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
        soundSlider.value = PlayerPrefs.GetFloat("Sound", -20);
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

    public void SetGamepadSensibility(float x)
    {
        PlayerPrefs.SetFloat("GamepadSensibilityData", x);

        if (!isMainScene)
            thirdPersonCameraController.ChangeSensitivity(x, true);
    }

    public void SetMouseSensibility(float x)
    {
        PlayerPrefs.SetFloat("MouseSensibilityData", x);

        if (!isMainScene)
            thirdPersonCameraController.ChangeSensitivity(x, false);
    }

    // Load Gamepad's Sensibility Data.
    private void LoadGamepadSensibilityData()
    {
        gamepadSensibility.value = PlayerPrefs.GetFloat("GamepadSensibilityData", 750f);
    }

    // Load Mouse's Data.
    private void LoadMouseSensibilityData()
    {
        mouseSensibility.value = PlayerPrefs.GetFloat("MouseSensibilityData", 50f);
    }

    // Set Cloud
    public void SetCloud(bool isCloud)
    {
        // Set if we can to use the cloud. We use a int for save a bool in PlayerPref. If isCloudint = 1, we can, else we can't.
        int isCloudint = isCloud ? 1 : 0;

        PlayerPrefs.SetInt("isCloud", isCloudint);
    }

    // Load Cloud Data.
    private void LoadCloudToogleData()
    {
        cloudToggle.isOn = PlayerPrefs.GetInt("isCloud", 1) == 1 ? true : false;
    }

    public void SetViewFPS(bool isViewFPS)
    {
        // Set if we can to view the FPS. We use a int for save a bool in PlayerPref. If isViewFPSInt = 1, we can, else we can't.
        int isViewFPSInt = isViewFPS ? 1 : 0;

        PlayerPrefs.SetInt("isViewFPS", isViewFPSInt);
        viewFPSText.SetActive(isViewFPS);
    }

    // Load Cloud Data.
    private void LoadViewFPSToogleData()
    {
        viewFPSToggle.isOn = PlayerPrefs.GetInt("isViewFPS", 0) == 1 ? true : false;
    }

}