using UnityEngine;

[CreateAssetMenu]
public class AttackData : ScriptableObject
{
    [SerializeField] private int baseDamage;

    [SerializeField] private int phyAttack;

    [SerializeField] private int magicAttack;

    [SerializeField] private bool isAoeAttack;

    public int BaseDamage { get { return baseDamage; } }
    public int PhyAttack { get { return phyAttack; } }
    public int MagicAttack { get { return magicAttack; } }
    public bool IsAoeAttack { get { return isAoeAttack; } }
}
