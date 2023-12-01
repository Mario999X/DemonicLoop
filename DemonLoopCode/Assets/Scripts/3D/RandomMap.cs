using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMap : MonoBehaviour
{
    [SerializeField] GameObject[] maps;

    [Header("El ultimo de la lista tiene que ser 0")]
    [SerializeField] int[] rarety;

    // Start is called before the first frame update
    void Start()
    {
        bool finish = false;

        for (int i = 0; i < maps.Length; i++)
        {
            if (Random.value >= (float) rarety[i] / 100 && !finish)
            {
                Instantiate(maps[i], transform.position, Quaternion.Euler(new Vector3(0, 90, 0))); 
                finish = true;
            }
        }

    }
}
