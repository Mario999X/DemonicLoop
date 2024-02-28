using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenuScript : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private TMP_Dropdown resolutionsDropdown;

    private Resolution[] resolutions;

    List<string> resolutionsName = new();

    private void Start()
    {
        SetResolutionsInArrayAndDropdown();

        SetDefaultValueFullScreen();

        SetDefaultValuesOnVolumeSliders();
    }

    private void SetResolutionsInArrayAndDropdown()
    {
        resolutions = Screen.resolutions;

        resolutionsDropdown.ClearOptions();

        var currentResolutionIndex = 0;

        for(int i = 0; i < resolutions.Length; i++)
        {
            var resolutionName = resolutions[i].width + " x " + resolutions[i].height;
            
            resolutionsName.Add(resolutionName);

            if(resolutions[i].width == Screen.currentResolution.width && 
            resolutions[i].height == Screen.currentResolution.height) currentResolutionIndex = i;
        }

        resolutionsDropdown.AddOptions(resolutionsName);
        resolutionsDropdown.value = currentResolutionIndex;

        resolutionsDropdown.RefreshShownValue();
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MainVolume", volume);

        PlayerPrefs.SetFloat("MainVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);

        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);

        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    private void SetDefaultValuesOnVolumeSliders()
    {
        var mainVolume = PlayerPrefs.GetFloat("MainVolume");
        var sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
        var musicVolume = PlayerPrefs.GetFloat("MusicVolume");

        SetVolume(mainVolume);

        GameObject.Find("MainVolumeSlider").GetComponent<Slider>().value = mainVolume;

        SetSFXVolume(sfxVolume);

        GameObject.Find("SFXVolumeSlider").GetComponent<Slider>().value = sfxVolume;

        SetMusicVolume(musicVolume);

        GameObject.Find("MusicVolumeSlider").GetComponent<Slider>().value = musicVolume;
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    private void SetDefaultValueFullScreen()
    {
        if(Screen.fullScreen) GameObject.Find("FullScreenToogle").GetComponent<Toggle>().isOn = true;
        else GameObject.Find("FullScreenToogle").GetComponent<Toggle>().isOn = false;
    }

    public void SetResolution(int resolutionIndex)
    {
        var selectedResolution = resolutions[resolutionIndex];

        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
    }

    public void HideSettingsView()
    {
        GetComponent<Canvas>().enabled = false;
        Time.timeScale = 1f;
    }
}
