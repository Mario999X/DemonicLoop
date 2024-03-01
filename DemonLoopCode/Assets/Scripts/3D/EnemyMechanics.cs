using System.Collections;
using UnityEngine;

public class EnemyMechanics : MonoBehaviour
{
    [Header("View conditions")]
    [SerializeField] private float distance = 100f;

    [Header("Enemy patrol")]
    [SerializeField] bool patrol = false;
    [SerializeField] float speed = 10f;
    [SerializeField] GameObject pointA;
    [SerializeField] GameObject pointB;

    bool change = false;
    bool attack = false;
    bool yietError = false;

    Vector3 direction = -Vector3.forward;

    EnterBattle _Battle;

    public bool Patrol { get { return patrol; }}

    private void Start()
    {
        pointA = transform.parent.GetChild(1).gameObject;
        pointB = transform.parent.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // En el caso que no haya guardado el script Lo busca  hasta encontrarlo (Esto se debe aque si por error coge el 'System' el cual va a ser destruido este se volvera de nuevo null y si se hace una vez no ira a por el nuevo 'System').
        if (_Battle == null)
        {
            _Battle = GameObject.Find("System").GetComponent<EnterBattle>();

            if (patrol)
            {
                StartCoroutine(Enemy_patrol());
            }
        }
        else
        {
            RaycastHit hit;
            direction = transform.right;

            // Crea una linea invisible de una determinada distancia el cual actua como un trigger y solo detecta los objetos que se encuentran en las capas selecionadas.
            if (Physics.Raycast(transform.position, direction, out hit, distance) && !_Battle.OneTime)
            {
                if (hit.transform.gameObject.layer == 3)
                {
                    Debug.DrawLine(transform.position, hit.transform.position, Color.green);

                    attack = true;
                    yietError = true;

                    if(!patrol) transform.GetChild(1).GetComponent<EnemyAnimationController>().WalkingAnimation();

                    hit.transform.GetComponent<PlayerMove>().enabled = false; // Desactiva el movimiento del jugador.

                    // Si encuentra alguno de los dos componentes en el objeto jugador los desactiva antes de iniciar la pelea.
                    if (hit.transform.GetComponent<KeyBoardControls>())
                        hit.transform.GetComponent<KeyBoardControls>().enabled = false;
                    if (hit.transform.GetComponent<ControllerControls>())
                        hit.transform.GetComponent<ControllerControls>().enabled = false;


                    // Antes de activar la pelea el enemigo se acerca al jugador.
                    if (Vector3.Distance(transform.position, hit.transform.position) < 1.5f) // Inicia el combate.
                    {
                        _Battle.StartBattle(transform.parent.gameObject, false);
                    }
                    else
                    {
                        transform.position = Vector3.MoveTowards(transform.position, hit.transform.position, speed * Time.deltaTime);
                    }
                }
                else { Debug.DrawRay(transform.position, direction * distance, Color.red); }
            }
            else
            { 
                Debug.DrawRay(transform.position, direction * distance, Color.red); 
                if (yietError) { _Battle.StartBattle(transform.parent.gameObject, false); }
            }
        }
    }

    IEnumerator Enemy_patrol()
    {
        while (true)
        {
            if (!attack && !_Battle.OneTime) // Cuando detecta al jugador este deja de estar en modo patrulla
            {
                if (change)
                {
                    transform.position = Vector3.MoveTowards(transform.position, pointB.transform.position, speed * Time.deltaTime);
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, pointA.transform.position, speed * Time.deltaTime);
                }

                if (Vector3.Distance(transform.position, pointA.transform.position) < 0.0001f)
                {
                    transform.rotation = Quaternion.Euler(0, Mathf.Atan2(pointB.transform.position.z - pointA.transform.position.z, pointB.transform.position.x - pointA.transform.position.x) * -180 / Mathf.PI, 0);
                    
                    change = true;
                }

                if (Vector3.Distance(transform.position, pointB.transform.position) < 0.0001f)
                {
                    transform.rotation = Quaternion.Euler(0, Mathf.Atan2(pointA.transform.position.z - pointB.transform.position.z, pointA.transform.position.x - pointB.transform.position.x) * -180 / Mathf.PI, 0);
                    
                    change = false;
                }
            }

            yield return new WaitForSeconds(0.00001f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Player")
        {
            _Battle.StartBattle(transform.parent.gameObject, true);
        }
    }
}