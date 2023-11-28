using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Esta clase se encarga de generar a los demonios que tenga almacenados en las listas según el nivel principal de la amenaza.
public class EnemyManagerGenerator : MonoBehaviour
{
    [SerializeField] List<GameObject> enemyPrefabsLevel1;

    [SerializeField] List<GameObject> enemyPrefabsLevel2;

    // Funcion para generar los demonios segun el nivel de amenaza que se indique.
    public GameObject GenerateDemon(int disasterLevel)
    {
        GameObject demon = null;
        int randomDemon;

        switch (disasterLevel)
        {
            case 1:
                randomDemon = Random.Range(0, enemyPrefabsLevel1.Count);

                demon = enemyPrefabsLevel1[randomDemon];
            break;

            case 2:
                randomDemon = Random.Range(0, enemyPrefabsLevel2.Count);

                demon = enemyPrefabsLevel2[randomDemon];
            break;
        }

        return demon;
    }

}
