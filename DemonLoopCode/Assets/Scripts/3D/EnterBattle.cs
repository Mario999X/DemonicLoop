using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterBattle : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject player;
    [SerializeField] Canvas fight;
    Animator crossfadeTransition;

    bool done = false;
    bool oneTime = false;

    Scene scene;

    public bool OneTime { get { return oneTime; } }

    LibraryStates libraryStates;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (scene != UnityEngine.SceneManagement.SceneManager.GetActiveScene())
        {
            done = false;
            scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        }

        if (!done && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Scene 2") // Cuando se inicia una escena espera que sea la correcta donde pueda encontrar los objetos "Player" y "Fight".
        {
            crossfadeTransition = GameObject.Find("Crossfade").GetComponent<Animator>();

            if (GameObject.Find("AlliesBattleZone").transform.childCount == 0)
            {
                Data.Instance.CharactersTeamStats.ForEach(x =>
                {
                    var go = Instantiate(x.CharacterPB, GameObject.Find("AlliesBattleZone").transform);

                    if(x.Level != 0)
                    {
                        var statsChar = go.GetComponent<Stats>();

                        statsChar.Level = x.Level;
                        statsChar.CurrentXP = x.CurrentXP;
                        statsChar.Health = x.Health;
                        statsChar.MaxHealth = x.MaxHealth;
                        statsChar.Mana = x.Mana;
                        statsChar.MaxMana = x.MaxMana;
                        statsChar.SP = x.SP;
                        statsChar.Strenght = x.Strenght;
                        statsChar.PhysicalDefense = x.PhysicalDefense;
                        statsChar.MagicAtk = x.MagicAtk;
                        statsChar.MagicDef = x.MagicDef;
                        statsChar.CriticalChance = x.CriticalChance;

                        statsChar.ListAtk = x.ListAtk;
                    }
                });
            }

            player = GameObject.Find("Player");
            fight = GameObject.Find("Fight").GetComponent<Canvas>();
            libraryStates = GetComponent<LibraryStates>();

            done = true;
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Scene 2") // Cuando no se encuentre en las escenas correspondientes las vuelven null. 
        {
            player = null;
            fight = null;
            crossfadeTransition = null;

            done = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && enemy != null) // Atajo para terminar la batalla.
        {
            FinishBattle();
        }
    }

    private int ObtainMaxLevelPlayer()
    {
        int maxLevelDetected = 0;

        List<int> playerlevels = new();

        if (GameObject.Find("AlliesBattleZone").transform.childCount > 0)
        {
            foreach (Transform child in GameObject.Find("AlliesBattleZone").transform)
            {
                playerlevels.Add(child.GetComponent<Stats>().Level);
            }

            maxLevelDetected = playerlevels.Max();
        }

        return maxLevelDetected;
    }

    private int SetEnemyLevel(int levelDetected)
    {
        int levelRandom = Random.Range(levelDetected - 2, levelDetected + 2 + 1);

        if (levelRandom < 1) levelRandom = 1;

        return levelRandom;
    }

    // Funcion para iniciar la batalla.
    public void StartBattle(GameObject enemy, bool sneak)
    {
        this.enemy = enemy;

        float totalExperience = 0;

        if (!oneTime)
        {
            var playerMaxLevel = ObtainMaxLevelPlayer();

            StartCoroutine(CrossfadeAnimation());

            StartCoroutine(GetComponent<CombatFlow>().CreateButtons());

            //Cuando entremos en combate suena la musica
            AudioManager.Instance.PlaySoundCombat();

            if (GameObject.Find("EnemyBattleZone").transform.childCount > 0)
            {
                foreach (Transform child in GameObject.Find("EnemyBattleZone").transform)
                    Destroy(child.gameObject);
            }

            this.enemy.GetComponent<EnemyGenerator>().ListEnemies.ForEach(x =>
            {
                GameObject go = Instantiate(x, GameObject.Find("EnemyBattleZone").transform);

                go.GetComponent<LevelSystem>().SetLevelEnemy(SetEnemyLevel(playerMaxLevel)); // Subida de nivel del enemigo
            });

            if (GameObject.Find("EnemyBattleZone").transform.childCount > 0)
            {
                foreach (Transform child in GameObject.Find("EnemyBattleZone").transform)
                    totalExperience += child.GetComponent<Stats>().DropXP;
            }

            GetComponent<CombatFlow>().TotalEXP = totalExperience;
            oneTime = true;

            libraryStates.IconState(); // Busca todos las entidades que sufran algun efecto de estado.

            fight.enabled = true;

            if (sneak)
            {
                Stats[] stats = GameObject.Find("EnemyBattleZone").GetComponentsInChildren<Stats>();

                foreach (Stats stat in stats)
                {
                    stat.Health -= stat.Health * 0.05f;
                }
            }
        }
    }

    // Funcion para finalizar batalla.
    public void FinishBattle()
    {
        StartCoroutine(CrossfadeAnimation());

        Destroy(enemy);
        
        SavePlayerCharacterStats();

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
        crossfadeTransition.SetBool("StartBattle", true);

        yield return new WaitForSeconds(0.5f);

        crossfadeTransition.SetBool("StartBattle", false);
    }

    private void SavePlayerCharacterStats()
    {
        foreach (Transform child in GameObject.Find("AlliesBattleZone").transform)
        {
            var statsChar = child.GetComponent<Stats>();

            var savedStats = Data.Instance.SearchCharacterTeamStats(child.name);

            savedStats.Level = statsChar.Level;
            savedStats.CurrentXP = statsChar.CurrentXP;
            savedStats.Health = statsChar.Health;
            savedStats.MaxHealth = statsChar.MaxHealth;
            savedStats.Mana = statsChar.Mana;
            savedStats.MaxMana = statsChar.MaxMana;
            savedStats.SP = statsChar.SP;
            savedStats.Strenght = statsChar.Strenght;
            savedStats.PhysicalDefense = statsChar.PhysicalDefense;
            savedStats.MagicAtk = statsChar.MagicAtk;
            savedStats.MagicDef = statsChar.MagicDef;
            savedStats.CriticalChance = statsChar.CriticalChance;

            savedStats.ListAtk = statsChar.ListAtk;

            Debug.Log("Character Saved after battle " + child.name);
        }
    }
}
