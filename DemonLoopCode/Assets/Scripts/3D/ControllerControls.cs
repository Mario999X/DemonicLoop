using UnityEngine;

public class ControllerControls : MonoBehaviour
{
    float Jspeed;

    PlayerMove player_move;

    // Start is called before the first frame update
    void Start()
    {
        player_move = GetComponent<PlayerMove>();

        Jspeed = player_move.JSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 speedV = player_move.SpeedV;

        // Cuando se pulsa la "A" en el controller el jugador salta.
        if (Input.GetButton("A") && player_move.OnFloor)
        {
            speedV.y = 0; speedV.y = Jspeed;

            player_move.SpeedV = speedV;
        }
    }
}
