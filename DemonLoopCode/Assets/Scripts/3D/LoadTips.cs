using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadTips : MonoBehaviour
{
    GameObject loadingScreen;
    public TextMeshProUGUI tipsLoad;
    List<string> tipsList = new List<string>();

    void Awake()
    {
        // [Primer GetChild(0) = LoadingScreen], [Segundo GetChild(0) = Fondo] y [Tercer GetChild(1) = Text Tips]
        loadingScreen = GameObject.Find("System").transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        tipsLoad = loadingScreen.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        Debug.Log("tipsLoad " + tipsLoad.name);
        LoadTipsFile();
        UpdateTextTips();
    }

    // Vamos a guardar todo lo que hay en la ruta del csv en tipsList
    public void LoadTipsFile()
    {
        // importante para usar Application.streamingAssetsPath tenemos que 
        // crear la carpeta antes en Assets
        string filePath = Path.Combine(Application.streamingAssetsPath, "Csv/Tips.csv");
        string[] lines = File.ReadAllLines(filePath);

        foreach (string line in lines)
        {
            Debug.Log(line);
            tipsList.Add(line);
        }

        UpdateTextTips();
    }

    // Pillara una de las lineas del tipsList de forma aleatoria y lo muestra
    public void UpdateTextTips()
    {
        if (tipsList.Count > 0)
        {
            string random = tipsList[UnityEngine.Random.Range(0, tipsList.Count)];
            tipsLoad.text = random;
        }
        else
        {
            Debug.Log("loadingScreen " + loadingScreen.name);
        }
    }
}
