using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AttackData{

    private int baseDamage;

    private int phyAttack;

    private int magicAttack;

    private int isAoeAttack;

    public int BaseDamage { get { return baseDamage; } set { baseDamage = value; } }
    public int PhyAttack { get { return phyAttack; } set { phyAttack = value; } }
    public int MagicAttack { get { return magicAttack; } set { magicAttack = value; } }
    public int IsAoeAttack { get { return isAoeAttack; } set { isAoeAttack = value; } }

    public AttackData(){}

    public AttackData(int baseDamage, int phyAttack, int magicAttack, int isAoeAttack){
        BaseDamage = baseDamage;
        PhyAttack = phyAttack;
        MagicAttack = magicAttack;
        IsAoeAttack = isAoeAttack;
    }
}

public class LibraryMove : MonoBehaviour
{
    // En esta clase se realiza el ataque, por lo tanto, es la que avisa a Stats que regule la barra de vida del personaje especifico.
    public delegate void HealthManager();
    public event HealthManager OnHealthChanged;

    private GameObject character;
    private GameObject target;

    private string fileLocation = Path.Combine(Application.dataPath, "Data", "Moves.csv");

    Dictionary<string, AttackData> attackCache = new();

    private void Start(){
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

        if(!healOrAttack){
            float damage = (attack.BaseDamage + (character_ST.Strenght * attack.PhyAttack) + (character_ST.MagicAtk * attack.MagicAttack) - (target_ST.MagicDef * attack.MagicAttack) - (target_ST.Defense * attack.PhyAttack));

            if (damage <= 0)
                target_ST.Health -= 1;
            else
                target_ST.Health -= damage;
        } else {

            target_ST.Health += attack.BaseDamage;
        }

        OnHealthChanged?.Invoke(); // Se avisan a los que estan suscritos a la funcion. Ver Start de la clase Stats.

        character = null; target = null;
    }

private void LoadAttacks(){
    StreamReader file = new (File.OpenRead(fileLocation));

    string separator = ",";
    string lines;

    file.ReadLine();
    while ((lines = file.ReadLine()) != null)
    {
        string[] row = lines.Split(separator);

        for (int i = 0; i < separator.Length; i++)
        {
            if ((i % 4) == 0)
            {
                string attackNameColumn = row[i].ToUpper();
                int baseDamageColumn = Convert.ToInt32(row[i + 1]);
                int phyAttackColumn = Convert.ToInt32(row[i + 2]);
                int magicAttackColumn = Convert.ToInt32(row[i + 3]);
                int isAoeAttackColumn = Convert.ToInt32(row[i + 4]);

                AttackData attackData = new (baseDamageColumn, phyAttackColumn, magicAttackColumn, isAoeAttackColumn);
                    
                attackCache.Add(attackNameColumn, attackData);

                Debug.Log("Ataque "+ attackNameColumn + " | danno base " + baseDamageColumn.ToString() + " | LOADED TO CACHE");
            }
        }
    }
}

private AttackData CheckAttack(string movement){
    AttackData attackData = new();

    if(attackCache.ContainsKey(movement.ToUpper())){
        attackData = attackCache[movement.ToUpper()];

        Debug.Log("Ataque "+ movement + " | danno base " + attackData.BaseDamage.ToString() + " | CACHE");
    }

    return attackData;
}

// Comprobamos si el ataque es para hacer daño o para curar.
public bool CheckAttackOrHeal(string movementName){
    bool healing = false;

    var attackInfo = CheckAttack(movementName);

    if(attackInfo.PhyAttack == 0 && attackInfo.MagicAttack == 0){
        healing = true;
    }

    return healing;
}

// Comprobamos si el ataque es AOE o single-target
public bool CheckAoeAttack(string movementName){
    bool isAOE = false;

    var attackInfo = CheckAttack(movementName);

    if(attackInfo.IsAoeAttack == 1){
        isAOE = true;
    }

    return isAOE;
}

// Función para limpiar la cache.
private void ResetCache(){
    attackCache.Clear();
}

}

