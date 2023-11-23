using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ActualStateData
{
    private GameObject character = null;
    private string state;
    private int turn = 0;

    public string State {  get { return state; } }
    public int Turn { get { return turn; } set { this.turn = value; } }
    public GameObject Character {  get { return character; } }

    public ActualStateData(string state, GameObject character)
    {
        this.state = state;
        this.character = character;
    }

    public ActualStateData(string state)
    {
        this.state = state;
    }
}

public class LibraryStates : MonoBehaviour
{
    private Dictionary<string, StateData> states = new();

    private List<ActualStateData> characterStates = new();

    public List<ActualStateData> CharacterStates { get { return characterStates;} }


    PlayerMove player;
    EnterBattle enterBattle;

    // Start is called before the first frame update
    private void Start()
    {
        LoadStates();

        enterBattle = GetComponent<EnterBattle>();
        player = GameObject.Find("Player").GetComponent<PlayerMove>();
    }

    private void LoadStates()
    {
        string[] pru = AssetDatabase.FindAssets("STD_");

        foreach (string p in pru)
        {
            string path = AssetDatabase.GUIDToAssetPath(p);
            ScriptableObject @object = AssetDatabase.LoadAssetAtPath<StateData>(path);

            states.Add(@object.name.Substring(4, @object.name.Length - 4).ToUpper(), @object as StateData);
        }
    }

    public IEnumerator StateEffectIndividual(GameObject target, string state){

        if(states.ContainsKey(state.ToUpper()))
        {
            ActualStateData actualState = new(state.ToUpper(), target);
            characterStates.Add(actualState);

            var targetST = target.GetComponent<Stats>();

            var stateData = states[state.ToUpper()];

            float time = 0;
            int lastTurn = -1;

            do
            {
                if (!enterBattle.OneTime)
                {
                    if (player.Movement)
                    {
                        time += Time.deltaTime;
                    }

                    if (time >= stateData.TimeMoving)
                    {
                        if (targetST.Health > 1) targetST.Health -= stateData.BaseDamage;

                        actualState.Turn++;
                        time = 0;
                    }
                }

                else
                {

                    if (lastTurn != actualState.Turn)
                    {

                        if (targetST.Health > 1) targetST.Health -= stateData.BaseDamage;
                        
                        Debug.Log("2D ---- Character: " + target.name + " | State: " + state + " | Turno Actual: " + actualState.Turn);
                        
                        lastTurn = actualState.Turn;
                    }
                }

                yield return new WaitForSeconds(0.000000001f);
            } while (actualState.Turn <= stateData.TurnsDuration);

            characterStates.Remove(actualState);
        }
    }

    public IEnumerator StateEffectGroup(string group, string state)
    {
        if (states.ContainsKey(state.ToUpper()))
        {
            ActualStateData actualState = new(state.ToUpper());
            characterStates.Add(actualState);

            Stats[] stats = GameObject.Find(group).GetComponentsInChildren<Stats>();
            StateData data = states[state.ToUpper()];
            float time = 0;
            int lastTurn = -1;

            do
            {
                Debug.Log("Turno Actual en estados 3D: " + actualState.Turn);

                if (!enterBattle.OneTime)
                {
                    if (player.Movement)
                    {
                        time += Time.deltaTime;
                    }

                    if (time >= data.TimeMoving)
                    {
                        foreach (Stats character in stats)
                        {
                            if (character.Health > 1)
                                character.Health -= data.BaseDamage;
                        }

                        actualState.Turn++;
                        time = 0;
                    }
                }
                else
                {

                    if (lastTurn != actualState.Turn)
                    {
                        foreach (Stats character in stats)
                        {
                            Debug.Log("3D ---- Character: " + character.name + " | State: " + state + " | Turno Actual: " + actualState.Turn);

                            if (character.Health > 1) character.Health -= data.BaseDamage;
                        }

                        lastTurn = actualState.Turn;
                    }
                }

                yield return new WaitForSeconds(0.000000001f);
            } while (actualState.Turn <= data.TurnsDuration);

            characterStates.Remove(actualState);
        }
    }

    public void RemoveCharacterWithState(GameObject character, string state){
        if(states.ContainsKey(state.ToUpper()))
        {
            var stateCharacter = characterStates.Find(x => x.Character == character && x.State == state);

            if(stateCharacter != null)
            {
                Debug.Log("Character found with that State, removing...");
                stateCharacter.Turn = 100; // 

            } else Debug.Log("Character not found with that State");
        }
    }
    
}
