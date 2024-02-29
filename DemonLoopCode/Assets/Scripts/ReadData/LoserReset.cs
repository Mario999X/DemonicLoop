using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoserReset : SaveSystem
{
    [SerializeField] Image imageLose;
    PlayerInventory playerInventory;

    void Start()
    {
        imageLose.GetComponent<Image>().enabled = false;
        playerInventory = GameObject.Find("System").GetComponent<PlayerInventory>();
    }

    // Cuando entren aqui se mostrara la imagen de que han perdido
    // y hara un reinicio de los datos a el inicial
    public IEnumerator ShowImage()
    {
        imageLose.GetComponent<Image>().enabled = true;

        bool saveRoom = false;

        if (Data.Instance.Room > 4) saveRoom = true;

        // Cuando morimos se reinicia los StatsPersistenceData
        // al que tenia cuando se inicio el juego
        // Remueve todos los aliados del jugador.
        Data.Instance.CharactersTeamStats.RemoveAll(x => !x.Protagonist);
        Data.Instance.CharactersBackupStats.RemoveAll(x => !x.Protagonist);

        // Resetea el nivel de jugador a 0.
        Data.Instance.CharactersTeamStats.ForEach(x => x.Level = 0);

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

         SaveData("Shop", false);

        yield return new WaitForSeconds(3);

        SceneManager.Instance.LoadScene(0);
    }
}