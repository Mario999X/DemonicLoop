using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR;
using System.Linq;
using System.Text.RegularExpressions;

public class Control_Election : MonoBehaviour
{
    [SerializeField] bool mando = false;
    [SerializeField] bool pc = false;

    [SerializeField] TextMeshProUGUI log;

    // Start is called before the first frame update
    void Start()
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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (pc && GameObject.Find("Player")) // Si esta corriendo en un ordenador y encuentra el objeto jugador.
        {
            GameObject player = GameObject.Find("Player");

            // Si el jugador no tiene el componente "KeyBoard_Controls" se lo añade.
            if (!player.GetComponent<KeyBoard_Controls>())
                player.AddComponent<KeyBoard_Controls>();

            List<string> controllers = Input.GetJoystickNames().ToList(); // Lista de mandos conectados.

            controllers.RemoveAll(controller => !Regex.IsMatch(controller, @"^[a-zA-Z]") || controller == null); // Elimina las variables vacías generadas por un error de unity.

            //Debug.Log(controllers.Count);

            // Si hay un mando conectado le añade al jugador el componente "Controller_Controls", si no se había añadido con anterioridad.
            if (!mando && controllers.Count > 0)
            {
                mando = true;
                Debug.Log("Connected");

                // En caso de desconectar el mando destruye el componente "Controller_Controls".
                if (!player.GetComponent<Controller_Controls>())
                    player.AddComponent<Controller_Controls>();
            }
            else if (mando && controllers.Count == 0)
            {
                mando = false;
                Debug.Log("Disconnected");

                Destroy(player.GetComponent<Controller_Controls>());
            }
        }
        // En el caso de no iniciarse en un ordenador y encontrar el objeto jugador, se le añade el componente "Controller_Controls".
        else if (!pc && GameObject.Find("Player")) 
        {
            GameObject player = GameObject.Find("Player");

            // En el caso de que jugador no contenga el componente "Controller_Controls" se le añade.
            if (!player.GetComponent<Controller_Controls>())
                player.AddComponent<Controller_Controls>();
        }
    }
}
