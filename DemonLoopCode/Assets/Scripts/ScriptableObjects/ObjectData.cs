using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class ObjectData : ScriptableObject
{
    [SerializeField] private Sprite icon;
    [TextArea]
    [SerializeField] private string description;

    public Sprite Icon {  get { return icon; } }
    public string Description { get { return description; } }

    public void Click(PlayerInventory inventory) 
    { 
        Debug.Log(name + " was clicked");

        
    }
}
