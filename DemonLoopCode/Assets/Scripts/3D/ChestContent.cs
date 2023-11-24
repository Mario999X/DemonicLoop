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
        List<ScriptableObject> objects = new List<ScriptableObject>(); // Lista  de objetos existentes.

        string[] pru = AssetDatabase.FindAssets("OBJ_"); // Busca todos loas assets que empiezan con "OBJ_".

        // Guarda todos los ScripableObject en la lista los objetos.
        foreach (string p in pru)
        {
            string path = AssetDatabase.GUIDToAssetPath(p);
            ScriptableObject @object = AssetDatabase.LoadAssetAtPath<ObjectData>(path);

            objects.Add(@object);
        }

        int obj = Random.Range(0, objects.Count); // El objeto que se selecciona de la lista.
        int count = Random.Range(minCount, maxCount + 1); // La cantidad que se le da de dicho objeto.
        float money = (float) System.Math.Round(Random.Range(minMoney, maxMoney + 1), 2); // La cantidad de dinero que se dara.

        Content content = new Content(money, count, objects[obj]); // Se guarda en la clase Content

        return content; // Se devuelve la clase dicha.
    }
}
