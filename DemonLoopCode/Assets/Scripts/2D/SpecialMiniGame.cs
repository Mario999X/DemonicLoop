using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpecialMiniGame : MonoBehaviour
{
    private Image barSpecial;

    private int countPress;
    private int totalPress;

    [SerializeField] int timeInitial;
    private int timeLeft;
    private float increaseBar = 0f;

    TextMeshProUGUI textTime;
    Scene scene;
    bool done = false;
    
    public float IncreaseBar { get { return increaseBar; } set { increaseBar = value; } }
    public int TotalPress { get { return totalPress; }}


    private void Update()
    {
        if (scene != UnityEngine.SceneManagement.SceneManager.GetActiveScene())
        {
            done = false;
            scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        }

        if (!done && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Scene 2")
        {
            barSpecial = GameObject.Find("PanelMiniGame").transform.GetChild(0).GetComponent<Image>();
            textTime = GameObject.Find("PanelMiniGame").transform.GetChild(1).GetComponent<TextMeshProUGUI>();
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
            if (textTime != null) textTime.text = string.Format("{00:00}:{01:00}", timeLeft / 60, timeLeft % 60);

            yield return null;

            // Se ira subiendo la barra por cada vez que pulsemos la tecla
            if (Input.GetKeyDown(KeyCode.N))
            {
                totalPress = ++countPress;

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
}