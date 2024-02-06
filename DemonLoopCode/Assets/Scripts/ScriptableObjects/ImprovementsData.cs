using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using static UnityEngine.GraphicsBuffer;

public enum ImprovementsTypes { Health, Mana, CriticalChance }

[CreateAssetMenu]
public class ImprovementsData : ScriptableObject
{
    [SerializeField] private Sprite icon;
    //maraRefined nombre de la moneda especial 
    //improvements
    //Las variables de las V las dejo aqui declaradas por
    // si en un futuro se quiere cambiar
    int powerHealthV1 = 100;
    int powerHealthV2 = 200;
    int powerHealthV3 = 250;

    int powerManaV1 = 100;
    int powerManaV2 = 200;
    int powerManaV3 = 250;

    int powerCriticalV1 = 5;
    int powerCriticalV2 = 10;
    int powerCriticalV3 = 15;

    bool riseHealthV1 = false;
    bool riseHealthV2 = false;
    bool riseHealthV3 = false;

    bool riseManaV1 = false;
    bool riseManaV2 = false;
    bool riseManaV3 = false;

    bool riseCriticalV1 = false;
    bool riseCriticalV2 = false;
    bool riseCriticalV3 = false;



    private FloatingTextCombat floatingText;

    [TextArea]
    [SerializeField] private string description;
    [SerializeField] private float costRefined;
    [SerializeField] private ImprovementsTypes improvementsType;

    [SerializeField] bool alliesTargets = false;
    [SerializeField] bool obtained = false;

    [SerializeField] GameObject buttonPrefab;

    float baseDamage = 1.5f;

    EnterBattle enterBattle;

    public Sprite Icon { get { return icon; } }
    public string Description { get { return description; } }
    public float CostRefined { get { return costRefined; } }
    public ImprovementsTypes ImprovementsType { get { return improvementsType; } }

    public bool Obtained { get { return obtained; } }
    public float BaseDamage { get { return baseDamage; } }

    // Cuando se hace click en este objeto.
    /*public void Click(PlayerInventory inventory)
    {
        floatingText = GameObject.Find("System").GetComponent<FloatingTextCombat>();

        this.inventory = inventory;
        enterBattle = GameObject.Find("System").GetComponent<EnterBattle>();

        if (enterBattle.OneTime)
        {
            if (improvementsType == ImprovementsTypes.Revive)
            {
                //GameObject.Find("System").GetComponent<CombatFlow>().GeneratePlayersDefeatedButtons(this);
            }
            //else GameObject.Find("System").GetComponent<CombatFlow>().GenerateTargetsButtons(alliesTargets, this);
        }
        else
        {
            if (ImprovementsTypes.Throwable != ImprovementsType)
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

                if (improvementsType == ImprovementsTypes.Revive)
                {
                    CreateButtonsPlayersDefeated(GameObject.Find("PartyButtons"));
                }
                else if (improvementsType != ImprovementsTypes.Throwable) CreateButtons(GameObject.Find("PartyButtons"));
            }
        }
    }*/

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
                bt.GetComponent<Button>().onClick.AddListener(delegate { BuyImprovement(stats); });
            }
        }
        /* else
         {
             GameObject[] players = GameObject.FindGameObjectsWithTag("Player").ToArray();

             foreach (GameObject pl in players)
             {
                 Debug.Log(spawnMoveBT);

                 GameObject bt = Instantiate(buttonPrefab, spawnMoveBT.transform.position, Quaternion.identity);
                 bt.transform.SetParent(spawnMoveBT.transform);
                 bt.name = "Ally " + pl.name;//Nombre de los botones que se van a generar
                 bt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pl.name.Substring(pl.name.IndexOf("_") + 1);
                 bt.GetComponent<Button>().onClick.AddListener(delegate { UserObjectS2(pl); });
             }
         }*/
    }

    /*private void CreateButtonsPlayersDefeated(GameObject spawnMoveBT)
    {
        List<GameObject> playersDefeated = GameObject.Find("System").GetComponent<CombatFlow>().PlayersDefeated;

        foreach (GameObject pl in playersDefeated)
        {
            GameObject bt = Instantiate(buttonPrefab, spawnMoveBT.transform.position, Quaternion.identity);
            bt.transform.SetParent(spawnMoveBT.transform);
            bt.name = "Ally " + pl.name;//Nombre de los botones que se van a generar
            bt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pl.name.Substring(pl.name.IndexOf("_") + 1);
            bt.GetComponent<Button>().onClick.AddListener(delegate { UserObjectS2(pl); });
        }
    }*/


    public void BuyImprovement(StatsPersistenceData target)
    {
        // Podria ponerse algun id a las mejoras
        // Suponiendo que sea el prota
        Debug.Log("target es prota " + target);
        switch (ImprovementsType)
        {
            case ImprovementsTypes.Health:
                if (!riseHealthV1)
                {
                    target.MaxHealth += powerHealthV1;
                    riseHealthV1 = true;
                }
                else if (!riseHealthV2 && HasV1(target))
                {
                    target.MaxHealth += powerHealthV2;
                    riseHealthV2 = true;
                }
                else if (!riseHealthV3 && HasV2(target))
                {
                    target.MaxHealth += powerHealthV3;
                    riseHealthV3 = true;
                }
                break;

            case ImprovementsTypes.Mana:
                if (!riseManaV1)
                {
                    target.MaxMana += powerManaV1;
                    riseManaV1 = true;
                }
                else if (!riseManaV2 && HasV1(target))
                {
                    target.MaxMana += powerManaV2;
                    riseManaV2 = true;
                }
                else if (!riseManaV3 && HasV2(target))
                {
                    target.MaxMana += powerManaV3;
                    riseManaV3 = true;
                }
                break;

            case ImprovementsTypes.CriticalChance:
                if (!riseCriticalV1)
                {
                    target.CriticalChance += powerCriticalV1;
                    riseCriticalV1 = true;
                }
                else if (!riseCriticalV2 && HasV1(target))
                {
                    target.CriticalChance += powerCriticalV2;
                    riseCriticalV2 = true;
                }
                else if (!riseCriticalV3 && HasV2(target))
                {
                    target.CriticalChance += powerCriticalV3;
                    riseCriticalV3 = true;
                }
                break;
        }//Fin del switch
    }

    private bool HasV1(StatsPersistenceData target)
    {
        switch (ImprovementsType)
        {
            case ImprovementsTypes.Health:
                return riseHealthV1;
            case ImprovementsTypes.Mana:
                return riseManaV1;
            case ImprovementsTypes.CriticalChance:
                return riseCriticalV1;
            default:
                return false;
        }
    }

    private bool HasV2(StatsPersistenceData target)
    {
        switch (ImprovementsType)
        {
            case ImprovementsTypes.Health:
                return riseHealthV2;
            case ImprovementsTypes.Mana:
                return riseManaV2;
            case ImprovementsTypes.CriticalChance:
                return riseCriticalV2;
            default:
                return false;
        }
    }
}
