using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ObjectStock
{
    private ObjectData data;
    private GameObject buttonINV3D;
    private GameObject buttonINV2D;
    private int count = 1;

    public ObjectData Data { get { return data; } }
    public int Count { get { return count; } set { this.count = value; } }
    public GameObject ButtonINV3D { get { return buttonINV3D; } set { this.buttonINV3D = value; } }
    public GameObject ButtonINV2D { get { return buttonINV2D; } set { this.buttonINV2D = value; } }

    public ObjectStock(ObjectData scriptableObject, int count)
    {
        this.data = scriptableObject;
        this.count = count;
    }
}
public class PlayerInventory : MonoBehaviour
{
    [Header("Referencia de boton")]
    [SerializeField] GameObject buttonRef3D;
    [SerializeField] GameObject buttonRef2D;

    [Header("UI grid de inventario")]
    [SerializeField] GameObject inventoryUI3D;
    [SerializeField] GameObject inventoryUI2D;

    [SerializeField] ObjectData[] data;

    bool inventoryState = false;
    bool done = false;
    bool dontOpenInventory = false;

    Scene scene;

    public bool InventoryState { get {  return inventoryState; } }
    public bool DontOpenInventory { get { return dontOpenInventory; } set { dontOpenInventory = value; } }

    Dictionary<string, ObjectStock> inventory = new Dictionary<string, ObjectStock>();

    EnterBattle enterBattle;

    void Update()
    {
        if (scene != UnityEngine.SceneManagement.SceneManager.GetActiveScene())
        {
            done = false;
            scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        }

        if (!done && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Title")
        {
            inventoryUI3D = GameObject.Find("Inventory").transform.GetChild(0).GetChild(0).gameObject;
            inventoryUI2D = GameObject.Find("MoveButtons");

            enterBattle = GetComponent<EnterBattle>();

            foreach (ObjectData data in data)
                AddObjectToInventory(data.name, data, 1);

            done = true;
        }
    }

    // Abre y cierra el inventario.
    public void OpenCloseInventory()
    {
        if (!dontOpenInventory)
        {
            if (!enterBattle.OneTime) // En el caso de no estar en batalla solo genera y destruye los botones del objeto en el inventario del mundo 3D.
            {
                if (!inventoryState) // En el caso de abrir inventario este crea los botones.
                {
                    GameObject.Find("Inventory").GetComponentInParent<Canvas>().enabled = true;

                    foreach (ObjectStock item in inventory.Values)
                    {
                        item.ButtonINV3D = CreateButtonINV3D(item);
                    }

                    Time.timeScale = 0f;

                    inventoryState = true;
                }
                else // En el caso contrario los elimina.
                {
                    foreach (ObjectStock item in inventory.Values)
                    {
                        Destroy(item.ButtonINV3D);
                    }

                    GameObject.Find("Inventory").GetComponentInParent<Canvas>().enabled = false;

                    GameObject.Find("Inventory").transform.GetChild(1).gameObject.SetActive(false);

                    Time.timeScale = 1f;

                    inventoryState = false;
                }
            }
            else // En el caso contrario solo genera los botones en el inventario de la batalla 2D.
            {
                if (!inventoryState) // En el caso de abrir inventario este crea los botones.
                {
                    // Localizamos combatflow
                    var combatFlowRef = GameObject.Find("System").GetComponent<CombatFlow>();

                    // Hacemos desaparecer a los botones targets de la batalla
                    if (combatFlowRef.EnemyBT.Count > 0)
                    {
                        combatFlowRef.EnemyBT.ForEach(bt => Destroy(bt));
                        combatFlowRef.EnemyBT.Clear();
                    }

                    foreach (ObjectStock item in inventory.Values)
                    {
                        item.ButtonINV2D = CreateButtonINV2D(item);

                        item.ButtonINV2D.transform.localScale = new Vector3(1f, 1f, 1f);
                    }

                    inventoryState = true;
                }
                else // En el caso contrario los elimina.
                {
                    EliminateINVButtons();

                    inventoryState = false;
                }
            }
        }
    }

    // Elimina o disminuye la cantidad de objetos del inventario segun la situacion.
    public void RemoveObjectFromInventory(string name)
    {
        if (inventory.ContainsKey(name.ToUpper())) // Comprueba que exista el objeto dado en el inventario.
        {
            Debug.Log("He entrao");
            if (inventory[name.ToUpper()].Count > 1) // Si objeto es mayor a uno disminuye su cantidad
            {

                Debug.Log("Counter of '" + name.ToUpper() + "' went down");
                inventory[name.ToUpper()].Count--;

                if (!enterBattle.OneTime) // En caso de no estar en batalla modifica el texto.
                    EditButtonINVText(inventory[name.ToUpper()]);
                else
                {
                    EliminateINVButtons();
                    inventoryState = false;
                }
            }
            else // En el caso contrario se remueve del diccionario.
            {
                Debug.Log("He entrao x2");
                Debug.Log(name.ToUpper() + " was eliminated from dictionary");

                if (!enterBattle.OneTime) // En caso de no estar en batalla elimina solo un bot�n.
                    Destroy(inventory[name.ToUpper()].ButtonINV3D);
                else
                {
                    EliminateINVButtons();
                    inventoryState = false;
                }

                inventory.Remove(name.ToUpper());
            }
        }
    }

    // A�ade un objeto al inventario.
    public void AddObjectToInventory(string name, ScriptableObject scriptableObject, int count)
    {
        var realName = name.Substring(4, name.Length - 4).ToUpper();

        if (!inventory.ContainsKey(realName)) // Cuando es un objeto nuevo se incluye al diccionario
        {
            Debug.Log("Add object to inventory" + name.ToUpper());
            inventory.Add(realName, new ObjectStock(scriptableObject as ObjectData, count));
        }
        else // Cuando el objeto ya existe aumenta su cantidad
        {
            Debug.Log("Object exist. Incrementing count of that object" + name.ToUpper());
            inventory[realName].Count += count;
        }
    }

    // Crea y devuelve el boton 3D
    private GameObject CreateButtonINV3D(ObjectStock stock)
    {
        GameObject button = Instantiate(buttonRef3D, Vector3.zero, Quaternion.identity);
        button.transform.SetParent(inventoryUI3D.transform);

        Button buttonCMP = button.GetComponent<Button>();
        buttonCMP.onClick.AddListener(() => { stock.Data.Click(this); });

        button.GetComponentInChildren<Image>().sprite = stock.Data.Icon;
        button.GetComponentInChildren<TextMeshProUGUI>().text = stock.Data.name.Substring(4, stock.Data.name.Length - 4) + " x" + stock.Count;

        return button;
    }

    // Crea y devuelve el boton 2D.
    private GameObject CreateButtonINV2D(ObjectStock stock)
    {
        GameObject button = Instantiate(buttonRef2D, Vector3.zero, Quaternion.identity);
        button.transform.SetParent(inventoryUI2D.transform);

        Button buttonCMP = button.GetComponent<Button>();
        buttonCMP.onClick.AddListener(() => { stock.Data.Click(this); });
        //Debug.Log("buttonCMP "+ buttonCMP.name);
        button.GetComponentInChildren<TextMeshProUGUI>().text = stock.Data.name.Substring(4, stock.Data.name.Length - 4) + " x" + stock.Count;
        
        return button;
    }

    // Edita el texto de los botones del inventario del mundo 3D.
    private void EditButtonINVText(ObjectStock data)
    {
        data.ButtonINV3D.GetComponentInChildren<TextMeshProUGUI>().text = data.Data.name.Substring(4, data.Data.name.Length - 4) + " x" + data.Count;
    }

    // Elimina los botones del inventario en la batalla 2D.
    public void EliminateINVButtons()
    {
        foreach (ObjectStock stock in inventory.Values)
        {
            Destroy(stock.ButtonINV2D);
        }
    }
}
