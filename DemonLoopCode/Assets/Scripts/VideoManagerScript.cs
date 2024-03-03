using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

// Clase usada para manejar la escena de videos.
public class VideoManagerScript : LoserReset
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] VideoClip clip;

    void Start()
    {
        // Boton para saltarse la cinematica y cambiar a la escena de titulo
        GameObject.Find("FightButton").GetComponent<Button>().onClick.AddListener(() => ChangeToTitleScene());

        // Cuando se finaliza el juego, el objeto system se encuentra en la jerarquia, asi podemos cambiar el clip y re utilizar la escena. Ademas, desactivamos el componente de musica.
        if (GameObject.Find("System"))
        {
            videoPlayer.clip = clip;
            GameObject.Find("Music").GetComponent<AudioSource>().enabled = false;
            StartCoroutine(ShowImage());
        }
    }

    void Update()
    {
        // Cuando el video termina salta automaticamente a la escena de titulo.
        if(videoPlayer.time >= videoPlayer.clip.length)
        {
            ChangeToTitleScene();
        }
    }
    
    // Funcion para cambiar a la escena de titulo. Si encuentra el objeto de System, vuelve a activar el componente de sonido.
    private void ChangeToTitleScene()
    {
        if(GameObject.Find("System")) GameObject.Find("Music").GetComponent<AudioSource>().enabled = true;
        
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }
}
