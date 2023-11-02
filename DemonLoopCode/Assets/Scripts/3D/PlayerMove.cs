using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private CharacterController controller;

    [Header("Player stats")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float Jspeed = 10f;
    [SerializeField] private float gravity = 9.82f;

    private float X, Z;
    private Vector3 speedV;

    public float JSpeed { get { return Jspeed; } }
    public Vector3 SpeedV { get { return speedV; } set { this.speedV = value; } }

    // Update is called once per frame
    void Update()
    {
        // Captura del los ejes X y Z
        X = Input.GetAxis("Horizontal");
        Z = Input.GetAxis("Vertical");

        // Capacidad de moverse atraves de los ejes X y Z
        Vector3 move = transform.right * X + transform.forward * Z;
        controller.Move(move * speed * Time.deltaTime);

        // Efecto gravitatorio en el objeto
        speedV.y += gravity * Time.deltaTime;
        controller.Move(speedV * Time.deltaTime);

        // Prueba para ver que no haya ningun error en el script 'Enter_Battle' al volver a la pantalla de título.
        if (Input.GetKeyDown(KeyCode.Escape)) { SceneManager.Instance.LoadScene(0); }
    }
}
