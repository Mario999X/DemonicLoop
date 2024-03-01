using System.Linq;
using UnityEngine;

public class CombatBoss : MonoBehaviour
{
    [SerializeField] int bossNumber = 0;
    private FloatingTextCombat floatingText;
    [SerializeField] GameObject[] enemyPrefabsLevel;
    [SerializeField] BattleModifiers[] battleModifiersDebuffs;
    [SerializeField] BattleModifiers[] battleModifiersBuffs;

    const int numHealth = 50;
    private CombatFlow combatFlow;
    private bool actionDone = false;

    void Start()
    {
        floatingText = GameObject.Find("System").GetComponent<FloatingTextCombat>();

        combatFlow = GameObject.Find("System").GetComponent<CombatFlow>();
    }

    public void CheckAtkEnemyBoss(GameObject targetSelected, LibraryBattleModifiers libraryBattleModifiers)
    {
        switch(bossNumber)
        {
            case 1:
                // Si el turno actual es divisible por 2 y de 0 se curara
                if (combatFlow.ActualTurn % 2 == 0)
                {
                    floatingText.ShowFloatingTextNumbers(gameObject, numHealth, Color.green);

                    GetComponent<Stats>().Health += numHealth;
                }
                // Si el turno actual es divisible por 5 y de 0 generara un enemigo
                // aleatorio pero si son 4 enemigos ya en el combate no lo hara
                else if (combatFlow.ActualTurn % 5 == 0)
                {
                    int newEnemy = Random.Range(0, enemyPrefabsLevel.Length);
                    if (combatFlow.NumEnemy() < 4)
                    {
                        floatingText.ShowFloatingText(gameObject, "Invader summoned", Color.magenta);
                        Instantiate(enemyPrefabsLevel[newEnemy], GameObject.Find("EnemyBattleZone").transform);
                        StartCoroutine(combatFlow.CreateButtons());
                    }
                }
                else
                {
                    // Se le aumentara el da�o critico de forma aleatoria
                    int c = Random.Range(0, 50);
                    GetComponent<Stats>().CriticalChance += c;
                }
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

                    if(randomCharacter == targetSelected)
                    {
                        players.Remove(randomCharacter);

                        numPlayerRandom = Random.Range(0, players.Count);

                        randomCharacter = players[numPlayerRandom];
                    }

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
                floatingText.ShowFloatingText(gameObject, "You can't defeat me!", Color.yellow);

                GetComponent<Stats>().Health = GetComponent<Stats>().MaxHealth;
                GetComponent<Stats>().Mana = GetComponent<Stats>().MaxMana;

                actionDone = true;
            }

           break;
        }
    }//Fin de CheckAtkEnemyBoos

    private void AddBattleModifierToRandomPlayer(LibraryBattleModifiers libraryBattleModifiers)
    {
        var players = combatFlow.Players;

        int numPlayerRandom = Random.Range(0, players.Length);

        int numModifierRandom = Random.Range(0, battleModifiersDebuffs.Length);

        libraryBattleModifiers.ActiveBattleModifier(players[numPlayerRandom], battleModifiersDebuffs[numModifierRandom]);
    }
}
