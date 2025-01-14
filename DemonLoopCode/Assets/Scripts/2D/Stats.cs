using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Posibles roles de los personajes
public enum CharacterRol
{
    Tank, Priest, Wizard, Knight
}

// Clase de estadisticas para los personajes.
public class Stats : MonoBehaviour
{
    private Image barHP;

    private Image barMana;

    private GameObject charFloatingTextSpaceNumbers;

    private GameObject charFloatingBattleModifierIconSpace;

    private TextMeshProUGUI levelText;

    private Image radialSP;

    [Header("Character Rol Components")]
    [SerializeField] CharacterRol rol = CharacterRol.Tank;
    [SerializeField] Sprite rolIcon;

    [Header("Level Components")]
    [SerializeField] int level = 1;
    [SerializeField] float currentXP;

    [Header("Stats Components")]
    [SerializeField] float health;
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float mana;
    [SerializeField] float maxMana = 100f;
    [SerializeField] float sp = 0;
    [SerializeField] float maxSP = 100f;
    [SerializeField] bool canYouLaunchAnSpecialAtk = false;
    [SerializeField] float strength = 15f;
    [SerializeField] float physicalDef = 12f;
    [SerializeField] float magicAtk = 0f;
    [SerializeField] float magicDef = 0f;
    [SerializeField] float criticalChance = 0f;
    [SerializeField] List<AttackData> listAtk = new();
    [SerializeField] bool absorbsDamageOfSameType = false;
    [SerializeField] Types type;
    [SerializeField] Sprite typeIcon;

    [SerializeField] AttackData atkSpecial;

    [Header("Drops Components")]
    [SerializeField] float moneyDrop = 1.1f;
    [SerializeField] private float dropXP = 0;
    [SerializeField] float moneyRefinedDrop = 0f;
    [SerializeField] bool boss = false;

    bool attacking = false;

    [SerializeField] List<ActualStateData> actualStates = new();

    public bool Attacking { get { return attacking; } }
    public CharacterRol Rol { get { return rol; } }
    public Sprite RolIcon { get { return rolIcon; }}
    public int Level { get { return level; } set { level = value; SetLevelText(level); } }
    public float CurrentXP { get { return currentXP; } set { currentXP = value; } }
    public float Health { get { return health; } set { health = value; if (barHP != null) OnAttackReceived(); } }
    public float MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
    public float Mana { get { return mana; } set { mana = value; if (barMana != null) OnManaChanged(); } }
    public float MaxMana { get { return maxMana; } set { maxMana = value; } }
    public float SP { get { return sp; } set { sp = value; if (radialSP != null) OnSPChanged(); } }
    public float MaxSP { get { return maxSP; } set { maxSP = value; } }
    public bool CanYouLaunchAnSpecialAtk { get { return canYouLaunchAnSpecialAtk; } }
    public float Strenght { get { return strength; } set { strength = value; } }
    public float PhysicalDefense { get { return physicalDef; } set { physicalDef = value; } }
    public float MagicAtk { get { return magicAtk; } set { magicAtk = value; } }
    public float MagicDef { get { return magicDef; } set { magicDef = value; } }
    public float CriticalChance { get { return criticalChance; } set { criticalChance = value; OnCriticalChance(); } }
    public List<AttackData> ListAtk { get { return listAtk; } set { listAtk = value; } }
    public List<string> ListNameAtk { get { return ObtainNameAttacks(); } }
    public GameObject CharFloatingTextSpaceNumbers { get { return charFloatingTextSpaceNumbers; } }
    public GameObject CharFloatingBattleModifierIconSpace { get { return charFloatingBattleModifierIconSpace; } }
    public bool AbsorbsDamageOfSameType { get { return absorbsDamageOfSameType; } }
    public Types Type { get { return type; } set { type = value; } }
    public Sprite TypeIcon { get { return typeIcon; }}
    public List<ActualStateData> ActualStates { get { return actualStates; } set { actualStates = value; } }
    public float MoneyDrop { get { return moneyDrop; } set { moneyDrop = value; } }
    public float MoneyRefinedDrop { get { return moneyRefinedDrop; } set { moneyRefinedDrop = value; } }
    public float DropXP { get { return dropXP; } set { dropXP = value; } }
    public string AtkSpecial { get { return ObtainNameAttackSpecial(); } }
    public bool Boss { get { return boss; } }

    // Funcion usada para las animaciones de combate
    private void AnimationAttack()
    {
        attacking = !attacking;
    }

    void Awake()
    {
        health = maxHealth;
        barHP = transform.GetChild(0).Find("BarHPFill").GetComponent<Image>();

        mana = maxMana;
        barMana = transform.GetChild(1).Find("BarManaFill").GetComponent<Image>();

        charFloatingTextSpaceNumbers = transform.GetChild(2).gameObject;
        charFloatingBattleModifierIconSpace = transform.GetChild(6).gameObject;

        levelText = transform.GetChild(4).GetComponent<TextMeshProUGUI>();

        SetLevelText(level);

        radialSP = transform.GetChild(5).Find("RadialSPFill").GetComponent<Image>();
        radialSP.fillAmount = sp;

        if (!canYouLaunchAnSpecialAtk)
        {
            radialSP.transform.parent.gameObject.SetActive(false);
        }
    }

    // Si en el caso de de que el jugador tenga mas ataques no podra usarlo
    // Solo puede aprender 4 ataques.
    public bool CheckListAtkMax()
    {
        var canILearn = true;

        if (listAtk.Count >= 4)
        {
            canILearn = false;
        }

        return canILearn;
    }

    private void OnAttackReceived()
    {
        if (health >= maxHealth)
        {
            health = maxHealth;
        }

        if (health <= 0)
        {
            health = 0;
        }

        barHP.fillAmount = health / maxHealth;

        if (health == 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnManaChanged()
    {
        if (mana >= maxMana)
        {
            mana = maxMana;
        }

        if (mana <= 0)
        {
            mana = 0;
        }

        barMana.fillAmount = mana / maxMana;
    }

    private void OnSPChanged()
    {
        if (sp >= maxSP)
        {
            sp = maxSP;
        }

        if (sp <= 0)
        {
            sp = 0;
        }

        radialSP.fillAmount = sp / maxSP;
    }

    private void OnCriticalChance()
    {
        if (criticalChance >= 100)
        {
            criticalChance = 100;
        }

        if (criticalChance <= 0)
        {
            criticalChance = 0;
        }

    }

    private string ObtainNameAttackSpecial()
    {
        return atkSpecial.name.Substring(4, atkSpecial.name.Length - 4).Replace("^", " ");
    }

    private List<string> ObtainNameAttacks()
    {
        List<string> nameList = new();

        listAtk.ForEach(x => {
            if (x != null)
                nameList.Add(x.name.Substring(4, x.name.Length - 4).Replace("^", " "));
        });

        return nameList;
    }

    public void SetAttack(AttackData attack)
    {
        var enoughSpace = CheckListAtkMax();

        if (enoughSpace) listAtk.Add(attack);
    }

    public void ForgetAttack(AttackData attack)
    {
        listAtk.Remove(attack);
    }

    public bool CheckIfIHaveThatAttack(AttackData attack)
    {
        var already = false;

        listAtk.ForEach(a => {
            if (a == attack) already = true;
        });

        return already;
    }

    public void Revive(float healthToRevive)
    {
        gameObject.SetActive(true);

        Health = healthToRevive;
    }

    public void SetLevelText(int level)
    {
        if (levelText != null)
        {
            levelText.text = level.ToString();
        }
    }
}
