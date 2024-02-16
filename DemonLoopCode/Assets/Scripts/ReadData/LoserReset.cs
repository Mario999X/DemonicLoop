using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoserReset : MonoBehaviour
{

    [SerializeField] Image imageLose;
    MoneyPlayer moneyPlayer;
    PlayerInventory playerInventory;
    StatsPersistenceData[] playerCharacters;

    void Start()
    {
        imageLose.GetComponent<Image>().enabled = false;
        moneyPlayer = GameObject.Find("System").GetComponent<MoneyPlayer>();
        playerInventory = GameObject.Find("System").GetComponent<PlayerInventory>();
        playerCharacters = Data.Instance.CharactersTeamStats.ToArray();
    }

    public IEnumerator ShowImage()
    {
        imageLose.GetComponent<Image>().enabled = true;


        ResetPersistence();

        ResetInventory();

        ResetMoney();
        ResetTeam();
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
        moneyPlayer.mara = 100f;
        moneyPlayer.maraRefined = 100f;
    }

    public void ResetTeam()
    {
        foreach (StatsPersistenceData member in playerCharacters)
        {
            if (member.Protagonist!=true)
            {
                Data.Instance.CharactersTeamStats.Remove(member);

            }

        }

    }


}