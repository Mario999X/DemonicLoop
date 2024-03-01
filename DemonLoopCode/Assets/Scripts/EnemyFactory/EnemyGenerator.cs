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
            GameObject gameObjectBody = transform.GetChild(2).gameObject;

            switch(disasterLevel)
            {
                case 1:
                    GameObject demon1 = Instantiate(enemyModels[disasterLevel - 1]);
                    demon1.transform.SetParent(gameObjectBody.transform);
                    demon1.transform.localRotation = Quaternion.Euler(0, -90f, 0);
                    demon1.transform.localPosition = demon1.transform.position + new Vector3(0,0.6f,0);
                    break;
                case 2:
                    GameObject demon2 = Instantiate(enemyModels[disasterLevel - 1]);
                    demon2.transform.SetParent(gameObjectBody.transform);
                    demon2.transform.localRotation = Quaternion.Euler(0, 0.3f, 0);
                    demon2.transform.localPosition = demon2.transform.position + new Vector3(0, -1.45f, 0);
                    break;
                case 3:
                    GameObject demon3 = Instantiate(enemyModels[disasterLevel - 1]);
                    demon3.transform.SetParent(gameObjectBody.transform);
                    demon3.transform.localRotation = Quaternion.Euler(0, 90, 0);
                    demon3.transform.localPosition = Vector3.zero;
                break;
            }
        } 

        for(int i = 1; i <= enemiesGenerated; i++ ){

            var enemy = enemyManagerGenerator.GenerateDemon(disasterLevel);
            
            listEnemies.Add(enemy);
        }
    }
}
