using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    //[SerializeField] ScriptableObject m_ScriptableObject;

    [Header("Referencia de boton")]
    [SerializeField] GameObject buttonRef3D;
    [SerializeField] GameObject buttonRef2D;

    [Header("UI grid de inventario")]
    [SerializeField] GameObject inventoryUI3D;
    [SerializeField] GameObject inventoryUI2D;
    [SerializeField] List<ScriptableObject> listScriptableObject = new();

    bool inventoryState = false;

    public bool InventoryState { get {  return inventoryState; } }

    Dictionary<string, ObjectStock> inventory = new Dictionary<string, ObjectStock>();

    EnterBattle enterBattle;

    // Start is called before the first frame update
    void Start()
    {
        //LoadObject();

        enterBattle = GetComponent<EnterBattle>();

        foreach (ScriptableObject m_ScriptableObject in listScriptableObject)
        {
            //Se van a�adir todos los Objectos que esten en nuestra lista
            AddObjectToInventory(m_ScriptableObject.name.Substring(4, m_ScriptableObject.name.Length - 4), m_ScriptableObject, 1);
        }

        foreach (var item in inventory)
        {
            Debug.Log(item.Key);
            Debug.Log(item.Value.Data);
            Debug.Log("item.Value.Count " + item.Value.Count);
        }

    }

    // Abre y cierra el inventario.
    public void OpenCloseInventoyry()
    {
        if (!enterBattle.OneTime) // En el caso de no estar en batalla solo genera y destruye los botones del objeto en el inventario del mundo 3D.
        {
            if (!inventoryState) // En el caso de abrir inventario este crea los botones.
            {
                inventoryUI3D.GetComponentInParent<Canvas>().enabled = true;

                foreach (ObjectStock item in inventory.Values)
                {
                    item.ButtonINV3D = CreateButtonINV3D(item);
                }

                inventoryState = true;
            }
            else // En el caso contrario los elimina.
            {
                foreach (ObjectStock item in inventory.Values)
                {
                    Destroy(item.ButtonINV3D);
                }

                inventoryUI3D.GetComponentInParent<Canvas>().enabled = false;

                inventoryState = false;
            }
        }
        else // En el caso contrario solo genera los botones en el inventario de la batalla 2D.
        {
            if (!inventoryState) // En el caso de abrir inventario este crea los botones.
            {
                foreach (ObjectStock item in inventory.Values)
                {
                    item.ButtonINV2D = CreateButtonINV2D(item);
                    
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

    // Elimina o disminuye la cantidad de objetos del inventario seg�n la situaci�n.
    public void RemoveObjectFromInventory(string name)
    {
        if (inventory.ContainsKey(name.ToUpper())) // Comprueba que exista el objeto dado en el inventario.
        {

            if (inventory[name.ToUpper()].Count > 1)
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
            else
            {
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
        if (!inventory.ContainsKey(name.ToUpper()))
        {
            Debug.Log("Add object to inventory");
            inventory.Add(name.ToUpper(), new ObjectStock(scriptableObject as ObjectData, count));
        }
        else
        {
            Debug.Log("Object exist. Incrementing count of that object");
            inventory[name.ToUpper()].Count += count;
        }
    }

    // Crea y devuelve el bot�n 2D
    private GameObject CreateButtonINV3D(ObjectStock stock)
    {
        GameObject button = Instantiate(buttonRef3D, Vector3.zero, Quaternion.identity);
        button.transform.SetParent(inventoryUI3D.transform);

        Button buttonCMP = button.GetComponent<Button>();
        buttonCMP.onClick.AddListener(() => { stock.Data.Click(this); });

        button.GetComponentInChildren<Image>().sprite = stock.Data.Icon;
        button.GetComponentInChildren<TextMeshProUGUI>().text = stock.Data.name + " x" + stock.Count;

        return button;
    }

    // Crea y devuelve el bot�n 2D.
    private GameObject CreateButtonINV2D(ObjectStock stock)
    {
        GameObject button = Instantiate(buttonRef2D, Vector3.zero, Quaternion.identity);
        button.transform.SetParent(inventoryUI2D.transform);

        Button buttonCMP = button.GetComponent<Button>();
        buttonCMP.onClick.AddListener(() => { stock.Data.Click(this); });
        Debug.Log("buttonCMP "+ buttonCMP.name);
        button.GetComponentInChildren<TextMeshProUGUI>().text = stock.Data.name + " x" + stock.Count;
        
        return button;
    }

    // Edita el texto de los botones del inventario del mundo 3D.
    private void EditButtonINVText(ObjectStock data)
    {
        data.ButtonINV3D.GetComponentInChildren<TextMeshProUGUI>().text = data.Data.name + " x" + data.Count;
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
