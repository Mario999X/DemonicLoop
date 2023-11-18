using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum ObjectTypes { Health, Mana }

[CreateAssetMenu]
public class ObjectData : ScriptableObject
{
    [SerializeField] private Sprite icon;
    [TextArea]
    [SerializeField] private string description;

    [SerializeField] private ObjectTypes objectType;
    [SerializeField] private float baseNum;

    CombatFlow combatFlow;
    GameObject character;

    public Sprite Icon { get { return icon; } }
    public string Description { get { return description; } }

    public ObjectTypes ObjectType { get { return objectType; } }
    public float BaseNum { get { return baseNum; } }

    Stats stats;

    

    // Cuando se hace click en este objeto.
    public void Click(PlayerInventory inventory)
    {
        Debug.Log(name + " was clicked");
        /*
            Inserte la función que debe realizar el objeto aquí.
        */

        inventory.RemoveObjectFromInventory(name);
    }
}