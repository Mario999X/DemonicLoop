using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using static UnityEditor.Progress;
using TMPro;
using UnityEditor.U2D.Animation;

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
    public string sceneName;
    public List<StatsPersistenceData> teamCharacterStats;
    public Dictionary<string, ObjectData> inventoryDictionary;
    public ScriptableObject dataScriptableObject;


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
    public GameObject player;
    MoneyPlayer money;
    PlayerInventory playerInventory;

    private void Start()
    {
        player = GetComponent<GameObject>();
        money = GetComponent<MoneyPlayer>();
        playerInventory = GetComponent<PlayerInventory>();
        if (playerInventory == null)
        {
            Debug.LogError("No se encontro el PlayerInventory");
        }
    }


    public void SaveData()
    {
        StatsPersistenceContainer savedController = new StatsPersistenceContainer(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);


        Debug.Log("SaveSystem.sceneName " + savedController.sceneName);

        // Dinero
        savedController.money = money.Money;
        savedController.moneyRefined = money.MoneyRefined;

        // Equipo mas Stats
        savedController.teamCharacterStats.AddRange(Data.Instance.CharactersTeamStats);

        // Inventario 
        foreach (var obj in playerInventory.inventory.Values)
        {
            savedController.inventory.Add(new ObjectStockData(obj.Data, obj.Count));
            savedController.inventoryDictionary.Add(obj.Data.name, obj.Data);

        }

        // Directorio donde se guarda la partida
        string saveDirectory = Path.Combine(Application.dataPath + "/SaveGame");
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }


        PlayerPrefs.SetString("SavedSceneName", savedController.sceneName);
        PlayerPrefs.Save();
        // Ruta  del archivo
        string filePath = Path.Combine(saveDirectory, "newStatsPersistenceData.json");

        // Convierte el objeto statsContainer a JSON y lo guardas en un archivo
        string json = JsonUtility.ToJson(savedController);
        File.WriteAllText(filePath, json);

        Debug.Log("Ruta guardado" + filePath);
    }





    // Cargar partida
    public void LoadData()
    {
        string filePath = Path.Combine(Application.dataPath + "/SaveGame", "newStatsPersistenceData.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            StatsPersistenceContainer loadedStats = JsonUtility.FromJson<StatsPersistenceContainer>(json);

            if (loadedStats != null)
            {
                // Carga la escena guardada
                UnityEngine.SceneManagement.SceneManager.LoadScene(loadedStats.sceneName);


                // Dinero
                MoneyPlayer money = GetComponent<MoneyPlayer>();
                money.Money = loadedStats.money;
                money.MoneyRefined = loadedStats.moneyRefined;

                // Team y Stats
                Data.Instance.CharactersTeamStats.Clear();
                foreach (var characterData in loadedStats.teamCharacterStats)
                {
                    Data.Instance.CharactersTeamStats.Add(characterData);
                }
                Debug.Log("Buscar loadedStats.inventory " + loadedStats.inventory);
                Debug.Log("playerInventory.inventory.Values " + playerInventory.inventory.Values);
                Debug.Log("loadedStats.inventoryDictionary " + loadedStats.inventoryDictionary);

                // Inventario 
                playerInventory.inventory.Clear();

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
                string ruta = Path.Combine(Application.dataPath + "/SaveGame", "newStatsPersistenceData.json");
                loadedStats = LoadController.LoadStats(ruta);
                Debug.Log("Datos cargados exitosamente.");
            }
            else
            {
                Debug.LogWarning("No se pudo cargar el archivo de datos.");
            }
        }
        else
        {
            Debug.Log("No hay datos guardados.");
        }
    }

    // Cargar partida
    public void LoadResetData()
    {
        string filePath = Path.Combine(Application.dataPath + "/SaveGame", "newResetPlay.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            StatsPersistenceContainer loadedStats = JsonUtility.FromJson<StatsPersistenceContainer>(json);

            if (loadedStats != null)
            {
                // Carga la escena guardada
                UnityEngine.SceneManagement.SceneManager.LoadScene(loadedStats.sceneName);


                // Dinero
                MoneyPlayer money = GetComponent<MoneyPlayer>();
                money.Money = loadedStats.money;
                money.MoneyRefined = loadedStats.moneyRefined;

                // Team y Stats
                Data.Instance.CharactersTeamStats.Clear();
                foreach (var characterData in loadedStats.teamCharacterStats)
                {
                    Data.Instance.CharactersTeamStats.Add(characterData);
                }
                Debug.Log("Buscar loadedStats.inventory " + loadedStats.inventory);
                Debug.Log("playerInventory.inventory.Values " + playerInventory.inventory.Values);
                Debug.Log("loadedStats.inventoryDictionary " + loadedStats.inventoryDictionary);

                // Inventario 
                playerInventory.inventory.Clear();

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
                string ruta = Path.Combine(Application.dataPath + "/SaveGame", "newResetPlay.json");
                loadedStats = LoadController.LoadStats(ruta);
                Debug.Log("Datos cargados exitosamente.");
            }
            else
            {
                Debug.LogWarning("No se pudo cargar el archivo de datos.");
            }
        }
        else
        {
            Debug.Log("No hay datos guardados.");
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
