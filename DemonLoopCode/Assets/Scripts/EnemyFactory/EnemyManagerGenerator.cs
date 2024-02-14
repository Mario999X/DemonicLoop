using UnityEngine;

// Esta clase se encarga de generar a los demonios que tenga almacenados en las listas según el nivel principal de la amenaza.
public class EnemyManagerGenerator : MonoBehaviour
{
    [Header("Normal Enemies")]
    [SerializeField] GameObject[] enemyPrefabsLevel1;

    [SerializeField] GameObject[] enemyPrefabsLevel2;

    [SerializeField] GameObject[] enemyPrefabsLevel3;

    [Header("Special Enemies")]
    [SerializeField] GameObject mimicEnemy;

    [SerializeField] GameObject boss1;
    [SerializeField] GameObject boss2;

    // Funcion para generar los demonios segun el nivel de amenaza que se indique.
    public GameObject GenerateDemon(int disasterLevel)
    {
        GameObject demon = null;
        int randomDemon;

        switch (disasterLevel)
        {
            case 0: // Mimic Enemy
                demon = mimicEnemy;
                break;

            // Normal Enemies Arrays
            case 1:
                randomDemon = Random.Range(0, enemyPrefabsLevel1.Length);

                demon = enemyPrefabsLevel1[randomDemon];
                break;

            case 2:
                randomDemon = Random.Range(0, enemyPrefabsLevel2.Length);

                demon = enemyPrefabsLevel2[randomDemon];
                break;

            case 3:
                randomDemon = Random.Range(0, enemyPrefabsLevel3.Length);

                demon = enemyPrefabsLevel3[randomDemon];
                break;

            // Bosses Enemies
            case 4: 
                demon = boss1;
                break;

            case 5:
                demon = boss2;
                break;
        }

        return demon;
    }

}