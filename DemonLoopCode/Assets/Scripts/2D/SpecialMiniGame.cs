using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class SpecialMiniGame : MonoBehaviour
{
    [SerializeField] private Image barSpecial;

    int countPress;
    int totalPress;

    [SerializeField] int timeInitial;
    int timeLeft;
    float increaseBar=0.1f;

    Text textTime;

    public void Start()
    {
        timeLeft = timeInitial;
        countPress = 0;
       
    }

    public IEnumerator ChargeSpecialAtk()
    {
        while (timeLeft > 0)
        {
            timeLeft -= Mathf.CeilToInt(Time.deltaTime);
            ShowTime();
            yield return null;
        
            // Se ira subiendo la barra por cada vez que pulsemos la tecla
            if (Input.GetKeyDown(KeyCode.N))
            {
                totalPress=++countPress;
                Debug.Log("Contador de veces pulsada "+countPress);
                Debug.Log("1ºTotal de veces pulsada " + totalPress);
                if (barSpecial!=null)
                {
                    barSpecial.fillAmount += increaseBar;
                    //Esto esta para que la barrera no pase sus limites
                    barSpecial.fillAmount = Mathf.Clamp01(barSpecial.fillAmount);
                }

            }
        }
        Debug.Log("2ºTotal de veces pulsada " + totalPress);
        Debug.Log("Contador de veces pulsada " + countPress);
    }


    //Mostramos el tiempo que le queda en un formato mas "bonito"
    private void ShowTime()
    {
        if (textTime!=null)
        {
            textTime.text = string.Format("{00:00}:{01:00}", timeLeft / 60, timeLeft % 60);
        }
    }


}
