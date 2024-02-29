using UnityEngine;

public class PlatformController : SaveSystem
{
    SaveSystem controller;

    bool done = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3 && !done)
        {
            Debug.Log("El jugador entro en la plataforma.");
            SaveData(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, true);
            done = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3)
            done = false;
    }

}
