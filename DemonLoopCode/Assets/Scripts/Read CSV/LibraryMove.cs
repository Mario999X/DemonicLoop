using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AttackData{

    private int baseDamage;

    private int phyAttack;

    private int magicAttack;

    public int baseDamageColumn { get { return baseDamage; } set { baseDamage = value; } }
    public int phyAttackColumn { get { return phyAttack; } set { phyAttack = value; } }
    public int magicAttackColumn { get { return magicAttack; } set { magicAttack = value; } }

    public AttackData(){}

    public AttackData(int baseDamageColumn, int phyAttackColumn, int magicAttackColumn){
        this.baseDamageColumn = baseDamageColumn;
        this.phyAttackColumn = phyAttackColumn;
        this.magicAttackColumn = magicAttackColumn;
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
            float damage = (attack.baseDamageColumn + (character_ST.Strenght * attack.phyAttackColumn) + (character_ST.MagicAtk * attack.magicAttackColumn) - (target_ST.MagicDef * attack.magicAttackColumn) - (target_ST.Defense * attack.phyAttackColumn));

            if (damage <= 0)
                target_ST.Health -= 1;
            else
                target_ST.Health -= damage;
        } else {

            target_ST.Health += attack.baseDamageColumn;
        }

        OnHealthChanged?.Invoke(); // Se avisan a los que estan suscritos a la funcion. Ver Start de la clase Stats.

        character = null; target = null;
    }

private AttackData CheckAttack(string movement){
    // Primero, comprobamos si se encuentra en el diccionario, que funciona a modo de cache durante una pelea.
    var foundInCache = false;

    AttackData attackData = new();

    if(attackCache.ContainsKey(movement.ToUpper())){
        attackData = attackCache[movement.ToUpper()];
        foundInCache = true;

        Debug.Log("Ataque "+ movement + " | danno base " + attackData.baseDamageColumn.ToString() + " | CACHE");
    }

    // Si no encontramos el ataque en la cache, lo buscamos en el fichero CSV. Una vez lo encontramos, lo almacenamos.
    if(!foundInCache){
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
                if (row[i].ToUpper() == movement.ToUpper())
                {
                    string attackNameColumn = row[i].ToUpper();
                    int baseDamageColumn = Convert.ToInt32(row[i + 1]);
                    int phyAttackColumn = Convert.ToInt32(row[i + 2]);
                    int magicAttackColumn = Convert.ToInt32(row[i + 3]);

                    attackData = new AttackData(baseDamageColumn, phyAttackColumn, magicAttackColumn);
                    
                    attackCache.Add(attackNameColumn, attackData);

                    Debug.Log("Ataque "+ attackNameColumn + " | danno base " + baseDamageColumn.ToString() + " | CSV");
                }
            }
        }
    }
}
    return attackData;
}

// Comprobamos si el ataque es para hacer daño o para curar.
public bool CheckAttackOrHeal(string movementName){
    bool healing = false;

    var attackInfo = CheckAttack(movementName);

    if(attackInfo.phyAttackColumn == 0 && attackInfo.magicAttackColumn == 0){
        healing = true;
    }

    return healing;
}

// Función para limpiar la cache.
private void ResetCache(){
    attackCache.Clear();
}

}
