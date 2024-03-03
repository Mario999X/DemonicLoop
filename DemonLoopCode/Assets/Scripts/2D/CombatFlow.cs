using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Clase para realizar ataques. Se almacena al personaje que ataca, el objetivo, el movimiento y si se ha ejecutado este movimiento.
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

// Clase para los aprendizajes de ataque. Se almacena al personaje y el ataque nuevo.
public class CharacterAndAttack
{
    private GameObject character;
    private AttackData newAttack;

    public GameObject Character { get { return character; } }
    public AttackData NewAttack { get { return newAttack; } }

    public CharacterAndAttack(GameObject character, AttackData newAttack)
    {
        this.character = character;
        this.newAttack = newAttack;
    }
}

// Clase encargada de controlar los combates y relacionados.
public class CombatFlow : MonoBehaviour
{
    private int actualTurn = 1;
    public int ActualTurn { get { return actualTurn; } set { actualTurn = value; } }

    [Header("Experience total to distributed. Comes from EnemyGenerator")]
    [SerializeField] private float totalEXP;
    public float TotalEXP { set { totalEXP = value; } }

    private List<CharacterMove> movements = new();
    private List<GameObject> enemys = new();
    private List<GameObject> playerBT = new();
    private List<GameObject> combatOptionsBT = new();
    private List<GameObject> moveBT = new();
    private List<GameObject> enemyBT = new();

    // Puntos para cargar las barras SP durante el combate 
    private const int SpGenerationPerAction = 10;

    public List<GameObject> EnemyBT { get { return enemyBT; } set { enemyBT = value; } }

    GameObject[] players;

    public GameObject[] Players { get { return players; } }

    List<GameObject> playersDefeated = new();
    public List<GameObject> PlayersDefeated { get { return playersDefeated; } }

    List<CharacterAndAttack> charactersWhoCanLearnAnAttack = new();

    public List<CharacterAndAttack> CharactersWhoCanLearnAnAttack { get { return charactersWhoCanLearnAnAttack; } }

    private GameObject character = null;
    public GameObject Character { get { return character; } }

    [Header("Components to spawn buttons")]
    [SerializeField] GameObject spawnPlayerBT, spawnMoveBT, spawnEnemyBT, spawnCombatOptionsBT, buttonRef;

    [Header("Characters speed of movement")]
    [SerializeField] float speed = 50f;

    private TextMeshProUGUI AllyActionBarText, EnemyActionBarText;

    private int moves = 0;

    private bool wait = false;
    bool done = false;

    private string movement = null;

    private LibraryMove library;
    private MoneyPlayer moneyPlayer;
    private PlayerInventory playerInventory;
    private LibraryStates statesLibrary;
    private LibraryBattleModifiers battleModifiersLibrary;
    private EnterBattle enterBattle;
    private SpecialMiniGame specialMiniGame;

    GameObject panelMiniGame;
    private List<GameObject> panelSpMini = new();

    Scene scene;

    private LearningAttacksManager learningAttacksManager;
    private PostBattleTeam postBattleTeam;

    LoserReset loserReset;

    private void Update()
    {
        if (scene != UnityEngine.SceneManagement.SceneManager.GetActiveScene())
        {
            done = false;
            scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        }

        if (!done && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Scene 2")
        {
            wait = false;

            library = GetComponent<LibraryMove>();
            moneyPlayer = GetComponent<MoneyPlayer>();
            playerInventory = GetComponent<PlayerInventory>();
            statesLibrary = GetComponent<LibraryStates>();
            battleModifiersLibrary = GetComponent<LibraryBattleModifiers>();
            enterBattle = GetComponent<EnterBattle>();
            specialMiniGame = GetComponent<SpecialMiniGame>();
            spawnCombatOptionsBT = GameObject.Find("CombatOptionsButtons");
            spawnEnemyBT = GameObject.Find("EnemyButtons");
            spawnPlayerBT = GameObject.Find("PlayerButtons");
            spawnMoveBT = GameObject.Find("MoveButtons");

            AllyActionBarText = GameObject.Find("AllyActionBar").transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            EnemyActionBarText = GameObject.Find("EnemyActionBar").transform.GetChild(0).GetComponent<TextMeshProUGUI>();

            panelMiniGame = GameObject.Find("PanelMiniGame");

            learningAttacksManager = GetComponent<LearningAttacksManager>();
            postBattleTeam = GetComponent<PostBattleTeam>();
            loserReset = GameObject.Find("Fight").GetComponent<LoserReset>();

            SetAllyActionBarInactive();
            SetEnemyActionBarInactive();

            GameObject.Find("PassTurnButton").GetComponent<Button>().onClick.AddListener(() => { PassTurn("Pass Turn"); });
            GameObject.Find("AttackButton").GetComponent<Button>().onClick.AddListener(() => { PlayerButtonAttacks(); });
            GameObject.Find("InventoryButton").GetComponent<Button>().onClick.AddListener(() => { LoadInventoryButtons(); });
            GameObject.Find("SpecialAttackButton").GetComponent<Button>().onClick.AddListener(() => { SpecialAttackTurn(); });
            GameObject.Find("PassTurnButton").SetActive(false);
            GameObject.Find("AttackButton").SetActive(false);
            GameObject.Find("InventoryButton").SetActive(false);
            GameObject.Find("SpecialAttackButton").SetActive(false);

            panelMiniGame.SetActive(false);

            LoadCombatOptionsButtons();

            done = true;
        }
    }

    // Preparamos los espacios para los botones de combate.
    private void LoadCombatOptionsButtons()
    {
        if (combatOptionsBT.Count > 0)
            combatOptionsBT.Clear();

        if (moveBT.Count > 0)
        {
            moveBT.ForEach(bt => Destroy(bt));
            moveBT.Clear();
        }

        if (enemyBT.Count > 0)
        {
            enemyBT.ForEach(bt => Destroy(bt));
            enemyBT.Clear();
        }

        foreach (Transform bt in spawnCombatOptionsBT.transform)
            combatOptionsBT.Add(bt.gameObject);
    }

    // Funcion para el boton de inventario en combate. Mostramos el inventario del jugador.
    public void LoadInventoryButtons()
    {
        foreach (GameObject moveBT in moveBT)
        {
            Destroy(moveBT);
        }
        moveBT.Clear();

        AudioManager.Instance.PlaySoundButtons();

        playerInventory.OpenCloseInventory();
    }

    // Funcion para generar los botones de los personajes vivos del jugador.
    private void GeneratePlayersButtons()
    {
        if (playerBT.Count > 0)
        {
            playerBT.ForEach(bt => Destroy(bt));
            playerBT.Clear();
        }

        foreach (GameObject pl in players)
        {
            //Comprobamos si el personaje esta muerto
            StartCoroutine(CharacterDead(pl, false));

            GameObject button = Instantiate(buttonRef, spawnPlayerBT.transform.position, Quaternion.identity);
            button.transform.SetParent(spawnPlayerBT.transform);
            button.name = "PlayerButton (" + pl.name + ")";

            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pl.name.Remove(pl.name.IndexOf("("));

            button.GetComponent<Button>().onClick.AddListener(delegate { GenerateOptionsButtons(pl); });

            button.transform.localScale = new Vector3(1f, 1f, 1f); // Al cambiar de resolucion, el boton aparecia con una escala distinta (?) asi que asi nos aseguramos que se mantenga.
            playerBT.Add(button);//Listado de botones generados
        }

        // Solucion al problema de actualizacion visual de vida y mana
        foreach(GameObject pl in players)
        {
            pl.GetComponent<Stats>().Health = pl.GetComponent<Stats>().Health;
            pl.GetComponent<Stats>().Mana = pl.GetComponent<Stats>().Mana;
        }
    }

    // Funcion usada para generar los botones de los objetivos. Si la variable itemOrAtk es nula, significa que es un ataque.
    public void GenerateTargetsButtons(bool targetPlayerOrEnemy, ObjectData itemOrAtk)
    {
        if (enemyBT.Count > 0)
        {
            enemyBT.ForEach(bt => Destroy(bt));
            enemyBT.Clear();
        }

        // Dependiendo del tipo de ataque, se cargan los botones de los aliados o los enemigos.
        if (!targetPlayerOrEnemy)
        {
            enemys.ForEach(enemy =>
            {
                GameObject button = Instantiate(buttonRef, spawnEnemyBT.transform.position, Quaternion.identity);
                button.transform.SetParent(spawnEnemyBT.transform);
                button.name = "EnemyButton (" + enemy.name + ")";
                button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = enemy.name.Remove(enemy.name.IndexOf("("));

                if (itemOrAtk == null) button.GetComponent<Button>().onClick.AddListener(delegate { EnemyButton(enemy); });
                else button.GetComponent<Button>().onClick.AddListener(delegate { itemOrAtk.UserObjectInBattle(enemy); });

                button.transform.localScale = new Vector3(1f, 1f, 1f);
                enemyBT.Add(button);
            });
        }
        else
        {
            foreach (GameObject pl in players)
            {
                GameObject button = Instantiate(buttonRef, spawnEnemyBT.transform.position, Quaternion.identity);
                button.transform.SetParent(spawnEnemyBT.transform);
                button.name = "PlayerButton (" + pl.name + ")";

                button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pl.name.Remove(pl.name.IndexOf("("));

                if (itemOrAtk == null) button.GetComponent<Button>().onClick.AddListener(delegate { EnemyButton(pl); });
                else button.GetComponent<Button>().onClick.AddListener(delegate { itemOrAtk.UserObjectInBattle(pl); });

                button.transform.localScale = new Vector3(1f, 1f, 1f);
                enemyBT.Add(button);
            }
        }
    }

    // Funcion para generar el boton de un personaje que haya sido revivido.
    private void GeneratePlayerBackToLifeButton(GameObject playerDefeated)
    {
        GameObject button = Instantiate(buttonRef, spawnPlayerBT.transform.position, Quaternion.identity);
        button.transform.SetParent(spawnPlayerBT.transform);
        button.name = "PlayerButton (" + playerDefeated.name + ")";

        button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = playerDefeated.name.Remove(playerDefeated.name.IndexOf("("));

        button.GetComponent<Button>().onClick.AddListener(delegate { GenerateOptionsButtons(playerDefeated); });

        button.transform.localScale = new Vector3(1f, 1f, 1f); // Al cambiar de resolucion, el boton aparecia con una escala distinta (?) asi que asi nos aseguramos que se mantenga.
        playerBT.Add(button);
    }

    // Funcion para generar los botones de los personajes que hayan sido derrotados.
    public void GeneratePlayersDefeatedButtons(ObjectData itemOrAtk)
    {
        if (enemyBT.Count > 0)
        {
            enemyBT.ForEach(bt => Destroy(bt));
            enemyBT.Clear();
        }

        playersDefeated.ForEach(playerDefeated =>
        {
            GameObject button = Instantiate(buttonRef, spawnEnemyBT.transform.position, Quaternion.identity);
            button.transform.SetParent(spawnEnemyBT.transform);
            button.name = "PlayerDefeated (" + playerDefeated.name + ")";
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = playerDefeated.name.Remove(playerDefeated.name.IndexOf("("));

            button.GetComponent<Button>().onClick.AddListener(delegate { itemOrAtk.UserObjectInBattle(playerDefeated); });

            button.transform.localScale = new Vector3(1f, 1f, 1f);
            enemyBT.Add(button);
        });
    }

    // Funcion para preparar a los aliados en el Array de players. se tiene en cuenta quienes se encuentra derrotados si se encuentran desactivados.
    public void SetPlayersInCombat()
    {
        players = GameObject.FindGameObjectsWithTag("Player").ToArray();

        foreach (Transform child in GameObject.Find("AlliesBattleZone").transform)
            if (!child.gameObject.activeSelf) playersDefeated.Add(child.gameObject);
    }

    // Funcion para resetear a los personajes del jugador del array de Players.
    public void ResetPlayersInCombat()
    {
        var tempList = players.ToList();

        tempList.Clear();
        players = tempList.ToArray();
    }

    // Funcion para detectar a los enemigos del combate y generar los de los aliados.
    public IEnumerator FindEnemiesAndCreateAlliesButtons()
    {
        yield return new WaitForSeconds(0.00000001f);

        enemys = GameObject.FindGameObjectsWithTag("Enemy").ToList();

        GeneratePlayersButtons();

        moveBT.ForEach(bt => { bt.SetActive(false); }); // Desactiva todos los botones movimiento.

        yield return null;
    }//Fin de CreateButtons


    // Funcion para generar las opciones del jugador en combate.
    public void GenerateOptionsButtons(GameObject player)
    {
        AudioManager.Instance.PlaySoundButtons();

        DesactivateAllButtons();

        if (!wait)
        {
            character = player;

            SetTextInAllyActionBar();

            combatOptionsBT.ForEach(bt => bt.SetActive(true));
        }
    }

    // Seleccion del jugador en botones de movimiento.
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
                Destroy(moveBT);

            moveBT.Clear();

            AudioManager.Instance.PlaySoundButtons();

            foreach (string listAtk in character.GetComponent<Stats>().ListNameAtk)
            {
                // Creamos un boton de movimiento.
                GameObject bt = Instantiate(buttonRef, spawnMoveBT.transform.position, Quaternion.identity);
                
                bt.transform.SetParent(spawnMoveBT.transform);
                bt.name = "NameAtk " + listAtk;
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
    }

    // Seleccion de movimiento.
    public void MovementButton(string movement)
    {
        this.movement = movement;
        var isAOE = library.CheckAoeAttack(movement);
        var targetsToLoad = library.CheckAttackOrHeal(movement);

        AudioManager.Instance.PlaySoundButtons();

        if (isAOE)
        {
            // Ataca o cura AOE, segun la segunda comprobacion
            if (!targetsToLoad) GoPlayerMultiTarget(character, enemys, movement, targetsToLoad);
            else GoPlayerMultiTarget(character, players.ToList(), movement, targetsToLoad);
        }
        else
        {
            // Genera los botones segun la segunda comprobacion
            GenerateTargetsButtons(targetsToLoad, null);
        }
    }

    // Seleccion de target.
    public void EnemyButton(GameObject enemy)
    {
        if (character != null && movement != null && !wait)
        {
            DesactivatePlayerButton();
            DesactivateAllButtons();

            AudioManager.Instance.PlaySoundButtons();
            StartCoroutine(GoPlayerSingleTarget(character, enemy, movement));
        }
    }

    // Angrega a la lista de movimientos un CharacterMove. Usado por los enemigos.
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
                Destroy(bt);
        });
    }

    // Funcion para personajes aliados. Ataque AOE.
    private void GoPlayerMultiTarget(GameObject character, List<GameObject> targets, string movement, bool targetsAliesOrEnemies)
    {
        DesactivatePlayerButton();
        DesactivateAllButtons();

        wait = true;

        AddSPValue(character.GetComponent<Stats>());

        targets.ForEach(t => { library.Library(character, t, movement, statesLibrary, battleModifiersLibrary); });//Aqui realiza el ataque

        foreach (GameObject t in targets.ToArray())
            StartCoroutine(CharacterDead(t, !targetsAliesOrEnemies));

        moves++;

        this.character = null; this.movement = null;

        wait = false;

        CheckIfIsEnemyTurn();
    }

    // Funcion para personajes aliados. Ataque Single Target.
    private IEnumerator GoPlayerSingleTarget(GameObject character, GameObject target, string movement)
    {
        wait = true;

        var SpMiniGame = library.CheckSpeacialAttack(movement);

        //Aqui hay que activar el panel del minijuego
        if (SpMiniGame)
        {
            foreach (Transform child in panelMiniGame.transform)
                panelSpMini.Add(child.gameObject);
            
            ActivatePanelSpMini();
            yield return new WaitForSeconds(0.2f);
            yield return StartCoroutine(specialMiniGame.ChargeSpecialAtk());
        }
        else
            AddSPValue(character.GetComponent<Stats>());

        DisablePanelSpMini();

        bool dontStop = true, change = true;

        Vector2 v = character.transform.position;

        do
        {
            Animator animator = character.GetComponent<Animator>();

            animator.SetInteger("State", 1);

            if (change)
                character.transform.position = Vector2.MoveTowards(character.transform.position, target.transform.position, speed * Time.deltaTime);
            else
                character.transform.position = Vector2.MoveTowards(character.transform.position, v, speed * Time.deltaTime);

            if (Vector2.Distance(character.transform.position, target.transform.position) < 100f && change)
            {
                animator.SetInteger("State", 2);

                yield return new WaitForSeconds(1f);

                library.Library(character, target, movement, statesLibrary, battleModifiersLibrary); // Realiza el ataque.

                change = false;

                if (character.GetComponent<Stats>().Attacking == false)
                {
                    animator.SetInteger("State", 1);
                    change = false;
                }
            }

            if (Vector2.Distance(character.transform.position, v) < 0.001f && !change)
            {
                dontStop = false;
                animator.SetInteger("State", 0);
            }

            yield return new WaitForSeconds(0.00001f);
        } while (dontStop);

        moves++;

        StartCoroutine(CharacterDead(target, true));
        StartCoroutine(CharacterDead(character, false));

        this.character = null; this.movement = null;

        wait = false;

        CheckIfIsEnemyTurn();

        yield return null;
    }

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
            if (characterTarget.GetComponent<Stats>().Health == 0)
            {
                if (players.Length != 0)
                    characterTarget = players[Random.Range(0, players.Length)];
                else
                    characterMove.Execute = false;
            }

            if (characterMove.Execute)
            {
                SetTextInEnemyActionBar(characterMove.Character.name, characterMove.Movement);
                AddSPValue(characterMove.Character.GetComponent<Stats>());

                bool isAOE = library.CheckAoeAttack(characterMove.Movement);
                if (isAOE)
                {
                    var healOrNot = library.CheckAttackOrHeal(characterMove.Movement);

                    if (healOrNot)
                        enemys.ForEach(t => { library.Library(characterMove.Character, t, characterMove.Movement, statesLibrary, battleModifiersLibrary); });
                    else
                        players.ToList().ForEach(t => { library.Library(characterMove.Character, t, characterMove.Movement, statesLibrary, battleModifiersLibrary); });


                    foreach (GameObject t in players.ToArray())
                        StartCoroutine(CharacterDead(t, false));
                }
                else
                {
                    bool dontStop = true, change = true, onAttack = false;

                    Vector2 v = characterMove.Character.transform.position;

                    Animator animator = characterMove.Character.GetComponent<Animator>();

                    animator.SetInteger("State", 1);

                    do
                    {
                        if (change && !onAttack)
                            characterMove.Character.transform.position = Vector2.MoveTowards(characterMove.Character.transform.position, characterTarget.transform.position, speed * Time.deltaTime);
                        else if (!change && !onAttack)
                            characterMove.Character.transform.position = Vector2.MoveTowards(characterMove.Character.transform.position, v, speed * Time.deltaTime);

                        if (Vector2.Distance(characterMove.Character.transform.position, characterTarget.transform.position) < 100f && change)
                        {
                            animator.SetInteger("State", 2);

                            yield return new WaitForSeconds(0.3f);

                            if (!onAttack)
                            {
                                library.Library(characterMove.Character, characterTarget, characterMove.Movement, statesLibrary, battleModifiersLibrary); // Realiza el ataque.

                                StartCoroutine(CharacterDead(characterTarget, false));

                                StartCoroutine(CharacterDead(characterMove.Character, true));

                                onAttack = true;
                            }

                            if (characterMove.Character.GetComponent<Stats>().Attacking == false)
                            {
                                animator.SetInteger("State", 1);
                                change = false;
                                onAttack = false;
                            }
                        }

                        if (Vector2.Distance(characterMove.Character.transform.position, v) < 0.001f && !change)
                        {
                            dontStop = false;
                            animator.SetInteger("State", 0);
                        }

                        yield return new WaitForSeconds(0.00001f);
                    } while (dontStop);
                }
            }

            // Una vez terminado el turno de los enemigos vuelve a activar los botones de los jugadores.
            playerBT.ForEach(bt =>
            {
                bt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.black;
                bt.GetComponent<Button>().interactable = true;
            });
        }

        movements.Clear();
        moves = 0;

        wait = false;

        NextTurn();

        yield return null;
    }

    // Funcion para comprobar si un personaje esta muerto segun su vida.
    private IEnumerator CharacterDead(GameObject target, bool enemy)
    {
        Stats targetST = target.GetComponent<Stats>();

        if (targetST.Health == 0)
        {
            if (enemy)
                DeleteEnemyFromList(target);
            else
            {
                DeleteAllieFromArray(target);
                moves--;
            }
        }
        yield return null;
    }

    // Funcion para que el personaje seleccionado recupere mana sin hacer ningun daño.
    public void PassTurn(string movement)
    {
        wait = true;

        AudioManager.Instance.PlaySoundButtons();

        DesactivatePlayerButton();
        DesactivateAllButtons();

        library.PassTurn(character, movement.ToUpper());

        moves++;

        character = null; 
        this.movement = null;

        wait = false;
        CheckIfIsEnemyTurn();
    }


    // Funcion para el inventario en el combate 2D
    public void InventoryTurn()
    {
        if (moveBT.Count > 0)
            moveBT.ForEach(bt => Destroy(bt));

        moveBT.Clear();

        wait = true;

        DesactivatePlayerButton();
        DesactivateAllButtons();

        moves++;

        character = null; 
        movement = null;

        wait = false;

        CheckIfIsEnemyTurn();
    }

    public void SpecialAttackTurn()
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
                Destroy(moveBT);

            moveBT.Clear();

            AudioManager.Instance.PlaySoundButtons();

            //Si el ATK es Special saldra
            if (library.CheckSpeacialAttack(character.GetComponent<Stats>().AtkSpecial.ToUpper()))
            {
                // Creamos un boton de movimiento.
                GameObject bt = Instantiate(buttonRef, spawnMoveBT.transform.position, Quaternion.identity);
                
                bt.transform.SetParent(spawnMoveBT.transform);
                bt.name = "NameAtk " + character.GetComponent<Stats>().AtkSpecial;//Nombre de los botones que se van a generar
                bt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = character.GetComponent<Stats>().AtkSpecial;
                bt.GetComponent<Button>().onClick.AddListener(delegate { MovementButton(character.GetComponent<Stats>().AtkSpecial); });
                bt.transform.localScale = new Vector3(1f, 1f, 1f);
                
                moveBT.Add(bt);

                // Comprobamos si tiene suficiente puntos especiales, si no lo es, desactivamos el boton.
                var isPointSpEnough = library.CheckIfAtkSpecialPoints(character, character.GetComponent<Stats>().AtkSpecial.ToUpper());
                if (!isPointSpEnough)
                    bt.GetComponent<Button>().interactable = false;
            }
        }
    }

    // Funcion para desactivar todos los botones activos, a excepcion de los aliados del jugador.
    private void DesactivateAllButtons()
    {
        moveBT.ForEach(bt => { bt.SetActive(false); }); // Desactiva todos los botones movimiento.

        combatOptionsBT.ForEach(bt => bt.SetActive(false)); // Desactiva todos los botones de opciones de combate.

        enemyBT.ForEach(bt => { bt.SetActive(false); }); // Desactiva todos los botones de targets en caso de que esten activados.

        if (playerInventory.InventoryState)
            playerInventory.OpenCloseInventory(); // Cierra el inventario en caso de estar activado
    }

    // Funcion para determinar/empezar el turno enemigo.
    private void CheckIfIsEnemyTurn()
    {
        SetAllyActionBarInactive();

        if (GameObject.FindGameObjectsWithTag("Enemy").ToArray().Length <= 0)
            CheckBattleStatus();

        // Espera a que todos los jugadores hagan sus movimientos.
        if (moves >= players.Length && !wait)
            StartCoroutine(GoEnemy());
    }

    // Funcion para pasar al siguiente turno. Se comprueba el estado de los modificadores de batalla y de los estados.
    private void NextTurn()
    {
        ActualTurn++;

        GameObject[] allCharacters = GameObject.FindGameObjectsWithTag("Player").ToArray().Concat(GameObject.FindGameObjectsWithTag("Enemy").ToArray()).ToArray();
        statesLibrary.CheckStates(allCharacters);
        
        if (players.Length == 0)
            CheckBattleStatus();
        else
        {
            SetEnemyActionBarInactive();

            // Paso de turno para los estados
            battleModifiersLibrary.PassTurnOfModifiers();

            if (GameObject.FindGameObjectsWithTag("Enemy").ToArray().Length <= 0)
                CheckBattleStatus();
        }
    }//fin de NextTurn

    // Funcion para desactivar al jugador que haya realizado una accion durante el turno.
    private void DesactivatePlayerButton()
    {
        playerBT.ForEach(bt =>
        {
            // Si el texto coincide con el nombre del jugador aplica los cambios a dicho boton.
            if (bt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text == character.name.Remove(character.name.IndexOf("("))) // character.name.Substring(1, character.name.Length - 1)
            {
                bt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.grey;
                bt.GetComponent<Button>().interactable = false;
            }
        });
    }

    // Funcion para comprobar el estado de la batalla.
    private void CheckBattleStatus()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").ToArray().Length == 0)
        {
            var experience = totalEXP / players.LongLength; // Reparto de experiencia

            postBattleTeam.ActivatePanel();

            int[] levelTemp = new int[players.Length];

            for (int i = 0; i < players.Length; i++)
            {
                levelTemp[i] = players[i].GetComponent<Stats>().Level;
            }

            GameObject.FindGameObjectsWithTag("Player").ToList().ForEach(p => p.GetComponent<LevelSystem>().GainExperienceFlatRate(experience));

            StartCoroutine(postBattleTeam.InfoPanelTeam(players));

            GameObject.FindGameObjectsWithTag("Player").ToList().ForEach(p =>
            {
                var i = 0;

                AttackData possibleAttack = null;
                
                if(p.GetComponent<Stats>().Level > levelTemp[i])
                {
                    if (p.GetComponent<LearnableAttacks>().CanILearnAttack(p.GetComponent<Stats>().Level))
                    {
                        possibleAttack = p.GetComponent<LearnableAttacks>().ReturnAttack(p.GetComponent<Stats>().Level);

                        if (!p.GetComponent<Stats>().CheckIfIHaveThatAttack(possibleAttack) && !p.GetComponent<Stats>().CheckListAtkMax())
                        {
                            charactersWhoCanLearnAnAttack.Add(new(p, possibleAttack));
                        }
                    }
                }
                
                i++;
            });

            if (charactersWhoCanLearnAnAttack.Count > 0) ProcessingNewAttacks();
            else StartCoroutine(FinishBattle());

        } else if (players.Length == 0)
        {
            StartCoroutine(FinishBattle());
        }
    }

    // Funcion para comprobar que el diccionario de personajs y ataques este vacio.
    public void CheckIfMoreCharactersWantNewAttack()
    {
        learningAttacksManager.HideInterface();

        if (charactersWhoCanLearnAnAttack.Count > 0) charactersWhoCanLearnAnAttack.RemoveAt(0); // Por algun motivo desconocido, daba nulo si no pongo el If; funciona sin el, pero asi nos evitamos el mensaje de nulo.

        if (charactersWhoCanLearnAnAttack.Count > 0) ProcessingNewAttacks();
        else StartCoroutine(FinishBattle());
    }

    // Funcion para terminar la batalla.
    public IEnumerator FinishBattle()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").ToArray().Length == 0)
        {
            AudioManager.Instance.StopSoundCombat();
          
            yield return new WaitForSeconds(3);
            
            enterBattle.FinishBattleAndEnterOverworld();
        }
        if (players.Length == 0)
        {
            SetEnemyActionBarInactive();
            
            StartCoroutine(loserReset.ShowImage());

            AudioManager.Instance.StopSoundCombat();

            yield return new WaitForSeconds(3);
        }

        // Se resetea la información del combate para el proximo encuentro
        battleModifiersLibrary.RemoveAllBattleModifiers();
        
        actualTurn = 0;
        moves = 0;
    }

    // Funcion para enviar la informacion al panel de aprendizaje de ataques.
    public void ProcessingNewAttacks()
    {
        learningAttacksManager.ActivatePanel();

        learningAttacksManager.SetNewAttackInfo(charactersWhoCanLearnAnAttack[0].Character, charactersWhoCanLearnAnAttack[0].NewAttack);
    }

    // Funcion de IA enemiga.
    public void CheckAtkEnemy()
    {
        foreach (GameObject enemy in enemys)
        {
            List<string> listAtkEnemy = enemy.GetComponent<Stats>().ListNameAtk;
            string nameAtkEnemy = "";

            GameObject target = null;

            bool isManaEnough = false;
            // Comprobamos si el mana es suficiente, si no lo es, desactivamos el boton.
            while (!isManaEnough && listAtkEnemy.Count > 0)
            {
                //Int 1 lista de ataques del enemigo
                int i = Random.Range(0, listAtkEnemy.Count);

                //Chequeamos si tiene mana o no
                isManaEnough = library.CheckIfManaIsEnough(enemy, listAtkEnemy[i]);

                if (isManaEnough)
                    nameAtkEnemy = listAtkEnemy[i];
                else
                    listAtkEnemy.Remove(listAtkEnemy[i]);
            }

            if (listAtkEnemy.Count <= 0)
                library.PassTurn(enemy, "PASS TURN");
            else
            {
                bool healOrNot = library.CheckAttackOrHeal(nameAtkEnemy);

                if (healOrNot)
                {
                    //Int e lista de enemigos
                    int e = Random.Range(0, enemys.Count);

                    target = enemys[e];
                    AddMovement(enemy, target, nameAtkEnemy);
                }
                else
                {
                    //Int z lista de jugadores
                    int z = Random.Range(0, players.Length);

                    target = players[z];
                    AddMovement(enemy, target, nameAtkEnemy);
                }

            }

            // Comprobacion de accion de jefe final
            if (enemy.GetComponent<Stats>().Boss)
                enemy.GetComponent<CombatBoss>().CheckAtkEnemyBoss(target, battleModifiersLibrary);
        }
    }

    // Funcion para eliminar a un enemigo de la lista. Se suma la moneda de juego que tenga el enemigo al jugador.
    public void DeleteEnemyFromList(GameObject enemy)
    {
        enemys.Remove(enemy);
        moneyPlayer.Money += enemy.GetComponent<Stats>().MoneyDrop;

        if (enemy.GetComponent<Stats>().Boss)
            moneyPlayer.MoneyRefined += enemy.GetComponent<Stats>().MoneyRefinedDrop;
    }

    // Funcion para eliminar aliados de la lista de personajes vivos.
    public void DeleteAllieFromArray(GameObject ally)
    {
        List<GameObject> actualPlayers = players.ToList();

        actualPlayers.Remove(ally);

        playersDefeated.Add(ally);

        players = actualPlayers.ToArray();

        GeneratePlayersButtons();
    }

    // Funcion para comprobar si un aliado ha sido revivido.
    public void CheckIfAnAllyHasRevived()
    {
        GameObject someoneHasRevived = null;

        if (playersDefeated.Count != 0)
        {
            playersDefeated.ForEach(x =>
            {
                if (x.GetComponent<Stats>().Health > 0)
                {
                    List<GameObject> actualPlayers = players.ToList();
                    actualPlayers.Add(x);
                    players = actualPlayers.ToArray();

                    someoneHasRevived = x;
                }
            });
        }

        if (someoneHasRevived != null)
        {
            playersDefeated.Remove(someoneHasRevived);

            someoneHasRevived.transform.SetAsLastSibling();

            character = someoneHasRevived;
            GeneratePlayerBackToLifeButton(someoneHasRevived);

            DesactivatePlayerButton();
            moves++;

            character = null;
        }
    }

    // Agrega puntos para hacer el ATK Special
    private void AddSPValue(Stats characterST)
    {
        if (characterST.CanYouLaunchAnSpecialAtk) characterST.SP += SpGenerationPerAction;
    }

    // Funcion para exponer la informacion del personaje seleccionado por el jugador.
    private void SetTextInAllyActionBar()
    {
        if (!AllyActionBarText.transform.parent.gameObject.activeSelf) AllyActionBarText.transform.parent.gameObject.SetActive(true);

        AllyActionBarText.text = $"{Character.name.Remove(Character.name.IndexOf("("))} | HP: {Character.GetComponent<Stats>().Health}/{Character.GetComponent<Stats>().MaxHealth} | Mana: {Character.GetComponent<Stats>().Mana}/{Character.GetComponent<Stats>().MaxMana} | SP: {Character.GetComponent<Stats>().SP}/{Character.GetComponent<Stats>().MaxSP}";
    }

    // Funcion para exponer la informacion del enemigo que ataque.
    private void SetTextInEnemyActionBar(string enemyName, string attackName)
    {
        if (!EnemyActionBarText.transform.parent.gameObject.activeSelf) EnemyActionBarText.transform.parent.gameObject.SetActive(true);

        EnemyActionBarText.text = $"{enemyName.Remove(enemyName.IndexOf("("))} | Attack: {attackName}";
    }

    // Funcion para desactivar la barra de accion del bando aliado.
    private void SetAllyActionBarInactive()
    {
        AllyActionBarText.transform.parent.gameObject.SetActive(false);
    }

    // Funcion para desactivar la barra de accion del bando enemigo.
    private void SetEnemyActionBarInactive()
    {
        EnemyActionBarText.transform.parent.gameObject.SetActive(false);
    }

    // Funcion para activar el panel de mini juego.
    private void ActivatePanelSpMini()
    {
        panelMiniGame.SetActive(true);
    }

    // Funcion para desactivar el panel de mini juego.
    private void DisablePanelSpMini()
    {
        panelMiniGame.SetActive(false);
    }

    // Funcion para devolver el numero de enemigos restante
    public int NumEnemy()
    {
        return enemys.Count;
    }
}