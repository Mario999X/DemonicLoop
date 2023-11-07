using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using System;

public class CharacterMove
{
    private GameObject character;
    private GameObject target;
    private string movement;

    public GameObject Character { get { return character; } }
    public GameObject Target { get { return target; } }
    public string Movement { get { return movement; } }

    public CharacterMove(GameObject character, GameObject target, string movement)
    {
        this.character = character;
        this.target = target;
        this.movement = movement;
    }
}

public class CombatFlow : MonoBehaviour
{
    List<CharacterMove> movements = new();
    List<GameObject> enemys = new();
    List<GameObject> playerBT = new();
    List<GameObject> moveBT = new();
    List<GameObject> enemyBT = new();

    GameObject[] players;
    GameObject character = null;

    [Header("Components to spanw buttons")]
    [SerializeField] GameObject spanwPlayerBT, spanwMoveBT, spanwEnemyBT, buttonRef;

    [Header("Characters speed")]
    [SerializeField] float speed = 50f;

    private int moves = 0;

    private bool wait = false;

    private string movement = null;

    private LibraryMove library;

    // Start is called before the first frame update
    void Start()
    {
        library = GetComponent<LibraryMove>();
    }

    private void GeneratePlayersButtons(){
        // Creamos un boton por todos los jugadores existentes.
        foreach (GameObject pl in players)
        {
            GameObject button = Instantiate(buttonRef, spanwPlayerBT.transform.position, Quaternion.identity);
            button.transform.SetParent(spanwPlayerBT.transform);
            button.name = "PlayerButton (" + pl.name + ")";
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pl.name.Substring(1, pl.name.Length - 1); // Quitamos la posición del jugador.
            button.GetComponent<Button>().onClick.AddListener(delegate { PlayerButton(pl); });
            playerBT.Add(button);//Listado de botones generados
        }
    }

    private void GenerateTargetsButtons(bool targetPlayerOrEnemy){
        // Despejamos la lista de la zona de botones enemigo. Asi evitamos que se coloquen uno encima de otro y que se mezclen.
        if(enemyBT.Count > 0){
            enemyBT.ForEach(bt => Destroy(bt));
            enemyBT.Clear();
        }

        // Dependiendo del tipo de ataque, se cargan los botones de X targets.
        if(!targetPlayerOrEnemy){
            enemys.ForEach(enemy =>
            {
            GameObject button = Instantiate(buttonRef, spanwEnemyBT.transform.position, Quaternion.identity);
            button.transform.SetParent(spanwEnemyBT.transform);
            button.name = "EnemyButton (" + enemy.name + ")";
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = enemy.name;
            button.GetComponent<Button>().onClick.AddListener(delegate { EnemyButton(enemy); });
            enemyBT.Add(button);
            });

        } else {

        foreach (GameObject pl in players)
        {
            GameObject button = Instantiate(buttonRef, spanwEnemyBT.transform.position, Quaternion.identity);
            button.transform.SetParent(spanwEnemyBT.transform);
            button.name = "PlayerButton (" + pl.name + ")";
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pl.name.Substring(1, pl.name.Length - 1); // Quitamos la posición del jugador.
            button.GetComponent<Button>().onClick.AddListener(delegate { EnemyButton(pl); });
            enemyBT.Add(button);//Listado de botones generados
        }
        }
    }

    public IEnumerator CreateButtons()
    {
        yield return new WaitForSeconds(0.00000001f);

        enemys = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        players = GameObject.FindGameObjectsWithTag("Player").ToArray();

        Array.Sort(players, (p1, p2) => p1.name.CompareTo(p2.name)); // Reorganiza el array de jugadores por su nombre de esta forma prevenimos un fallo al asignar botones.
        enemys.Sort((p1, p2) => p1.name.CompareTo(p2.name)); // Reorganiza la lista de enemigos por su nombre de esta forma prevenimos un fallo al asignar botones.

        GeneratePlayersButtons();

        moveBT.ForEach(bt => { bt.SetActive(false); }); // Desactiva todos los botones movimiento.

        yield return null;
    }
    

    // Selección de jugador.
    public void PlayerButton(GameObject player)
    {
        if (!wait)
        {
            this.character = player;
            //Debug.Log("player save");

            foreach (GameObject moveBT in moveBT)
            {
                Destroy(moveBT);

            }
            moveBT.Clear();

            foreach (string listAtk in player.GetComponent<Stats>().ListAtk)
            {
                // Creamos un boton de movimiento.
                GameObject bt = Instantiate(buttonRef, spanwMoveBT.transform.position, Quaternion.identity);
                bt.transform.SetParent(spanwMoveBT.transform);
                bt.name = "NameAtk " + listAtk;//Nombre de los botones que se van a generar
                bt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = listAtk;
                bt.GetComponent<Button>().onClick.AddListener(delegate { MovementButton(listAtk); });
                moveBT.Add(bt);
            }
        }
    }

    // Selección de movimiento.
    public void MovementButton(string movement)
    {
        this.movement = movement;

        var isAOE = library.CheckAoeAttack(movement);
        var targetsToLoad = library.CheckAttackOrHeal(movement);

        if(isAOE){
            // Ataca o cura AOE, segun la segunda comprobacion
            if(!targetsToLoad){
                GoPlayerMultiTarget(character, enemys, movement);

            } else GoPlayerMultiTarget(character, players.ToList(), movement);
            
        } else {
            // Genera los botones según la segunda comprobacion
            GenerateTargetsButtons(targetsToLoad);
        }
    }

    // Selección de enemigo.
    public void EnemyButton(GameObject enemy)
    {
        if (this.character != null && this.movement != null && !wait)
        {
            // Impide que vuelva a ser selecionado el mismo personaje.
            playerBT.ForEach(bt =>
            {
                // Si el texto coincide con el nombre del jugador aplica los cambios a dicho boton.
                if (bt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text == character.name.Substring(1, character.name.Length - 1))
                {
                    bt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.grey;
                    bt.GetComponent<Button>().enabled = false;
                }
            });

            moveBT.ForEach(bt => { bt.SetActive(false); }); // Desactiva todos los botones movimiento.

            //enemyBT.ForEach(bt => Debug.Log("ENEMY BUTTON" + bt));

            enemyBT.ForEach(bt => { bt.SetActive(false); }); // Desactiva todos los botones enemigo.

            StartCoroutine(GoPlayerSingleTarget(character, enemy, movement));
        }
    }

    // Añade a la lista de movimientos los movimientos.
    public void AddMovement(GameObject character, GameObject target, string movement)
    {
        movements.Add(new CharacterMove(character, target, movement));
    }

    // Destruye el boton del enemigo.
    public void DestroyButton(GameObject @object)
    {
        enemyBT.ForEach(bt =>
        {
            if (bt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text == @object.name.Substring(1, character.name.Length - 1))
            {
                Destroy(bt);
            }
        });
    }

    private void GoPlayerMultiTarget(GameObject character, List<GameObject> targets, string movement)
    {
        // Impide que vuelva a ser selecionado el mismo personaje.
        playerBT.ForEach(bt =>
        {
            // Si el texto coincide con el nombre del jugador aplica los cambios a dicho boton.
            if (bt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text == character.name.Substring(1, character.name.Length - 1))
            {
                bt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.grey;
                bt.GetComponent<Button>().enabled = false;
            }
        });

        moveBT.ForEach(bt => { bt.SetActive(false); }); // Desactiva todos los botones movimiento.

        enemyBT.ForEach(bt => { bt.SetActive(false); }); // Desactiva todos los botones enemigo en caso de que esten activados.

        wait = true;

        targets.ForEach(t => {library.Library(character, t, movement);});

        moves++;

        this.character = null; this.movement = null;

        wait = false;

        // Espera a que todos los jugadores hagan sus movimientos.
        if (moves >= players.Length && !wait)
        {
            StartCoroutine(GoEnemy());
        }
    }

    // Ejecuta acción del jugador.
    IEnumerator GoPlayerSingleTarget(GameObject character, GameObject target, string movement)
    {
        wait = true;

        bool dontStop = true, change = true;

        Vector2 v = character.transform.position;

        do
        {
            if (change)
            {
                character.transform.position = Vector2.MoveTowards(character.transform.position, target.transform.position, speed * Time.deltaTime);
            }
            else
            {
                character.transform.position = Vector2.MoveTowards(character.transform.position, v, speed * Time.deltaTime);
            }

            if (Vector2.Distance(character.transform.position, target.transform.position) < 100f && change)
            {
                library.Library(character, target, movement); // Realiza el ataque.

                change = false;
            }

            if (Vector2.Distance(character.transform.position, v) < 0.001f && !change)
            {
                dontStop = false;
            }

            yield return new WaitForSeconds(0.00001f);
        } while (dontStop);

        moves++;

        this.character = null; this.movement = null;

        wait = false;

        // Espera a que todos los jugadores hagan sus movimientos.
        if (moves >= players.Length && !wait)
        {
            StartCoroutine(GoEnemy());
        }

        yield return null;
    }

    // Ejecuta las acciones del enemigo.
    private IEnumerator GoEnemy()
    {
        wait = true;

        foreach (GameObject enemy in enemys)
        {
            int i = UnityEngine.Random.Range(0, players.Length);
            AddMovement(enemy, players[i], "Punch");
        }

        //Debug.Log("Enemys selected move");

        //int count = 0;

        foreach (CharacterMove characterMove in movements)
        {
            bool dontStop = true, change = true;

            Vector2 v = characterMove.Character.transform.position;

            do
            {
                if (change)
                {
                    characterMove.Character.transform.position = Vector2.MoveTowards(characterMove.Character.transform.position, characterMove.Target.transform.position, speed * Time.deltaTime);
                }
                else
                {
                    characterMove.Character.transform.position = Vector2.MoveTowards(characterMove.Character.transform.position, v, speed * Time.deltaTime);
                }

                if (Vector2.Distance(characterMove.Character.transform.position, characterMove.Target.transform.position) < 100f && change)
                {
                    //count++;

                    //Debug.Log(characterMove.Character.name + ": " + count);

                    library.Library(characterMove.Character, characterMove.Target, characterMove.Movement); // Realiza el ataque.

                    change = false;
                }

                if (Vector2.Distance(characterMove.Character.transform.position, v) < 0.001f && !change)
                {
                    dontStop = false;
                }

                yield return new WaitForSeconds(0.00001f);
            } while (dontStop);
        }

        // Una vez terminado el turno de los enemigos vuelve a activar los botones de los jugadores.
        playerBT.ForEach(bt =>
        {
            bt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.black;
            bt.GetComponent<Button>().enabled = true;
        });

        movements.Clear();
        moves = 0;

        wait = false;

        yield return null;
    }
}