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
    EnterBattle enterBattle;
    public Sprite Icon { get { return icon; } }
    public string Description { get { return description; } }

    public ObjectTypes ObjectType { get { return objectType; } }
    public float BaseNum { get { return baseNum; } }

    // Cuando se hace click en este objeto.
    public void Click(PlayerInventory inventory)
    {
        enterBattle = GameObject.Find("System").GetComponent<EnterBattle>();
        combatFlow = GameObject.Find("System").GetComponent<CombatFlow>();
        Debug.Log(name + " was clicked");
        /*
            Inserte la función que debe realizar el objeto aquí.
        */

        if (enterBattle.OneTime)
        {
            UserObject(this, combatFlow.Character);
        }

        GameObject.Find("System").GetComponent<CombatFlow>().InventoryTurn();

        inventory.RemoveObjectFromInventory(name);

    }

    public ObjectData UserObject(ObjectData CheckObject, GameObject @character)
    {

        Debug.Log("@character " + @character);
        Debug.Log("objectType " + CheckObject.ObjectType);
        Debug.Log("objectType " + CheckObject.name);

        Stats player = @character.GetComponent<Stats>();
        Debug.Log("player " + player.name);
        switch (CheckObject.ObjectType)
        {
            case ObjectTypes.Health:
                Debug.Log("Pocion de Cura");
                Debug.Log("player.Health " + player.Health);
                Debug.Log("CheckObject.BaseNum " + CheckObject.BaseNum);
                player.Health += CheckObject.BaseNum;
                break;
            case ObjectTypes.Mana:
                Debug.Log("Pocion de Mana");
                Debug.Log("player.Health " + player.Health);
                Debug.Log("CheckObject.BaseNum " + CheckObject.BaseNum);
                player.Mana += CheckObject.BaseNum;
                break;
        }

        return CheckObject;
    }
}