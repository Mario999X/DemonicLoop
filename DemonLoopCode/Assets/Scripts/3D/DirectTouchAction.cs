using System.Collections;
using System.Linq;
using UnityEngine;

public class DirectTouchAction : MonoBehaviour
{
    [Header("Referenced state")]
    [SerializeField] private StateData state;
    
    [Header("Individual Damage to all team members")]
    [SerializeField] float damage;
    [SerializeField] int timeSleep = 10;
    
    private bool wait = false;

    DamageVisualEffect visualEffect;

    void Start()
    {
        visualEffect = GameObject.Find("Global Volume").GetComponent<DamageVisualEffect>();
    }

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

        //visualEffect.Auch();

        LibraryStates states = GameObject.Find("System").GetComponent<LibraryStates>();

        StatsPersistenceData[] aliados = GameObject.Find("System").GetComponent<Data>().CharactersTeamStats.ToArray();

        // Por cada aliado se le hace un da�o establecido.
        foreach (StatsPersistenceData stats in aliados)
        {
            stats.Health -= damage;
        }

        // En el caso de que la trampa aplique un estado.
        if (state != null)
        {
            foreach (StatsPersistenceData character in GameObject.Find("System").GetComponent<Data>().CharactersTeamStats)
                character.ActualStates.Add(new ActualStateData(ObtainStateName()));
        }

        yield return new WaitForSeconds(timeSleep);

        wait = false;
    }
}
