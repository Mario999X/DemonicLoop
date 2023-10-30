using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryMove : MonoBehaviour
{
    GameObject character;
    GameObject target;

    public void Library(GameObject character, GameObject target, string movement)
    {
        this.character = character;
        this.target = target;

        Invoke(movement, 0);
    }

    public void punch()
    {
        Stats target_ST = target.GetComponent<Stats>();
        Stats character_ST = character.GetComponent<Stats>();

        float damage = 5 + character_ST.Strenght - target_ST.Defense;

        if (damage <= 0)
            target_ST.Health -= 1;
        else
            target_ST.Health -= damage;

        character = null; target = null;
    }
}
