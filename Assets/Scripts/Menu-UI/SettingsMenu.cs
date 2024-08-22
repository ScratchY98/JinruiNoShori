using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Linq;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioSource audioSource;

    public Dropdown resolutionDropdown;

    Resolution[] resolutions;

    public Slider soundSlider;

    public void Start()
    {

        LoadResolution();
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
    }

    public void SetFullScreen(bool isFullScreen)
    {
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

    private void LoadResolution()
    {
        int width = PlayerPrefs.GetInt("ResolutionW", 1920);
        int height = PlayerPrefs.GetInt("ResolutionH", 1080);

        Screen.SetResolution(width, height, Screen.fullScreen);
    }
}