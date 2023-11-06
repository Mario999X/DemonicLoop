using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.IO;

public class LibraryMove : MonoBehaviour
{
    // En esta clase se realiza el ataque, por lo tanto, es la que avisa a Stats que regule la barra de vida del personaje especifico.
    public delegate void HealthManager();
    public event HealthManager OnAttackReceived;

    GameObject character;
    GameObject target;

    public void Library(GameObject character, GameObject target, string movement)
    {
        this.character = character;
        this.target = target;

        string ubicacionArchivo = Path.Combine(Application.dataPath, "Data", "Moves.csv");
        StreamReader archivo = new StreamReader(File.OpenRead(ubicacionArchivo));

        string separador = ",";
        string linea;

        archivo.ReadLine();
        while ((linea = archivo.ReadLine()) != null)
        {
            string[] fila = linea.Split(separador);
            string NAME = fila[0];

            for (int i = 0; i < separador.Length; i++)
            {
                if ((i % 4) == 0)
                {
                    if (fila[i].ToUpper() == movement.ToUpper())
                    {
                        int BASE_DAMAGE = Convert.ToInt32(fila[i + 1]);
                        int PHY_ATTACK = Convert.ToInt32(fila[i + 2]);
                        int MAGIC_ATTACK = Convert.ToInt32(fila[i + 3]);
                        Stats target_ST = target.GetComponent<Stats>();
                        Stats character_ST = character.GetComponent<Stats>();

                        Debug.Log($"Ataque " + fila[i] + " | danno base " + BASE_DAMAGE.ToString());

                        float damage = (BASE_DAMAGE + (character_ST.Strenght * PHY_ATTACK) + (character_ST.MagicAtk * MAGIC_ATTACK) - (target_ST.MagicDef * MAGIC_ATTACK) - (target_ST.Defense * PHY_ATTACK));

                        if (damage <= 0)
                            target_ST.Health -= 1;
                        else
                            target_ST.Health -= damage;

                        OnAttackReceived?.Invoke(); // Se avisan a los que estan suscritos a la funcion. Ver Start de la clase Stats.
                        character = null; target = null;

                        //Debug.Log("Name {0} danno base {1} danno phy {2} danno magic {3} ", NAME, BASE_DAMAGE, PHY_ATTACK, MAGIC_ATTACK);
                    }

                }
            }

        }
    }



    /*public void Punch()
    {
        Stats target_ST = target.GetComponent<Stats>();
        Stats character_ST = character.GetComponent<Stats>();

        float damage = 5 + character_ST.Strenght - target_ST.Defense;

        if (damage <= 0)
            target_ST.Health -= 1;
        else
            target_ST.Health -= damage;

        character = null; target = null;
    }*/
}
