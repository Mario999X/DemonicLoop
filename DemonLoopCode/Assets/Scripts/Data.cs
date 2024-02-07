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
    public float Money { get { return money; } set {  money = value; } }
    public List<StatsPersistenceData> CharactersTeamStats { get { return charactersTeamStats;} set { charactersTeamStats = value; }}
    public List<StatsPersistenceData> CharactersBackupStats { get { return charactersBackupStats;} set { charactersBackupStats = value; }}

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(this); }
    }

    public StatsPersistenceData SearchCharacterTeamStats(string characterName)
    {
        StatsPersistenceData charStats = null;

        charactersTeamStats.ForEach(x => {
            if(x.CharacterPB.name + "(Clone)" == characterName)
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
