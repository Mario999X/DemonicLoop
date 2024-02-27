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
    TextMeshProUGUI tipsLoad;
    List<string> tipsList=new List<string>();

    void Awake()
    {
        // Primer GetChild(0) = LoadingScreen,
        // Segundo GetChild(0) = Fondo
        // Tercer GetChild(0) = Text
        loadingScreen = GameObject.Find("System").transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        tipsLoad = loadingScreen.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        Debug.Log("tipsLoad " + tipsLoad.name);
        // Directorio donde esta el Csv

        LoadTipsFile();
        UpdateTextTips();


    }



    public void LoadTipsFile()
    {
        string fileName = "Tips.csv";
        string folderName = "Csv";
        string folderPath = Path.Combine(Application.persistentDataPath, folderName);
        Debug.Log("folderPath " + folderPath);
        string filePath = Path.Combine(folderPath, fileName);
        Debug.Log("filePath " + filePath);

        StreamReader fileRead = new StreamReader(filePath);
        string separador = ",";
        string line;
        fileRead.ReadLine();

        while ((line = fileRead.ReadLine()) != null)
        {
            string[] row = line.Split(separador);
            string text = row[0];
            Debug.Log("text " + text);
            tipsList.Add(text);
        }

        //fileRead.Close();
    }

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
