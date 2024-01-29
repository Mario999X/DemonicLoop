using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerInteract : MonoBehaviour
{
    [SerializeField] float distance = 10f;
    [SerializeField] LayerMask layer;

    [SerializeField] bool click = false;

    MoneyPlayer playerMoney;

    public bool Click { set { this.click = value; } }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerMoney == null)
            playerMoney = GameObject.Find("System").GetComponent<MoneyPlayer>();

        RaycastHit hit;

        // Solo hace de intermediario
        // Cuando el jugador mire un objeto interactuable pueda interactuar con el.
        if (Physics.Raycast(transform.position, transform.right, out hit, distance, layer) && click)
        {
            Debug.DrawLine(transform.position, hit.point, Color.green);
            //Debug.Log(hit.transform.tag);

            // Identificamos el objeto para realizar el intercambio de datos correcto.
            switch (hit.transform.tag)
            {
                case "Chest": // En caso de ser un cofre.
                    PlayerInventory inventory = GameObject.Find("System").GetComponent<PlayerInventory>();

                    // Si el objeto contiene el componente ChestContent lo ejecuta
                    if (hit.transform.GetComponent<ChestContent>())
                    {
                        Content content = hit.transform.GetComponent<ChestContent>().chest();

                        playerMoney.Money += content.Money; // Modifica el dinero.

                        if (content.Count > 0) // Si no sedan 0 objetos se incluyen al inventario.
                        {
                            inventory.AddObjectToInventory(content.Data.name.Substring(4, content.Data.name.Length - 4), content.Data, content.Count); // Incluye el objeto al invetario.
                        }

                        Destroy(hit.transform.GetComponent<ChestContent>()); // Destruye el scrip en el objeto.
                    }
                    break;
                case "Slave Shop":
                case "Normal Shop":
                    if (transform.GetComponentInParent<KeyBoardControls>())
                        transform.GetComponentInParent<KeyBoardControls>().Shopping = hit.transform.GetComponent<ShoppingSystem>();

                    hit.transform.GetComponent<ShoppingSystem>().OpenCloseShop();
                    break;
                    
                case "LeverWithoutOrder":
                    hit.transform.GetComponent<LeverWithoutOrderData>().ActivateDesactivateLever();
                    break;
            }

            click = false;
        }
        else
        {
            Debug.DrawRay(transform.position, transform.right * distance, Color.red);

            if (click) { click = false; }
        }
    }
}
