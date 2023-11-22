using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
   
    private LibraryMove move;

    private Image barHP;

    private Image barMana;

    private GameObject charFloatingTextSpace;

    [SerializeField] float health;
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float mana;
    [SerializeField] float maxMana = 100f;
    [SerializeField] float strength = 15f;
    [SerializeField] float physicalDef = 12f;
    [SerializeField] float magicAtk = 0f;
    [SerializeField] float magicDef = 0f;
    [SerializeField] float criticalChance;
    [SerializeField] float moneyDrop = 1.1f;
    [SerializeField] List<string> listAtk = new();

    [SerializeField] Types type;

    public float Health { get { return health; } set { health = value; OnAttackReceived(); } }
    public float Mana { get { return mana; } set { mana = value; OnManaChanged(); } }
    public float Strenght { get { return strength; } }
    public float PhysicalDefense { get { return physicalDef; } }
    public float MagicAtk { get { return magicAtk; } }
    public float MagicDef { get { return magicDef; } }
    public float CriticalChance { get { return criticalChance; } }
    public float MoneyDrop { get { return moneyDrop; } }
    public List<string> ListAtk { get { return listAtk; } set { listAtk = value; } }
    public GameObject CharFloatingTextSpace { get { return charFloatingTextSpace; } }

    public Types Type { get { return type; } }

    void Start()
    {
        health = maxHealth;
        barHP = transform.GetChild(0).Find("BarHPFill").GetComponent<Image>();

        mana = maxMana;
        barMana = transform.GetChild(1).Find("BarManaFill").GetComponent<Image>();

        move = GameObject.Find("System").GetComponent<LibraryMove>();

        charFloatingTextSpace = transform.GetChild(2).gameObject;
    }

    private void FixedUpdate()
    {
        CheckListAtk();
    }

    // Si en el caso de de que el jugador tenga mas ataques no podra usarlo
    // Solo puede usar 4 ataques que son los espacios acordados
    private void CheckListAtk()
    {
        if (listAtk.Count > 4)
        {
            listAtk.Remove(listAtk[listAtk.Count - 1]);
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
}
