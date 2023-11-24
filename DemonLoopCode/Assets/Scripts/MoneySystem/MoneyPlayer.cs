using UnityEngine;
using UnityEngine.UI;

public class MoneyPlayer : MonoBehaviour
{

    public float money = 0f;

    [SerializeField] Text textMoney;

    public float Money { get { return money; } set { this.money = value; SetMoneyInText(); } }

    // Start is called before the first frame update
    void Start()
    {
        SetMoneyInText();
    }

    // Update is called once per frame
    void Update()
    {

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
