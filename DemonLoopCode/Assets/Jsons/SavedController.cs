using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

[System.Serializable]
public class ObjectStockData
{
    public string name;
    public int count;

    public ObjectStockData(string name, int count)
    {
        this.name = name;
        this.count = count;
    }
}

[System.Serializable]
public class StatsPersistenceContainer
{
    public GameObject player;
    public int playerLevel;
    public float playerExperience;
    public float playerHealth;
    public float playerMana;
    public float playerAttack;
    public float playerDefense;
    public List<ObjectStockData> inventory;
    public float money;
    public float moneyRefined;
    public string sceneName;

    public StatsPersistenceContainer(string sceneName)
    {
        this.sceneName = sceneName;
        inventory = new List<ObjectStockData>();
    }
}


public class SavedController : MonoBehaviour
{
    public GameObject player;
    MoneyPlayer money;

    private void Start()
    {
        player = GetComponent<GameObject>();
        money = GetComponent<MoneyPlayer>();
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

        //statsContainer.playerLevel = 10;
        //statsContainer.playerExperience = 1000;
        Debug.Log("savedController.sceneName " + savedController.sceneName);
       

        savedController.money = money.Money;
        savedController.moneyRefined = money.MoneyRefined;

        // Directorio donde se guarda
        string saveDirectory = Path.Combine(Application.dataPath + "/SaveGame");
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }
        //Vector3 positionPlayer = savedController.player.transform.position;

           // PlayerPrefs.SetFloat("PlayerPosX", savedController.player.transform.position.x);
           // PlayerPrefs.SetFloat("PlayerPosY", savedController.player.transform.position.y);
           // PlayerPrefs.SetFloat("PlayerPosZ", savedController.player.transform.position.z);

            PlayerPrefs.SetString("SavedSceneName", savedController.sceneName);
            PlayerPrefs.Save();
        // Ruta  del archivo
        string filePath = Path.Combine(saveDirectory, "newStatsPersistenceData.json");

        // Convierte el objeto statsContainer a JSON y lo guardas en un archivo
        string json = JsonUtility.ToJson(savedController);
        File.WriteAllText(filePath, json);

        Debug.Log("Ruta guardado" + filePath);
    }






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
                MoneyPlayer money=GetComponent<MoneyPlayer>();
                money.Money = loadedStats.money;
                money.MoneyRefined = loadedStats.moneyRefined;

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
