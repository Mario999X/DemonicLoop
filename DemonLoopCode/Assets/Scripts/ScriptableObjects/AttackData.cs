using UnityEngine;


[CreateAssetMenu]
public class AttackData : ScriptableObject
{
    [SerializeField] private float baseDamage;

    [SerializeField] private int phyAttack;

    [SerializeField] private int magicAttack;

    [SerializeField] private bool isAoeAttack;

    [SerializeField] private string type; //Fire,Water, Plant, Light

    public float BaseDamage { get { return baseDamage; } }
    public int PhyAttack { get { return phyAttack; } }
    public int MagicAttack { get { return magicAttack; } }
    public bool IsAoeAttack { get { return isAoeAttack; } }
    public string Type { get { return type; } }
}
