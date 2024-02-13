using System.Collections.Generic;
using UnityEngine;

public class CombatBoss : MonoBehaviour
{
    private FloatingTextCombat floatingText;
    [SerializeField] List<GameObject> enemyPrefabsLevel;
    List<string> listAtkEnemy = new();
    Stats stats;
    LibraryMove libraryMove;
    const int numHealth = 50;
    private CombatFlow combatFlow;
    float health;
    float criticalChance;

    // Start is called before the first frame update
    void Start()
    {
        floatingText = GameObject.Find("System").GetComponent<FloatingTextCombat>();

        listAtkEnemy = GetComponent<Stats>().ListNameAtk;
        combatFlow = GameObject.Find("System").GetComponent<CombatFlow>();
        stats = GetComponent<Stats>();
        libraryMove = GetComponent<LibraryMove>();
        health = GetComponent<Stats>().Health;
        criticalChance = GetComponent<Stats>().CriticalChance;
    }



    public void CheckAtkEnemyBoos()
    {
        Debug.Log(" ");
        Debug.Log("combatFlow.ActualTurn "+ combatFlow.ActualTurn);
        Debug.Log("health "+ health);
        Debug.Log("criticalChance " + criticalChance);
        if (combatFlow.ActualTurn / 2 == 0)
        {
            floatingText.ShowFloatingTextNumbers(gameObject, numHealth, Color.green);
            health = health + numHealth;
        }
        else if (combatFlow.ActualTurn / 5 == 0)
        {
            int newEnemy = UnityEngine.Random.Range(0, enemyPrefabsLevel.Count);
            if (combatFlow.NumEnemy()<4)
            {
                floatingText.ShowFloatingText(gameObject, "Invasor Aparecio", Color.magenta);
                Instantiate(enemyPrefabsLevel[newEnemy], GameObject.Find("EnemyBattleZone").transform);
                StartCoroutine(combatFlow.CreateButtons());
            }
            
        }
        else
        {
            int c = UnityEngine.Random.Range(0, 50);
            criticalChance = criticalChance + c;
        }


    }//Fin de CheckAtkEnemyBoos


}
