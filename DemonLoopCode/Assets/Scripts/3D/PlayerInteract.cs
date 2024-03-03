using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Componentes de interacción")]
    [SerializeField] float distance = 10f;
    [SerializeField] LayerMask layer;

    bool click = false;

    MoneyPlayer playerMoney;

    public bool Click { set { this.click = value; } }

    // Update is called once per frame
    void FixedUpdate()
    {
        // En el caso que "playerMoney" sea null sigue buscandolo.
        if (playerMoney == null)
            playerMoney = GameObject.Find("System").GetComponent<MoneyPlayer>();

        RaycastHit hit;

        // Solo hace de intermediario
        // Cuando el jugador mire un objeto interactuable pueda interactuar con el.
        if (Physics.Raycast(transform.position, transform.right, out hit, distance, layer) && click)
        {
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
                            inventory.AddObjectToInventory(content.Data.name.Substring(4, content.Data.name.Length - 4), content.Data, content.Count); // Incluye el objeto al invetario.

                        Destroy(hit.transform.GetComponent<ChestContent>()); // Destruye el scrip en el objeto.
                    }
                    break;

                case "Mimic": // En el caso de un mimico se inicia combate.
                    GameObject.Find("System").GetComponent<EnterBattle>().StartBattle(hit.transform.gameObject, false);
                    break;
                // En el caso de una tienda se abre la interfaz de la tienda.
                case "Slave Shop":
                case "Normal Shop":
                case "Special Shop":
                    if (!GameObject.Find("Shop").GetComponent<Canvas>().enabled)
                    {
                        if (transform.GetComponentInParent<KeyBoardControls>())
                            transform.GetComponentInParent<KeyBoardControls>().Shopping = hit.transform.GetComponent<ShoppingSystem>();

                        hit.transform.GetComponent<ShoppingSystem>().OpenCloseShop();
                    }
                    break;
                case "LeverWithoutOrder": // Para activar/desactivar palancas las cuales no requieren de un orden de activacion. 
                    hit.transform.GetComponent<LeverWithoutOrderData>().ActivateDesactivateLever();
                    break;
                case "LeverWithOrder": // Para activar/desactivar palancas las cuales requieren de un orden de activacion. 
                    hit.transform.GetComponent<LeverWithOrderData>().ActivateDesactivateLever();
                    break;
            }

            click = false;
        }
        else
            if (click) { click = false; }
    }
}
