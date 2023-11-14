using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyPlayer : MonoBehaviour
{

    public float money = 1.1f;

    [SerializeField] Text textMoney;

    public float Money { get { return money; } set { this.money = value; } }

    // Start is called before the first frame update
    void Start()
    {
        textMoney.text="Money: "+Money+"€";
    }

    // Update is called once per frame
    void Update()
    {

    }


    //Funcion para de cada en un futuro cuando compremos Items
    public void BuyItems(GameObject Iteam,float wastedMoney)
    {
        money -= wastedMoney;
    }
}
