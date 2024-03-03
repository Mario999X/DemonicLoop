using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class DeadZone : MonoBehaviour
{
    // Si el jugador se cae fuera del mapa lo devolvemos al principio del mapa.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
            other.gameObject.transform.localPosition = new Vector3(0f, 1.15f, 0f);
    }
}
