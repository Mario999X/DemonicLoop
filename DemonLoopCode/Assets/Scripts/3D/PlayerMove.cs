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
    [SerializeField] bool poison = false;

    int oneTime = 0;

    [Header("Check ground")]
    [SerializeField] float altitude = 1f;
    [SerializeField] float radius = 0.05f;
    [SerializeField] LayerMask layer;

    private bool onFloor = false;

    private Vector3 speedV;

    public bool Movement
    {
        get
        {
            if (X != 0 || Z != 0)
                return true;
            else
                return false;
        }
    }
    public float JSpeed { get { return Jspeed; } }
    public bool OnFloor { get { return onFloor; } }
    public Vector3 SpeedV { get { return speedV; } set { this.speedV = value; } }

    StatesLibrary states;

    void Start()
    {
        states = GameObject.Find("System").GetComponent<StatesLibrary>();
    }

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

        Vector3 spherePosition = new Vector3(transform.position.x, (transform.position.y - altitude), transform.position.z);
        RaycastHit hit;

        if (Physics.SphereCast(spherePosition, radius, -Vector3.up, out hit, 0.1f, layer))
        {
            this.onFloor = true;
        }
        else
        {
            this.onFloor = false;
        }

        if (poison && oneTime == 0)
        {
            StartCoroutine(states.StateEffectGroup("Aliados", "poison"));
            oneTime++;
        }

        // Prueba para ver que no haya ningun error en el script 'Enter_Battle' al volver a la pantalla de título.
        if (Input.GetKeyDown(KeyCode.Escape)) { SceneManager.Instance.LoadScene(0); }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 spherePosition = new Vector3(transform.position.x, (transform.position.y - altitude), transform.position.z);
        Gizmos.DrawSphere(spherePosition, radius);
    }
}
