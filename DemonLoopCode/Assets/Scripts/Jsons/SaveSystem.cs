using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
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

[System.Serializable]
public class StatsPersistenceContainer
{
    public GameObject player;
    public List<ObjectStockData> inventory;
    public float money;
    public float moneyRefined;
    public int bossRoom;
    public int room;
    public int floor;
    public int saveRoom;
    public string sceneName;
    public List<StatsPersistenceData> teamCharacterStats;
    public Dictionary<string, ObjectData> inventoryDictionary;
    public ScriptableObject dataScriptableObject;
    public bool onRun;

    public StatsPersistenceContainer(string sceneName)
    {
        this.sceneName = sceneName;
        inventory = new List<ObjectStockData>();

        teamCharacterStats = new List<StatsPersistenceData>();
        inventoryDictionary = new Dictionary<string, ObjectData>();
        dataScriptableObject = ScriptableObject.CreateInstance<ScriptableObject>();
    }
}


public class SaveSystem : MonoBehaviour
{
    [SerializeField] GameObject player;
    PlayerInventory playerInventory;

    private void Start()
    {
        player = GetComponent<GameObject>();
        playerInventory = GetComponent<PlayerInventory>();
    }

    // Guardaremos todas las cosas que tengamos en ese momento
    public void SaveData(string sceneName, bool runState)
    {
        // Directorio donde se guarda la partida
        string fileName = "newStatsPersistenceData.json";
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

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
        savedController.teamCharacterStats.AddRange(Data.Instance.CharactersTeamStats);

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

                // Team y Stats
                Data.Instance.CharactersTeamStats.Clear();
                foreach (var characterData in loadedStats.teamCharacterStats)
                    Data.Instance.CharactersTeamStats.Add(characterData);

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

                // Ruta del Json donde esta guardado 
                string fileNameLoad = "newStatsPersistenceData.json";
                string filePathLoad = Path.Combine(Application.persistentDataPath, fileNameLoad);
                loadedStats = LoadController.LoadStats(filePathLoad);
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

public class LoadController : MonoBehaviour
{
    public static StatsPersistenceContainer LoadStats(string file)
    {
        if (File.Exists(file))
        {
            string json = File.ReadAllText(file);
            return JsonUtility.FromJson<StatsPersistenceContainer>(json);
        }
        else
        {
            Debug.LogWarning("No se encontro el archivo " + file);
            return null;
        }
    }
}
