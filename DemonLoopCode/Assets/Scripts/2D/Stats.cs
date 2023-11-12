using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    LibraryMove move;

    Image barHP;

    [SerializeField] float health;
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float strength = 15f;
    [SerializeField] float physicalDef = 12f;
    [SerializeField] float magicAtk = 0f;
    [SerializeField] float magicDef = 0f;
    [SerializeField] List<string> listAtk = new();

    [SerializeField] Types type;

    public float Health { get { return health; } set { this.health = value; } }
    public float Strenght { get { return strength; } }
    public float PhysicalDefense { get { return physicalDef; } }
    public float MagicAtk { get { return magicAtk; } }
    public float MagicDef { get { return magicDef; } }
    public List<string> ListAtk { get { return listAtk; } set { this.listAtk = value; } }
    public Types Type { get { return type; } }

    void Start()
    {
        health = maxHealth;
        barHP = transform.GetChild(0).Find("BarHPFill").GetComponent<Image>();

        move = GameObject.Find("System").GetComponent<LibraryMove>();

        move.OnHealthChanged += OnAttackReceived;
    }

    private void Update()
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

        if (this.health <= 0)
        {
            this.health = 0;
        }

        barHP.fillAmount = health / maxHealth;

        if (this.health == 0)
        {
            gameObject.SetActive(false);
        }
    }//Fin de OnAttackReceived
}
