using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoserReset : MonoBehaviour
{
    [SerializeField] Image imageLose;
    [SerializeField] TextMeshProUGUI textInfo;
    PlayerInventory playerInventory;

    void Start()
    {
        if (imageLose != null) imageLose.GetComponent<Image>().enabled = false;
        if (textInfo != null) textInfo.GetComponent<TextMeshProUGUI>().enabled = false;
        playerInventory = GameObject.Find("System").GetComponent<PlayerInventory>();
    }

    // Cuando entren aqui se mostrara la imagen de que han perdido
    // y hara un reinicio de los datos a el inicial
    public IEnumerator ShowImage()
    {
        if (imageLose != null) imageLose.GetComponent<Image>().enabled = true;
        if (textInfo != null) textInfo.GetComponent<TextMeshProUGUI>().enabled = true;

        // Cuando morimos se reinicia los StatsPersistenceData
        // al que tenia cuando se inicio el juego
        // Remueve todos los aliados del jugador.
        Data.Instance.CharactersTeamStats.RemoveAll(x => !x.Protagonist);
        Data.Instance.CharactersBackupStats.RemoveAll(x => !x.Protagonist);

        // Resetea el nivel de jugador a 0.
        Data.Instance.CharactersTeamStats.ForEach(x => x.Level = 1);

        // Vacia el inventario del jugador.
        if (playerInventory != null)
        {
            playerInventory.ResetInventario();
        }

        // Reinicia los datos de generacion de salas.
        Data.Instance.BossRoom = 4;
        Data.Instance.Room = 0;
        Data.Instance.SaveRoom = 0;
        Data.Instance.Floor = 0;

        GameObject.Find("System").GetComponent<SaveSystem>().SaveData("Shop", false);

        yield return new WaitForSeconds(3);

        if (textInfo != null) SceneManager.Instance.LoadSceneName("Title");
    }
}