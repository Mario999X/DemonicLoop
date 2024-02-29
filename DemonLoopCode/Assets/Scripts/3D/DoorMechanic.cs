using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMechanic : MonoBehaviour
{
    [SerializeField] bool goToShop = false;
    [SerializeField] bool killAll = false;

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
                    SceneManager.Instance.LoadSceneName("Shop");
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
