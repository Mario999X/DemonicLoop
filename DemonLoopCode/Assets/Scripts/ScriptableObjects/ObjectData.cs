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

        if (enterBattle.OneTime)
        {
            UserObject(combatFlow.Character);
        }

        GameObject.Find("System").GetComponent<CombatFlow>().InventoryTurn();

        inventory.RemoveObjectFromInventory(name);

    }

    public void UserObject(GameObject @character)
    {
        Stats player = @character.GetComponent<Stats>();
        switch (ObjectType)
        {
            case ObjectTypes.Health:
                //Debug.Log("Pocion de Cura");
                player.Health += BaseNum;
                break;
            case ObjectTypes.Mana:
                //Debug.Log("Pocion de Mana");
                player.Mana += BaseNum;
                break;
        }
    }//Fin de UserObject
}