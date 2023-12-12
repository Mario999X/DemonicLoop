using System.Collections;
using UnityEngine;

public class EnterBattle : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject player;
    [SerializeField] Canvas fight;
    Animator crossfadeTransition;

    bool done;
    bool oneTime = false;

    public bool OneTime { get { return oneTime; } }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!done && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Scene 2") // Cuando se inicia una escena espera que sea la correcta donde pueda encontrar los objetos "Player" y "Fight".
        {
            crossfadeTransition = GameObject.Find("Crossfade").GetComponent<Animator>();

            player = GameObject.Find("Player");
            fight = GameObject.Find("Fight").GetComponent<Canvas>();
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Scene 2") // Cuando no se encuentre en las escenas correspondientes las vuelven null. 
        {
            player = null; 
            fight = null;
            crossfadeTransition = null;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && enemy != null) // Atajo para terminar la batalla.
        {
            FinishBattle();
        }
    }

    // Funcion para iniciar la batalla.
    public void StartBattle(GameObject enemy, bool sneak)
    {
        this.enemy = enemy;

        if (!oneTime)
        {
            StartCoroutine(CrossfadeAnimation());
            
            this.enemy.GetComponent<EnemyGenerator>().ListEnemies.ForEach(x => Instantiate(x, GameObject.Find("EnemyBattleZone").transform));

            GetComponent<CombatFlow>().TotalEXP = this.enemy.GetComponent<EnemyGenerator>().TotalEXP;

            StartCoroutine(GetComponent<CombatFlow>().CreateButtons()); 
            oneTime = true;

            fight.enabled = true;

            if (sneak) 
            {
                Stats[] stats = GameObject.Find("EnemyBattleZone").GetComponentsInChildren<Stats>();

                foreach (Stats stat in stats)
                {
                    stat.Health -= (stat.Health * 0.05f);
                }
            }
        }
    }

    // Funcion para finalizar batalla.
    public void FinishBattle()
    {
        StartCoroutine(CrossfadeAnimation());

        Destroy(enemy);

        player.GetComponent<PlayerMove>().enabled = true; // Activa el movimiento del jugador.

        // Si el objeto jugador contiene alguno de los siguientes componentes los activa.
        if (player.GetComponent<KeyBoardControls>())
            player.GetComponent<KeyBoardControls>().enabled = true;
        if (player.GetComponent<ControllerControls>())
            player.GetComponent<ControllerControls>().enabled = true;

        oneTime = false;

        fight.enabled = false;
    }

    private IEnumerator CrossfadeAnimation()
    {
        Debug.Log("Ejecutando primera transicion");
        crossfadeTransition.SetBool("StartBattle", true);

        yield return new WaitForSeconds(0.5f);

        crossfadeTransition.SetBool("StartBattle", false);
    }
}
