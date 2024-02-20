using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;

    [Header("Componentes de la pantalla de carga")]
    [SerializeField] private Slider slider;
    [SerializeField] private Canvas _loadingCanvas;
    SavedController savedController;

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
        savedController = GetComponent<SavedController>();
        _loadingCanvas.enabled = false;
    }

    // Carga la escena por el n�mero de posici�n.
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
    }

    void FixedUpdate()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Title" && !done)
        {
            GameObject.Find("Start").GetComponent<Button>().onClick.AddListener(() => { LoadScene(2); });
            GameObject.Find("Exit").GetComponent<Button>().onClick.AddListener(() => { ExitGame(); });
            GameObject.Find("Settings").GetComponent<Button>().onClick.AddListener(() => { ShowSettingsView(); });
            GameObject.Find("Continue").GetComponent<Button>().onClick.AddListener(() => { savedController.LoadData(); });
            GameObject.Find("AppVerText").GetComponent<TextMeshProUGUI>().text = "App ver. " + Application.version;
            Debug.Log("Done");

            done = true;
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Title")
        {
            done = false;
        }
    }
}
