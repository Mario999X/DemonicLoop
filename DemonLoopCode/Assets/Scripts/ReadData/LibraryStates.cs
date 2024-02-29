using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.ParticleSystem;

public class ActualStateData
{
    private string state;
    private int turn = 0;

    public string State {  get { return state; } }
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

    private void LoadStates()
    {
        StateData[] stateDatas = Resources.LoadAll<StateData>("Data/States");

        foreach (StateData stateData in stateDatas)
        {
            states.Add(stateData.name.Substring(4, stateData.name.Length - 4).ToUpper(), stateData);
        }
    }

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
                    if (stateData.State.ToUpper() == state.ToUpper())
                    {
                        stateData.Turn = 0;
                        newState = true;
                    }
                }
            }

            if (!newState)
                targetStates.Add(new ActualStateData(state));

            if (enterBattle.OneTime)
                IconState(target);
            else
                damageVisualEffect.Auch();

            Stats targetStats = target.GetComponent<Stats>();
            StateData statedata = states[state.ToUpper()];

            if (targetStats.Health > (targetStats.Health - statedata.BaseDamage))
            {
                targetStats.Health -= statedata.BaseDamage;

                floatingText.ShowFloatingTextNumbers(target, -statedata.BaseDamage, Color.magenta);
            }
            else
            {
                float damageDone = (1 - targetStats.Health);

                targetStats.Health = 1;

                floatingText.ShowFloatingTextNumbers(target, damageDone, Color.magenta);
            }
        }
    }

    public void CheckStates(GameObject[] characters)
    {
        foreach(GameObject character in characters)
        {
            Stats stats = character.GetComponent<Stats>();
            List<ActualStateData> statesFinished = new List<ActualStateData>();

            foreach (ActualStateData stateData in stats.ActualStates)
            {
                StateData statedata = states[stateData.State.ToUpper()];

                stats.Health -= statedata.BaseDamage;

                floatingText.ShowFloatingTextNumbers(character, -statedata.BaseDamage, Color.magenta);

                stateData.Turn++;

                if (statedata.TurnsDuration <= stateData.Turn)
                    statesFinished.Add(stateData);
            }

            foreach(ActualStateData stateData1 in statesFinished)
            {
                Destroy(character.transform.Find(stateData1.State.ToUpper()).gameObject);
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
                //Debug.Log("BURNT " + actualState.State.ToUpper());

                GameObject icon = Instantiate(Burnt, character.transform.position, Quaternion.identity);
                icon.name = "BURNT";
                icon.transform.SetParent(character.transform);

                icon.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else
            {
                //Debug.Log("POISON " + actualState.State.ToUpper());
                
                GameObject icon = Instantiate(Poison, character.transform.position, Quaternion.identity);
                icon.name = "POISON";
                icon.transform.SetParent(character.transform);
                    
                icon.transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
    }

    public void RemoveCharacterWithState(GameObject character, string state)
    {
        Debug.Log(state.ToUpper());

        if(states.ContainsKey(state.ToUpper()))
        {
            ActualStateData removeState = null;

            foreach(ActualStateData actualState in character.GetComponent<Stats>().ActualStates)
            {
                if(actualState.State.ToUpper().Equals(state.ToUpper()))
                {
                    Debug.Log("Character found with that State, removing...");
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
