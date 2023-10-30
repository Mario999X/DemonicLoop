using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Shader_ajustment : MonoBehaviour
{
    private int PosID = Shader.PropertyToID("_Position");
    private int SizeID = Shader.PropertyToID("_Size");

    private Material wallMatirial_Actual, wallMatirial_Last;
    
    [SerializeField] private new Camera camera;
    [SerializeField] private LayerMask layer;

    GameObject @object = null;

    void Awake()
    {
        camera = Camera.main; // Busca la camara principal.
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, camera.transform.position);

        Vector3 dir = camera.transform.position - transform.position;
        Ray ray = new Ray(transform.position, dir.normalized);
        
        RaycastHit hit;

        // Traza una linea entre el jugador y la cámara, cual quier objeto que se encuentre en la capa asignada se guardara y se le modificada el marametro tamaño de su material.
        // Guarda hasta dos materiales de objetos distintos.
        // Si en la line no se encuantra en colision con ningún objeto pero estuvo con en contacto con alguno este modificara el tamaño del circulo a 0.
        if (Physics.Raycast(ray, out hit, distance, layer))
        {
            Debug.DrawRay(transform.position, dir.normalized * distance, Color.green);

            // Si el objeto que esta en colisión sigue siendo el mismo este seguira guardando su material como el actual y modificando sus valores.
            // En el caso contrario este se guadara como el material que había antes.
            if (@object == hit.transform.gameObject)
            {
                wallMatirial_Actual = hit.transform.GetComponent<Renderer>().material;

                var view = camera.WorldToViewportPoint(transform.position);
                wallMatirial_Actual.SetVector(PosID, view);

                wallMatirial_Actual.SetFloat(SizeID, 1);
            }
            else if (@object != null)
            {
                wallMatirial_Last = @object.GetComponent<Renderer>().material;
                @object = null;
            }

            @object = hit.transform.gameObject;
        }
        else
        {
            Debug.DrawRay(transform.position, dir.normalized * distance, Color.red);

            // En caso de que los materiales no esten vacios y no haya algun objeto en colision esto modifican el tamaño del circulo a 0 y los vuelve null.
            if (wallMatirial_Actual != null)
            {
                wallMatirial_Actual.SetFloat(SizeID, 0);
                wallMatirial_Actual = null;
            }

            if (wallMatirial_Last != null)
            {
                wallMatirial_Last.SetFloat(SizeID, 0);
                wallMatirial_Last = null;
            }
        }
    }
}
