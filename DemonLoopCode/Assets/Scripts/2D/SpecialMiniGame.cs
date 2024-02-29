using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpecialMiniGame : MonoBehaviour
{
    public Image barSpecial;

    int countPress;
    int totalPress;

    [SerializeField] int timeInitial;
    int timeLeft;
    float increaseBar = 0f;

    TextMeshProUGUI textTime;
    TextMeshProUGUI textInf;
    Scene scene;
    bool done = false;
    public float IncreaseBar { get { return increaseBar; } set { increaseBar = value; } }


    private void Update()
    {
        if (scene != UnityEngine.SceneManagement.SceneManager.GetActiveScene())
        {
            done = false;
            scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        }

        if (!done && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Scene 2")
        {
            Debug.Log("Vuelta a cargar los componentes.");

            barSpecial = GameObject.Find("PanelMiniGame").transform.GetChild(0).GetComponent<Image>();
            textTime = GameObject.Find("PanelMiniGame").transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            textInf = GameObject.Find("PanelMiniGame").transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            timeLeft = timeInitial;
            countPress = 0;

            done = true;
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Scene 2")
        {
            done = false;
        }
    }

    public IEnumerator ChargeSpecialAtk()
    {
        Clear();
        timeLeft = timeInitial;

        while (timeLeft > 0)
        {
            timeLeft -= Mathf.CeilToInt(Time.deltaTime);
            ShowTime();
            yield return null;

            // Se ira subiendo la barra por cada vez que pulsemos la tecla
            if (Input.GetKeyDown(KeyCode.N))
            {
                totalPress = ++countPress;
                Debug.Log("countPress " + countPress);
                if (barSpecial != null)
                {
                    increaseBar += 0.01f;
                    barSpecial.fillAmount = increaseBar;

                    barSpecial.fillAmount = Mathf.Clamp01(barSpecial.fillAmount);

                    if (increaseBar > 1f)
                    {
                        timeLeft = 0;
                        barSpecial.fillAmount = 0f;
                        Mathf.Clamp01(barSpecial.fillAmount);
                    }
                }
            }
        }

    }

    public void Clear()
    {
        totalPress = 0;
        countPress = 0;
        increaseBar = 0.01f;
        barSpecial.fillAmount = 0f;
        Mathf.Clamp01(barSpecial.fillAmount);
    }


    //Mostramos el tiempo que le queda en un formato mas "bonito"
    private void ShowTime()
    {
        if (textTime != null)
        {
            textTime.text = string.Format("{00:00}:{01:00}", timeLeft / 60, timeLeft % 60);
        }
    }


}