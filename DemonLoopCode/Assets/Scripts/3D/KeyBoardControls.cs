using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KeyBoardControls : MonoBehaviour
{
    float Jspeed;

    PlayerMove player_move;
    PlayerInventory player_inventory;
    EnterBattle enterBattle;

    // Start is called before the first frame update
    void Start()
    {
        player_move = GetComponent<PlayerMove>();
        player_inventory = GameObject.Find("System").GetComponent<PlayerInventory>();
        enterBattle = GameObject.Find("System").GetComponent<EnterBattle>();

        Jspeed = player_move.JSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 speedV = player_move.SpeedV;

        // Cuando se pulsa el espacio el jugador salta.
        if (Input.GetKey(KeyCode.Space) && player_move.OnFloor)
        {
            speedV.y = 0; speedV.y = Jspeed;

            player_move.SpeedV = speedV;
        }

        // Abrir y cerrar el inventario solo cuando el jugador no se encuentre en batalla.
        if (Input.GetKeyDown(KeyCode.Escape) && !enterBattle.OneTime)
            player_inventory.OpenCloseInventoyry();
    }
}
