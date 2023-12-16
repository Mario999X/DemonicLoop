using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class CharacterMove
{
    private GameObject character;
    private GameObject target;
    private string movement;
    private bool execute = true;

    public GameObject Character { get { return character; } }
    public GameObject Target { get { return target; } set { Target = value; } }
    public string Movement { get { return movement; } }
    public bool Execute { get { return execute; } set { execute = value; } }

    public CharacterMove(GameObject character, GameObject target, string movement)
    {
        this.character = character;
        this.target = target;
        this.movement = movement;
    }
}

public class CombatFlow : MonoBehaviour
{

    private int actualTurn = 1;
    public int ActualTurn { get { return actualTurn; } set { actualTurn = value; } }

    [Header("Experience total to distributed. Comes from EnemyGenerator")]
    [SerializeField] private float totalEXP;
    public float TotalEXP { set { totalEXP = value; }}

    private List<CharacterMove> movements = new();
    private List<GameObject> enemys = new();
    private List<GameObject> playerBT = new();
    private List<GameObject> combatOptionsBT = new();
    private List<GameObject> moveBT = new();
    private List<GameObject> enemyBT = new();

    public List<GameObject> EnemyBT { get { return enemyBT; } set { enemyBT = value; }}

    GameObject[] players;

    List<GameObject> playersDefeated = new();
    public List<GameObject> PlayersDefeated { get { return playersDefeated; }}

    private GameObject character = null;
    public GameObject Character { get { return character; } }

    [Header("Components to spawn buttons")]
    [SerializeField] GameObject spawnPlayerBT, spawnMoveBT, spawnEnemyBT, spawnCombatOptionsBT, buttonRef;

    [Header("Characters speed of movement")]
    [SerializeField] float speed = 50f;
    [SerializeField] Text WINLOSE;

    private int moves = 0;

    private bool wait = false;
    bool done = false;

    private string movement = null;

    private LibraryMove library;

    private MoneyPlayer moneyPlayer;

    private PlayerInventory playerInventory;

    private LibraryStates statesLibrary;

    private EnterBattle enterBattle;

    Scene scene;

    private void Update()
    {
        if (scene != UnityEngine.SceneManagement.SceneManager.GetActiveScene())
        {
            done = false;
            scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        }

        if (!done && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Scene 2")
        {
            library = GetComponent<LibraryMove>();
            moneyPlayer = GetComponent<MoneyPlayer>();
            playerInventory = GetComponent<PlayerInventory>();
            statesLibrary = GetComponent<LibraryStates>();
            enterBattle = GetComponent<EnterBattle>();
            spawnCombatOptionsBT = GameObject.Find("CombatOptionsButtons");
            spawnEnemyBT = GameObject.Find("EnemyButtons");
            spawnPlayerBT = GameObject.Find("PlayerButtons");
            spawnMoveBT = GameObject.Find("MoveButtons");
            WINLOSE = GameObject.Find("WINLOSE").GetComponent<Text>();

            GameObject.Find("PassTurnButton").GetComponent<Button>().onClick.AddListener(() => { PassTurn("Pass Turn"); });
            GameObject.Find("AttackButton").GetComponent<Button>().onClick.AddListener(() => { PlayerButtonAttacks(); });
            GameObject.Find("InventoryButton").GetComponent<Button>().onClick.AddListener(() => { LoadInventoryButtons(); });

            GameObject.Find("PassTurnButton").SetActive(false);
            GameObject.Find("AttackButton").SetActive(false);
            GameObject.Find("InventoryButton").SetActive(false);

            LoadCombatOptionsButtons();
            done = true;
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Scene 2")
        {
            done = false;
        }
    }

    private void LoadCombatOptionsButtons()
    {
        foreach (Transform bt in spawnCombatOptionsBT.transform)
        {
            combatOptionsBT.Add(bt.gameObject);
        }
    }

    public void LoadInventoryButtons()
    {
        foreach (GameObject moveBT in moveBT)
        {
            Destroy(moveBT);
        }
        moveBT.Clear();

        playerInventory.OpenCloseInventory();
    }

    private void GeneratePlayersButtons()
    {
        Debug.Log("Turno Actual: " + ActualTurn);

        if (playerBT.Count > 0)
        {
            /*
            En el caso de que haya mas players/botones de lo permitido los borra y hace limpieza.

            En otras palabras como limpiar la cache
            */
            playerBT.ForEach(bt => Destroy(bt));
            playerBT.Clear();
        }

        // Creamos un boton por todos los jugadores existentes.
        foreach (GameObject pl in players)
        {
            //Comprobamos si el player que tiene asignado el boton esta muerto o no
            StartCoroutine(CharacterDead(pl, false));

            GameObject button = Instantiate(buttonRef, spawnPlayerBT.transform.position, Quaternion.identity);
            button.transform.SetParent(spawnPlayerBT.transform);
            button.name = "PlayerButton (" + pl.name + ")";
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pl.name.Substring(1, pl.name.Length - 1); // Quitamos la posición del jugador.
            button.GetComponent<Button>().onClick.AddListener(delegate { GenerateOptionsButtons(pl); });


            button.transform.localScale = new Vector3(1f, 1f, 1f); // Al cambiar de resolucion, el boton aparecia con una escala distinta (?) asi que asi nos aseguramos que se mantenga.
            playerBT.Add(button);//Listado de botones generados
        }
        //Debug.Log("1- playerBT "+playerBT.Count);

    }//Fin de GeneratePlayersButtons


    public void GenerateTargetsButtons(bool targetPlayerOrEnemy, ObjectData itemOrAtk)
    {
        //itemOrAtk null es que no es un item

        // Despejamos la lista de la zona de botones enemigo. Asi evitamos que se coloquen uno encima de otro y que se mezclen.
        if (enemyBT.Count > 0)
        {
            enemyBT.ForEach(bt => Destroy(bt));
            enemyBT.Clear();
        }

        // Dependiendo del tipo de ataque, se cargan los botones de X targets.
        if (!targetPlayerOrEnemy)
        {
            enemys.ForEach(enemy =>
            {
                Debug.Log("targetEnemy " + targetPlayerOrEnemy.ToString());
                GameObject button = Instantiate(buttonRef, spawnEnemyBT.transform.position, Quaternion.identity);
                button.transform.SetParent(spawnEnemyBT.transform);
                button.name = "EnemyButton (" + enemy.name + ")";
                button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = enemy.name;
                if (itemOrAtk == null)
                {
                    button.GetComponent<Button>().onClick.AddListener(delegate { EnemyButton(enemy); });
                }
                else
                {
                    button.GetComponent<Button>().onClick.AddListener(delegate { itemOrAtk.UserObject(enemy); });
                }

                button.transform.localScale = new Vector3(1f, 1f, 1f);
                enemyBT.Add(button);
            });
        }
        else
        {
            foreach (GameObject pl in players)
            {
                Debug.Log("targetPlayer " + targetPlayerOrEnemy.ToString());
                GameObject button = Instantiate(buttonRef, spawnEnemyBT.transform.position, Quaternion.identity);
                button.transform.SetParent(spawnEnemyBT.transform);
                button.name = "PlayerButton (" + pl.name + ")";
                button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pl.name.Substring(1, pl.name.Length - 1); // Quitamos la posición del jugador.
                if (itemOrAtk == null)
                {
                    button.GetComponent<Button>().onClick.AddListener(delegate { EnemyButton(pl); });
                }
                else
                {
                    button.GetComponent<Button>().onClick.AddListener(delegate { itemOrAtk.UserObject(pl); });
                }

                button.transform.localScale = new Vector3(1f, 1f, 1f);
                enemyBT.Add(button);//Listado de botones generados
            }
        }

        //Debug.Log("1- enemyBT " + enemyBT.Count);

    }//Fin de GenerateTargetsButtons

    public IEnumerator CreateButtons()
    {
        yield return new WaitForSeconds(0.00000001f);

        enemys = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        players = GameObject.FindGameObjectsWithTag("Player").ToArray();

        Array.Sort(players, (p1, p2) => p1.name.CompareTo(p2.name)); // Reorganiza el array de jugadores por su nombre de esta forma prevenimos un fallo al asignar botones.

        GeneratePlayersButtons();

        moveBT.ForEach(bt => { bt.SetActive(false); }); // Desactiva todos los botones movimiento.

        yield return null;
    }//Fin de CreateButtons


    // Funcion para generar las opciones del jugador en combate.
    public void GenerateOptionsButtons(GameObject player)
    {
        DesactivateAllButtons();

        if (!wait)
        {
            this.character = player;

            combatOptionsBT.ForEach(bt => bt.SetActive(true));
        }

    }

    // Selección de jugador.
    public void PlayerButtonAttacks()
    {

        if (!wait)
        {
            if (enemyBT.Count > 0)
            {
                enemyBT.ForEach(bt => Destroy(bt));
                enemyBT.Clear();
            }

            if (playerInventory.InventoryState)
                playerInventory.OpenCloseInventory();

            foreach (GameObject moveBT in moveBT)
            {
                Destroy(moveBT);
            }
            moveBT.Clear();

            foreach (string listAtk in character.GetComponent<Stats>().ListNameAtk)
            {
                // Creamos un boton de movimiento.
                GameObject bt = Instantiate(buttonRef, spawnMoveBT.transform.position, Quaternion.identity);
                bt.transform.SetParent(spawnMoveBT.transform);
                bt.name = "NameAtk " + listAtk;//Nombre de los botones que se van a generar
                bt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = listAtk;
                bt.GetComponent<Button>().onClick.AddListener(delegate { MovementButton(listAtk); });

                bt.transform.localScale = new Vector3(1f, 1f, 1f);
                moveBT.Add(bt);

                // Comprobamos si el mana es suficiente, si no lo es, desactivamos el boton.
                var isManaEnough = library.CheckIfManaIsEnough(character, listAtk.ToUpper());
                if (!isManaEnough)
                {
                    bt.GetComponent<Button>().interactable = false;
                }
            }
        }
    }//Fin de PlayerButton

    // Selección de movimiento.
    public void MovementButton(string movement)
    {
        this.movement = movement;

        var isAOE = library.CheckAoeAttack(movement);
        var targetsToLoad = library.CheckAttackOrHeal(movement);

        if (isAOE)
        {
            // Ataca o cura AOE, segun la segunda comprobacion
            if (!targetsToLoad)
            {
                GoPlayerMultiTarget(character, enemys, movement, targetsToLoad);
            }
            else GoPlayerMultiTarget(character, players.ToList(), movement, targetsToLoad);
        }
        else
        {
            // Genera los botones según la segunda comprobacion
            GenerateTargetsButtons(targetsToLoad, null);
        }
    }//Fin de MovementButton

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

            DesactivateAllButtons();

            StartCoroutine(GoPlayerSingleTarget(character, enemy, movement));
        }
    }//Fin de EnemyButton


    // Aniade a la lista de movimientos los movimientos.
    public void AddMovement(GameObject character, GameObject target, string movement)
    {
        movements.Add(new CharacterMove(character, target, movement));
    }//Fin de AddMovement

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
    }//Fin de DestroyButton

    private void GoPlayerMultiTarget(GameObject character, List<GameObject> targets, string movement, bool targetsAliesOrEnemies)
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

        DesactivateAllButtons();

        wait = true;

        targets.ForEach(t => { library.Library(character, t, movement, statesLibrary); });

        foreach (GameObject t in targets.ToArray())
        {
            StartCoroutine(CharacterDead(t, !targetsAliesOrEnemies));
        }

        moves++;

        this.character = null; this.movement = null;

        wait = false;

        CheckIfIsEnemyTurn();
       

    }//Fin de GoPlayerMultiTarget

    // Ejecuta acción del jugador.
    private IEnumerator GoPlayerSingleTarget(GameObject character, GameObject target, string movement)
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
                library.Library(character, target, movement, statesLibrary); // Realiza el ataque.

                change = false;
            }

            if (Vector2.Distance(character.transform.position, v) < 0.001f && !change)
            {
                dontStop = false;
            }

            yield return new WaitForSeconds(0.00001f);
        } while (dontStop);

        StartCoroutine(CharacterDead(target, true));

        moves++;

        this.character = null; this.movement = null;

        wait = false;

        CheckIfIsEnemyTurn();

        yield return null;
    }//Fin de GoPlayerSingleTarget

    // Ejecuta las acciones del enemigo.
    private IEnumerator GoEnemy()
    {   
        wait = true;

        //Chequea que ataque tiene el enemigo y puede usar
        CheckAtkEnemy();


        foreach (CharacterMove characterMove in movements)
        {
            GameObject characterTarget = characterMove.Target;

            // Deteccion si el target aliado esta muerto o no.
            if(characterTarget.GetComponent<Stats>().Health == 0)
            {
                //alliesEnemyAttackList.Remove(characterTarget);

                if(players.Length != 0)
                {
                    characterTarget = players[UnityEngine.Random.Range(0, players.Length)];
                } 
                else 
                {
                    characterMove.Execute = false;
                }
            }

            if(characterMove.Execute)
            {
            bool dontStop = true, change = true;

            Vector2 v = characterMove.Character.transform.position;

            do
            {
                if (change)
                {
                    characterMove.Character.transform.position = Vector2.MoveTowards(characterMove.Character.transform.position, characterTarget.transform.position, speed * Time.deltaTime);
                }
                else
                {
                    characterMove.Character.transform.position = Vector2.MoveTowards(characterMove.Character.transform.position, v, speed * Time.deltaTime);
                }

                if (Vector2.Distance(characterMove.Character.transform.position, characterTarget.transform.position) < 100f && change)
                {

                    library.Library(characterMove.Character, characterTarget, characterMove.Movement, statesLibrary); // Realiza el ataque.

                    StartCoroutine(CharacterDead(characterTarget, false));

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

        }

        movements.Clear();
        moves = 0;

        wait = false;
        

        NextTurn();

        yield return null;
    }//Fin de GoEnemy

    // Funcion para comprobar si un personaje esta muerto segun su vida.
    private IEnumerator CharacterDead(GameObject target, bool enemy)
    {
        Stats targetST = target.GetComponent<Stats>();

        if (targetST.Health == 0)
        {
            Debug.Log(target.name + " is dead" + " | enemigo: " + enemy);

            if (enemy)
            {
                /*
                // Se busca en los botones enemigos
                enemyBT.ForEach(bt =>
                {
                    // Si el texto coincide con el nombre del enemigo, se deshabilita el boton.
                    if (bt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text == target.name.Substring(1, target.name.Length - 1))
                    {
                        bt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.grey;
                        bt.GetComponent<Button>().enabled = false;
                    }
                });
                */

                enemys.Remove(target);

                //Cuando el enemigo muera nos dara una cantidad X de dinero
                moneyPlayer.Money += targetST.MoneyDrop;

            }
            else
            {

                List<GameObject> actualPlayers = players.ToList();

                actualPlayers.Remove(target);

                playersDefeated.Add(target);

                players = actualPlayers.ToArray();

                GeneratePlayersButtons();
            }
        }
        yield return null;
    }

    // Funcion para que el personaje seleccionado recupere mana sin hacer ningun daño. 
    public void PassTurn(string movement)
    {
        wait = true;
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
        DesactivateAllButtons();

        library.PassTurn(character, movement.ToUpper());

        moves++;

        this.character = null; this.movement = null;

        wait = false;
        CheckIfIsEnemyTurn();
    }


    // Funcion para el inventario en el combate 2D
    public void InventoryTurn()
    {
        if (moveBT.Count > 0)
        {
            moveBT.ForEach(bt => Destroy(bt));
        }
        moveBT.Clear();

        wait = true;

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

        DesactivateAllButtons();

        moves++;

        this.character = null; this.movement = null;

        wait = false;

        CheckIfIsEnemyTurn();
        
    }

    // Funcion para desactivar todos los botones activos, a excepcion de los aliados del jugador.
    private void DesactivateAllButtons()
    {

        moveBT.ForEach(bt => { bt.SetActive(false); }); // Desactiva todos los botones movimiento.

        combatOptionsBT.ForEach(bt => bt.SetActive(false)); // Desactiva todos los botones de opciones de combate.

        enemyBT.ForEach(bt => { bt.SetActive(false); }); // Desactiva todos los botones de targets en caso de que esten activados.
    }

    // Funcion para determinar/empezar el turno enemigo.
    private void CheckIfIsEnemyTurn()
    {
        BattleStatus();
        // Espera a que todos los jugadores hagan sus movimientos.
        if (moves >= players.Length && !wait)
        {
            StartCoroutine(GoEnemy());
        }
    }//fin de CheckIfIsEnemyTurn

    private void NextTurn()
    {
        ActualTurn++;

        // Paso de turno para los estados
        statesLibrary.CharacterStates.ForEach(x => {
            x.Turn++;
            if(x.Character != null)
            {
                if(x.Character.CompareTag("Enemy")) StartCoroutine(CharacterDead(x.Character, true)); // Comprobacion de enemigo fallecido
            }
        });

        players.ToList().ForEach(x => StartCoroutine(CharacterDead(x, false))); // Comprobacion de aliado fallecido

        BattleStatus();
        
    }//fin de NextTurn


    private async void BattleStatus()
    {
        
        if (enemys.Count == 0)
        {
            var experience = totalEXP / players.LongLength;

            Debug.Log("Experiencia: " + experience);

            players.ToList().ForEach(p => p.GetComponent<LevelSystem>().GainExperienceFlatRate(experience));
            
            //Se debe mostrar una pantalla de WIN
            WINLOSE.enabled = true;
            WINLOSE.text = "WIN";
            await System.Threading.Tasks.Task.Delay(2000);
            enterBattle.FinishBattle();

            // Se resetea la información del combate para el proximo encuentro
            actualTurn = 0;
            moves = 0; 
        }
        if (playerBT.Count == 0)
        {
            WINLOSE.enabled = true;
            WINLOSE.text = "LOSE";
            await System.Threading.Tasks.Task.Delay(2000);
            enterBattle.FinishBattle();
        }

        WINLOSE.enabled = false;

    }//Fin de BattleStatus


    public void CheckAtkEnemy()
    {
        foreach (GameObject enemy in enemys)
        {
            List<string> listAtkEnemy = enemy.GetComponent<Stats>().ListNameAtk;
            string nameAtkEnemy="";

            bool isManaEnough = false;
            // Comprobamos si el mana es suficiente, si no lo es, desactivamos el boton.
            while (!isManaEnough && listAtkEnemy.Count>0)
            {
                //Int 1 lista de ataques del enemigo
                int i = UnityEngine.Random.Range(0, listAtkEnemy.Count);

                //Chequeamos si tiene mana o no
                isManaEnough = library.CheckIfManaIsEnough(enemy, listAtkEnemy[i]);

                if (isManaEnough)
                {
                    nameAtkEnemy=listAtkEnemy[i];
                }
                else
                {
                    listAtkEnemy.Remove(listAtkEnemy[i]);
                }
                   
            }

            if (listAtkEnemy.Count <= 0)
            {
                library.PassTurn(enemy, "PASS TURN");
            }
            else
            {
                //Int 2 lista de jugadores
                int z = UnityEngine.Random.Range(0, players.Length);

                AddMovement(enemy, players[z], nameAtkEnemy);

            }

        }


    }//Fin de CheckAtkEnemy

}