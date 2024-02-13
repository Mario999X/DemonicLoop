using System.Collections.Generic;
using UnityEngine;

// Esta clase se encarga de generar a los demonios que tenga almacenados en las listas según el nivel principal de la amenaza.
public class EnemyManagerGenerator : MonoBehaviour
{
    [SerializeField] GameObject mimicEnemy;
    [SerializeField] List<GameObject> enemyPrefabsLevel0;

    [SerializeField] List<GameObject> enemyPrefabsLevel1;

    [SerializeField] List<GameObject> enemyPrefabsLevel2;

    [SerializeField] List<GameObject> enemyPrefabsLevel3;

    // Funcion para generar los demonios segun el nivel de amenaza que se indique.
    public GameObject GenerateDemon(int disasterLevel)
    {
        GameObject demon = null;
        int randomDemon;

        switch (disasterLevel)
        {
            case -1: // En el caso de mimicos
                demon = mimicEnemy;
                break;

            case 0: //En el 0 solo jefes
                randomDemon = Random.Range(0, enemyPrefabsLevel0.Count);

                demon = enemyPrefabsLevel0[randomDemon];
                break;
                
            case 1:
                randomDemon = Random.Range(0, enemyPrefabsLevel1.Count);

                demon = enemyPrefabsLevel1[randomDemon];
                break;

            case 2:
                randomDemon = Random.Range(0, enemyPrefabsLevel2.Count);

                demon = enemyPrefabsLevel2[randomDemon];
                break;

            case 3:
                randomDemon = Random.Range(0, enemyPrefabsLevel3.Count);

                demon = enemyPrefabsLevel3[randomDemon];
                break;
        }

        return demon;
    }

}