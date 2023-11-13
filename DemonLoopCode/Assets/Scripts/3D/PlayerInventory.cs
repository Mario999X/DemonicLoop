using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStock
{
    private ScriptableObject data;
    private int count = 1;

    public int Count { get { return count; } set { this.count = value; } }
    public ScriptableObject Data { get { return data; } }

    public ObjectStock(ScriptableObject scriptableObject) 
    {
        this.data = scriptableObject;
    }
}
public class PlayerInventory : MonoBehaviour
{
    [SerializeField] ScriptableObject m_ScriptableObject;
    
    Dictionary<string, ObjectStock> inventory = new Dictionary<string, ObjectStock>();

    // Start is called before the first frame update
    void Start()
    {
        AddObjectToInventory(m_ScriptableObject.name, new ObjectStock(m_ScriptableObject));

        foreach (var item in inventory)
        {
            Debug.Log(item.Key);
            Debug.Log(item.Value.Data);
            Debug.Log(item.Value.Count);
        }

        AddObjectToInventory(m_ScriptableObject.name, new ObjectStock(m_ScriptableObject));

        foreach (var item in inventory)
        {
            Debug.Log(item.Key);
            Debug.Log(item.Value.Data);
            Debug.Log(item.Value.Count);
        }
    }

    public void RemoveObjectFromInventory(string name)
    {
        if (inventory.ContainsKey(name))
        {
            if (inventory[name].Count > 1) 
            {
                inventory[name].Count--;
            }
            else
            {
                inventory.Remove(name);
            }
        }
    }

    public void AddObjectToInventory(string name, ObjectStock type)
    {
        if (!inventory.ContainsKey(name))
        {
            Debug.Log("Add object to inventory");
            inventory.Add(name, type);
        }
        else
        {
            Debug.Log("Object exist. Incrementing count of that object");
            inventory[name].Count++;
        }
    }
}
