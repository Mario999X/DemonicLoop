using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System.Text.RegularExpressions;

public class ControlElection : MonoBehaviour
{
    [SerializeField] bool controller = false;
    [SerializeField] bool pc = false;

    [SerializeField] TextMeshProUGUI log;

    bool done = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        // Identifica donde se esta corriendo el juego.
        if (!done)
        {
            #if UNITY_EDITOR // Si unity es iniciado en el editor.
                        if (log != null) log.text = "Running in editor";
                        pc = true;
            #elif UNITY_STANDALONE_OSX // Si unity es iniciado en el un MacOS.
                        if (log != null) log.text = "Running in MacOS";
                        pc = true;
            #elif UNITY_STANDALONE_WIN // Si unity es iniciado en Windows.
                        if (log != null) log.text = "Running in Windows";
                        pc = true;
            #elif UNITY_STANDALONE_LINUX // Si unity es iniciado en Linux.
                        if (log != null) log.text = "Running in Linux";
                        pc = true;
            #else // Esta opcion se puede ver como: en el caso de que unity no se inicie en un ordenador. 
                        if (log != null) log.text = "Running on console";
                        pc = flase;
            #endif

            done = true;
        }

        if (pc && GameObject.Find("Player")) // Si esta corriendo en un ordenador y encuentra el objeto jugador.
        {
            GameObject player = GameObject.Find("Player");

            // Si el jugador no tiene el componente "KeyBoard_Controls" se lo a�ade.
            if (!player.GetComponent<KeyBoardControls>())
                player.AddComponent<KeyBoardControls>();

            List<string> controllers = Input.GetJoystickNames().ToList(); // Lista de mandos conectados.

            controllers.RemoveAll(controller => !Regex.IsMatch(controller, @"^[a-zA-Z]") || controller == null); // Elimina las variables vacias generadas por un error de unity.

            // Si hay un controller conectado le a�ade al jugador el componente "Controller_Controls", si no se habia anadido con anterioridad.
            if (!controller && controllers.Count > 0)
            {
                controller = true;

                // En caso de desconectar el controller destruye el componente "Controller_Controls".
                if (!player.GetComponent<ControllerControls>())
                    player.AddComponent<ControllerControls>();
            }
            else if (controller && controllers.Count == 0)
            {
                controller = false;

                Destroy(player.GetComponent<ControllerControls>());
            }
        }
        // En el caso de no iniciarse en un ordenador y encontrar el objeto jugador, se le a�ade el componente "Controller_Controls".
        else if (!pc && GameObject.Find("Player")) 
        {
            GameObject player = GameObject.Find("Player");

            // En el caso de que jugador no contenga el componente "Controller_Controls" se le anade.
            if (!player.GetComponent<ControllerControls>())
                player.AddComponent<ControllerControls>();
        }
    }
}
