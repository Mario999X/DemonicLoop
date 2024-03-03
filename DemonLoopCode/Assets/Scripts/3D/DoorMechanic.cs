using TMPro;
using UnityEngine;

public class DoorMechanic : MonoBehaviour
{
    [Header("Condiciones de la puerta")]
    [SerializeField] bool goToShop = false;
    [SerializeField] bool killAll = false;
    [SerializeField] bool theEnd = false;

    bool done = false;

    void Update()
    {
        // Si el jugador no se encuentra en la tienda le aparecera un objetivo a realizar.
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Shop")
        {
            if (killAll) // El objetivo de matar a todos los enemigos de una sala.
                GameObject.Find("Objective").GetComponent<TextMeshProUGUI>().text = $"Objective:\nEnemies remaining: {GameObject.FindGameObjectsWithTag("Enemy3D").Length}";
            else if (goToShop) // El objetivo de matar al boss de la sala.
                GameObject.Find("Objective").GetComponent<TextMeshProUGUI>().text = "Objective:\nKill the boss";
            else // El objetivo de buscar el portal.
                GameObject.Find("Objective").GetComponent<TextMeshProUGUI>().text = "Objective:\nFind the gate";
        }
    }

    // Son las condiciones que el jugador tiene que cumplir para ir a la siguiente sala.
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player" && !done)
        {
            // Si no tiene que matar ningun boss o acabar con todos los enemigos de una sala el jugador puede ir a la siguiente sin ningun problema.
            if (!goToShop && !killAll)
            {
                SceneManager.Instance.LoadSceneName("Scene 2");
                done = true;
            }
            else
            {
                // No puede ir a la siguiente sala sin haber matado al boss de la sala.
                if (!GameObject.Find("Boss") && goToShop)
                {
                    if (!theEnd) SceneManager.Instance.LoadSceneName("Shop");
                    else SceneManager.Instance.LoadSceneName("VideoScene");
                    done = true;
                }
                // No puede ir a la siguiente sala sin haber acabado con todos los enemigos de la sala.
                else if (GameObject.FindGameObjectsWithTag("Enemy3D").Length <= 0 && killAll)
                {
                    SceneManager.Instance.LoadSceneName("Scene 2");
                    done = true;
                }
            }
        }
    }
}
