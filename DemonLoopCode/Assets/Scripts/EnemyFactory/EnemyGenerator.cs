using System.Collections.Generic;
using UnityEngine;

// Esta clase sera la que lleven los enemigos visibles en el mundo 3D.
public class EnemyGenerator : MonoBehaviour
{
    [Header("Enemy Difficulty Level")]
    [SerializeField] int disasterLevel = 1;

    [Header("Enemy experience count")]
    [SerializeField] float totalEXP;
    public float TotalEXP { get { return totalEXP; } }

    //[SerializeField] // Comentar o descomentar si se quiere ver la generacion de enemigos en el inspector.
    private List<GameObject> listEnemies = new(); // Lista de enemigos que apareceran en batalla.

    public int DisasterLevel { get { return disasterLevel; } }
    public List<GameObject> ListEnemies { get { return listEnemies; } }

    private EnemyManagerGenerator enemyManagerGenerator;
    private const int MinNumEnemies = 1;
    private const int MaxNumEnemies = 4;

    bool done = false;

    // Start is called before the first frame update
    void Update()
    {
        if (!done && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Scene 2")
        {
            enemyManagerGenerator = GameObject.Find("System").GetComponent<EnemyManagerGenerator>();

            SpawnEnemies();
            done = true;
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Scene 2")
        {
            done = false;
        }
    }

    // Funcion para generar enemigos y agregarlos al listado.
    private void SpawnEnemies()
    { 
        var enemiesGenerated = Random.Range(MinNumEnemies, MaxNumEnemies);

        for(int i = 1; i <= enemiesGenerated; i++ ){

            //Debug.Log("GENERANDO ENEMIGO: " + i);
            var enemy = enemyManagerGenerator.GenerateDemon(disasterLevel);
            
            totalEXP += enemy.GetComponent<Stats>().DropXP;
            listEnemies.Add(enemy);
        }
    }
}
