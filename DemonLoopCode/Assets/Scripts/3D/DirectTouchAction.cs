using System.Collections;
using UnityEngine;

public class DirectTouchAction : MonoBehaviour
{
    [Header("Referenced state")]
    [SerializeField] private StateData state;
    [Header("Individual Damage to all team members")]
    [SerializeField] float damage;
    [SerializeField] int timeSleep = 10;
    private bool wait = false;

    private string ObtainStateName()
    {
        return state.name.Substring(4, state.name.Length - 4).ToUpper();
    }

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

        LibraryStates states = GameObject.Find("System").GetComponent<LibraryStates>();

        Stats[] aliados = GameObject.Find("AlliesBattleZone").GetComponentsInChildren<Stats>();

        // Por cada aliado se le hace un da�o establecido.
        foreach (Stats stats in aliados)
        {
            stats.Health -= damage;
        }

        // En el caso de que la trampa aplique un estado.
        if (state != null)
        {
            bool notFound = false;

            // Si hay un estado verifica si es el mismo que va aplicar.
            if (states.CharacterStates.Count > 0)
            {
                // En el caso de que exista resetea los turnos que lleva.
                foreach (ActualStateData actual in states.CharacterStates)
                {
                    if (actual.State.Equals(state))
                    {
                        notFound = true;
                        actual.Turn = 0;
                    }
                }
            }

            // En caso de no haber se le inicia un nuevo estado.
            if (!notFound)
                StartCoroutine(states.StateEffectGroup("AlliesBattleZone", ObtainStateName()));
        }

        yield return new WaitForSeconds(timeSleep);

        wait = false;
    }
}
