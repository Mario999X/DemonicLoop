using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMap : MonoBehaviour
{
    [SerializeField] GameObject[] maps;

    // Start is called before the first frame update
    void Start()
    {
        int x = 0;
        float rand = Random.value;

        rand = rand * 10;

        x = (int) Mathf.Clamp(rand, 0, maps.Length - 1);

        Instantiate(maps[x], transform.position, Quaternion.Euler(new Vector3(0, 90, 0)));
    }
}
