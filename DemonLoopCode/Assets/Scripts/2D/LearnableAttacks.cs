using UnityEngine;

public class LearnableAttacks : MonoBehaviour
{
    [SerializeField] AttackData[] ownLearnableAttacks;

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
