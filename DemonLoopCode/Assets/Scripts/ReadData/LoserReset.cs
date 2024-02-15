using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class LoserReset : MonoBehaviour
{

    [SerializeField] Image imageLose;
    GameObject[] players;

    PlayerInventory playerInventory;


    List<ObjectStock> initialObject;

    void Start()
    {
        imageLose.GetComponent<Image>().enabled = false;

        playerInventory = GetComponent<PlayerInventory>();


        initialObject = new List<ObjectStock>();
    }

    public IEnumerator ShowImage()
    {
        imageLose.GetComponent<Image>().enabled = true;


        ResetPersistence();

        ResetInventory();
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
        //playerInventory.inventory.Clear();

        foreach (var initialObj in initialObject)
        {
            ObjectStock resetInitialStock = new ObjectStock(initialObj.Data, 0);

            initialObject.Add(resetInitialStock);
        }

        /* foreach (ObjectStock item in initialObject)
         {
             playerInventory.EliminateINVButtons();
             Destroy(item.ButtonINV3D);
             playerInventory.inventory.Remove(item.Data.name.ToUpper());
         }

         GameObject.Find("Inventory").GetComponentInParent<Canvas>().enabled = false;

         GameObject.Find("Inventory").transform.GetChild(1).gameObject.SetActive(false);*/
    }


}