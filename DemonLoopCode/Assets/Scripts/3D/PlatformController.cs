using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    SaveSystem controller;
    void Start()
    {
        controller = GameObject.Find("System").GetComponent<SaveSystem>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            Debug.Log("El jugador entró en la plataforma.");
            controller.SaveData();
        }
    }

}
