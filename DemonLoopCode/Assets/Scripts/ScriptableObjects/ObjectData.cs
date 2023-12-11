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
    [SerializeField] Types type;

    float baseDamage = 1.5f;
    
    CombatFlow combatFlow;
    EnterBattle enterBattle;
    PlayerInventory inventory;

    public Sprite Icon { get { return icon; } }
    public string Description { get { return description; } }

    public ObjectTypes ObjectType { get { return objectType; } }
    public Types Type { get { return type; } }
    public float BaseNum { get { return baseNum; } }

    public float BaseDamage { get { return baseDamage; } }

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
                switch (Type)
                {
                    case Types.Fire:
                        if (target.Type == Types.Plant)
                        {
                            target.Health -= BaseNum * baseDamage;
                        }
                        break;

                    case Types.Plant:
                        if (target.Type == Types.Water)
                        {
                            target.Health -= BaseNum * baseDamage;
                        }
                        break;

                    case Types.Water:
                        if (target.Type == Types.Fire)
                        {
                            target.Health -= BaseNum * baseDamage;
                        }
                        break;

                    case Types.Light:
                        if (target.Type == Types.Darkness)
                        {
                            target.Health -= BaseNum * baseDamage;
                        }
                        break;

                    case Types.Darkness:
                        if (target.Type == Types.Light)
                        {
                            target.Health -= BaseNum * baseDamage;
                        }
                        break;
                }
                // target.Health -= BaseNum;
                break;

        }
        inventory.RemoveObjectFromInventory(name.Substring(4, name.Length - 4));
    }//Fin de UserObject

    private string ObtainStateName()
    {
        return stateAsociated.name.Substring(4, stateAsociated.name.Length - 4).ToUpper();
    }
}