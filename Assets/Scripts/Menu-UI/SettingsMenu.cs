using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Rendering;

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
        LoadResolutionData();
        LoadFullScreenData();
        LoadSoundData();

        if(isMainScene)
            LoadTitanSmokeToogleData();

        audioMixer.GetFloat("Sound", out float soundValueForSlider);
        soundSlider.value = soundValueForSlider;

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

        Screen.fullScreen = true;
    }

    public void SetSoundVolume(float volume)
    {
        audioMixer.SetFloat("Sound", volume);

        PlayerPrefs.SetFloat("Sound", volume);
    }

    private void LoadSoundData()
    {
       
        soundSlider.value = PlayerPrefs.GetFloat("Sound", 0);
    }

    public void SetTitanSmoke(bool isTitanSmoke)
    {
        // Set if we can to use the smoke of titan. We use a int for save a bool in PlayerPref. If isTitanSmokeInt = 1, we can, else we can't.
        int isTitanSmokeInt = isTitanSmoke ? 1 : 0;

        PlayerPrefs.SetInt("isTitanSmoke", isTitanSmokeInt);
    }

    private void LoadTitanSmokeToogleData()
    {
        titanSmokeToggle.isOn = PlayerPrefs.GetInt("isTitanSmoke", 1) == 1 ? true : false;
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;

        int isFullScreenInt = isFullScreen ? 1 : 0;

        PlayerPrefs.SetInt("isFullScreen", isFullScreenInt);
    }

    private void LoadFullScreenData()
    {
        int isFullScreenInt = PlayerPrefs.GetInt("isFullScreen", 1);

        bool isFullScreen = isFullScreenInt == 1 ? true : false;

        fullScreenToggle.isOn = isFullScreen;
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        int width = resolution.width;
        int height = resolution.height;

        PlayerPrefs.SetInt("ResolutionW", width);
        PlayerPrefs.SetInt("ResolutionH", height);
    }

    private void LoadResolutionData()
    {
        int width = PlayerPrefs.GetInt("ResolutionW", 1920);
        int height = PlayerPrefs.GetInt("ResolutionH", 1080);

        Screen.SetResolution(width, height, Screen.fullScreen);
    }
}