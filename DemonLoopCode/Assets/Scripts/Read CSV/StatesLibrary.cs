using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
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
    Dictionary<string, StatesData> states = new Dictionary<string, StatesData>();

    string filePath = Path.Combine(Application.dataPath, "Data", "States.csv");

    EnterBattle enterBattle;

    // Start is called before the first frame update
    void Start()
    {
        enterBattle = GetComponent<EnterBattle>();

        if (File.Exists(filePath))
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(',');

                    states.Add(values[0].ToUpper(), new StatesData(float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3])));
                }
            }
        }
        else
            Debug.LogError("File not found");

        //foreach (KeyValuePair<string, StatesData> item in states)
        //{
        //    Debug.Log("Name: " + item.Key + ", time: " + item.Value.TimeDuration + ", turns: " + item.Value.TurnsDuration);
        //}
    }

    public IEnumerator StateEffectGroup(PlayerMove player, string group, string state)
    {
        yield return null;

        if (states.ContainsKey(state.ToUpper()))
        {
            Stats[] stats = GameObject.Find(group).GetComponentsInChildren<Stats>();
            StatesData data = states[state.ToUpper()];
            float time = 0;
            int turns = 0;

            do
            {
                Debug.Log(player.Movement);

                if (player.Movement && !enterBattle.OneTime)
                {
                    time += Time.deltaTime;
                }

                if (time >= data.TimeDuration)
                {
                    foreach (Stats character in stats)
                    {
                        if (character.Health > 1)
                            character.Health -= (data.BaseDamage);
                    }

                    turns++;
                    time = 0;
                }

                yield return new WaitForSeconds(0.000000001f);
            } while (!enterBattle.OneTime && turns != data.TurnsDuration);
        }
    }
}
