using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;

    [Header("Componentes de la pantalla de carga")]
    [SerializeField] private Slider slider;
    [SerializeField] private Canvas _loadingCanvas;
    SaveSystem SaveSystem;

    bool done = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SaveSystem = GetComponent<SaveSystem>();
        _loadingCanvas.enabled = false;
    }

    // Carga la escena por el numero de posicion.
    public async void LoadScene(int scene)
    {
        if (slider.value != 0)
            slider.value = 0;

        AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene);
        operation.allowSceneActivation = false; // Impide que se active la escena.

        _loadingCanvas.enabled = true;

        float progressValue;

        // Muestra por pantalla el progreso de carga de escena.
        do
        {
            progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            while (slider.value != progressValue) 
            { 
                await System.Threading.Tasks.Task.Delay(1); 
                slider.value = Mathf.MoveTowards(slider.value, progressValue, 0.5f * Time.deltaTime); 
            }
        } while (progressValue < 0.9f);

        operation.allowSceneActivation = true; // Permite que se active la escena.

        await System.Threading.Tasks.Task.Delay(1000);

        _loadingCanvas.enabled = false;
    }

    // Carga la escena por su nombre.
    public async void LoadSceneName(string scene)
    {
        if (slider.value != 0)
            slider.value = 0;

        AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene);
        operation.allowSceneActivation = false; // Impide que se active la escena.

        _loadingCanvas.enabled = true;

        float progressValue;

        // Muestra por pantalla el progreso de carga de escena.
        do
        {
            progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            while (slider.value != progressValue)
            {
                await System.Threading.Tasks.Task.Delay(5);
                slider.value = Mathf.MoveTowards(slider.value, progressValue, 0.5f * Time.deltaTime);
            }
        } while (progressValue < 0.9f);

        operation.allowSceneActivation = true; // Permite que se active la escena.

        await System.Threading.Tasks.Task.Delay(1000);

        _loadingCanvas.enabled = false;
    }

    // Cierra la aplicación.
    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowSettingsView()
    {
        GameObject.Find("SettingsView").GetComponent<Canvas>().enabled = true;
        Time.timeScale = 0f;
    }

    void Update()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Title" && !done)
        {
            SaveSystem.LoadData();
            GameObject play = GameObject.Find("Start");

            if (!Data.Instance.OnRun)
            {
                play.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "New Game";
                play.GetComponent<Button>().onClick.AddListener(() => { LoadSceneName("Shop"); });
            }
            else
            {
                play.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Continue";
                play.GetComponent<Button>().onClick.AddListener(() => { LoadSceneName(Data.Instance.SceneName); });
            }

            GameObject.Find("Exit").GetComponent<Button>().onClick.AddListener(() => { ExitGame(); });
            GameObject.Find("Settings").GetComponent<Button>().onClick.AddListener(() => { ShowSettingsView(); });
            GameObject.Find("AppVerText").GetComponent<TextMeshProUGUI>().text = "App ver. " + Application.version;
            Debug.Log("Done");

            done = true;
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Title" && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "VideoScene")
        {
            GameObject.Find("SettingsBtn").GetComponent<Button>().onClick.AddListener(() => ShowSettingsView());
            done = false;
        }
    }
}
