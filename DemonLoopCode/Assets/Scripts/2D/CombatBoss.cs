using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatBoss : MonoBehaviour
{
    [SerializeField] int bossNumber = 0;
    private FloatingTextCombat floatingText;
    [SerializeField] List<GameObject> enemyPrefabsLevel;
    const int numHealth = 50;
    private CombatFlow combatFlow;
    float health;
    float criticalChance;

    // Start is called before the first frame update
    void Start()
    {
        floatingText = GameObject.Find("System").GetComponent<FloatingTextCombat>();

        combatFlow = GameObject.Find("System").GetComponent<CombatFlow>();
        health = GetComponent<Stats>().Health;
        criticalChance = GetComponent<Stats>().CriticalChance;
    }



    public void CheckAtkEnemyBoss(GameObject targetSelected)
    {
        switch(bossNumber)
        {
            case 1:

                if (combatFlow.ActualTurn / 2 == 0)
                {
                    floatingText.ShowFloatingTextNumbers(gameObject, numHealth, Color.green);
                    health += numHealth;
                }
                else if (combatFlow.ActualTurn / 5 == 0)
                {
                    int newEnemy = Random.Range(0, enemyPrefabsLevel.Count);
                    if (combatFlow.NumEnemy() < 4)
                    {
                        floatingText.ShowFloatingText(gameObject, "Invader summoned", Color.magenta);
                        Instantiate(enemyPrefabsLevel[newEnemy], GameObject.Find("EnemyBattleZone").transform);
                        StartCoroutine(combatFlow.CreateButtons());
                    }
                
                }
            else
            {
                int c = Random.Range(0, 50);
                criticalChance += c;
            }
                break;
            
            case 2:

                if(combatFlow.ActualTurn / 2 == 0)
                {
                    floatingText.ShowFloatingText(gameObject, "Plague released", Color.yellow);



                } else if(combatFlow.ActualTurn / 4 == 0)
                {
                    var players = combatFlow.Players.ToList();

                    int r = Random.Range(0, players.Count);

                    var randomCharacter = players[r];

                    if(randomCharacter == targetSelected)
                    {
                        players.Remove(randomCharacter);

                        r = Random.Range(0, players.Count);

                        randomCharacter = players[r];
                    }

                    randomCharacter.GetComponent<Stats>().Health = 1;
                }
                
                break;
        }


    }//Fin de CheckAtkEnemyBoos


}
