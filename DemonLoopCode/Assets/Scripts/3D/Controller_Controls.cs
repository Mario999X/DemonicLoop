using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Controls : MonoBehaviour
{
    float Jspeed;

    Player_Move player_move;

    // Start is called before the first frame update
    void Start()
    {
        player_move = GetComponent<Player_Move>();

        Jspeed = player_move.JSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 speedV = player_move.SpeedV;

        // Cuando se pulsa la "A" en el mando el jugador salta.
        if (Input.GetButtonDown("A"))
        {
            speedV.y = 0; speedV.y = Jspeed;

            player_move.SpeedV = speedV;
        }
    }
}
