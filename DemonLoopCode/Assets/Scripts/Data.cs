using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    static Data instance;
    public static Data Instance { get { return instance; } }

    int saveRoom;
    int room;
    int floor;
    float money;

    [SerializeField] private List<StatsPersistenceData> charactersTeamStats = new();

    [SerializeField] private List<StatsPersistenceData> charactersBackupStats = new();

    public int SaveRoom { get { return saveRoom; } set { saveRoom = value; } }
    public int Room { get { return room; } set { room = value; } }
    public int Floor { get { return floor; } set { floor = value; } }
    public float Money { get { return money; } set { money = value; } }
    public List<StatsPersistenceData> CharactersTeamStats { get { return charactersTeamStats; } set { charactersTeamStats = value; } }
    public List<StatsPersistenceData> CharactersBackupStats { get { return charactersBackupStats; } set { charactersBackupStats = value; } }

    EnterBattle enterBattle;
    [SerializeField] PlayerMove player = null;
    LibraryStates libraryStates;
    DamageVisualEffect visualEffect;

    // Start is called before the first frame update
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
                    //Debug.Log(data.name);

                    if (data.ActualStates.Count > 0)
                    {
                        List<ActualStateData> statesToRemove = new List<ActualStateData>();

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

    public StatsPersistenceData SearchCharacterTeamStats(string characterName)
    {
        StatsPersistenceData charStats = null;

        charactersTeamStats.ForEach(x => {
            if (x.CharacterPB.name + "(Clone)" == characterName)
            {
                //Debug.Log("x.CharacterPB.name(Clone)");
                charStats = x;
            }
        });

        return charStats;
    }

    public void SwitchActiveTeamPositions(StatsPersistenceData firstCharacter, StatsPersistenceData secondCharacter)
    {
        var firstIndex = charactersTeamStats.IndexOf(firstCharacter);
        var secondIndex = charactersTeamStats.IndexOf(secondCharacter);

        //Debug.Log(firstIndex);
        //Debug.Log(secondIndex);

        charactersTeamStats[firstIndex] = secondCharacter;
        charactersTeamStats[secondIndex] = firstCharacter;
    }

    public void SwitchBackupTeamPositions(StatsPersistenceData firstCharacter, StatsPersistenceData secondCharacter)
    {
        var firstIndex = charactersBackupStats.IndexOf(firstCharacter);
        var secondIndex = charactersBackupStats.IndexOf(secondCharacter);

        //Debug.Log(firstIndex);
        //Debug.Log(secondIndex);

        charactersBackupStats[firstIndex] = secondCharacter;
        charactersBackupStats[secondIndex] = firstCharacter;
    }

    public void SwitchBetweenTeamPositionsFirstCharActiveTeam(StatsPersistenceData firstCharacter, StatsPersistenceData secondCharacter)
    {
        var firstIndex = charactersTeamStats.IndexOf(firstCharacter);
        var secondIndex = charactersBackupStats.IndexOf(secondCharacter);

        //Debug.Log(firstIndex);
        //Debug.Log(secondIndex);

        charactersTeamStats[firstIndex] = secondCharacter;
        charactersBackupStats[secondIndex] = firstCharacter;
    }

    public void SwitchBetweenTeamPositionsFirstCharBackupTeam(StatsPersistenceData firstCharacter, StatsPersistenceData secondCharacter)
    {
        var firstIndex = charactersBackupStats.IndexOf(firstCharacter);
        var secondIndex = charactersTeamStats.IndexOf(secondCharacter);

        //Debug.Log(firstIndex);
        //Debug.Log(secondIndex);

        charactersBackupStats[firstIndex] = secondCharacter;
        charactersTeamStats[secondIndex] = firstCharacter;
    }
}
