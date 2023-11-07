using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class StatesData
{
    private float baseDamage;
    private float timeDuration;
    private float turnsDuration;

    public float BaseDamage { get { return baseDamage; } }
    public float TimeDuration { get { return timeDuration; } }
    public float TurnsDuration { get { return turnsDuration; } }

    public StatesData(float baseDamage, float timeDuration, float turnsDuration) 
    {
        this.baseDamage = baseDamage;
        this.timeDuration = timeDuration;
        this.turnsDuration = turnsDuration;
    }
}
public class StatesLibrary : MonoBehaviour
{
    int turns = 0;

    Dictionary<string, StateData> states = new Dictionary<string, StateData>();

    public int Turns { set { this.turns = turns++; } }

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
        yield return null;

        if (states.ContainsKey(state.ToUpper()))
        {
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

                        turns++;
                        time = 0;
                    }
                }
                else
                {
                    if (lastTurn != turns)
                    {
                        foreach (Stats character in stats)
                        {
                            if (character.Health > 1)
                                character.Health -= (data.BaseDamage);
                        }

                        lastTurn = turns;
                    }
                }

                yield return new WaitForSeconds(0.000000001f);
            } while (turns != data.TurnsDuration);
        }
    }
}
