using System.Collections.Generic;
using UnityEngine;

// Esta clase sera la que lleven los enemigos visibles en el mundo 3D.
public class EnemyGenerator : MonoBehaviour
{
    [Header("Enemy Difficulty Level")]
    [SerializeField] int disasterLevel = 1;

    [SerializeField] private GameObject[] enemyModels;

    private List<GameObject> listEnemies = new(); // Lista de enemigos que apareceran en batalla.

    public int DisasterLevel { get { return disasterLevel; } }
    public List<GameObject> ListEnemies { get { return listEnemies; } }

    private EnemyManagerGenerator enemyManagerGenerator;
    private const int MinNumEnemies = 1;
    private const int MaxNumEnemies = 4;

    void Start()
    {
        enemyManagerGenerator = GameObject.Find("System").GetComponent<EnemyManagerGenerator>();

        SpawnEnemies();
    }

    // Funcion para generar enemigos y agregarlos al listado.
    private void SpawnEnemies()
    {
        int enemiesGenerated;

        if (disasterLevel == 0 || disasterLevel > 3)
        {
            enemiesGenerated = 1;

        } else
        {
            enemiesGenerated = Random.Range(MinNumEnemies, MaxNumEnemies);
            var gameObjectBody = transform.GetChild(2);

            switch(disasterLevel)
            {
                case 1:
                    Instantiate(enemyModels[disasterLevel - 1], gameObjectBody.transform.position, Quaternion.identity, gameObjectBody.transform);
                break;
                case 2:
                    Instantiate(enemyModels[disasterLevel - 1], gameObjectBody.transform.position, Quaternion.identity, gameObjectBody.transform);
                break;
                case 3:
                    Instantiate(enemyModels[disasterLevel - 1], gameObjectBody.transform.position, Quaternion.identity, gameObjectBody.transform);
                break;
            }
        } 

        for(int i = 1; i <= enemiesGenerated; i++ ){

            var enemy = enemyManagerGenerator.GenerateDemon(disasterLevel);
            
            listEnemies.Add(enemy);
        }
    }
}
