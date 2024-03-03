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

    // Devuelve el nombre del estado que aplica.
    private string ObtainStateName()
    {
        return state.name.Substring(4, state.name.Length - 4).ToUpper(); 
    }

    // Si el jugador toca la trampa inicia el dano, estados que aplica y efectos visuales.
    private void OnTriggerStay(Collider other)
    {
        // No se recibe dano mientras esta en combate.
        if (other.gameObject.layer == 3 && !wait && !GameObject.Find("System").GetComponent<EnterBattle>().OneTime)
            StartCoroutine(TrupAction()); // Inicia la trampa y espera a que se vuelva a activar.
    }

    // El dano y estados que aplica que aplica la trampa, y efectos visuales.
    IEnumerator TrupAction()
    {
        wait = true;

        visualEffect.Auch(); // Animacion de palpitacion.

        LibraryStates states = GameObject.Find("System").GetComponent<LibraryStates>();

        StatsPersistenceData[] aliados = GameObject.Find("System").GetComponent<Data>().CharactersTeamStats.ToArray();

        // Por cada aliado se le hace un dano establecido.
        foreach (StatsPersistenceData stats in aliados)
            stats.Health -= damage;

        // En el caso de que la trampa aplique un estado.
        if (state != null)
            // Aplica el estado a todo el grupo.
            foreach (StatsPersistenceData character in GameObject.Find("System").GetComponent<Data>().CharactersTeamStats)
                character.ActualStates.Add(new ActualStateData(ObtainStateName()));

        yield return new WaitForSeconds(timeSleep);

        wait = false;
    }
}
