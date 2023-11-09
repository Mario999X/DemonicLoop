using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LibraryMove : MonoBehaviour
{
    // En esta clase se realiza el ataque, por lo tanto, es la que avisa a Stats que regule la barra de vida del personaje especifico.
    public delegate void HealthManager();
    public event HealthManager OnHealthChanged;

    private GameObject character;
    private GameObject target;

    private Dictionary<string, AttackData> attackCache = new();

    private void Start()
    {
        LoadAttacks();
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

            if (damage <= 0)
                target_ST.Health -= 1;

            else
                target_ST.Health -= damage;
        }
        else
        {
            target_ST.Health += attack.BaseDamage;
        }

        OnHealthChanged?.Invoke(); // Se avisan a los que estan suscritos a la funcion. Ver Start de la clase Stats.

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

            Debug.Log("Ataque " + atkName + " | danno base " + (@object as AttackData).BaseDamage + " | LOADED TO CACHE");
        }
    }//Fin de LoadAttacks

    // Se llama para recibir la clase base de ataques. Se obtiene su informacion esencial.
    private AttackData CheckAttack(string movement)
    {
        AttackData attackData = null;

        if (attackCache.ContainsKey(movement.ToUpper()))
        {
            attackData = attackCache[movement.ToUpper()];

            Debug.Log("Ataque " + movement.ToUpper() + " | danno base " + attackData.BaseDamage.ToString() + " | CACHE");

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

    // Función para limpiar la cache.
    private void ResetCache()
    {
        attackCache.Clear();
    }//Fin de ResetCache

    public float DamageFull(Stats target_ST, Stats character_ST, AttackData attack)
    {
        float damage;
        float damagePhyAttack = (character_ST.Strenght * attack.PhyAttack);
        float damageMagic = (character_ST.MagicAtk * attack.MagicAttack);
        float defenseMagic = (target_ST.MagicDef * attack.MagicAttack);
        float defensePhy = (target_ST.Defense * attack.PhyAttack);

        float damageType = DamageType(target_ST, attack);

        float damageTypeEnhancer = TypeEnhancer(character_ST, attack);

        //Lo hemos hecho asi para que se vea mejor
        damage = (attack.BaseDamage + damagePhyAttack + damageMagic - defenseMagic - defensePhy);
        damage = (damageType * damage) + (damageTypeEnhancer * damage);
        return damage;
    }//Fin de DamageFull

    public float DamageType(Stats target_ST, AttackData attack)
    {
        //Fire,Water, Plant, Light
        float damageType = 0.0f;
        switch (attack.Type)
        {
            case "Fire":
                if (target_ST.Type == "Plant")
                {
                    damageType = 1.5f;
                }
                if (target_ST.Type == "Water")
                {
                    damageType = 0.5f;
                }
                if (target_ST.Type == "Light" && target_ST.Type == "Fire")
                {
                    damageType = 1f;
                }
                break;

            case "Plant":
                if (target_ST.Type == "Water")
                {
                    damageType = 1.5f;
                }
                if (target_ST.Type == "Fire")
                {
                    damageType = 0.5f;
                }
                if (target_ST.Type == "Light" && target_ST.Type == "Plant")
                {
                    damageType = 1f;
                }
                break;

            case "Water":
                if (target_ST.Type == "Fire")
                {
                    damageType = 1.5f;
                }
                if (target_ST.Type == "Plant")
                {
                    damageType = 0.5f;
                }
                if (target_ST.Type == "Light" && target_ST.Type == "Water")
                {
                    damageType = 1f;
                }
                break;
        }


        return damageType;
    }//Fin de DamageType

    public float TypeEnhancer(Stats character_ST, AttackData attack)
    {
        //Cambiar los datos de los potenciadores de los tipos
        //Fire,Water, Plant, Light
        float damage = 0f;
        if (character_ST.Type == "Fire" && attack.Type == "Fire")
        {
            damage = 2.5f;
        }
        if (character_ST.Type == "Water" && attack.Type == "Water")
        {
            damage = 2.5f;
        }
        if (character_ST.Type == "Plant" && attack.Type == "Plant")
        {
            damage = 2.5f;
        }
        if (character_ST.Type == "Light" && attack.Type == "Light")
        {
            damage = 2.5f;
        }

        return damage;
    }//Fin de TypeEnhancer
}

