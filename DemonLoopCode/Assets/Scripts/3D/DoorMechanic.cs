using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMechanic : MonoBehaviour
{
    [SerializeField] bool goToShop = false;
    [SerializeField] bool killAll = false;
    [SerializeField] bool theEnd = false;

    bool done = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player" && !done)
        {
            if (!goToShop && !killAll)
            {
                SceneManager.Instance.LoadSceneName("Scene 2");
                done = true;
            }
            else
            {
                if (!GameObject.Find("Boss") && goToShop)
                {
                    if (!theEnd) SceneManager.Instance.LoadSceneName("Shop");
                    else SceneManager.Instance.LoadSceneName("VideoScene");
                    done = true;
                }
                else if (GameObject.FindGameObjectsWithTag("Enemy3D").Length <= 0 && killAll)
                {
                    SceneManager.Instance.LoadSceneName("Scene 2");
                    done = true;
                }
            }
        }
    }
}
