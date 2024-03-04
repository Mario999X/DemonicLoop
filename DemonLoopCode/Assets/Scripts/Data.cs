using System.Collections.Generic;
using UnityEngine;

// Clase principal para almacenamiento de datos y manejo de juego. Singleton.
public class Data : MonoBehaviour
{
    static Data instance;
    public static Data Instance { get { return instance; } }

    int saveRoom;
    int bossRoom = 4;
    int room;
    int floor;
    float money;
    bool onRun = false;
    string sceneName;

    // -- PLAYER TEAM LISTS
    [SerializeField] private List<StatsPersistenceData> charactersTeamStats = new();

    [SerializeField] private List<StatsPersistenceData> charactersBackupStats = new();

    public int SaveRoom { get { return saveRoom; } set { saveRoom = value; } }
    public int BossRoom { get { return bossRoom; } set { bossRoom = value; } }
    public int Room { get { return room; } set { room = value; } }
    public int Floor { get { return floor; } set { floor = value; } }
    public float Money { get { return money; } set { money = value; } }
    public bool OnRun { get { return onRun; } set { onRun = value; } }
    public string SceneName { get { return sceneName; } set { sceneName = value; } }
    public MoneyPlayer MoneyPlayer { get { return GetComponent<MoneyPlayer>(); } }
    public PlayerInventory PlayerInventory { get { return GetComponent<PlayerInventory>(); } }
    public List<StatsPersistenceData> CharactersTeamStats { get { return charactersTeamStats; } set { charactersTeamStats = value; } }
    public List<StatsPersistenceData> CharactersBackupStats { get { return charactersBackupStats; } set { charactersBackupStats = value; } }

    EnterBattle enterBattle;
    [SerializeField] PlayerMove player = null;
    LibraryStates libraryStates;
    DamageVisualEffect visualEffect;

    void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(this); }
    }

    private void Start()
    {
        enterBattle = GetComponent<EnterBattle>();
        libraryStates = GetComponent<LibraryStates>();

        #if UNITY_EDITOR
        foreach (StatsPersistenceData data in charactersTeamStats)
            data.Health = data.MaxHealth;
        #endif
    }

    void FixedUpdate()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Scene 2")
        {
            if (player == null)
            {
                player = GameObject.Find("Player").GetComponent<PlayerMove>();
                visualEffect = GameObject.Find("Global Volume").GetComponent<DamageVisualEffect>();
            }

            if (!enterBattle.OneTime) // 3D individual
            {
                foreach (StatsPersistenceData data in charactersTeamStats)
                {

                    if (data.ActualStates.Count > 0)
                    {
                        List<ActualStateData> statesToRemove = new();

                        visualEffect.Auch();

                        foreach (ActualStateData actualState in data.ActualStates)
                        {
                            StateData stateData = libraryStates.State(actualState.State);

                            if (player.Movement)
                            {
                                data.Time += Time.deltaTime;
                            }


                            if (data.Time >= stateData.TimeMoving)
                            {
                                if ((data.Health - stateData.BaseDamage) > 1)
                                    data.Health = data.Health - stateData.BaseDamage;
                                else
                                    data.Health = 1;

                                actualState.Turn++;
                                data.Time = 0;
                            }

                            if (stateData.TurnsDuration <= actualState.Turn)
                                statesToRemove.Add(actualState);
                        }

                        foreach (ActualStateData actualState in statesToRemove)
                            data.ActualStates.Remove(actualState);
                    }
                }
            }
        }
    }

    // Funcion para buscar a un personaje en especifico.
    public StatsPersistenceData SearchCharacterTeamStats(string characterName)
    {
        StatsPersistenceData charStats = null;

        charactersTeamStats.ForEach(x => {
            if (x.CharacterPB.name + "(Clone)" == characterName)
            {
                charStats = x;
            }
        });

        return charStats;
    }

    // Funcion para intercambiar posiciones dentro del equipo activo.
    public void SwitchActiveTeamPositions(StatsPersistenceData firstCharacter, StatsPersistenceData secondCharacter)
    {
        var firstIndex = charactersTeamStats.IndexOf(firstCharacter);
        var secondIndex = charactersTeamStats.IndexOf(secondCharacter);

        charactersTeamStats[firstIndex] = secondCharacter;
        charactersTeamStats[secondIndex] = firstCharacter;
    }

    // Funcion para intercambiar posiciones dentro del equipo secundario.
    public void SwitchBackupTeamPositions(StatsPersistenceData firstCharacter, StatsPersistenceData secondCharacter)
    {
        var firstIndex = charactersBackupStats.IndexOf(firstCharacter);
        var secondIndex = charactersBackupStats.IndexOf(secondCharacter);

        charactersBackupStats[firstIndex] = secondCharacter;
        charactersBackupStats[secondIndex] = firstCharacter;
    }

    // Funcion para intercambiar posiciones entre el equipo activo y el secundario. Primer personaje siendo del Activo.
    public void SwitchBetweenTeamPositionsFirstCharActiveTeam(StatsPersistenceData firstCharacter, StatsPersistenceData secondCharacter)
    {
        var firstIndex = charactersTeamStats.IndexOf(firstCharacter);
        var secondIndex = charactersBackupStats.IndexOf(secondCharacter);

        charactersTeamStats[firstIndex] = secondCharacter;
        charactersBackupStats[secondIndex] = firstCharacter;
    }

    // Funcion para intercambiar posiciones entre el equipo activo y el secundario. Primer personaje del Secundario.
    public void SwitchBetweenTeamPositionsFirstCharBackupTeam(StatsPersistenceData firstCharacter, StatsPersistenceData secondCharacter)
    {
        var firstIndex = charactersBackupStats.IndexOf(firstCharacter);
        var secondIndex = charactersTeamStats.IndexOf(secondCharacter);

        charactersBackupStats[firstIndex] = secondCharacter;
        charactersTeamStats[secondIndex] = firstCharacter;
    }
}
