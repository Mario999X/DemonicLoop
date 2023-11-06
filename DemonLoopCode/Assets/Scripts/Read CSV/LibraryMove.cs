using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AttackData{

    private int baseDamage;

    private int phyAttack;

    private int magicAttack;

    public int BaseDamage { get { return baseDamage; } set { baseDamage = value; } }
    public int PhyAttack { get { return phyAttack; } set { phyAttack = value; } }
    public int MagicAttack { get { return magicAttack; } set { magicAttack = value; } }

    public AttackData(){}

    public AttackData(int BaseDamage, int PhyAttack, int MagicAttack){
        this.BaseDamage = BaseDamage;
        this.PhyAttack = PhyAttack;
        this.MagicAttack = MagicAttack;
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

private AttackData CheckAttack(string movement){
    // Primero, comprobamos si se encuentra en el diccionario, que funciona a modo de cache durante una pelea.
    var foundInCache = false;

    AttackData attackData = new();

    if(attackCache.ContainsKey(movement.ToUpper())){
        attackData = attackCache[movement.ToUpper()];
        foundInCache = true;

        Debug.Log("Ataque "+ movement + " | danno base " + attackData.BaseDamage.ToString() + " | CACHE");
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
        //string name = row[0];

        for (int i = 0; i < separator.Length; i++)
        {
            if ((i % 4) == 0)
            {
                if (row[i].ToUpper() == movement.ToUpper())
                {
                    string attackName = row[i].ToUpper();
                    int BaseDamage = Convert.ToInt32(row[i + 1]);
                    int PhyAttack = Convert.ToInt32(row[i + 2]);
                    int MagicAttack = Convert.ToInt32(row[i + 3]);

                    attackData = new AttackData(BaseDamage, PhyAttack, MagicAttack);
                    
                    attackCache.Add(attackName, attackData);

                    Debug.Log("Ataque "+ attackName + " | danno base " + BaseDamage.ToString() + " | CSV");
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

    if(attackInfo.PhyAttack == 0 && attackInfo.MagicAttack == 0){
        healing = true;
    }

    return healing;
}
}
