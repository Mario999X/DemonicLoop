using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.VersionControl.Asset;

public class Content
{
    float money;
    int count;
    ScriptableObject data;

    public float Money { get { return money; } }
    public int Count { get { return count; } }
    public ScriptableObject Data { get { return data; } }

    public Content(float money, int count, ScriptableObject data) 
    {
        this.money = money;
        this.count = count;
        this.data = data;
    }
}
public class ChestContent : MonoBehaviour
{
    [Header("Rango de dinero dado")]
    [SerializeField] private float maxMoney;
    [SerializeField] private float minMoney;

    [Header("Rango de cantidad de un objeto dado")]
    [SerializeField] private int maxCount;
    [SerializeField] private int minCount;

    public Content chest()
    {
        Content content = null;
        List<ScriptableObject> objects = new List<ScriptableObject>();

        string[] pru = AssetDatabase.FindAssets("STD_");

        foreach (string p in pru)
        {
            string path = AssetDatabase.GUIDToAssetPath(p);
            ScriptableObject @object = AssetDatabase.LoadAssetAtPath<StateData>(path);

            objects.Add(@object);
        }

        int obj = Random.Range(0, objects.Count);
        int count = Random.Range(minCount, maxCount + 1);
        float money = Random.Range(minMoney, maxMoney + 1);

        content = new Content(money, count, objects[obj]);

        return content;
    }
}
