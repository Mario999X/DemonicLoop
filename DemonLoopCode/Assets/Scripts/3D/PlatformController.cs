using System.Collections;
using UnityEngine;

public class PlatformController : SaveSystem
{
    SaveSystem controller;

    bool done = false;

    // Cuando el jugador se ponga encima del pedestal de guardado este guardara la partida.
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

    // Para que el jugador pueda volver a guardar la partida este tendra que bajarse del pedestal de guardado.
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3)
            done = false;
    }

    // Muestra un texto al jugador para que sepa que la partida se ha guardado.
    IEnumerator saving()
    {
        Canvas canvas = GameObject.Find("Guardando").GetComponent<Canvas>();
        canvas.enabled = true;

        yield return new WaitForSeconds(1f);

        canvas.enabled = false;
    }
}
