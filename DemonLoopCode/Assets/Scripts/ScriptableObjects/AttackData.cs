using UnityEngine;

public enum Types { FIRE, WATER, PLANT, LIGHT, DARKNESS }

public enum ActionStates { NONE, HEAL, INFLICT }

[CreateAssetMenu]
public class AttackData : ScriptableObject
{
    [SerializeField] private float baseDamage;

    [SerializeField] private int phyAttack;

    [SerializeField] private int magicAttack;

    [SerializeField] private bool isAoeAttack;

    [SerializeField] private float manaCost;

    [SerializeField] private ActionStates generateAState;

    [SerializeField] private StateData stateAsociated;

    [SerializeField] private float probabilityOfState = 0;

    [SerializeField] private Types type;

    public float BaseDamage { get { return baseDamage; } }
    public int PhyAttack { get { return phyAttack; } }
    public int MagicAttack { get { return magicAttack; } }
    public bool IsAoeAttack { get { return isAoeAttack; } }
    public float ManaCost { get { return manaCost; } }
    public ActionStates GenerateAState { get { return generateAState; } }
    public string StateGenerated { get { return ObtainStateName(); } } // De esta forma se obtiene su nombre y se aplica en StatesLibrary sin tener que escribirlo a mano.
    public float ProbabilityOfState { get { return probabilityOfState; }}

    public Types Type { get { return type; } }

    private string ObtainStateName()
    {
        return stateAsociated.name.Substring(4, stateAsociated.name.Length - 4).ToUpper();
    }
}
