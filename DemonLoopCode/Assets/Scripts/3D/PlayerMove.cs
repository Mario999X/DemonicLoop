using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private CharacterController controller;

    [Header("Player stats")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float Wspeed = 10f;
    [SerializeField] private float Rspeed = 12f;
    [SerializeField] private float Jspeed = 10f;
    [SerializeField] private float gravity = 9.82f;
    private float X, Z;

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
    public bool Speed 
    { 
        set 
        { 
            if (value == true)
                speed = Rspeed;
            else
                speed = Wspeed;
        } 
    }
    public bool OnFloor { get { return onFloor; } }
    public Vector3 SpeedV { get { return speedV; } set { this.speedV = value; } }

    LibraryStates states;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Captura del los ejes X y Z
        Z = -Input.GetAxis("Horizontal");
        X = Input.GetAxis("Vertical");

        // Capacidad de moverse a trav�s de los ejes X y Z
        Vector3 move = transform.right * X + transform.forward * Z;
        controller.Move(move * speed * Time.deltaTime);

        // Efecto gravitatorio en el objeto
        speedV.y += gravity * Time.deltaTime;
        controller.Move(speedV * Time.deltaTime);

        Vector3 spherePosition = new Vector3(transform.position.x, (transform.position.y - altitude), transform.position.z);
        RaycastHit hit;

        // Detecion de si se encuentra en el suele y eliminar a acumulacion gravitacional.
        if (Physics.SphereCast(spherePosition, radius, -Vector3.up, out hit, 0.1f, layer))
        {
            this.onFloor = true; 
            speedV.y = 0;
        }
        else
            this.onFloor = false;

        if (X > 0 && Z > 0) { transform.GetChild(0).transform.rotation = Quaternion.Euler(0, -45, 0); }
        else if (X < 0 && Z < 0) { transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 135, 0); }
        else if (X < 0 && Z > 0) { transform.GetChild(0).transform.rotation = Quaternion.Euler(0, -135, 0); }
        else if (X > 0 && Z < 0) { transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 45, 0); }
        else
        {
            if (X > 0) { transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 0); }
            if (X < 0) { transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 180, 0); }
            if (Z > 0) { transform.GetChild(0).transform.rotation = Quaternion.Euler(0, -90, 0); }
            if (Z < 0) { transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 90, 0); }
        }

        // Prueba para ver que no haya ningun error en el script 'Enter_Battle' al volver a la pantalla de t�tulo.
        if (Input.GetKeyDown(KeyCode.M)) { SceneManager.Instance.LoadScene(0); }
        if (Input.GetKeyDown(KeyCode.R)) { SceneManager.Instance.LoadScene(1); }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 spherePosition = new Vector3(transform.position.x, (transform.position.y - altitude), transform.position.z);
        Gizmos.DrawSphere(spherePosition, radius);
    }
}
