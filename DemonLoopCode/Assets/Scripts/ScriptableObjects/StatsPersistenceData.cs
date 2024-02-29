using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StatsPersistenceData : ScriptableObject
{
    [SerializeField] private GameObject characterPB;
    [SerializeField] [TextArea] string description;
    [SerializeField] private int level;
    [SerializeField] private float currentXP;
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private float mana;
    [SerializeField] private float maxMana;
    [SerializeField] private float sp;
    [SerializeField] private float strength;
    [SerializeField] private float physicalDef;
    [SerializeField] private float magicAtk;
    [SerializeField] private float magicDef;
    [SerializeField] private float criticalChance;
    [SerializeField] private List<AttackData> listAtk;
    [SerializeField] float cost;
    [SerializeField] bool protagonist = false;
    private List<ActualStateData> actualStates = new List<ActualStateData>();
    float time = 0;

    public string Description { get { return description; } }
    public float Time { get { return time; } set { time = value; } }
    public float Cost { get { return cost; } set { cost = value; } }
    public GameObject CharacterPB { get { return characterPB; } }
    public int Level { get { return level; } set { level = value; } }
    public float CurrentXP { get { return currentXP; } set { currentXP = value; } }
    public float Health { get { return health; } set { health = value; OnHealthChanged(); } }
    public float MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
    public float Mana { get { return mana; } set { mana = value; OnManaChanged(); } }
    public float MaxMana { get { return maxMana; } set { maxMana = value; } }
    public float SP { get { return sp; } set { sp = value; } }
    public float Strenght { get { return strength; } set { strength = value; } }
    public float PhysicalDefense { get { return physicalDef; } set { physicalDef = value; } }
    public float MagicAtk { get { return magicAtk; } set { magicAtk = value; } }
    public float MagicDef { get { return magicDef; } set { magicDef = value; } }
    public float CriticalChance { get { return criticalChance; } set { criticalChance = value; } }
    public List<AttackData> ListAtk { get { return listAtk; } set { listAtk = value; } }
    public bool Protagonist { get { return protagonist; } }
    public List<ActualStateData> ActualStates {  get { return actualStates; } set { actualStates = value; } }
    
    private void OnManaChanged()
    {
        if (mana >= maxMana)
        {
            mana = maxMana;
        }

        if (mana <= 0)
        {
            mana = 0;
        }
    }

    private void OnHealthChanged()
    {
        if (health >= maxHealth)
        {
            health = maxHealth;
        }

        if (health <= 0)
        {
            health = 0;
        }

    }//Fin de OnAttackReceived
}
