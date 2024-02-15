using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class LoserReset : MonoBehaviour
{

    [SerializeField] Image imageLose;
    GameObject[] players;
    MoneyPlayer moneyPlayer;
    PlayerInventory playerInventory;


    List<ObjectStock> initialObject;

    void Start()
    {
        imageLose.GetComponent<Image>().enabled = false;
        moneyPlayer = GetComponent<MoneyPlayer>();
        playerInventory = GameObject.Find("System").GetComponent<PlayerInventory>();


        initialObject = new List<ObjectStock>();
    }

    public IEnumerator ShowImage()
    {
        imageLose.GetComponent<Image>().enabled = true;


        ResetPersistence();

        ResetInventory();

       // ResetMoney();

        yield return new WaitForSeconds(3);


        MoveSceneInitial();
    }

    public void MoveSceneInitial()
    {
        // ID 0 es el titulo
        // ID 1 es el Scene 2
        // ID 3 es el Shop
        SceneManager.Instance.LoadScene(0);
    }

    public void ResetPersistence()
    {
        // Cuando morimos se reinicia los StatsPersistenceData
        // al que tenia cuando se inicio el juego
        for (var i = 0; i < Data.Instance.CharactersTeamStats.Count; i++)
        {
            Data.Instance.CharactersTeamStats[i].Level = 0;
        }

    }

    public void ResetInventory()
    {
        if (playerInventory != null)
        {
            playerInventory.ResetInventario();

        }
    }

    public void ResetMoney()
    {
        moneyPlayer.Money = 100f;
        moneyPlayer.MoneyRefined = 100f;
    }


}