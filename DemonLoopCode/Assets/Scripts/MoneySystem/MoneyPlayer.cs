using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoneyPlayer : MonoBehaviour
{
    [SerializeField] float money = 0f;

    [SerializeField] Text textMoney;

    public float Money { get { return money; } set { money = value; SetMoneyInText(); } }

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

        if (!done && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Title" && !done)
        {
            textMoney = GameObject.Find("TextMoney").GetComponent<Text>();
            SetMoneyInText();
            done = true;
        }
    }

    private void SetMoneyInText()
    {
        textMoney.text= "Mara: " + Money;
    }
}
