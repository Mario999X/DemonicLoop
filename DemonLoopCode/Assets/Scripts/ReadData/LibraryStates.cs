using System.Collections.Generic;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ActualStateData
{
    public string state;
    public int turn = 0;
    public string State { get { return state; } }
    public int Turn { get { return turn; } set { turn = value; Debug.Log(turn); } }
    public ActualStateData(string state)
    {
        this.state = state;
    }
}

public class LibraryStates : MonoBehaviour
{
    [SerializeField] GameObject Burnt;
    [SerializeField] GameObject Poison;
    private Dictionary<string, StateData> states = new();

    public StateData State(string value) { return states[value.ToUpper()]; }

    private FloatingTextCombat floatingText;

    private EnterBattle enterBattle;
    DamageVisualEffect damageVisualEffect;

    bool done = false;

    Scene scene;

    void Start()
    {
        enterBattle = GetComponent<EnterBattle>();
        floatingText = GetComponent<FloatingTextCombat>();
        LoadStates();
    }

    private void FixedUpdate()
    {
        if (scene != UnityEngine.SceneManagement.SceneManager.GetActiveScene())
        {
            done = false;
            scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        }

        if (!done && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "SceneName 2")
        {
            damageVisualEffect = GameObject.Find("Global Volume").GetComponent<DamageVisualEffect>();

            done = true;
        }
    }

    // Cargamos los States
    private void LoadStates()
    {
        StateData[] stateDatas = Resources.LoadAll<StateData>("Data/States");

        // Limpiamos el nombre de esta forma STD_Burnt a Burnt
        foreach (StateData stateData in stateDatas)
        {
            states.Add(stateData.name.Substring(4, stateData.name.Length - 4).ToUpper(), stateData);
        }
    }

    // Añadimos los estados a los al personaje que se lo aplique 
    // ya sea alguno de los aliados o enemigos
    public void AddState(GameObject target, string state)
    {
        if (states.ContainsKey(state.ToUpper()))
        {
            List<ActualStateData> targetStates = target.GetComponent<Stats>().ActualStates;

            bool newState = false;

            if (targetStates.Count > 0)
            {
                foreach (ActualStateData stateData in targetStates)
                {
                    // Si ya tiene aplicado un estado actualizara el contador de sus turnos
                    if (stateData.State.ToUpper() == state.ToUpper())
                    {
                        stateData.Turn = 0;
                        newState = true;
                    }
                }
            }

            // Si el estado es nuevo, se agrega a la lista de los estados actuales
            if (!newState)
                targetStates.Add(new ActualStateData(state));

            // Mostraremos los efectos visuales
            if (enterBattle.OneTime)
                IconState(target);
            else
                damageVisualEffect.Auch();

            Stats targetStats = target.GetComponent<Stats>();
            StateData statedata = states[state.ToUpper()];

            // Comprobamos la vida y calcula el daño que le aplicara
            if (targetStats.Health > (targetStats.Health - statedata.BaseDamage))
            {
                targetStats.Health -= statedata.BaseDamage;
                floatingText.ShowFloatingTextNumbers(target, -statedata.BaseDamage, Color.magenta);
            }
            else
            {
                // Calcula el daño y si se excede la salud del personaje se estable a 1
                float damageDone = (1 - targetStats.Health);
                targetStats.Health = 1;
                floatingText.ShowFloatingTextNumbers(target, damageDone, Color.magenta);
            }
        }
    }

    // Verificamos los estados en el que este los personajes
    public void CheckStates(GameObject[] characters)
    {
        foreach (GameObject character in characters)
        {
            Stats stats = character.GetComponent<Stats>();
            List<ActualStateData> statesFinished = new List<ActualStateData>();
            foreach (ActualStateData stateData in stats.ActualStates)
            {
                StateData statedata = states[stateData.State.ToUpper()];
                stats.Health -= statedata.BaseDamage;
                floatingText.ShowFloatingTextNumbers(character, -statedata.BaseDamage, Color.magenta);
                stateData.Turn++;

                // Comprobamos que si el estado ha alcanzado su duraccion maxima de turnos
                if (statedata.TurnsDuration <= stateData.Turn)
                    statesFinished.Add(stateData);
            }

            foreach (ActualStateData stateData1 in statesFinished)
            {
                // Destruimos el icono visual del efecto que tenga el personaje
                Destroy(character.transform.Find(stateData1.State.ToUpper()).gameObject);
                // Removemos el finalizado de la lista de estados actuales del personaje
                stats.ActualStates.Remove(stateData1);
            }
        }
    }

    // Esta funcion se encarga de buscar las entidades con un estado y les pone el icono de estado.
    public void IconState(GameObject character)
    {
        foreach (ActualStateData actualState in character.GetComponent<Stats>().ActualStates)
        {
            if (actualState.State.ToUpper() == "BURNT")
            {
                GameObject icon = Instantiate(Burnt, character.transform.position, Quaternion.identity);
                icon.name = "BURNT";
                icon.transform.SetParent(character.transform);
                icon.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else
            {
                GameObject icon = Instantiate(Poison, character.transform.position, Quaternion.identity);
                icon.name = "POISON";
                icon.transform.SetParent(character.transform);
                icon.transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
    }

    // Esta funcion se encarga de eliminar el estado que se encuentre el jugador
    public void RemoveCharacterWithState(GameObject character, string state)
    {
        if (states.ContainsKey(state.ToUpper()))
        {
            ActualStateData removeState = null;

            foreach (ActualStateData actualState in character.GetComponent<Stats>().ActualStates)
            {
                if (actualState.State.ToUpper().Equals(state.ToUpper()))
                {
                    removeState = actualState;
                }
            }

            if (removeState != null)
            {
                Destroy(character.transform.Find(state.ToUpper()).gameObject);
                character.GetComponent<Stats>().ActualStates.Remove(removeState);
            }
        }
    }
}
