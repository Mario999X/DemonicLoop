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
        // Si la escena ha cambiado resetea los valores.
        if (scene != UnityEngine.SceneManagement.SceneManager.GetActiveScene())
        {
            done = false;
            scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();

            player = null;
            fight = null;
            crossfadeTransition = null;
            oneTime = false;
            done = false;
        }

        // Cuando se inicia una escena espera que sea la correcta donde pueda encontrar los objetos "Player" y "Fight".
        if (!done && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Scene 2") 
        {
            crossfadeTransition = GameObject.Find("Crossfade").GetComponent<Animator>();

            StartCoroutine(RespawnAlliesInBattle());

            player = GameObject.Find("Player");
            fight = GameObject.Find("Fight").GetComponent<Canvas>();
            libraryStates = GetComponent<LibraryStates>();

            done = true;
        }
    }

    // Spawnea los aliados dentro del canvas de combate.
    public IEnumerator RespawnAlliesInBattle()
    {
        GameObject alliesBattleZone = GameObject.Find("AlliesBattleZone");

        // Si ya hay alguno spawneado se destruyen.
        if (alliesBattleZone.transform.childCount > 0)
        {
            GetComponent<CombatFlow>().ResetPlayersInCombat();

            foreach (Transform child in alliesBattleZone.transform) Destroy(child.gameObject);

            yield return new WaitForSeconds(0.5f);
        }

        // Se spawnean de nuevo con los datos nuevos.
        if (alliesBattleZone.transform.childCount == 0)
        {
            Data.Instance.CharactersTeamStats.ForEach(x =>
            {
                GameObject go = Instantiate(x.CharacterPB, alliesBattleZone.transform);

                Stats statsChar = go.GetComponent<Stats>();

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
                statsChar.ActualStates = x.ActualStates;
                statsChar.ListAtk = x.ListAtk;
            });
        }

        GetComponent<CombatFlow>().SetPlayersInCombat(); // Para la persistencia de los que estan muertos.
    }

    // Devuelve el aliado de mayor nivel del grupo del jugador.
    private int ObtainMaxLevelPlayer()
    {
        int maxLevelDetected = 0;

        List<int> playerlevels = new();

        if (GameObject.Find("AlliesBattleZone").transform.childCount > 0)
        {
            foreach (Transform child in GameObject.Find("AlliesBattleZone").transform)
                playerlevels.Add(child.GetComponent<Stats>().Level);

            maxLevelDetected = playerlevels.Max();
        }

        return maxLevelDetected;
    }

    // Pone el nivel de los enemigos teniendo en cuenta el mayor nivel que haya en todo el grupo del jugador.
    // El radio del nivel del enemigo es entre dos niveles por abajo a dos niveles por encima del mayor nivel que haya en todo el grupo del jugador.
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

            StartCoroutine(GetComponent<CombatFlow>().FindEnemiesAndCreateAlliesButtons()); // Genera los botones.

            //Cuando entremos en combate suena la musica
            AudioManager.Instance.PlaySoundCombat();

            // Elimina cualquier enemigo que quedara de la anterior pelea.
            if (GameObject.Find("EnemyBattleZone").transform.childCount > 0)
                foreach (Transform child in GameObject.Find("EnemyBattleZone").transform)
                    Destroy(child.gameObject);

            this.enemy.GetComponent<EnemyGenerator>().ListEnemies.ForEach(x =>
            {
                GameObject go = Instantiate(x, GameObject.Find("EnemyBattleZone").transform);

                go.GetComponent<LevelSystem>().SetLevelEnemy(SetEnemyLevel(playerMaxLevel)); // Subida de nivel del enemigo
            });

            // Recuenta la cantidad de experiencia que daran los enemigos.
            if (GameObject.Find("EnemyBattleZone").transform.childCount > 0)
                foreach (Transform child in GameObject.Find("EnemyBattleZone").transform)
                    totalExperience += child.GetComponent<Stats>().DropXP;

            GetComponent<CombatFlow>().TotalEXP = totalExperience;
            oneTime = true;

            foreach (GameObject character in GameObject.FindGameObjectsWithTag("Player"))
                libraryStates.IconState(character); // Busca todos las entidades que sufran algun efecto de estado.

            GameObject.Find("Fight").GetComponent<Canvas>().enabled = true;

            // Si era un ataque sigiloso se les quitara algo de vida a los enemigos al principio del combate.
            if (sneak)
            {
                Stats[] stats = GameObject.Find("EnemyBattleZone").GetComponentsInChildren<Stats>();

                foreach (Stats stat in stats)
                    stat.Health -= stat.Health * 0.15f;
            }
        }
    }

    // Funcion para finalizar batalla.
    public void FinishBattleAndEnterOverworld()
    {
        StartCoroutine(CrossfadeAnimation());

        // Si el enemigo era un mimico este soltara objetos ademas de dinero y experiencia.
        if (enemy.CompareTag("Mimic"))
        {
            Content content = enemy.GetComponent<ChestContent>().chest();

            PlayerInventory inventory = GetComponent<PlayerInventory>();

            if (content.Count > 0)
                inventory.AddObjectToInventory(content.Data.name.Substring(4, content.Data.name.Length - 4), content.Data, content.Count);
        }

        SavePlayerCharacterStats();

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

    // Animacion de inicio y fin de pelea.
    private IEnumerator CrossfadeAnimation()
    {
        crossfadeTransition.SetBool("StartBattle", true);

        yield return new WaitForSeconds(0.5f);

        crossfadeTransition.SetBool("StartBattle", false);
    }

    // Guarda las estadisticas y datos de cada personaje del grupo.
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
            savedStats.ActualStates = statsChar.ActualStates;
            savedStats.ListAtk = statsChar.ListAtk;
        }

        GetComponent<TeamViewManager>().SetActiveTeamData();
    }
}
