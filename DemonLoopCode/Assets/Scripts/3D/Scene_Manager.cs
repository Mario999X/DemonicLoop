using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene_Manager : MonoBehaviour
{
    public static Scene_Manager Instance;

    [Header("Componentes de la pantalla de carga")]
    [SerializeField] private Slider slider;
    [SerializeField] private Canvas _loadingCanvas;

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
            _loadingCanvas.enabled = false;
    }

    // Carga la escena por el número de posición.
    public async void LoadScene(int scene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
        operation.allowSceneActivation = false; // Impide que se active la escena.

        _loadingCanvas.enabled = true;

        float progressValue;

        // Muestra por pantalla el progreso de carga de escena.
        do
        {
            await System.Threading.Tasks.Task.Delay(100);

            progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            slider.value = Mathf.MoveTowards(slider.value, progressValue, 3 * Time.deltaTime);
        } while (operation.progress < 0.9f);

        slider.value = 1;

        operation.allowSceneActivation = true; // Permite que se active la escena.

        await System.Threading.Tasks.Task.Delay(1000);

        _loadingCanvas.enabled = false;
    }

    // Carga la escena por su nombre.
    public async void LoadSceneName(string scene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
        operation.allowSceneActivation = false; // Impide que se active la escena.

        _loadingCanvas.enabled = true;

        float progressValue;

        // Muestra por pantalla el progreso de carga de escena.
        do
        {
            await System.Threading.Tasks.Task.Delay(100);

            progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            slider.value = Mathf.MoveTowards(slider.value, progressValue, 3 * Time.deltaTime);
        } while (operation.progress < 0.9f);

        slider.value = 1;

        operation.allowSceneActivation = true; // Permite que se active la escena.

        await System.Threading.Tasks.Task.Delay(1000);

        _loadingCanvas.enabled = false;
    }

    void FixedUpdate()
    {
        // En el caso de no ser "Scene 1" activa el componente "Enter_Battle".
        if(SceneManager.GetActiveScene().name != "Scene 1")
        {
            GetComponent<Enter_Battle>().Start = true;
            GetComponent<Enter_Battle>().enabled = true;
        }    
        else 
        {
            GetComponent<Enter_Battle>().Start = false;
            GetComponent<Enter_Battle>().enabled = false;
        }
    }
}
