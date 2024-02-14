using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoserReset : MonoBehaviour
{

    [SerializeField] Image imageLose;
    GameObject[] players;

    List<StatsPersistenceData> initialStateTeam;
    List<PlayerInventory> initialPlayerInventory;
    StatsPersistenceData statsPersistenceData;
    MoneyPlayer moneyPlayer;
    PlayerInventory playerInventory;

    List<ObjectStock> initialObject;

    float initialMoney = 0f;
    float initialMoneyBoss = 0f;
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        statsPersistenceData = GetComponent<StatsPersistenceData>();
        moneyPlayer = GetComponent<MoneyPlayer>();
        playerInventory = GetComponent<PlayerInventory>();

        initialObject = new List<ObjectStock>();
    }

    public void ShowImage(bool end)
    {
        imageLose.enabled = end;

        ResetPersistence();

        ResetInitialInventory();

        ResetInitialMoney();
        
        MoveSceneInitial();
    }

    public void MoveSceneInitial()
    {
        // ID 0 es el titulo
        // ID 1 es el Scene 2
        // ID 3 es el Shop
        SceneManager.Instance.LoadScene(0);
    }

    public void SaveInitial()
    {
        SaveInitialState();
        SaveInitialInventory();
        SaveInitialMoney();
    }

    public void SaveInitialState()
    {
        // Guardamos los StatsPersistenceData al inicio del juego
        foreach (var player in players)
        {
            var initalState = player.GetComponent<StatsPersistenceData>();

            var savedInitalState = SearchCharacterTeamStats(player.name);

            savedInitalState.Level = initalState.Level;
            savedInitalState.CurrentXP = initalState.CurrentXP;
            savedInitalState.Health = initalState.Health;
            savedInitalState.MaxHealth = initalState.MaxHealth;
            savedInitalState.Mana = initalState.Mana;
            savedInitalState.MaxMana = initalState.MaxMana;
            savedInitalState.SP = initalState.SP;
            savedInitalState.Strenght = initalState.Strenght;
            savedInitalState.PhysicalDefense = initalState.PhysicalDefense;
            savedInitalState.MagicAtk = initalState.MagicAtk;
            savedInitalState.MagicDef = initalState.MagicDef;
            savedInitalState.CriticalChance = initalState.CriticalChance;
            savedInitalState.ActualStates = initalState.ActualStates;

            savedInitalState.ListAtk = initalState.ListAtk;

            Debug.Log("Character Saved after battle " + player.name);
        }
    }

    public void ResetPersistence()
    {
        // Cuando morimos se reinicia los StatsPersistenceData
        // al que tenia cuando se inicio el juego
        foreach (var player in players)
        {
            var currentState = player.GetComponent<StatsPersistenceData>();

            var initalState = SearchCharacterTeamStats(player.name);

            currentState.Level = initalState.Level;
            currentState.CurrentXP = initalState.CurrentXP;
            currentState.Health = initalState.Health;
            currentState.MaxHealth = initalState.MaxHealth;
            currentState.Mana = initalState.Mana;
            currentState.MaxMana = initalState.MaxMana;
            currentState.SP = initalState.SP;
            currentState.Strenght = initalState.Strenght;
            currentState.PhysicalDefense = initalState.PhysicalDefense;
            currentState.MagicAtk = initalState.MagicAtk;
            currentState.MagicDef = initalState.MagicDef;
            currentState.CriticalChance = initalState.CriticalChance;
            currentState.ActualStates = initalState.ActualStates;

            currentState.ListAtk = initalState.ListAtk;

            Debug.Log("Character Saved after battle " + player.name);
        }
    }

    public StatsPersistenceData SearchCharacterTeamStats(string characterName)
    {
        StatsPersistenceData charStats = null;

        initialStateTeam.ForEach(x =>
        {
            if (x.CharacterPB.name + "(Clone)" == characterName)
            {
                //Debug.Log("x.CharacterPB.name(Clone)");
                charStats = x;
            }
        });

        return charStats;
    }


    public void SaveInitialInventory()
    {
        // Guardamos los Objectos del inventario al iniciar el juego
        foreach (var objectData in playerInventory.inventory.Values)
        {
            ObjectStock initialStock = new ObjectStock(objectData.Data, objectData.Count);
            initialObject.Add(initialStock);
        }
    }//Fin de SaveInitialInventory

    public void ResetInitialInventory()
    {
        playerInventory.inventory.Clear();

        foreach (var initialObj in initialObject)
        {
            ObjectStock resetInitialStock = new ObjectStock(initialObj.Data, initialObj.Count);

            initialObject.Add(resetInitialStock);
        }
    }

    public void SaveInitialMoney()
    {
        initialMoney = moneyPlayer.Money;
        initialMoneyBoss = moneyPlayer.MoneyRefined;
    }

    public void ResetInitialMoney()
    {
        moneyPlayer.Money = initialMoney;
        moneyPlayer.MoneyRefined = initialMoneyBoss;
    }

}