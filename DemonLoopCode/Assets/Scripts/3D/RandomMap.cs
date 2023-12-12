using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMap : MonoBehaviour
{
    [Header("Mapas de selección random")]
    [SerializeField] GameObject[] maps;

    [Header("Mapas obligatorios")]
    [SerializeField] GameObject[] oblMaps;

    [Header("El ultimo de la lista tiene que ser 100")]
    [SerializeField] int[] rarety;

    static int room = 0, floor = 0;

    [SerializeField] int actualroom;

    // Start is called before the first frame update
    void Start()
    {
        actualroom = room;

        if (room == 5)
        {
            Instantiate(oblMaps[0], transform.position, Quaternion.Euler(new Vector3(0, 90, 0)));
            room = 0;
        }
        else
        {
            bool finish = false;

            for (int i = 0; i < maps.Length; i++)
            {
                if (Random.value <= ((float)rarety[i] / 100) && !finish)
                {
                    Instantiate(maps[i], transform.position, Quaternion.Euler(new Vector3(0, 90, 0)));
                    finish = true;
                }
            }

            room++;
        }
    }
}
