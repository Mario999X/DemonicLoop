using UnityEngine;

public enum ObjectTypes { Health, Mana, HealState, Throwable }

[CreateAssetMenu]
public class ObjectData : ScriptableObject
{
    [SerializeField] private Sprite icon;

    [TextArea]
    [SerializeField] private string description;
    [SerializeField] private ObjectTypes objectType;
    [SerializeField] private float baseNum;
    [SerializeField] private StateData stateAsociated;
    [SerializeField] bool targetsToLoad = false;

    CombatFlow combatFlow;
    EnterBattle enterBattle;
    PlayerInventory inventory;

    public Sprite Icon { get { return icon; } }
    public string Description { get { return description; } }

    public ObjectTypes ObjectType { get { return objectType; } }
    public float BaseNum { get { return baseNum; } }
    
    // Cuando se hace click en este objeto.
    public void Click(PlayerInventory inventory)
    {
        enterBattle = GameObject.Find("System").GetComponent<EnterBattle>();
        combatFlow = GameObject.Find("System").GetComponent<CombatFlow>();
        this.inventory = inventory;
        if (enterBattle.OneTime)
        {
            combatFlow.GenerateTargetsButtons(targetsToLoad, this);

        }


    }
    public void UserObject(GameObject @character)
    {
        GameObject.Find("System").GetComponent<CombatFlow>().InventoryTurn();
        Stats target = @character.GetComponent<Stats>();
        switch (ObjectType)
        {
            case ObjectTypes.Health:
                //Debug.Log("Pocion de Cura");
                target.Health += BaseNum;
                break;

            case ObjectTypes.Mana:
                //Debug.Log("Pocion de Mana");
                target.Mana += BaseNum;
                break;

            case ObjectTypes.HealState:
                GameObject.Find("System").GetComponent<LibraryStates>().RemoveCharacterWithState(@character, ObtainStateName());
                break;

            case ObjectTypes.Throwable:
                //En este caso le tiramos algo al enemigo
                target.Health -= BaseNum;
                break;

        }
        inventory.RemoveObjectFromInventory(name.Substring(4, name.Length - 4));
    }//Fin de UserObject

    private string ObtainStateName()
    {
        return stateAsociated.name.Substring(4, stateAsociated.name.Length - 4).ToUpper();
    }
}