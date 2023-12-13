using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prueba : MonoBehaviour
{
    [SerializeField] AttackData[] attackDatas;

    void Start()
    {
        attackDatas = Resources.LoadAll<AttackData>("Data/Attacks");

        foreach (AttackData attackData in attackDatas)
        {
            Debug.Log(attackData.name);
        }
    }
}
