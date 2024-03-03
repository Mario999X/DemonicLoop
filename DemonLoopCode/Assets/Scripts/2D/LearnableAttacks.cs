using UnityEngine;

// Clase encargada de mantener la informacion de los ataques que puede aprender un personaje segun su nivel.
public class LearnableAttacks : MonoBehaviour
{
    [SerializeField] private AttackData[] ownLearnableAttacks;

    public bool CanILearnAttack(int level)
    {
        bool canI = false;

        foreach (AttackData x in ownLearnableAttacks)
        {
            if(x.LevelRequired == level) canI = true;
        }

        return canI;
    }

    public AttackData ReturnAttack(int level) 
    {
        AttackData attack = null;

        foreach (AttackData x in ownLearnableAttacks)
        {
            if(x.LevelRequired == level) attack = x;
        }

        return attack;
    }
}
