using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Clase encargada de la vista de ajustes.
public class SettingsMenuScript : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private TMP_Dropdown resolutionsDropdown;

    private Resolution[] resolutions;

    List<string> resolutionsName = new();

    private Scene scene;

    private bool done = false;

    private Button returnToTitleBtn;

    private void Start()
    {
        SetResolutionsInArrayAndDropdown();

        SetDefaultValueFullScreen();

        SetDefaultValuesOnVolumeSliders();

        returnToTitleBtn = GameObject.Find("ReturnToTitleBtn").GetComponent<Button>();

        returnToTitleBtn.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            SceneManager.Instance.LoadSceneName("Title");
            gameObject.GetComponent<Canvas>().enabled = false;
        });
    }

    private void Update()
    {
        if (scene != UnityEngine.SceneManagement.SceneManager.GetActiveScene())
        {
            done = false;
            scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        }

        if (!done && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Title")
        {
            returnToTitleBtn.gameObject.SetActive(false); // Si estamos en el titulo, este boton no se muestra.

            done = true;
        }
        
        if (!done && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Title")
        {
            returnToTitleBtn.gameObject.SetActive(true);

            done = true;
        }
    }

    // Recogemos las resoluciones posibles del equipo y las mostramos en el dropdown de resoluciones.
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

    // Funcion para manejar el volumen general del juego.
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MainVolume", volume);

        PlayerPrefs.SetFloat("MainVolume", volume);
    }

    // Funcion para manejar el volumen de los efectos sonoros del juego.
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);

        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    // Funcion para manejar el volumen de la musica del juego.
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);

        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    // Funcion para poner los valores almacenados en PlayerPreferences.
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

    // Funcion para la pantalla completa.
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    // Funcion para poner el valor por defecto en el Toogle.
    private void SetDefaultValueFullScreen()
    {
        if(Screen.fullScreen) GameObject.Find("FullScreenToogle").GetComponent<Toggle>().isOn = true;
        else GameObject.Find("FullScreenToogle").GetComponent<Toggle>().isOn = false;
    }

    // Funcion para cambiar la resolucion del juego.
    public void SetResolution(int resolutionIndex)
    {
        var selectedResolution = resolutions[resolutionIndex];

        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
    }

    // Funcion para esconder la vista de ajustes.
    public void HideSettingsView()
    {
        GetComponent<Canvas>().enabled = false;
        Time.timeScale = 1f;
    }
}
