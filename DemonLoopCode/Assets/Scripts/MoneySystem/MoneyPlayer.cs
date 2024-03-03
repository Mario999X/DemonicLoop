using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoneyPlayer : MonoBehaviour
{
    [SerializeField] float mara;
    [SerializeField] float maraRefined; //Dinero que solo te lo da al ganar aun jefe

    [SerializeField] List<TextMeshProUGUI> textMoney;
    [SerializeField] List<TextMeshProUGUI> textMoneyRefined;

    public float Money { get { return mara; } set { mara = value; SetMoneyInText(); } }
    public float MoneyRefined { get { return maraRefined; } set { maraRefined = value; SetMoneyRefinedInText(); } }
    
    bool done = false;

    Scene scene;

    void Update()
    {
        // Cuando se cambia de escena se actualiza a se registra la actual.
        if (scene != UnityEngine.SceneManagement.SceneManager.GetActiveScene())
        {
            done = false;
            scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        }

        // Una vez detectado que esta en la escena correcta y que no es la misma este actualiza a los nuevos datos.
        if (!done && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Title")
        {
            GameObject[] textObject = GameObject.FindGameObjectsWithTag("Money");
            GameObject[] textObjectRefined = GameObject.FindGameObjectsWithTag("MoneyRefined");

            // Coge todos los textos que pertenezcan como indicador visual de la cantidad de dinero que tiene.
            foreach (GameObject text in textObject)
                textMoney.Add(text.GetComponent<TextMeshProUGUI>());

            textMoney.RemoveAll(text => text.IsUnityNull()); // Por si acaso metio en la lista removemos todos los nulos.
            SetMoneyInText(); // Actualiza el texto.

            // Coge todos los textos que pertenezcan como indicador visual de la cantidad de dinero especial que tiene.
            foreach (GameObject textRefined in textObjectRefined)
                textMoneyRefined.Add(textRefined.GetComponent<TextMeshProUGUI>());

            textMoneyRefined.RemoveAll(textRefined => textRefined.IsUnityNull()); // Por si acaso metio en la lista removemos todos los nulos.
            SetMoneyRefinedInText(); // Actualiza el texto.

            done = true;
        }
    }

    // Muestra el dinero de forma visual al jugador.
    public void SetMoneyInText()
    {
        if (textMoney != null)
            foreach (TextMeshProUGUI text in textMoney)
                text.text = $"Mara: {mara}";
    }

    // Muestra el dinero especial de forma visual al jugador.
    public void SetMoneyRefinedInText()
    {
        if (textMoneyRefined != null)
            foreach (TextMeshProUGUI text in textMoneyRefined)
                text.text = $"Mara Refined: {maraRefined}";
    }
}
