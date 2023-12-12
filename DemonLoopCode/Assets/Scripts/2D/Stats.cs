using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    private Image barHP;

    private Image barMana;

    private GameObject charFloatingTextSpaceNumbers;

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
    [SerializeField] float criticalChance;
    [SerializeField] List<AttackData> listAtk = new();
    [SerializeField] bool absorbsDamageOfSameType = false;
    [SerializeField] Types type;

    [Header("Drops Components")]
    [SerializeField] float moneyDrop = 1.1f;
    [SerializeField] private float dropXP = 0;

    public int Level { get { return level; } set { level = value; }}
    public float CurrentXP { get { return currentXP; } set { currentXP = value; }}

    public float Health { get { return health; } set { health = value; OnAttackReceived(); } }
    public float MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
    public float Mana { get { return mana; } set { mana = value; OnManaChanged(); } }
    public float Strenght { get { return strength; } }
    public float PhysicalDefense { get { return physicalDef; } }
    public float MagicAtk { get { return magicAtk; } }
    public float MagicDef { get { return magicDef; } }
    public float CriticalChance { get { return criticalChance; } }
    public List<AttackData> ListAtk { get { return listAtk; } set { listAtk = value; }}
    public List<string> ListNameAtk { get { return ObtainNameAttacks(); }}
    public GameObject CharFloatingTextSpaceNumbers { get { return charFloatingTextSpaceNumbers; } }
    public bool AbsorbsDamageOfSameType { get { return absorbsDamageOfSameType; }}
    public Types Type { get { return type; } }

    public float MoneyDrop { get { return moneyDrop; } }
    public float DropXP { get { return dropXP; } set { dropXP = value; }}

    void Start()
    {
        health = maxHealth;
        barHP = transform.GetChild(0).Find("BarHPFill").GetComponent<Image>();

        mana = maxMana;
        barMana = transform.GetChild(1).Find("BarManaFill").GetComponent<Image>();

        charFloatingTextSpaceNumbers = transform.GetChild(2).gameObject;
    }

    // Si en el caso de de que el jugador tenga mas ataques no podra usarlo
    // Solo puede usar 4 ataques que son los espacios acordados
    private void CheckListAtkMax()
    {
        if (listAtk.Count > 4)
        {
            listAtk.Remove(listAtk[^1]); // == listAtk.Count - 1
        }
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

    // TODO: Funcion para agregar un ataque a la lista pasandole un nombre, hace falta revisarlo.
    public void SetAttack(AttackData attack)
    {   
        CheckListAtkMax(); // Aqui se realizará la comprobacion del max de ataques por personaje.

        listAtk.Add(attack);
    }
}
