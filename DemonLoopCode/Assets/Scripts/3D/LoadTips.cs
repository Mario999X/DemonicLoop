using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class LoadTips : MonoBehaviour
{
    private bool done;
    
    GameObject loadingScreen;
    
    private TextMeshProUGUI tipsLoad;
    
    List<string> tipsList = new();

    void Awake()
    {
        done = false;
        // [Primer GetChild(0) = LoadingScreen]
        loadingScreen = GameObject.Find("System").transform.GetChild(0).gameObject;
        // [Primer GetChild(0) = Fondo] y [Segundo GetChild(1) = Text Tips]
        tipsLoad = loadingScreen.transform.GetChild(0).transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        //Debug.Log("tipsLoad " + tipsLoad.name);

        LoadTipsFile();
    }

    // Vamos a guardar todo lo que hay en la ruta del csv en tipsList
    private void LoadTipsFile()
    {
        // importante para usar Application.streamingAssetsPath tenemos que 
        // crear la carpeta antes en Assets
        string filePath = Path.Combine(Application.streamingAssetsPath, "Csv/Tips.csv");
        string[] lines = File.ReadAllLines(filePath);

        foreach (string line in lines)
            tipsList.Add(line);
    }

    private void FixedUpdate()
    {
        // Si el canvas de pantalla de cargado a sido activada este mostrara un tip aleatorio.
        if(loadingScreen.GetComponent<Canvas>().enabled && done == false)
        {
            UpdateTextTips();
            done = true;
        } else if(!loadingScreen.GetComponent<Canvas>().enabled)
        {
            done = false;
        }
    }

    // Pillara una de las lineas del tipsList de forma aleatoria y lo muestra en el canvas de carga.
    private void UpdateTextTips()
    {
        if (tipsList.Count > 0)
            tipsLoad.text = tipsList[Random.Range(0, tipsList.Count)];
    }
}
