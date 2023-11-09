using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ActualStateData
{
    string state;
    int turn = 0;

    public string State {  get { return state; } }
    public int Turn { get { return turn; } set { this.turn = value; } }

    public ActualStateData(string state)
    {
        this.state = state;
    }
}
public class StatesLibrary : MonoBehaviour
{
    List<ActualStateData> state = new List<ActualStateData>();

    Dictionary<string, StateData> states = new Dictionary<string, StateData>();

    public List<ActualStateData> State { get { return state;} }

    PlayerMove player;
    EnterBattle enterBattle;

    // Start is called before the first frame update
    void Start()
    {
        string[] pru = AssetDatabase.FindAssets("STD_");

        foreach (string p in pru)
        {
            string path = AssetDatabase.GUIDToAssetPath(p);
            ScriptableObject @object = AssetDatabase.LoadAssetAtPath<StateData>(path);

            states.Add(@object.name.Substring(4, @object.name.Length - 4).ToUpper(), @object as StateData);
        }

        enterBattle = GetComponent<EnterBattle>();
        player = GameObject.Find("Player").GetComponent<PlayerMove>();
    }

    public IEnumerator StateEffectGroup(string group, string state)
    {
        if (states.ContainsKey(state.ToUpper()))
        {
            ActualStateData actualState = new ActualStateData(state.ToUpper());
            this.state.Add(actualState);

            Stats[] stats = GameObject.Find(group).GetComponentsInChildren<Stats>();
            StateData data = states[state.ToUpper()];
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

                    if (time >= data.TimeMoving)
                    {
                        foreach (Stats character in stats)
                        {
                            if (character.Health > 1)
                                character.Health -= (data.BaseDamage);
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
                            if (character.Health > 1)
                                character.Health -= (data.BaseDamage);
                        }

                        lastTurn = actualState.Turn;
                    }
                }

                yield return new WaitForSeconds(0.000000001f);
            } while (actualState.Turn != data.TurnsDuration);
        }
    }
}
