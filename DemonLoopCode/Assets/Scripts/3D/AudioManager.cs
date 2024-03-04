using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Fuentes de sonido")]
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;

    [Header("Musico o efectos de sonido")]
    [SerializeField] private AudioClip sfxClip;
    [SerializeField] private AudioClip defaultMusic;
    [SerializeField] private AudioClip combatMusic;

    private AudioClip almacenTempMusic;//Almacenamos la musica de forma temporal

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        // Crea una instancia.
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        // Reproduce la musica de serie.
        musicAudioSource.clip = defaultMusic;
        musicAudioSource.Play();
    }

    // Reproduce la musica de combate.
    public void PlaySoundCombat()
    {
        almacenTempMusic = musicAudioSource.clip;

        musicAudioSource.clip = combatMusic; 
        musicAudioSource.Play();
    }

    // Para la musica de combate y reproduce la que estuviera sonando anteriormente.
    public void StopSoundCombat()
    {
        musicAudioSource.Stop();
        musicAudioSource.clip = almacenTempMusic;
        musicAudioSource.Play();
    }

    // Reproduce el efecto de sonido al pulsar un boton.
    public void PlaySoundButtons()
    {
        sfxAudioSource.PlayOneShot(sfxClip);
    }
}
