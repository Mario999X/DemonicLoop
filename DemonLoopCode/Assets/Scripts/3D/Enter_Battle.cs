using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enter_Battle : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject player;
    [SerializeField] Canvas fight;

    bool start;

    public bool Start { set { this.start = value; } }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (start) // Cuando se inicia una escena espera que sea la correcta donde pueda encontrar los objetos "Player" y "Fight".
        {
            player = GameObject.Find("Player");
            fight = GameObject.Find("Fight").GetComponent<Canvas>();
        }
        else if (player != null || fight != null) // Cuando no se encuentre en las escenas correspondientes las vuelven null. 
        {
            player = null; 
            fight = null;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && enemy != null) // Atajo para terminar la batalla.
        {
            FinishBattle();
        }
    }

    // Función para iniciar la batalla.
    public void StartBattle(GameObject enemy/*, bool sneak*/)
    {
        this.enemy = enemy;

        fight.enabled = true;
    }

    // Función para finalizar batalla.
    public void FinishBattle()
    {
        Destroy(enemy);

        player.GetComponent<Player_Move>().enabled = true; // Activa el movimiento del jugador.

        // Si el objeto jugador contiene alguno de los siguientes componentes los activa.
        if (player.GetComponent<KeyBoard_Controls>())
            player.GetComponent<KeyBoard_Controls>().enabled = true;
        if (player.GetComponent<Controller_Controls>())
            player.GetComponent<Controller_Controls>().enabled = true;

        fight.enabled = false;
    }
}
