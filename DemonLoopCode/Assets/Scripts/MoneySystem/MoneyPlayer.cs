using UnityEngine;
using UnityEngine.UI;

public class MoneyPlayer : MonoBehaviour
{

    public float money = 0f;

    [SerializeField] Text textMoney;

    public float Money { get { return money; } set { this.money = value; SetMoneyInText(); } }

    bool done = false;

    // Start is called before the first frame update
    void Update()
    {
        if (!done && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Scene 2" && !done)
        {
            textMoney = GameObject.Find("TextMoney").GetComponent<Text>();
            SetMoneyInText();
            done = true;
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Scene 2")
        {
            done = false;
        }
    }

    private void SetMoneyInText()
    {
        textMoney.text="Money: "+ Money + " Mara";
    }


    //Funcion para de cada en un futuro cuando compremos Items
    public void BuyItems(GameObject Iteam,float wastedMoney)
    {
        money -= wastedMoney;
    }
}
