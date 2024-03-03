using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public enum ImprovementsTypes { Health, Mana, CriticalChance, Discount }

[CreateAssetMenu]
public class ImprovementsData : ScriptableObject
{
    [SerializeField] private Sprite icon;
    int powerHealthV1 = 100;
    int powerHealthV2 = 200;
    int powerHealthV3 = 250;

    int powerManaV1 = 100;
    int powerManaV2 = 200;
    int powerManaV3 = 250;

    int powerCriticalV1 = 5;
    int powerCriticalV2 = 10;
    int powerCriticalV3 = 15;

    public float discountV = 0.02f;
    float discountV1 = 0.02f;
    float discountV2 = 0.05f;
    float discountV3 = 0.07f;

    public int idHealth = 1;
    public int idMana = 1;
    public int idCriticalChance = 1;
    public int idDiscount = 1;

    [TextArea]
    [SerializeField] private string description;
    [SerializeField] private float costRefined;
    [SerializeField] public ImprovementsTypes improvementsType;
    [SerializeField] public bool isVersion;
    [SerializeField] GameObject buttonPrefab;

    public Sprite Icon { get { return icon; } }
    public string Description { get { return description; } set { description = value; } }
    public float CostRefined { get { return costRefined; } set { costRefined = value; } }
    public float DiscountV { get { return discountV; } set { discountV = value; } }
    public ImprovementsTypes ImprovementsType { get { return improvementsType; } }

    // Funcion para comprar en la tienda de mejoras pero 
    // en un orden numerico de la mejora es decir
    // no te puedes comprar la mejora V3 sin tener antes la mejora V1 y la V2
    public string BuyImprovement(StatsPersistenceData target)
    {
        // Podria ponerse algun id a las mejoras
        // Suponiendo que sea el prota
        switch (ImprovementsType)
        {
            case ImprovementsTypes.Health:
                if (isVersion == true && idHealth == 1)
                {
                    target.MaxHealth += powerHealthV1;
                    target.Health = target.MaxHealth;
                    idHealth = 2;
                }
                else if (isVersion == true && idHealth == 2)
                {
                    target.MaxHealth += powerHealthV2;
                    target.Health = target.MaxHealth;
                    idHealth = 3;
                }
                else if (isVersion == true && idHealth == 3)
                {
                    target.MaxHealth += powerHealthV3;
                    target.Health = target.MaxHealth;
                }
                break;

            case ImprovementsTypes.Mana:
                if (isVersion == true && idMana == 1)
                {
                    target.MaxMana += powerManaV1;
                    target.Mana = target.MaxMana;
                    idMana = 2;
                }
                else if (isVersion == true && idMana == 2)
                {
                    target.MaxMana += powerManaV2;
                    target.Mana = target.MaxMana;
                    idMana = 3;
                }
                else if (isVersion == true && idMana == 3)
                {
                    target.MaxMana += powerManaV3;
                    target.Mana = target.MaxMana;
                }
                break;

            case ImprovementsTypes.CriticalChance:
                if (isVersion == true && idCriticalChance == 1)
                {
                    target.CriticalChance += powerCriticalV1;
                    idCriticalChance = 2;
                }
                else if (isVersion == true && idCriticalChance == 2)
                {
                    target.CriticalChance += powerCriticalV2;
                    idCriticalChance = 3;
                }
                else if (isVersion == true && idCriticalChance == 3)
                {
                    target.CriticalChance += powerCriticalV3;
                }
                break;

            case ImprovementsTypes.Discount:
                if (isVersion == true && idDiscount == 1)
                {
                    idDiscount = 2;
                }
                else if (isVersion == true && idDiscount == 2)
                {
                    idDiscount = 3;
                }
                else if (isVersion == true && idDiscount == 3)
                {
                }
                break;
        }//Fin del switch

        return this.name;
    }

    // Se aplica los descuentos en los Items en la tienda
    // dependiendo de que version tenga el jugador
    public void DiscountItems(ObjectData[] objdata)
    {
        float discountVTemp = 0f;
        if (isVersion == true && idDiscount == 1)
        {
            for (int i = 0; i < objdata.Length; i++)
            {
                discountVTemp = objdata[i].Cost * discountV1;
                objdata[i].Cost = objdata[i].Cost - discountVTemp;
            }
        }
        else if (isVersion == true && idDiscount == 2)
        {
            for (int i = 0; i < objdata.Length; i++)
            {
                discountVTemp = objdata[i].Cost * discountV2;
                objdata[i].Cost = objdata[i].Cost - discountVTemp;
            }
        }
        else if (isVersion == true && idDiscount == 3)
        {
            for (int i = 0; i < objdata.Length; i++)
            {
                discountVTemp = objdata[i].Cost * discountV3;
                objdata[i].Cost = objdata[i].Cost - discountVTemp;
            }
        }
    }//Fin de DiscountItems

    // Se aplica los descuentos en la tienda de mejoras
    // dependiendo de que version tenga el jugador
    public void DiscountImprovements(ImprovementsData[] objdata)
    {
        if (isVersion == true && idDiscount == 1)
        {
            for (int i = 0; i < objdata.Length; i++)
            {
                discountV = objdata[i].CostRefined * discountV1;
                objdata[i].CostRefined = objdata[i].CostRefined - discountV;
            }
        }
        else if (isVersion == true && idDiscount == 2)
        {
            for (int i = 0; i < objdata.Length; i++)
            {
                discountV = objdata[i].CostRefined * discountV2;
                objdata[i].CostRefined = objdata[i].CostRefined - discountV;
            }
        }
        else if (isVersion == true && idDiscount == 3)
        {
            for (int i = 0; i < objdata.Length; i++)
            {
                discountV = objdata[i].CostRefined * discountV3;
                objdata[i].CostRefined = objdata[i].CostRefined - discountV;
            }
        }
    }//Fin de DiscountImprovements

    // Se aplica los descuentos en la tienda de los esclavos
    // dependiendo de que version tenga el jugador
    public void DiscountSlaves(StatsPersistenceData[] slavesjdata)
    {
        if (isVersion == true && idDiscount == 1)
        {
            for (int i = 0; i < slavesjdata.Length; i++)
            {
                discountV = slavesjdata[i].Cost * discountV1;
                slavesjdata[i].Cost = slavesjdata[i].Cost - discountV;
            }
        }
        else if (isVersion == true && idDiscount == 2)
        {
            for (int i = 0; i < slavesjdata.Length; i++)
            {
                discountV = slavesjdata[i].Cost * discountV2;
                slavesjdata[i].Cost = slavesjdata[i].Cost - discountV;
            }
        }
        else if (isVersion == true && idDiscount == 3)
        {
            for (int i = 0; i < slavesjdata.Length; i++)
            {
                discountV = slavesjdata[i].Cost * discountV3;
                slavesjdata[i].Cost = slavesjdata[i].Cost - discountV;
            }
        }
    }//Fin de DiscountSlaves

}
