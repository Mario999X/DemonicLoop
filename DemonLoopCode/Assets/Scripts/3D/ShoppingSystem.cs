using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ShoppingSystem : MonoBehaviour
{
    [Header("Prefabricado del botón")]
    [SerializeField] GameObject button;

    bool done = false;

    int quantity = 1;

    Image icon;
    GameObject iconSlave;

    TextMeshProUGUI description;
    TextMeshProUGUI buy;

    GameObject displayzone;

    Canvas canvas;

    Scene scene;

    Dictionary<string, ScriptableObject> stock = new();
    List<ImprovementsData> stockImprovements = new List<ImprovementsData>();

    PlayerInventory inventory;
    Data party;
    ScriptableObject data;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.Find("System").GetComponent<PlayerInventory>();
        party = GameObject.Find("System").GetComponent<Data>();

        // Dependiendo de que tipo de tienda guarda y modifica lo que tiene que utilizar
        switch (gameObject.tag)
        {
            case "Normal Shop": // En el caso de la tienda normal este tiene que cargar todos los objetos y guardar los componentes.
                ObjectData[] objects = Resources.LoadAll<ObjectData>("Data/Objects");
                foreach (ObjectData obj in objects)
                {
                    string objName = obj.name.Substring(4, obj.name.Length - 4).Replace("^", " ").ToUpper();
                    stock.Add(objName, obj);
                }

                displayzone = GameObject.Find("Object display");
                icon = displayzone.transform.GetChild(0).GetComponent<Image>();

                description = displayzone.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
                buy = displayzone.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>();

                displayzone.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(() => Buy());
                displayzone.SetActive(false);
                break;

            case "Slave Shop": // En el caso de la tienda de esclavos este tiene que cargar todos los aliados y guardar los componentes que se van a utilizar.
                List<StatsPersistenceData> slaves = Resources.LoadAll<StatsPersistenceData>("Data/CharactersStatsPersistance").ToList();
                List<StatsPersistenceData> group = party.CharactersTeamStats;
                List<StatsPersistenceData> backup = party.CharactersBackupStats;

                // Remueve todos los aliados ya obtenidos de la tienda.
                group.ForEach(g => { if (slaves.Contains(g)) slaves.Remove(g); });
                backup.ForEach(b => { if (slaves.Contains(b)) slaves.Remove(b); });

                slaves.ForEach(s => stock.Add(s.name.Substring(s.name.IndexOf("_") + 1), s));

                displayzone = GameObject.Find("Slave display");
                iconSlave = displayzone.transform.GetChild(0).gameObject;

                description = displayzone.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();

                buy = displayzone.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();

                displayzone.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => Buy());
                displayzone.SetActive(false);
                break;

            case "Special Shop": // En el caso de la tienda especial este tiene que cargar todos los objetos de mejora permanente.
                ImprovementsData[] mejoras = Resources.LoadAll<ImprovementsData>("Data/Improvements");
                foreach (ImprovementsData obj in mejoras)
                {
                    string objNamePWD = obj.name.Substring(4, obj.name.Length - 4).Replace("^", " ").ToUpper();
                    stock.Add(objNamePWD, obj);
                }

                displayzone = GameObject.Find("Special display");

                description = displayzone.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
                buy = displayzone.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();

                displayzone.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => Buy());
                displayzone.SetActive(false);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (scene != UnityEngine.SceneManagement.SceneManager.GetActiveScene())
        {
            done = false;
            scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        }

        // Busca y guarda el canvas de la tienda.
        if (!done && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Shop")
        {
            canvas = GameObject.Find("Shop").GetComponent<Canvas>();
            done = true;
        }
    }

    private bool HasPurchased()
    {
        foreach (var improvement_ in stockImprovements)
            if (improvement_.isVersion)
                return true;

        return false;
    }

    // Cierra y abre la interfaz de la tienda.
    public void OpenCloseShop()
    {
        if (canvas != null)
        {
            canvas.transform.GetChild(0).GetChild(5).GetComponent<Button>().onClick.RemoveAllListeners();
            canvas.transform.GetChild(0).GetChild(5).GetComponent<Button>().onClick.AddListener(() => OpenCloseShop());

            canvas.enabled = !canvas.enabled;
            inventory.DontOpenInventory = !inventory.DontOpenInventory;

            // Destruye todos los botones generados para evitar duplicaciones.
            foreach (Transform child in GameObject.Find("Objects List").transform.GetChild(0).transform)
                Destroy(child.gameObject);

            GameObject iconSlaveGO = GameObject.Find("IconSlave");
            if(iconSlaveGO) Destroy(iconSlaveGO);
            
            if (displayzone.activeSelf) // Desactiva la zona de esposicion de los objetos, compañeros o mejoras.
                displayzone.SetActive(false);

            // Destruye y desactiva el grupo de compañeros de reserva.
            if (GameObject.Find("GroupOrGulab").GetComponent<Image>().enabled)
            {
                foreach (Transform child in GameObject.Find("GroupOrGulab").transform)
                    Destroy(child.gameObject);

                GameObject.Find("GroupOrGulab").GetComponent<Image>().enabled = false;
            }


            // Genera las botones de la mercancia.
            if (canvas.enabled)
            {
                foreach (ScriptableObject scriptable in stock.Values)
                {
                    GameObject obj = Instantiate(button, canvas.transform.GetChild(0).GetChild(0).GetChild(0));
                    obj.transform.SetParent(canvas.transform.GetChild(0).GetChild(0).GetChild(0).transform);
                    string nam = scriptable.name.Substring(4, scriptable.name.Length - 4).Replace("^", " ").ToUpper();
                    obj.name = nam;
                    obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = nam;
                    obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.black;
                    obj.GetComponent<Button>().onClick.AddListener(() => { ItemSelected(scriptable); });

                    ColorBlock colors = new ColorBlock();
                    colors.normalColor = new Color(255, 255, 255, 1);
                    colors.pressedColor = new Color(255, 255, 255, 1);
                    colors.disabledColor = new Color(255, 255, 255, 1);
                    colors.highlightedColor = new Color(255, 0, 0, 1f);
                    colors.selectedColor = new Color(255, 255, 0, 1f);
                    colors.colorMultiplier = 1;

                    obj.GetComponent<Button>().colors = colors;
                }
            }
        }

    }

    // Este solo se utiliza en la tienda normal para que el jugador pueda comprar un objeto varias veces a la vez.
    public void QuantityChange(bool disminuir)
    {
        if (!disminuir)
            ++quantity;
        else
            quantity = Mathf.Max(1, --quantity);

        GameObject.Find("Cantidad").GetComponent<TextMeshProUGUI>().text = quantity.ToString();

        ObjectData data = this.data as ObjectData;

        buy.text = $"Cost: {(data.Cost * quantity)}Ma";
    }

    // Muestra la informacion de la mercancia.
    public void ItemSelected(ScriptableObject @object)
    {
        displayzone.SetActive(true);
        data = @object;

        quantity = 1;

        // Dependiendo del tipo de tienda tiene que mostrar unas cosas o otras.
        switch (gameObject.tag)
        {
            case "Normal Shop":
                GameObject.Find("Cantidad").GetComponent<TextMeshProUGUI>().text = quantity.ToString();

                ObjectData data = @object as ObjectData;

                icon.sprite = data.Icon;
                description.text = data.Description;
                buy.text = $"Cost: {data.Cost}Ma";
                break;
            case "Slave Shop":
                var iconSlaveGO = GameObject.Find("IconSlave");
                if(iconSlaveGO) Destroy(iconSlaveGO);

                StatsPersistenceData slave = @object as StatsPersistenceData;

                var sprite = slave.CharacterPB.transform.GetChild(3).gameObject;

                var spriteGo = Instantiate(sprite, iconSlave.transform.position, Quaternion.identity, iconSlave.transform);
                spriteGo.name = "IconSlave";
                spriteGo.transform.localPosition = Vector3.zero;

                description.text = slave.Description;
                buy.text = $"Cost: {slave.Cost}Ma";
                break;

            case "Special Shop":
                ImprovementsData improvement = @object as ImprovementsData;

                description.text = improvement.Description;
                buy.text = $"Cost: {improvement.CostRefined}MaraRef";

                break;
        }
    }

    // Cuando el jugador da al boton de comprar esta funcion se encarga de realizar todo el proceso de la transaccion.
    public void Buy()
    {
        MoneyPlayer money = GameObject.Find("System").GetComponent<MoneyPlayer>();
        MoneyPlayer moneyRef = GameObject.Find("System").GetComponent<MoneyPlayer>();

        float totalCost;

        // Dependiendo del tipo de tienda tiene que mostrar unas cosas o otras.
        switch (gameObject.tag)
        {
            case "Normal Shop":
                ObjectData data = this.data as ObjectData;
                totalCost = (data.Cost * quantity);

                if ((money.Money - totalCost) >= 0)
                {
                    money.Money -= totalCost;
                    inventory.AddObjectToInventory(data.name, data, quantity);
                }
                break;
            case "Slave Shop":
                StatsPersistenceData slave = this.data as StatsPersistenceData;
                totalCost = (slave.Cost * quantity);

                if ((money.Money - totalCost) >= 0)
                {
                    // En el caso de que el grupo este lleno se le dara la opcion al jugador de si quiere incorporarlo al grupo o almacenarlo.
                    // En el caso contrario se incluira automaticamente al grupo.
                    if (party.CharactersTeamStats.Count < 4)
                    {
                        party.CharactersTeamStats.Add(slave);

                        money.Money -= totalCost;

                        stock.Remove(slave.name.Substring(slave.name.IndexOf("_") + 1));

                        Destroy(GameObject.Find(slave.name.Substring(slave.name.IndexOf("_") + 1).ToUpper()));
                        
                        GameObject.Find("System").GetComponent<TeamViewManager>().SetActiveTeamData();

                        displayzone.SetActive(false);
                    }
                    else
                        ChooseGG(slave, money);
                }
                break;
            case "Special Shop":
                displayzone.SetActive(false);
                List<StatsPersistenceData> target = Resources.LoadAll<StatsPersistenceData>("Data/CharactersStatsPersistance").ToList();
                string nombrePWR;
                ImprovementsData improv = this.data as ImprovementsData;
                ObjectData objdata = this.data as ObjectData;

                if (!improv.isVersion && !HasPurchased())
                    return;
                if ((money.MoneyRefined - improv.CostRefined) >= 0)
                {
                    money.MoneyRefined -= improv.CostRefined;
                    target.ForEach(x =>
                    {
                        if (x.Protagonist == true)
                        {
                            nombrePWR = improv.BuyImprovement(x);

                            UpdateImprovementsId(nombrePWR);
                            nombrePWR = "";
                        };
                    });

                }
                break;
        }
    }

    // Incorpora las mejoras permanentes.
    public void UpdateImprovementsId(string nombrePWR)
    {
        ImprovementsData[] mejoras = Resources.LoadAll<ImprovementsData>("Data/Improvements");
        ObjectData[] objects = Resources.LoadAll<ObjectData>("Data/Objects");
        StatsPersistenceData[] slaves = Resources.LoadAll<StatsPersistenceData>("Data/CharactersStatsPersistance");
        ImprovementsData improv = this.data as ImprovementsData;

        for (int i = 0; i < mejoras.Length; i++)
        {
            if (mejoras[i].name == nombrePWR)
            {
                string objName = mejoras[i].name.Substring(4, mejoras[i].name.Length - 4).Replace("^", " ").ToUpper();
                AudioManager.Instance.PlaySoundButtons();
                HideButtons(objName);

                switch (mejoras[i].ImprovementsType)
                {
                    case ImprovementsTypes.Health:
                        mejoras[i + 1].isVersion = true;
                        mejoras[i].isVersion = false;
                        stock.Remove(objName);
                        mejoras[i + 1].idHealth = mejoras[i].idHealth;
                        break;
                    case ImprovementsTypes.Mana:
                        mejoras[i + 1].isVersion = true;
                        mejoras[i].isVersion = false;
                        stock.Remove(objName);
                        mejoras[i + 1].idMana = mejoras[i].idMana;
                        break;
                    case ImprovementsTypes.CriticalChance:
                        mejoras[i + 1].isVersion = true;
                        mejoras[i].isVersion = false;
                        stock.Remove(objName);
                        mejoras[i + 1].idCriticalChance = mejoras[i].idCriticalChance;
                        break;
                    case ImprovementsTypes.Discount:
                        mejoras[i + 1].isVersion = true;
                        mejoras[i].isVersion = false;

                        stock.Remove(objName);

                        for (int z = 0; z < 1; z++)
                        {
                            mejoras[z].DiscountItems(objects);
                            mejoras[z].DiscountImprovements(mejoras);
                            mejoras[z].DiscountSlaves(slaves);
                        }

                        mejoras[i + 1].idDiscount = mejoras[i].idDiscount;
                        mejoras[i + 1].DiscountV = mejoras[i].DiscountV;
                        break;
                }
            }
        }

        displayzone.SetActive(false);
    }

    // Oculta los botones de las mejoras permanentes ya obtenidas.
    void HideButtons(string mejoraName)
    {
        Transform button = canvas.transform.GetChild(0).GetChild(0).GetChild(0).Find(mejoraName);

        if (button != null)
            button.gameObject.SetActive(false);
    }


    // Las opciones de que se quiere hacer con el esclavo obtenido.
    void ChooseGG(StatsPersistenceData slave, MoneyPlayer money)
    {
        GameObject choose = GameObject.Find("GroupOrGulab");
        choose.GetComponent<Image>().enabled = true;

        GameObject group = Instantiate(button, choose.transform);
        group.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Party";
        group.GetComponent<Button>().onClick.AddListener(() => { Party(choose, slave, money); });

        GameObject gulab = Instantiate(button, choose.transform);
        gulab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Dimensional pocket";

        gulab.GetComponent<Button>().onClick.AddListener(() =>
        {
            party.CharactersBackupStats.Add(slave);

            money.Money -= slave.Cost;

            foreach (Transform child in choose.transform)
                Destroy(child.gameObject);

            choose.GetComponent<Image>().enabled = false;

            stock.Remove(slave.name.Substring(slave.name.IndexOf("_") + 1));

            Destroy(GameObject.Find(slave.name.Substring(slave.name.IndexOf("_") + 1).ToUpper()));

            displayzone.SetActive(false);

            GameObject.Find("System").GetComponent<TeamViewManager>().SetBackupTeamData();
        });
    }

    // Guarda el esclavo que vamos a sustituir he incorpora el nuevo.
    void Party(GameObject choose, StatsPersistenceData slave, MoneyPlayer money)
    {
        foreach (Transform child in choose.transform)
            Destroy(child.gameObject);

        foreach (StatsPersistenceData member in party.CharactersTeamStats)
        {
            if (!member.Protagonist)
            {
                GameObject move = Instantiate(button, choose.transform);
                move.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = member.name.Substring(member.name.IndexOf("_") + 1);
                move.GetComponent<Button>().onClick.AddListener(() =>
                {
                    party.CharactersBackupStats.Add(member);
                    party.CharactersTeamStats.Remove(member);
                    party.CharactersTeamStats.Add(slave);

                    money.Money -= slave.Cost;

                    foreach (Transform child in choose.transform)
                        Destroy(child.gameObject);

                    choose.GetComponent<Image>().enabled = false;

                    stock.Remove(slave.name.Substring(slave.name.IndexOf("_") + 1));

                    Destroy(GameObject.Find(slave.name.Substring(slave.name.IndexOf("_") + 1).ToUpper()));

                    displayzone.SetActive(false);

                    GameObject.Find("System").GetComponent<TeamViewManager>().SetActiveTeamData();
                    GameObject.Find("System").GetComponent<TeamViewManager>().SetBackupTeamData();
                });
            }
        }
    }
}
