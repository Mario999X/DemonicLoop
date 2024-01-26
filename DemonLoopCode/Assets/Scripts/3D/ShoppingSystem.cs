using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShoppingSystem : MonoBehaviour
{
    [SerializeField] GameObject button;

    bool done = false;

    int quantity = 0;

    Canvas canvas;

    Scene scene;

    Dictionary<string, ObjectData> stock = new();

    PlayerInventory inventory;
    ScriptableObject data;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.Find("System").GetComponent<PlayerInventory>();

        switch (gameObject.tag)
        {
            case "Normal Shop":
                ObjectData[] objects = Resources.LoadAll<ObjectData>("Data/Objects");

                foreach (ObjectData obj in objects)
                {
                    string atkName = obj.name.Substring(4, obj.name.Length - 4).Replace("^", " ").ToUpper();

                    stock.Add(atkName, obj);

                    //Debug.Log("Ataque " + atkName + " | danno base " + (@object as AttackData).BaseDamage + " | LOADED TO CACHE");
                }
                break;
            case "Slave Shop":
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

        if (!done && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Shop")
        {
            canvas = GameObject.Find("Shop").GetComponent<Canvas>();
        }
    }

    public void OpenCloseShop()
    {
        if (canvas != null)
        {
            canvas.enabled = !canvas.enabled;
            inventory.DontOpenInventory = !inventory.DontOpenInventory;

            for (int i = 0; i < stock.Count; i++)
            {
                GameObject obj = Instantiate(button, canvas.transform.GetChild(0).transform);
                obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = stock.Keys.ElementAt(i).ToString();
                obj.GetComponent<Button>().onClick.AddListener(() => { ItemSelected(stock.Values.ElementAt(i)); });
            }
        }
    }

    public void QuantityChange(bool disminuir)
    {
        if (!disminuir)
            ++quantity;
        else
            quantity = Mathf.Max(0, --quantity);
    }

    public void ItemSelected(ScriptableObject @object)
    {
        data = @object;
    }
}
