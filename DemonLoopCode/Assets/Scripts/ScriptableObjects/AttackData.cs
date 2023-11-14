using UnityEngine;

public enum Types { FIRE, WATER, PLANT, LIGHT, DARKNESS }

[CreateAssetMenu]
public class AttackData : ScriptableObject
{
    [SerializeField] private float baseDamage;

    [SerializeField] private int phyAttack;

    [SerializeField] private int magicAttack;

    [SerializeField] private bool isAoeAttack;

    [SerializeField] private float manaCost;

    [SerializeField] private Types type;

    public float BaseDamage { get { return baseDamage; } }
    public int PhyAttack { get { return phyAttack; } }
    public int MagicAttack { get { return magicAttack; } }
    public bool IsAoeAttack { get { return isAoeAttack; } }
    public float ManaCost { get { return manaCost; } }
    public Types Type { get { return type; } }
}
