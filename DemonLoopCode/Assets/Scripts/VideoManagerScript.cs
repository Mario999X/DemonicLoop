using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoManagerScript : MonoBehaviour
{
   [SerializeField] VideoPlayer videoPlayer;

    void Start()
    {
        GameObject.Find("FightButton").GetComponent<Button>().onClick.AddListener(() => ChangeToTitleScene());
    }

    void Update()
    {
        if(videoPlayer.time >= videoPlayer.clip.length)
        {
            ChangeToTitleScene();
        }
    }

    private void ChangeToTitleScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }
}
