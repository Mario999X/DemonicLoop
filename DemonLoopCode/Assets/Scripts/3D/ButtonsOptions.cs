using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsOptions : MonoBehaviour
{
    // Carga de escena por su posicion en el array.
    public void loadScene(int scene)
    {
        SceneManager.Instance.LoadScene(scene);
    }

    // Carga de escena por su nombre.
    public void loadSceneByName(string scene)
    {
        SceneManager.Instance.LoadSceneName(scene);
    }

    // Cierra la aplicación.
    public void exitGame()
    {
        Application.Quit();
    }
}
