using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    private Stats characterST;
    private LearnableAttacks characterLearneableAttacks;
    [SerializeField] private float requiredXP;
    public float RequiredXP { get { return requiredXP; } set { requiredXP = value; } }

    [SerializeField][Range(1f, 300f)] private float adittionMultiplier = 300;
    [SerializeField][Range(2f, 4f)] private float powerMultiplier = 2;
    [SerializeField][Range(7f, 14f)] private float divisionMultiplier = 7;

    private const float NormalStatUpgrade = 5;

    private const float EffectiveStatUpgrade = 10;

    private const float CriticalStatUpgrade = 3;

    private void Awake()
    {
        characterST = GetComponent<Stats>();
        

        if (characterST.CompareTag("Player"))
        {
            characterLearneableAttacks = GetComponent<LearnableAttacks>();

            requiredXP = CalculateRequireXp();
        }
    }

    public void GainExperienceFlatRate(float xpGained)
    {
        characterST.CurrentXP += xpGained;

        Debug.Log("Personaje: " + characterST.name + " obtiene experiencia " + xpGained);

        if (characterST.CurrentXP > requiredXP)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        //Debug.Log("Personaje: " + characterST.name + " subio de nivel");

        characterST.Level++;

        characterST.SetLevelText(characterST.Level);

        characterST.CurrentXP = Mathf.RoundToInt(characterST.CurrentXP - requiredXP);

        IncrementStats();

        requiredXP = CalculateRequireXp();
    }



    public void SetLevelEnemy(int setLevel)
    {
        if (characterST.GetComponent<Stats>().Level < setLevel)
        {
            while (characterST.GetComponent<Stats>().Level < setLevel)
            {
                characterST.GetComponent<Stats>().Level++;
                IncrementStats();

                characterST.GetComponent<Stats>().DropXP += 30;
                characterST.GetComponent<Stats>().MoneyDrop += 30;

                //Debug.Log("Enemigo: " + characterST.GetComponent<Stats>().name + " subio de nivel: " + characterST.GetComponent<Stats>().Level);
            }
        }
    }

    private void IncrementStats()
    {
        
        switch (characterST.Rol)
        {
            case CharacterRol.Tank:
                characterST.MaxHealth += EffectiveStatUpgrade;
                characterST.MaxMana += NormalStatUpgrade;
                characterST.Strenght += NormalStatUpgrade;
                characterST.PhysicalDefense += EffectiveStatUpgrade;
                characterST.MagicAtk += NormalStatUpgrade;
                characterST.MagicDef += EffectiveStatUpgrade;
                characterST.CriticalChance += CriticalStatUpgrade;
                break;

            case CharacterRol.Priest:
                characterST.MaxHealth += EffectiveStatUpgrade;
                characterST.MaxMana += EffectiveStatUpgrade;
                characterST.Strenght += NormalStatUpgrade;
                characterST.PhysicalDefense += NormalStatUpgrade;
                characterST.MagicAtk += NormalStatUpgrade;
                characterST.MagicDef += EffectiveStatUpgrade;
                characterST.CriticalChance += CriticalStatUpgrade;
                break;

            case CharacterRol.Wizard:
                characterST.MaxHealth += NormalStatUpgrade;
                characterST.MaxMana += EffectiveStatUpgrade;
                characterST.Strenght += NormalStatUpgrade;
                characterST.PhysicalDefense += NormalStatUpgrade;
                characterST.MagicAtk += EffectiveStatUpgrade;
                characterST.MagicDef += EffectiveStatUpgrade;
                characterST.CriticalChance += CriticalStatUpgrade;
                break;

            case CharacterRol.Knight:
                characterST.MaxHealth += EffectiveStatUpgrade;
                characterST.MaxMana += NormalStatUpgrade;
                characterST.Strenght += EffectiveStatUpgrade;
                characterST.PhysicalDefense += EffectiveStatUpgrade;
                characterST.MagicAtk += NormalStatUpgrade;
                characterST.MagicDef += NormalStatUpgrade;
                characterST.CriticalChance += CriticalStatUpgrade;
                break;

        }

        characterST.Health = characterST.MaxHealth;
        characterST.Mana = characterST.MaxMana;

        if (characterST.CompareTag("Player"))
        {
            var possibleAttackToLearn = characterLearneableAttacks.CanILearnAttack(characterST.Level);

            if (possibleAttackToLearn)
            {
                characterST.SetAttack(characterLearneableAttacks.ReturnAttack(characterST.Level));
            }
        }
    }

    private int CalculateRequireXp()
    {
        var solveForRequireXp = 0;

        for (int levelCycle = 1; levelCycle <= characterST.Level; levelCycle++)
        {
            solveForRequireXp += (int)Mathf.Floor(levelCycle + adittionMultiplier * Mathf.Pow(powerMultiplier, levelCycle / divisionMultiplier));
        }

        return solveForRequireXp / 4;

    }


}
