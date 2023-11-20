using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LibraryMove : MonoBehaviour
{
    // Tipos de daño por tipo posibles
    private const float SuperEffective = 1.5F;

    private const float NotVeryEffective = 0.5f;

    private const float NormalEffective = 1f;  

    // Daño al tener similitud de tipo respecto ataque y atacante
    private const float SameTypeDamage = 2.5f;

    private const float CriticalDamageMultiplier = 2f;

    private GameObject character;
    private GameObject target;

    private Dictionary<string, AttackData> attackCache = new();

    private FloatingTextCombat floatingText;

    private void Start()
    {
        LoadAttacks();

        floatingText = GetComponent<FloatingTextCombat>();
    }

    // Función que realiza un movimiento a un solo objetivo.
    public void Library(GameObject character, GameObject target, string movement)
    {
        this.character = character;
        this.target = target;

        Stats target_ST = target.GetComponent<Stats>();
        Stats character_ST = character.GetComponent<Stats>();

        var attack = CheckAttack(movement);

        var healOrAttack = CheckAttackOrHeal(movement);

        if (!healOrAttack)
        {
            float damage = DamageFull(target_ST, character_ST, attack);
            
            target_ST.Health -= damage;
        }
        else
        {
            target_ST.Health += attack.BaseDamage;

            floatingText.ShowFloatingText(target, attack.BaseDamage, Color.green);
        }
        if (attack.ManaCost!=0) ManaManager(attack, character_ST);

        character = null; target = null;
    }//Fin de Library

    // Carga inicial de ataques a la "cache"
    private void LoadAttacks()
    {
        string[] moves = AssetDatabase.FindAssets("ATK_");

        foreach (string m in moves)
        {
            string path = AssetDatabase.GUIDToAssetPath(m);

            ScriptableObject @object = AssetDatabase.LoadAssetAtPath<AttackData>(path);

            var atkName = @object.name.Substring(4, @object.name.Length - 4).Replace("^", " ").ToUpper();

            attackCache.Add(atkName, @object as AttackData);

            //Debug.Log("Ataque " + atkName + " | danno base " + (@object as AttackData).BaseDamage + " | LOADED TO CACHE");
        }
    }//Fin de LoadAttacks

    // Se llama para recibir la clase base de ataques. Se obtiene su informacion esencial.
    private AttackData CheckAttack(string movement)
    {
        AttackData attackData = null;

        if (attackCache.ContainsKey(movement.ToUpper()))
        {
            attackData = attackCache[movement.ToUpper()];

            //Debug.Log("Ataque " + movement.ToUpper() + " | danno base " + attackData.BaseDamage.ToString() + " | CACHE");
        }
        else
        {
            Debug.Log("ATAQUE NO ENCONTRADO, RECURRIENDO A PUNCH");

            attackData = attackCache["PUNCH"];
        }
        return attackData;
    }//Fin de CheckAttack

    // Comprobamos si el ataque es para hacer daño o para curar.
    public bool CheckAttackOrHeal(string movementName)
    {
        bool healing = false;

        var attackInfo = CheckAttack(movementName);

        if (attackInfo.PhyAttack == 0 && attackInfo.MagicAttack == 0)
        {
            healing = true;
        }

        return healing;
    }//Fin de CheckAttackOrHeal

    // Comprobamos si el ataque es AOE o single-target
    public bool CheckAoeAttack(string movementName)
    {
        bool isAOE = false;

        var attackInfo = CheckAttack(movementName);

        if (attackInfo.IsAoeAttack)
        {
            isAOE = true;
        }

        return isAOE;
    }//Fin de CheckAoeAttack

    public bool CheckIfManaIsEnough(GameObject characterST, string movementName){
        bool manaEnough = false;

        var attackInfo = CheckAttack(movementName);
        
        if(characterST.GetComponent<Stats>().Mana >= attackInfo.ManaCost)
        {
            manaEnough = true;
        }

        return manaEnough;
    }

    // Funcion para ejecutar el ataque que sirve para pasar de turno.
    public void PassTurn(GameObject characterST, string movementName){
        var attackData = CheckAttack(movementName);

        ManaManager(attackData, characterST.GetComponent<Stats>());
    }

    // Función para limpiar la cache.
    private void ResetCache()
    {
        attackCache.Clear();
    }//Fin de ResetCache

    private float DamageFull(Stats target_ST, Stats character_ST, AttackData attack)
    {
        float damage;
        float damagePhyAttack = (character_ST.Strenght * attack.PhyAttack);
        float damageMagic = (character_ST.MagicAtk * attack.MagicAttack);
        float defenseMagic = (target_ST.MagicDef * attack.MagicAttack);
        float defensePhy = (target_ST.PhysicalDefense * attack.PhyAttack);

        float damageType = DamageType(target_ST, attack);

        float damageTypeEnhancer = TypeEnhancer(character_ST, attack);

        float criticalDamage = CriticalDamage(character_ST.CriticalChance);

        //Lo hemos hecho asi para que se vea mejor
        damage = (attack.BaseDamage + damagePhyAttack + damageMagic - defenseMagic - defensePhy);
        damage = (damageType * damage) + (damageTypeEnhancer * damage);
        damage *= criticalDamage;

        if(damage <= 0) damage = 1;

        if(criticalDamage == CriticalDamageMultiplier)
        {
            floatingText.ShowFloatingText(target, damage, Color.red);

        } else floatingText.ShowFloatingText(target, damage, Color.white);

        Debug.Log(character_ST.name + " ataca a " +  target_ST.name + " con ataque: " + attack.name + " con un daño de: " + damage);
        return damage;
    }//Fin de DamageFull

    private float DamageType(Stats target_ST, AttackData attack)
    {
        float damageType = NormalEffective;

        switch (attack.Type)
        {
            case Types.FIRE:
                if (target_ST.Type == Types.PLANT)
                {
                    damageType = SuperEffective;
                }
                if (target_ST.Type == Types.WATER)
                {
                    damageType = NotVeryEffective;
                }
                break;

            case Types.PLANT:
                if (target_ST.Type == Types.WATER)
                {
                    damageType = SuperEffective;
                }
                if (target_ST.Type == Types.FIRE)
                {
                    damageType = NotVeryEffective;
                }
                break;

            case Types.WATER:
                if (target_ST.Type == Types.FIRE)
                {
                    damageType = SuperEffective;
                }
                if (target_ST.Type == Types.PLANT)
                {
                    damageType = NotVeryEffective;
                }
                break;

            case Types.LIGHT:
                if(target_ST.Type == Types.DARKNESS)
                {
                    damageType = SuperEffective;
                }
                break;
            
            case Types.DARKNESS:
                if(target_ST.Type == Types.LIGHT){
                    damageType = SuperEffective;
                }
                break;
        }
        
        return damageType;
    }//Fin de DamageType

    private float TypeEnhancer(Stats character_ST, AttackData attack)
    {
        //Cambiar los datos de los potenciadores de los tipos
        float damage = 0.0f;

        if(character_ST.Type == attack.Type) damage = SameTypeDamage;

        return damage;
    }//Fin de TypeEnhancer

    // Se devuelve un x2 en el daño, si no, se mantiene en 1 y el daño no es modificado.
    private float CriticalDamage(float CriticalStat)
    {
        float damage = NormalEffective;

        if(Random.Range(0, 100f) < CriticalStat) damage = CriticalDamageMultiplier;

        return damage;
    }

    private void ManaManager(AttackData attack, Stats characterStats){
        characterStats.Mana -= attack.ManaCost;

        // Para hacerlo más facil de ver visualmente, los ataques fisicos cuentan con un -, asi esos se vuelven positivos y los "positivos reales" se muestran negativos.
        floatingText.ShowFloatingText(characterStats.gameObject, -attack.ManaCost, Color.blue);
    }

}

