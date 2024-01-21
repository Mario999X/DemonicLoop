using UnityEngine;

public enum Types { Fire, Water, Plant, Light, Darkness }

public enum ActionStates { None, Heal, Inflict }

[CreateAssetMenu]
public class AttackData : ScriptableObject
{
    [SerializeField] private float baseDamage;

    [SerializeField] [Range(0,1)] private int phyAttack;

    [SerializeField] [Range(0,1)] private int magicAttack;

    [SerializeField] private bool isAoeAttack;

    [SerializeField] private float manaCost;

    [SerializeField] private bool berserker;

    [SerializeField] private ActionStates generateAState;

    [SerializeField] private StateData stateAsociated;

    [SerializeField] [Range(0,100)] private float probabilityOfState = 0;

    [SerializeField] private Types type;

    [SerializeField] private bool lifeTheft;

    [SerializeField] private bool manaTheft;

    [SerializeField] [Range(0,100)] private int levelRequired;

    [SerializeField] private BattleModifiers battleModifierAsociated;

   
    public float BaseDamage { get { return baseDamage; } }
    public int PhyAttack { get { return phyAttack; } }
    public int MagicAttack { get { return magicAttack; } }
    public bool IsAoeAttack { get { return isAoeAttack; } }
    public float ManaCost { get { return manaCost; } }
    public bool Berserker { get { return berserker; } }
    public ActionStates GenerateAState { get { return generateAState; } }
    public string StateGenerated { get { return ObtainStateName(); } } // De esta forma se obtiene su nombre y se aplica en StatesLibrary sin tener que escribirlo a mano.
    public float ProbabilityOfState { get { return probabilityOfState; } }
    public bool LifeTheft { get { return lifeTheft; } }
    public bool ManaTheft { get { return manaTheft; } }
    public Types Type { get { return type; } }

    public int LevelRequired { get { return levelRequired; } }

    public BattleModifiers BattleModifierAsociated { get { return battleModifierAsociated; } }

    private string ObtainStateName()
    {
        return stateAsociated.name.Substring(4, stateAsociated.name.Length - 4).ToUpper();
    }
}