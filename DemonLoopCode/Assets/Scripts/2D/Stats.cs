using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    [SerializeField] GameObject system;
    LibraryMove library;
    Image barLifes;
    [SerializeField] float strength = 15f;
    [SerializeField] float health;
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float defense = 12f;
    [SerializeField] float magicAtk = 0f;
    [SerializeField] float magicDef = 0f;

    public float MagicAtk { get { return magicAtk; } }
    public float MagicDef { get { return magicDef; } }
    public float Strenght { get { return strength; } }
    public float Defense { get { return defense; } }
    public float Health { get { return health; } set { this.health = value; } }

    void Start()
    {
        health = maxHealth;
        barLifes = transform.GetChild(0).GetComponent<Image>();

        library = system.GetComponent<LibraryMove>();

        library.OnAttackReceived += OnAttackReceived;
    }


    private void Update()
    {

    }

    void FixedUpdate()
    {

    }

     private void OnAttackReceived(){
        if (this.health <= 0){
            this.health = 0;
        }

        barLifes.fillAmount = health / maxHealth;

        if (this.health == 0){
           gameObject.SetActive(false);
        }

    }
}
