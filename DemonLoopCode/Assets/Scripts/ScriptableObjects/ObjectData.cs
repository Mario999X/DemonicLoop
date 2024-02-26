using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public enum ObjectTypes { Health, Mana, HealState, Throwable, Revive }

[CreateAssetMenu]
public class ObjectData : ScriptableObject
{
    private FloatingTextCombat floatingText;
    [SerializeField] private Sprite icon;

    [TextArea]
    [SerializeField] private string description;
    [SerializeField] private float cost;
    [SerializeField] private ObjectTypes objectType;
    [SerializeField] private float baseNum;
    [SerializeField] private StateData stateAsociated;
    [SerializeField] private bool alliesTargets = false;
    [SerializeField] private Types type;
    [SerializeField] private GameObject buttonPrefab;

    private EnterBattle enterBattle;
    private PlayerInventory inventory;

    public Sprite Icon { get { return icon; } }
    public string Description { get { return description; } }
    public float Cost { get { return cost; } set { cost = value; } }
    public ObjectTypes ObjectType { get { return objectType; } }
    public Types Type { get { return type; } }
    public float BaseNum { get { return baseNum; } }

    // Cuando se hace click en este objeto.
    public void Click(PlayerInventory inventory)
    {
        floatingText = GameObject.Find("System").GetComponent<FloatingTextCombat>();

        this.inventory = inventory;
        enterBattle = GameObject.Find("System").GetComponent<EnterBattle>();

        if (enterBattle.OneTime)
        {
            if(objectType == ObjectTypes.Revive)
            {
                GameObject.Find("System").GetComponent<CombatFlow>().GeneratePlayersDefeatedButtons(this);
            } else GameObject.Find("System").GetComponent<CombatFlow>().GenerateTargetsButtons(alliesTargets, this);
        }
        else
        {
            if (ObjectTypes.Throwable != ObjectType)
            {
                GameObject.Find("Inventory").transform.GetChild(1).gameObject.SetActive(true);

                GameObject[] buttons = GameObject.FindGameObjectsWithTag("Buttons");

                if (buttons.Length > 0)
                {
                    foreach (GameObject bt in buttons)
                    {
                        Destroy(bt);
                    }
                }

                if (objectType == ObjectTypes.Revive)
                {
                    CreateButtonsPlayersDefeated(GameObject.Find("PartyButtons"));
                }
                else if (objectType != ObjectTypes.Throwable) CreateButtons(GameObject.Find("PartyButtons"));
            }
        }
    }

    void CreateButtons(GameObject spawnMoveBT)
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Shop")
        {
            List<StatsPersistenceData> charactersTeamStats = GameObject.Find("System").GetComponent<Data>().CharactersTeamStats;

            foreach (StatsPersistenceData stats in charactersTeamStats)
            {
                Debug.Log(spawnMoveBT);

                GameObject bt = Instantiate(buttonPrefab, spawnMoveBT.transform.position, Quaternion.identity);
                bt.transform.SetParent(spawnMoveBT.transform);
                bt.name = "Ally " + stats.name;//Nombre de los botones que se van a generar
                bt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = stats.name.Substring(stats.name.IndexOf("_") + 1);
                bt.GetComponent<Button>().onClick.AddListener(delegate { UserObjectInOverworld(stats); });
            }
        }
        else
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player").ToArray();

            foreach (GameObject pl in players)
            {
                Debug.Log(spawnMoveBT);

                GameObject bt = Instantiate(buttonPrefab, spawnMoveBT.transform.position, Quaternion.identity);
                bt.transform.SetParent(spawnMoveBT.transform);
                bt.name = "Ally " + pl.name;//Nombre de los botones que se van a generar
                bt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pl.name.Substring(pl.name.IndexOf("_") + 1);
                bt.GetComponent<Button>().onClick.AddListener(delegate { UserObjectInBattle(pl); });
            }
        }
    }

    private void CreateButtonsPlayersDefeated(GameObject spawnMoveBT)
    {
        List<GameObject> playersDefeated = GameObject.Find("System").GetComponent<CombatFlow>().PlayersDefeated;

        foreach (GameObject pl in playersDefeated)
        {
            GameObject bt = Instantiate(buttonPrefab, spawnMoveBT.transform.position, Quaternion.identity);
            bt.transform.SetParent(spawnMoveBT.transform);
            bt.name = "Ally " + pl.name;//Nombre de los botones que se van a generar
            bt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pl.name.Substring(pl.name.IndexOf("_") + 1);
            bt.GetComponent<Button>().onClick.AddListener(delegate { UserObjectInBattle(pl); });
        }
    }

    public void UserObjectInOverworld(StatsPersistenceData target)
    {
        AudioManager.Instance.PlaySoundButtons();
        switch (ObjectType)
        {
            case ObjectTypes.Health:
                if(baseNum == 0) target.Health = target.MaxHealth;
                else target.Health += BaseNum;

                break;

            case ObjectTypes.Mana:
                if(baseNum == 0) target.Mana = target.MaxMana;
                else target.Mana += BaseNum;

                break;

            case ObjectTypes.HealState:
                GameObject.Find("System").GetComponent<LibraryStates>().RemoveCharacterWithState(target.CharacterPB, ObtainStateName());
                break;

            case ObjectTypes.Revive:
                target.Health = baseNum;
                break;
        }

        inventory.RemoveObjectFromInventory(name.Substring(4, name.Length - 4));
    }//Fin de UserObjectSP

    public void UserObjectInBattle(GameObject @character)
    {
        if (enterBattle.OneTime)
            GameObject.Find("System").GetComponent<CombatFlow>().InventoryTurn();
        else
            GameObject.Find("Inventory").transform.GetChild(1).gameObject.SetActive(false);
        
        Stats target = @character.GetComponent<Stats>();

        switch (ObjectType)
        {
            case ObjectTypes.Health:
                if(baseNum == 0)
                {
                    target.Health = target.MaxHealth;

                    floatingText.ShowFloatingTextNumbers(@character, target.MaxHealth, Color.green); 
                } else
                {
                    target.Health += BaseNum;
                    floatingText.ShowFloatingTextNumbers(@character, BaseNum, Color.green);
                }
                break;

            case ObjectTypes.Mana:
                if(baseNum == 0)
                {
                    target.Mana = target.MaxMana;

                    floatingText.ShowFloatingTextNumbers(@character, target.MaxMana, Color.blue); 
                } else
                {
                    target.Mana += BaseNum;
                    floatingText.ShowFloatingTextNumbers(@character, BaseNum, Color.blue);
                }
                break;

            case ObjectTypes.HealState:
                GameObject.Find("System").GetComponent<LibraryStates>().RemoveCharacterWithState(@character, ObtainStateName());
                break;
            
            case ObjectTypes.Revive:
                target.Revive(baseNum);
                floatingText.ShowFloatingText(character, "Revived", Color.yellow);
                GameObject.Find("System").GetComponent<CombatFlow>().CheckIfAnAllyHasRevived();

                break;

            case ObjectTypes.Throwable:
                //En este caso le tiramos algo al enemigo
                switch (Type)
                {
                    case Types.Fire:
                        if (target.Type == Types.Plant)
                        {
                            target.Health -= BaseNum;
                        }
                        break;

                    case Types.Plant:
                        if (target.Type == Types.Water)
                        {
                            target.Health -= BaseNum;
                        }
                        break;

                    case Types.Water:
                        if (target.Type == Types.Fire)
                        {
                            target.Health -= BaseNum;
                        }
                        break;

                    case Types.Light:
                        if (target.Type == Types.Darkness)
                        {
                            target.Health -= BaseNum;
                        }
                        break;

                    case Types.Darkness:
                        if (target.Type == Types.Light)
                        {
                            target.Health -= BaseNum;
                        }
                        break;
                }

                if(target.Health <= 0)
                {
                    if(target.gameObject.CompareTag("Enemy")) GameObject.Find("System").GetComponent<CombatFlow>().DeleteEnemyFromList(target.gameObject);
                    else GameObject.Find("System").GetComponent<CombatFlow>().DeleteAllieFromArray(target.gameObject);
                }
                
                break;

        }

        inventory.RemoveObjectFromInventory(name.Substring(4, name.Length - 4));
    }//Fin de UserObjectS2

    private string ObtainStateName()
    {
        return stateAsociated.name.Substring(4, stateAsociated.name.Length - 4).ToUpper();
    }
}