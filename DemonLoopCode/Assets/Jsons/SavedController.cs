using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using static UnityEditor.Progress;
using TMPro;

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
    /* public int playerLevel;
     public float playerExperience;
     public float playerHealth;
     public float playerMana;
     public float playerAttack;
     public float playerDefense;*/
    public List<ObjectStockData> inventory;
    public float money;
    public float moneyRefined;
    public string sceneName;
    public List<StatsPersistenceData> teamCharacterStats;
    //public List <ObjectData> dataList;
    public Dictionary<string, ObjectData> inventoryDictionary;
    public ScriptableObject dataScriptableObject;


    public StatsPersistenceContainer(string sceneName)
    {
        this.sceneName = sceneName;
        inventory = new List<ObjectStockData>();

        teamCharacterStats = new List<StatsPersistenceData>();
        inventoryDictionary = new Dictionary<string, ObjectData>();
        dataScriptableObject = ScriptableObject.CreateInstance<ScriptableObject>();
        // dataList = new List<ObjectData>();

    }
}


public class SavedController : MonoBehaviour
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
            Debug.LogError("No se encontró el componente PlayerInventory. Asegúrate de que esté adjunto al mismo GameObject que SavedController.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            SaveData();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            LoadData();
        }
    }

    public void SaveData()
    {
        StatsPersistenceContainer savedController = new StatsPersistenceContainer(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);


        Debug.Log("savedController.sceneName " + savedController.sceneName);

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
           // playerInventory.AddObjectToInventory(obj.Data.name, savedController.dataScriptableObject as ObjectData, obj.Count);

            //playerInventory.inventory.Add(obj.Data.name,new ObjectStock(savedController.dataScriptableObject as ObjectData, obj.Count));

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

                // Cargamos los datos del statsContainer en tu juego

                // Dinero
                MoneyPlayer money = GetComponent<MoneyPlayer>();
                money.Money = loadedStats.money;
                money.MoneyRefined = loadedStats.moneyRefined;

                // Team y Stats
                Data.Instance.CharactersTeamStats.Clear();
                Data.Instance.CharactersTeamStats.AddRange(Data.Instance.CharactersTeamStats);

                Debug.Log("Buscar loadedStats.inventory " + loadedStats.inventory);
                Debug.Log("playerInventory.inventory.Values " + playerInventory.inventory.Values);
                Debug.Log("loadedStats.inventoryDictionary " + loadedStats.inventoryDictionary);

                // Inventario 
                playerInventory.inventory.Clear();

                /*if (loadedStats.inventoryDictionary !=null)
                {
                    loadedStats.inventoryDictionary.Clear();
                }*/

                ObjectData[] objects = Resources.LoadAll<ObjectData>("Data/Objects");

                /* foreach (ObjectData obj in objects)
                 {
                     Debug.Log("cargando Objeto " + obj.name);
                     string objName = obj.name.Substring(4, obj.name.Length - 4).Replace("^", " ").ToUpper();

                     Debug.Log("loadedStats.inventory " + loadedStats.inventory);

                     //loadedStats.inventory.Add(new ObjectStockData(obj, loadedStats.inventory.Count));
                     //loadedStats.inventoryDictionary.Add(obj.name, obj);

                 }*/
                foreach (var item in loadedStats.inventory)
                {
                    playerInventory.AddObjectToInventory(playerInventory.name,item.objData ,item.count);
                }

                if (loadedStats.inventoryDictionary != null)
                {
                    foreach (var objName in loadedStats.inventoryDictionary)
                    {
                        var realObjName = objName.Key.Substring(4, objName.Key.Length - 4).ToUpper();
                        playerInventory.AddObjectToInventory(realObjName, objName.Value, loadedStats.inventoryDictionary.Count);
                    }
                }


                /*foreach (var objName in loadedStats.inventory)
                {
                    var realObjName = objName.objData.name.Substring(4, objName.objData.name.Length - 4).ToUpper();
                    playerInventory.AddObjectToInventory(realObjName, loadedStats.dataScriptableObject as ObjectData, objName.count);
                    foreach (string objDictionary in loadedStats.inventoryDictionary.Keys)
                    {
                        
                        if (realObjName.ToUpper() == objDictionary.ToUpper())
                        {
                            Debug.Log("Se esta añadiendo objeto "+ realObjName);
                            //playerInventory.AddObjectToInventory(realObjName, loadedStats.dataScriptableObject as ObjectData, objName.count);
                            //playerInventory.AddObjectToInventory(realObjName, loadedStats.inventoryDictionary[realObjName], objName.count);
                           
                        }
                    }
                }*/


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
