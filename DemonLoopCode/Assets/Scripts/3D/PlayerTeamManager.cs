using UnityEngine;

public class PlayerTeamManager : MonoBehaviour
{
    [SerializeField] private GameObject [] playersArrayTeam;

    public GameObject [] PlayersArrayTeam { get { return playersArrayTeam; } set { playersArrayTeam = value; }}
}
