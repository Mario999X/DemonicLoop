using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoManagerScript : LoserReset
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] VideoClip clip;

    void Start()
    {
        GameObject.Find("FightButton").GetComponent<Button>().onClick.AddListener(() => ChangeToTitleScene());

        if (GameObject.Find("System"))
        {
            videoPlayer.clip = clip;
            StartCoroutine(ShowImage());
        }
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
