using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoneyPlayer : MonoBehaviour
{
    [SerializeField] public float mara;
    [SerializeField] public float maraRefined;//Dinero que solo te lo da al ganar aun jefe

    [SerializeField] List<TextMeshProUGUI> textMoney;
    [SerializeField] List<TextMeshProUGUI> textMoneyRefined;

    public float Money { get { return mara; } set { mara = value; SetMoneyInText(); } }
    public float MoneyRefined { get { return maraRefined; } set { maraRefined = value; SetMoneyRefinedInText(); } }
    bool done = false;

    Scene scene;

    // Start is called before the first frame update
    void Update()
    {
        if (scene != UnityEngine.SceneManagement.SceneManager.GetActiveScene())
        {
            done = false;
            scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        }

        if (!done && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Title")
        {
            GameObject[] textObject = GameObject.FindGameObjectsWithTag("Money");
            GameObject[] textObjectRefined = GameObject.FindGameObjectsWithTag("MoneyRefined");

            foreach (GameObject text in textObject)
                textMoney.Add(text.GetComponent<TextMeshProUGUI>());

            textMoney.RemoveAll(text => text.IsUnityNull());
            SetMoneyInText();

            foreach (GameObject textRefined in textObjectRefined)
                textMoneyRefined.Add(textRefined.GetComponent<TextMeshProUGUI>());

            textMoneyRefined.RemoveAll(textRefined => textRefined.IsUnityNull());
            SetMoneyRefinedInText();

            done = true;
        }
    }

    public void SetMoneyInText()
    {
        if (textMoney != null)
        {
            foreach (TextMeshProUGUI text in textMoney)
            text.text = $"Mara: {mara}";
        }
    }

    public void SetMoneyRefinedInText()
    {
        foreach (TextMeshProUGUI text in textMoneyRefined)
            text.text = $"Mara Refined: {maraRefined}";
    }
}
