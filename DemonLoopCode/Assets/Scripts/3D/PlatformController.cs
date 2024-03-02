using System.Collections;
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
            StartCoroutine(saving());
            done = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3)
            done = false;
    }

    IEnumerator saving()
    {
        Canvas canvas = GameObject.Find("Guardando").GetComponent<Canvas>();
        canvas.enabled = true;

        yield return new WaitForSeconds(1f);

        canvas.enabled = false;
    }
}
