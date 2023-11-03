using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    LibraryMove library;
    Image barLifes;
    [SerializeField] float strength = 15f;
    [SerializeField] float health;
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float defense = 12f;
    [SerializeField] float magicAtk = 0f;
    [SerializeField] float magicDef = 0f;
    [SerializeField] List<string> listAtk = new List<string>();


    public float MagicAtk { get { return magicAtk; } }
    public float MagicDef { get { return magicDef; } }
    public float Strenght { get { return strength; } }
    public float Defense { get { return defense; } }
    public float Health { get { return health; } set { this.health = value; } }

    public List<string> ListAtk { get { return listAtk; } set { this.listAtk = value; } }

    void Start()
    {
        health = maxHealth;
        barLifes = transform.GetChild(0).GetComponent<Image>();

        library = GameObject.Find("System").GetComponent<LibraryMove>();

        library.OnAttackReceived += OnAttackReceived;
    }


    private void Update()
    {
        CheckListAtk();
    }

    void FixedUpdate()
    {

    }

    //Si en el caso de de que el jugador tenga mas ataques no podra usarlos
    // Solo puede usar 4 ataques que son los espacios acordados
    private void CheckListAtk()
    {
        if (listAtk.Count > 4)
        {
            listAtk.Remove(listAtk[listAtk.Count - 1]);
        }
    }

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

        barLifes.fillAmount = health / maxHealth;

        if (this.health == 0)
        {
            gameObject.SetActive(false);
        }

    }
}
