using UnityEngine;

public enum ObjectTypes { Health, Mana, HealState }

[CreateAssetMenu]
public class ObjectData : ScriptableObject
{
    [SerializeField] private Sprite icon;

    [TextArea]
    [SerializeField] private string description;
    [SerializeField] private ObjectTypes objectType;
    [SerializeField] private float baseNum;
    [SerializeField] private StateData stateAsociated;

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

            GameObject.Find("System").GetComponent<CombatFlow>().InventoryTurn();
        }

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

            case ObjectTypes.HealState:
                GameObject.Find("System").GetComponent<LibraryStates>().RemoveCharacterWithState(@character, ObtainStateName());
                break;

        }
    }//Fin de UserObject

    private string ObtainStateName()
    {
        return stateAsociated.name.Substring(4, stateAsociated.name.Length - 4).ToUpper();
    }
}