using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    private Stats characterST;
    [SerializeField] private float requiredXP;
    public float RequiredXP { get { return requiredXP; } set { requiredXP = value; }}

    [SerializeField] [Range(1f,300f)] private float adittionMultiplier = 300;
    [SerializeField] [Range(2f,4f)] private float powerMultiplier = 2;
    [SerializeField] [Range(7f,14f)] private float divisionMultiplier = 7;

    private void Start()
    {
        characterST = GetComponent<Stats>();
        requiredXP = CalculateRequireXp();
    }

    public void GainExperienceFlatRate(float xpGained)
    {
        characterST.CurrentXP += xpGained;

        Debug.Log("Personaje: " + characterST.name + " obtiene experiencia " + xpGained);
        
        if(characterST.CurrentXP > requiredXP)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        Debug.Log("Personaje: " + characterST.name + " subio de nivel");
        characterST.Level++;
        characterST.CurrentXP = Mathf.RoundToInt(characterST.CurrentXP - requiredXP); 

        // DEPENDE EL ROL, por ahora GENERICO
        IncrementStatsGeneric();

        requiredXP = CalculateRequireXp();
    } 

    private void IncrementStatsGeneric()
    {
        characterST.MaxHealth += 20;
        characterST.Health = characterST.MaxHealth;
    }

    private int CalculateRequireXp()
    {
        var solveForRequireXp = 0;

        for (int levelCycle = 1; levelCycle <= characterST.Level; levelCycle++)
        {
            solveForRequireXp += (int) Mathf.Floor(levelCycle + adittionMultiplier * Mathf.Pow(powerMultiplier, levelCycle / divisionMultiplier));
        }

        return solveForRequireXp / 4;

    }


}
