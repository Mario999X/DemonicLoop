using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[Serializable]
public class ObjectStockData
{
    public ObjectData objData;
    public int count;

    public ObjectStockData(ObjectData scriptableObject, int count)
    {
        this.objData = scriptableObject;
        this.count = count;
    }
}

[Serializable]
public class StatsPersistenceContainer
{
    public GameObject player;
    public float money;
    public float moneyRefined;
    public int bossRoom;
    public int room;
    public int floor;
    public int saveRoom;
    public string sceneName;
    public bool onRun;
    public ScriptableObject dataScriptableObject;
    public Dictionary<string, ObjectData> inventoryDictionary;
    public List<StatsPersistenceData> teamCharacterStats;
    public List<StatsPersistenceData> teamCharacterBackupStats;
    public List<ObjectStockData> inventory;
    public List<StatsIndividual> Teamstats;
    public List<StatsIndividual> Backupstats;

    public StatsPersistenceContainer(string sceneName)
    {
        this.sceneName = sceneName;
        inventory = new List<ObjectStockData>();

        teamCharacterStats = new List<StatsPersistenceData>();
        inventoryDictionary = new Dictionary<string, ObjectData>();
        dataScriptableObject = ScriptableObject.CreateInstance<ScriptableObject>();
        Teamstats = new List<StatsIndividual>();
        Backupstats = new List<StatsIndividual>();
    }
}

[Serializable]
public class StatsIndividual
{
    public int level;
    public float currentXP;
    public float health;
    public float maxHealth;
    public float mana;
    public float maxMana;
    public float sp;
    public float strength;
    public float physicalDef;
    public float magicAtk;
    public float magicDef;
    public float criticalChance;
    public float time = 0;
    public List<AttackData> listAtk = new List<AttackData>();
    public List<ActualStateData> actualStates = new List<ActualStateData>();

    public StatsIndividual(int level, float currentXP, float health, float maxHealth, float mana, float maxMana, float sp, float strength, float physicalDef, float magicAtk, float magicDef, float criticalChance, float time, List<AttackData> listAtk, List<ActualStateData> actualStates)
    {
        this.level = level;
        this.currentXP = currentXP;
        this.health = health;
        this.maxHealth = maxHealth;
        this.mana = mana;
        this.maxMana = maxMana;
        this.sp = sp;
        this.strength = strength;
        this.physicalDef = physicalDef;
        this.magicAtk = magicAtk;
        this.magicDef = magicDef;
        this.criticalChance = criticalChance;
        this.time = time;
        this.listAtk = listAtk;
        this.actualStates = actualStates;
    }
}

public class SaveSystem : MonoBehaviour
{
    PlayerInventory playerInventory;

    private void Start()
    {
        playerInventory = GetComponent<PlayerInventory>();
    }

    // Guardaremos todas las cosas que tengamos en ese momento
    public void SaveData(string sceneName, bool runState)
    {
        // Directorio donde se guarda la partida
        string fileName = "newStatsPersistenceData.json";
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(filePath)) File.Delete(filePath);

        StreamWriter file = new StreamWriter(File.Open(filePath, FileMode.OpenOrCreate)); // Si el archivo no existe lo crea.

        StatsPersistenceContainer savedController = new StatsPersistenceContainer(sceneName);

        Debug.Log("SaveSystem.sceneName " + savedController.sceneName);

        savedController.onRun = runState;

        // Dinero
        savedController.money = Data.Instance.MoneyPlayer.Money;
        savedController.moneyRefined = Data.Instance.MoneyPlayer.MoneyRefined;

        // Salas
        savedController.bossRoom = Data.Instance.BossRoom;
        savedController.room = Data.Instance.Room;
        savedController.floor = Data.Instance.Floor;
        savedController.saveRoom = Data.Instance.SaveRoom;

        // Equipo mas Stats
        if (Data.Instance.CharactersTeamStats.Count > 0)
            savedController.teamCharacterStats.AddRange(Data.Instance.CharactersTeamStats);
        
        if (Data.Instance.CharactersBackupStats.Count > 0)
            savedController.teamCharacterBackupStats.AddRange(Data.Instance.CharactersBackupStats);
        
        Data.Instance.CharactersTeamStats.ForEach(x =>
        {
            savedController.Teamstats.Add(new StatsIndividual
                (
                    x.Level,
                    x.CurrentXP,
                    x.Health,
                    x.MaxHealth,
                    x.Mana,
                    x.MaxMana,
                    x.SP,
                    x.Strenght,
                    x.PhysicalDefense,
                    x.MagicAtk,
                    x.MagicDef,
                    x.CriticalChance,
                    x.Time,
                    x.ListAtk,
                    x.ActualStates
                )
            );
        });
        
        Data.Instance.CharactersBackupStats.ForEach(x =>
        {
            savedController.Backupstats.Add(new StatsIndividual
                (
                    x.Level,
                    x.CurrentXP,
                    x.Health,
                    x.MaxHealth,
                    x.Mana,
                    x.MaxMana,
                    x.SP,
                    x.Strenght,
                    x.PhysicalDefense,
                    x.MagicAtk,
                    x.MagicDef,
                    x.CriticalChance,
                    x.Time,
                    x.ListAtk,
                    x.ActualStates
                )
            );
        });

        // Inventario
        foreach (var obj in Data.Instance.PlayerInventory.Inventory.Values)
        {
            savedController.inventory.Add(new ObjectStockData(obj.Data, obj.Count));
            savedController.inventoryDictionary.Add(obj.Data.name, obj.Data);
        }

        // Convertir el objeto statsContainer a JSON y lo guardas en un archivo
        string json = JsonUtility.ToJson(savedController);
        file.WriteLine(json);

        file.Close();

        Debug.Log("Ruta guardado" + filePath);
    }


    // Cargar los datos de la partida
    public void LoadData()
    {
        string fileName = "newStatsPersistenceData.json";
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        // En caso de que exista el archivo.
        if (File.Exists(filePath))
        {
            string jsonContent;
            using (StreamReader reader = new StreamReader(filePath))
            {
                jsonContent = reader.ReadToEnd();
            }
            StatsPersistenceContainer loadedStats = JsonUtility.FromJson<StatsPersistenceContainer>(jsonContent);

            if (loadedStats != null)
            {
                Data.Instance.SceneName = loadedStats.sceneName;

                Data.Instance.OnRun = loadedStats.onRun;

                // Dinero
                MoneyPlayer money = GetComponent<MoneyPlayer>();
                money.Money = loadedStats.money;
                money.MoneyRefined = loadedStats.moneyRefined;

                // Salas
                Data.Instance.BossRoom = loadedStats.bossRoom;
                Data.Instance.Room = loadedStats.room;
                Data.Instance.Floor = loadedStats.floor;
                Data.Instance.SaveRoom = loadedStats.saveRoom;

                // Team
                Data.Instance.CharactersTeamStats.Clear();
                foreach (var characterData in loadedStats.teamCharacterStats)
                    Data.Instance.CharactersTeamStats.Add(characterData);
                
                Data.Instance.CharactersBackupStats.Clear();
                foreach (var characterData in loadedStats.teamCharacterBackupStats)
                    Data.Instance.CharactersBackupStats.Add(characterData);

                if (loadedStats.Teamstats.Count > 0)
                    for (int x = 0; x < Data.Instance.CharactersTeamStats.Count; x++)
                    {
                        Data.Instance.CharactersTeamStats[x].Level = loadedStats.Teamstats[x].level;
                        Data.Instance.CharactersTeamStats[x].CurrentXP = loadedStats.Teamstats[x].currentXP;
                        Data.Instance.CharactersTeamStats[x].Health = loadedStats.Teamstats[x].health;
                        Data.Instance.CharactersTeamStats[x].MaxHealth = loadedStats.Teamstats[x].maxHealth;
                        Data.Instance.CharactersTeamStats[x].Mana = loadedStats.Teamstats[x].mana;
                        Data.Instance.CharactersTeamStats[x].MaxMana = loadedStats.Teamstats[x].maxMana;
                        Data.Instance.CharactersTeamStats[x].SP = loadedStats.Teamstats[x].sp;
                        Data.Instance.CharactersTeamStats[x].Strenght = loadedStats.Teamstats[x].strength;
                        Data.Instance.CharactersTeamStats[x].PhysicalDefense = loadedStats.Teamstats[x].physicalDef;
                        Data.Instance.CharactersTeamStats[x].MagicAtk = loadedStats.Teamstats[x].magicAtk;
                        Data.Instance.CharactersTeamStats[x].MagicDef = loadedStats.Teamstats[x].magicDef;
                        Data.Instance.CharactersTeamStats[x].CriticalChance = loadedStats.Teamstats[x].criticalChance;
                        Data.Instance.CharactersTeamStats[x].Time = loadedStats.Teamstats[x].time;
                        Data.Instance.CharactersTeamStats[x].ListAtk = loadedStats.Teamstats[x].listAtk;
                        Data.Instance.CharactersTeamStats[x].ActualStates = loadedStats.Teamstats[x].actualStates;
                    }

                if (loadedStats.Backupstats.Count > 0)
                    for (int x = 0; x < Data.Instance.CharactersBackupStats.Count; x++)
                    {
                        Data.Instance.CharactersBackupStats[x].Level = loadedStats.Backupstats[x].level;
                        Data.Instance.CharactersBackupStats[x].CurrentXP = loadedStats.Backupstats[x].currentXP;
                        Data.Instance.CharactersBackupStats[x].Health = loadedStats.Backupstats[x].health;
                        Data.Instance.CharactersBackupStats[x].MaxHealth = loadedStats.Backupstats[x].maxHealth;
                        Data.Instance.CharactersBackupStats[x].Mana = loadedStats.Backupstats[x].mana;
                        Data.Instance.CharactersBackupStats[x].MaxMana = loadedStats.Backupstats[x].maxMana;
                        Data.Instance.CharactersBackupStats[x].SP = loadedStats.Backupstats[x].sp;
                        Data.Instance.CharactersBackupStats[x].Strenght = loadedStats.Backupstats[x].strength;
                        Data.Instance.CharactersBackupStats[x].PhysicalDefense = loadedStats.Backupstats[x].physicalDef;
                        Data.Instance.CharactersBackupStats[x].MagicAtk = loadedStats.Backupstats[x].magicAtk;
                        Data.Instance.CharactersBackupStats[x].MagicDef = loadedStats.Backupstats[x].magicDef;
                        Data.Instance.CharactersBackupStats[x].CriticalChance = loadedStats.Backupstats[x].criticalChance;
                        Data.Instance.CharactersBackupStats[x].Time = loadedStats.Backupstats[x].time;
                        Data.Instance.CharactersBackupStats[x].ListAtk = loadedStats.Backupstats[x].listAtk;
                        Data.Instance.CharactersBackupStats[x].ActualStates = loadedStats.Backupstats[x].actualStates;
                    }

                Debug.Log("Buscar loadedStats.inventory " + loadedStats.inventory);
                Debug.Log("playerInventory.inventory.Values " + playerInventory.Inventory.Values);
                Debug.Log("loadedStats.inventoryDictionary " + loadedStats.inventoryDictionary);

                // Inventario 
                playerInventory.Inventory.Clear();

                if (loadedStats.inventoryDictionary != null)
                {
                    loadedStats.inventoryDictionary.Clear();
                }

                ObjectData[] objects = Resources.LoadAll<ObjectData>("Data/Objects");

                int i = 0;
                foreach (var item in loadedStats.inventory)
                {
                    Debug.Log("objects[i].name " + objects[i].name + " i " + i);
                    if (objects[i] = item.objData)
                    {
                        playerInventory.AddObjectToInventory(objects[i].name, item.objData, item.count);
                        i++;
                    }
                }
                int c = 0;
                if (loadedStats.inventoryDictionary != null)
                {
                    foreach (var objName in loadedStats.inventoryDictionary)
                    {
                        Debug.Log("objects[c].name " + objects[c].name + " c " + c);
                        var realObjName = objects[c].name.Substring(4, objects[c].name.Length - 4).ToUpper();
                        if (objects[i] = objName.Value)
                        {
                            playerInventory.AddObjectToInventory(objects[i].name, objects[i], loadedStats.inventoryDictionary.Count);
                            c++;
                        }
                    }
                }

                Debug.Log("Datos cargados exitosamente.");
            }
            else
            {
                Debug.LogWarning("No se pudo cargar el archivo de datos");
            }
        }
        else
        {
            Debug.Log("No hay datos guardados");
        }
    }
}