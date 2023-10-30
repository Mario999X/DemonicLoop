using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    [SerializeField] float strength = 15f;
    [SerializeField] float health;
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float defense = 12f;
    [SerializeField] Image barHP;

    public float Strenght { get { return strength; } }
    public float Defense { get { return defense; } }
    public float Health { get { return health; } set { this.health = value; } }

    void Start()
    {
        health = maxHealth;
    }

private void Update()
    {
        barHP.fillAmount = health / maxHealth;
        StateLife();
    }

void StateLife()
    {
        if (this.health <= 0)
        {
            this.health = 0;

            gameObject.SetActive(false);

        }
    }
}
