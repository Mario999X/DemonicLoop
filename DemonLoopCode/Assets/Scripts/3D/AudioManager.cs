using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioClip sfxClip;
    [SerializeField] private AudioClip defaultMusic;
    [SerializeField] private AudioClip combatMusic;

    private AudioClip almacenTempMusic;//Almacenamos la musica de forma temporal
    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        PlayMusicDefault();
    }

    public void PlayMusicDefault() 
    { 
        musicAudioSource.clip = defaultMusic;
        musicAudioSource.Play();
    }


    public void PlaySoundCombat()
    {
        almacenTempMusic=musicAudioSource.clip;

        musicAudioSource.clip = combatMusic; 
        musicAudioSource.Play();
    }

    public void StopSoundCombat()
    {
        musicAudioSource.Stop();
        musicAudioSource.clip = almacenTempMusic;
        musicAudioSource.Play();
    }

    public void PlaySoundButtons()
    {
        sfxAudioSource.PlayOneShot(sfxClip);
    }
}
