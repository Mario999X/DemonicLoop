using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using UnityEngine;

public class DirectTuchAction : MonoBehaviour
{
    [SerializeField] string state; // Para aplicar ningun estado escribe null.
    [SerializeField] float damage;
    [SerializeField] int timeSleep = 10;
    bool wait = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 3 && !wait)
        {
            StartCoroutine(TrupAction()); // Inicia la trampa y espera aque se vuelva a activar.
        }
    }

    IEnumerator TrupAction()
    {
        wait = true;

        StatesLibrary states = GameObject.Find("System").GetComponent<StatesLibrary>();

        Stats[] aliados = GameObject.Find("Aliados").GetComponentsInChildren<Stats>();

        // Por cada aliado se le hace un daño establecido.
        foreach (Stats stats in aliados)
        {
            stats.Health -= damage;
        }

        // En el caso de que la trampa aplique un estado.
        if (state.ToUpper() != "NULL")
        {
            bool notFound = false;

            // Si hay un estado verifica si es el mismo que va aplicar.
            if (states.State.Count > 0)
            {
                // En el caso de que exista resetea los turnos que lleva.
                foreach (ActualStateData actual in states.State)
                {
                    if (actual.State.Equals(state.ToUpper()))
                    {
                        notFound = true;
                        actual.Turn = 0;
                    }
                }
            }

            // En caso de no haber se le inicia un nuevo estado.
            if (!notFound)
                StartCoroutine(states.StateEffectGroup("Aliados", state));
        }

        yield return new WaitForSeconds(timeSleep);

        wait = false;
    }
}
