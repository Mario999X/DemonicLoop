using System.Linq;
using UnityEngine;

// Clase encargada de manejar el comportamiento de los jefes finales en un combate.
public class CombatBoss : MonoBehaviour
{
    [SerializeField] private int bossNumber = 0;
    private FloatingTextCombat floatingText;
    [SerializeField] private GameObject[] enemyPrefabsLevel;
    [SerializeField] private BattleModifiers[] battleModifiersDebuffs;
    [SerializeField] private BattleModifiers[] battleModifiersBuffs;

    private const int numHealth = 50;
    private CombatFlow combatFlow;
    private bool actionDone = false;

    void Start()
    {
        floatingText = GameObject.Find("System").GetComponent<FloatingTextCombat>();

        combatFlow = GameObject.Find("System").GetComponent<CombatFlow>();
    }

    // Funcion para aplicar el comportamiento de un jefe final.
    public void CheckAtkEnemyBoss(GameObject targetSelected, LibraryBattleModifiers libraryBattleModifiers)
    {
        switch(bossNumber)
        {
            case 1:
                if (combatFlow.ActualTurn % 2 == 0)
                {
                    floatingText.ShowFloatingTextNumbers(gameObject, numHealth, Color.green);

                    GetComponent<Stats>().Health += numHealth;
                }
                
                if (combatFlow.ActualTurn % 5 == 0)
                {
                    int newEnemy = Random.Range(0, enemyPrefabsLevel.Length);
                    if (combatFlow.NumEnemy() < 3)
                    {
                        floatingText.ShowFloatingText(gameObject, "Invader summoned", Color.magenta);
                        Instantiate(enemyPrefabsLevel[newEnemy], GameObject.Find("EnemyBattleZone").transform);
                        StartCoroutine(combatFlow.FindEnemiesAndCreateAlliesButtons());
                        
                    } else floatingText.ShowFloatingText(gameObject, "Hahaha", Color.magenta);
                }

                int c = Random.Range(0, 50);
                GetComponent<Stats>().CriticalChance += c;
                break;
            
            case 2:

                if(combatFlow.ActualTurn % 2 == 0)
                {
                    floatingText.ShowFloatingText(gameObject, "Plague released", Color.yellow);

                    AddBattleModifierToRandomPlayer(libraryBattleModifiers);
                }
                
                if(combatFlow.ActualTurn % 4 == 0)
                {

                    var players = combatFlow.Players.ToList();

                    int numPlayerRandom = Random.Range(0, players.Count);

                    var randomCharacter = players[numPlayerRandom];

                    randomCharacter.GetComponent<Stats>().Health = 1;
                }
                
           break;

           case 3:

            GetComponent<Stats>().Type = (Types)Random.Range(0,5);
            floatingText.ShowFloatingText(gameObject, "Type changed: " + GetComponent<Stats>().Type, Color.yellow);

            if(combatFlow.ActualTurn % 2 == 0)
            {
                AddBattleModifierToRandomPlayer(libraryBattleModifiers);
            }

            if(combatFlow.ActualTurn % 3 == 0)
            {
                int numModifierRandom = Random.Range(0, battleModifiersBuffs.Length);
                libraryBattleModifiers.ActiveBattleModifier(gameObject, battleModifiersBuffs[numModifierRandom]);
            }

            if(!actionDone && GetComponent<Stats>().Health <= GetComponent<Stats>().MaxHealth * 0.5f)
            {
                floatingText.ShowFloatingText(gameObject, "\nYou can't defeat me!", Color.yellow);

                GetComponent<Stats>().Health = GetComponent<Stats>().MaxHealth;
                GetComponent<Stats>().Mana = GetComponent<Stats>().MaxMana;

                actionDone = true;
            }

           break;
        }
    }

    // Funcion usada para un comportamiento comun entre jefes finales. Se encarga de poner un modificador de batalla a un aliado.
    private void AddBattleModifierToRandomPlayer(LibraryBattleModifiers libraryBattleModifiers)
    {
        var players = combatFlow.Players;

        int numPlayerRandom = Random.Range(0, players.Length);

        int numModifierRandom = Random.Range(0, battleModifiersDebuffs.Length);

        libraryBattleModifiers.ActiveBattleModifier(players[numPlayerRandom], battleModifiersDebuffs[numModifierRandom]);
    }
}
