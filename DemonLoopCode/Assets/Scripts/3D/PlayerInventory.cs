using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectStock
{
    private ScriptableObject data;
    private GameObject buttonINV3D;
    private int count = 1;

    public int Count { get { return count; } set { this.count = value; } }
    public ScriptableObject Data { get { return data; } }
    public GameObject ButtonINV3D { get { return buttonINV3D; } }

    public ObjectStock(ScriptableObject scriptableObject, GameObject buttonINV3D) 
    {
        this.data = scriptableObject;
        this.buttonINV3D = buttonINV3D;
    }
}
public class PlayerInventory : MonoBehaviour
{
    [SerializeField] ScriptableObject m_ScriptableObject;

    [Header("Referencia de boton")]
    [SerializeField] GameObject buttonRef3D;
    [SerializeField] GameObject buttonRef2D;

    [Header("UI grid de inventario")]
    [SerializeField] GameObject inventoryUI;
    
    Dictionary<string, ObjectStock> inventory = new Dictionary<string, ObjectStock>();

    // Start is called before the first frame update
    void Start()
    {
        AddObjectToInventory(m_ScriptableObject.name, m_ScriptableObject);

        foreach (var item in inventory)
        {
            Debug.Log(item.Key);
            Debug.Log(item.Value.Data);
            Debug.Log(item.Value.Count);
        }

        AddObjectToInventory(m_ScriptableObject.name, m_ScriptableObject);

        foreach (var item in inventory)
        {
            Debug.Log(item.Key);
            Debug.Log(item.Value.Data);
            Debug.Log(item.Value.Count);
        }
    }

    public void RemoveObjectFromInventory(string name)
    {
        if (inventory.ContainsKey(name.ToUpper()))
        {
            if (inventory[name.ToUpper()].Count > 1) 
            {
                Debug.Log("Counter of '" + name.ToUpper() + "' went down");
                inventory[name.ToUpper()].Count--;
                EditButtonINV3DText(inventory[name.ToUpper()]);
            }
            else
            {
                Debug.Log(name.ToUpper() + " was eliminated from dictionary");
                Destroy(inventory[name.ToUpper()].ButtonINV3D);
                inventory.Remove(name.ToUpper());
            }
        }
    }

    public void AddObjectToInventory(string name, ScriptableObject scriptableObject)
    {
        if (!inventory.ContainsKey(name.ToUpper()))
        {
            Debug.Log("Add object to inventory");

            inventory.Add(name.ToUpper(), new ObjectStock(scriptableObject, CreateButtonINV3D(scriptableObject as ObjectData, 1)));
        }
        else
        {
            Debug.Log("Object exist. Incrementing count of that object");
            inventory[name.ToUpper()].Count++;
            EditButtonINV3DText(inventory[name.ToUpper()]);
        }
    }

    public GameObject CreateButtonINV3D(ObjectData data, int count)
    {
        GameObject button = Instantiate(buttonRef3D, inventoryUI.transform.position, Quaternion.identity);
        button.transform.SetParent(inventoryUI.transform);

        button.GetComponent<Image>().enabled = false;

        Button buttonCMP = button.GetComponent<Button>();
        buttonCMP.onClick.AddListener(() => { data.Click(this); });

        button.transform.GetChild(0).GetComponent<Image>().sprite = data.Icon;
        button.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = data.name + " x" +  count;

        return button;
    }

    public void EditButtonINV3DText(ObjectStock data)
    {
        data.ButtonINV3D.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = data.Data.name + " x" + data.Count;
    }
}
