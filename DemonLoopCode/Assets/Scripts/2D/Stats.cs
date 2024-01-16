using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum CharacterRol {
    Tank, Priest, Wizard, Knight
}

public class Stats : MonoBehaviour
{
    private Image barHP;

    private Image barMana;

    private GameObject charFloatingTextSpaceNumbers;

    private TextMeshProUGUI levelText;

    [Header("Character Rol Components")]
    [SerializeField] CharacterRol rol = CharacterRol.Tank;

    // IMAGEN ASOCIADA AL ROL

    [Header("Level Components")]
    [SerializeField] int level = 1;
    [SerializeField] float currentXP;

    [Header("Stats Components")]
    [SerializeField] float health;
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float mana;
    [SerializeField] float maxMana = 100f;
    [SerializeField] float strength = 15f;
    [SerializeField] float physicalDef = 12f;
    [SerializeField] float magicAtk = 0f;
    [SerializeField] float magicDef = 0f;
    [SerializeField] float criticalChance = 0f;
    [SerializeField] List<AttackData> listAtk = new();
    [SerializeField] bool absorbsDamageOfSameType = false;
    [SerializeField] Types type;

    [Header("Drops Components")]
    [SerializeField] float moneyDrop = 1.1f;
    [SerializeField] private float dropXP = 0;

    public CharacterRol Rol { get { return rol; } }

    public int Level { get { return level; } set { level = value; }}
    public float CurrentXP { get { return currentXP; } set { currentXP = value; }}

    public float Health { get { return health; } set { health = value; if(barHP != null) OnAttackReceived(); } } // @TODO: Mecanica de ataque sorpresa no funcional, la barra de vida es nula
    public float MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
    public float Mana { get { return mana; } set { mana = value; if(barMana != null) OnManaChanged(); } }
    public float MaxMana { get { return maxMana; } set { maxMana = value; } }
    public float Strenght { get { return strength; } set { strength = value; } }
    public float PhysicalDefense { get { return physicalDef; } set { physicalDef = value; } }
    public float MagicAtk { get { return magicAtk; } set { magicAtk = value; } }
    public float MagicDef { get { return magicDef; } set { magicDef = value; } }
    public float CriticalChance { get { return criticalChance; } set { criticalChance = value; } }
    public List<AttackData> ListAtk { get { return listAtk; } set { listAtk = value; }}
    public List<string> ListNameAtk { get { return ObtainNameAttacks(); }}
    public GameObject CharFloatingTextSpaceNumbers { get { return charFloatingTextSpaceNumbers; } }
    public bool AbsorbsDamageOfSameType { get { return absorbsDamageOfSameType; }}
    public Types Type { get { return type; } }

    public float MoneyDrop { get { return moneyDrop; } set { moneyDrop = value; } }
    public float DropXP { get { return dropXP; } set { dropXP = value; }}

    void Start()
    {
        health = maxHealth;
        barHP = transform.GetChild(0).Find("BarHPFill").GetComponent<Image>();

        mana = maxMana;
        barMana = transform.GetChild(1).Find("BarManaFill").GetComponent<Image>();

        charFloatingTextSpaceNumbers = transform.GetChild(2).gameObject;

        levelText = transform.GetChild(4).GetComponent<TextMeshProUGUI>();

        SetLevelText(level);
    }

    // Si en el caso de de que el jugador tenga mas ataques no podra usarlo
    // Solo puede usar 4 ataques que son los espacios acordados
    private bool CheckListAtkMax()
    {
        var space = true;

        if (listAtk.Count >= 4)
        {   
            space = false;
            
            Debug.Log(gameObject.name + " Ya tiene 4 ataques, no es posible agregar mas");
        }

        return space;
    }//Fin de CheckListAtk

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
    }//Fin de OnAttackReceived

    private void OnManaChanged()
    {
        if(mana >= maxMana)
        {
            mana = maxMana;
        }

        if(mana <= 0)
        {
            mana = 0;
        }

        barMana.fillAmount = mana / maxMana;
    }

    private List<string> ObtainNameAttacks()
    {
        List<string> nameList = new();

        listAtk.ForEach(x => {
            if(x != null) 
                nameList.Add(x.name.Substring(4, x.name.Length - 4).Replace("^", " ")); 
            });

        return nameList;
    }

    public void SetAttack(AttackData attack)
    {   
        var enoughSpace = CheckListAtkMax();

        if(enoughSpace) listAtk.Add(attack);
    }

    public void Revive(float healthToRevive)
    {   
        gameObject.SetActive(true);

        Health = healthToRevive;
    }

    public void SetLevelText(int level)
    {
        if(levelText != null)
        {
            levelText.text = level.ToString();
        } else Debug.Log("No me dio tiempo beach");
    }
}
