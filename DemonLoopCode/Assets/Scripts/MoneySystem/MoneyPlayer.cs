using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoneyPlayer : MonoBehaviour
{
    [SerializeField] private float mara;

    [SerializeField] List<TextMeshProUGUI> textMoney;

    public float Money { get { return mara; } set { mara = value; SetMoneyInText(); } }

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

            foreach (GameObject text in textObject)
                textMoney.Add(text.GetComponent<TextMeshProUGUI>());

            textMoney.RemoveAll(text => text.IsUnityNull());
            SetMoneyInText();

            done = true;
        }
    }

    private void SetMoneyInText()
    {
        foreach (TextMeshProUGUI text in textMoney)
            text.text = $"Mara: {mara}";
    }
}
