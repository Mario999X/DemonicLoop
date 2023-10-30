using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons_Options : MonoBehaviour
{
    // Carga de escena por su posicion en el array.
    public void loadScene(int scene)
    {
        Scene_Manager.Instance.LoadScene(scene);
    }

    // Carga de escena por su nombre.
    public void loadSceneByName(string scene)
    {
        Scene_Manager.Instance.LoadSceneName(scene);
    }

    // Cierra la aplicación.
    public void exitGame()
    {
        Application.Quit();
    }
}
