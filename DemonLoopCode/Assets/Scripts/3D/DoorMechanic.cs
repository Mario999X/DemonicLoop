using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMechanic : MonoBehaviour
{
    bool done = false;

    private void OnTriggerEnter(Collider other)
    {
        if (GameObject.FindGameObjectsWithTag("Enemy3D").Length <= 0)
        {
            if (other.name == "Player" && !done)
            {
                SceneManager.Instance.LoadScene(1);
                done = true;
            }
        }
    }
}
