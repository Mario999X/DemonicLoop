using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShoppingSystem : MonoBehaviour
{
    [SerializeField] GameObject button;

    bool done = false;

    int quantity = 1;

    Image icon;
    
    TextMeshProUGUI description;
    TextMeshProUGUI buy;

    [SerializeField] GameObject displayzone;

    Canvas canvas;

    Scene scene;

    Dictionary<string, ScriptableObject> stock = new();

    PlayerInventory inventory;
    Data party;
    [SerializeField] ScriptableObject data;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.Find("System").GetComponent<PlayerInventory>();
        party = GameObject.Find("System").GetComponent<Data>();

        switch (gameObject.tag)
        {
            case "Normal Shop":
                ObjectData[] objects = Resources.LoadAll<ObjectData>("Data/Objects");

                foreach (ObjectData obj in objects)
                {
                    string objName = obj.name.Substring(4, obj.name.Length - 4).Replace("^", " ").ToUpper();

                    stock.Add(objName, obj);

                    //Debug.Log("Ataque " + objName + " | danno base " + (@object as AttackData).BaseDamage + " | LOADED TO CACHE");
                }

                displayzone = GameObject.Find("Object display");
                icon = displayzone.transform.GetChild(0).GetComponent<Image>();
                
                description = displayzone.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
                buy = displayzone.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>();
                
                displayzone.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(() => Buy());
                displayzone.SetActive(false);
                break;
            case "Slave Shop":
                List<StatsPersistenceData> slaves = Resources.LoadAll<StatsPersistenceData>("Data/CharactersStatsPersistance").ToList();

                List<StatsPersistenceData> group = party.CharactersTeamStats;

                group.ForEach(g => { if (slaves.Contains(g)) slaves.Remove(g); });

                slaves.ForEach(s => stock.Add(s.name.Substring(s.name.IndexOf("_") + 1), s));

                displayzone = GameObject.Find("Slave display");
                icon = displayzone.transform.GetChild(0).GetComponent<Image>();

                description = displayzone.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();

                buy = displayzone.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();

                displayzone.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => Buy());
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

        if (!done && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Shop")
        {
            canvas = GameObject.Find("Shop").GetComponent<Canvas>();

            canvas.transform.GetChild(0).GetChild(4).GetComponent<Button>().onClick.RemoveAllListeners();
            canvas.transform.GetChild(0).GetChild(4).GetComponent<Button>().onClick.AddListener(() => OpenCloseShop());

            done = true;
        }
    }

    public void OpenCloseShop()
    {
        if (canvas != null)
        {
            canvas.enabled = !canvas.enabled;
            inventory.DontOpenInventory = !inventory.DontOpenInventory;

            foreach (Transform child in GameObject.Find("Objects List").transform.GetChild(0).transform)
            {
                Destroy(child.gameObject);
            }

            if (displayzone.activeSelf)
                displayzone.SetActive(false);

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

    public void QuantityChange(bool disminuir)
    {
        if (!disminuir)
            ++quantity;
        else
            quantity = Mathf.Max(1, --quantity);

        GameObject.Find("Cantidad").GetComponent<TextMeshProUGUI>().text = quantity.ToString();

        switch (gameObject.tag)
        {
            case "Normal Shop":
                ObjectData data = this.data as ObjectData;

                buy.text = $"Cost: {(data.Cost * quantity)}Ma";
                break;
            case "Slave Shop":
                break;
        }
    }

    public void ItemSelected(ScriptableObject @object)
    {
        displayzone.SetActive(true);
        data = @object;

        quantity = 1;

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
                StatsPersistenceData slave = @object as StatsPersistenceData;

                icon.sprite = slave.CharacterPB.transform.GetChild(3).GetComponent<Image>().sprite;
                buy.text = $"Cost: {slave.Cost}Ma";
                break;
        }
    }

    public void Buy()
    {
        MoneyPlayer money = GameObject.Find("System").GetComponent<MoneyPlayer>();
        float totalCost;

        switch (gameObject.tag)
        {
            case "Normal Shop":
                ObjectData data = this.data as ObjectData;
                totalCost = (data.Cost * quantity);

                if ((money.Money - totalCost) >= 0)
                {
                    money.Money -= totalCost;
                    inventory.AddObjectToInventory(data.name, data, quantity);

                    //Debug.Log($"Se han comprado {quantity} {data.name}");
                }
                break;
            case "Slave Shop":
                StatsPersistenceData slave = this.data as StatsPersistenceData;
                totalCost = (slave.Cost * quantity);

                if ((money.Money - totalCost) >= 0)
                {
                    money.Money -= totalCost;
                    
                    if (party.CharactersTeamStats.Count < 4)
                    {
                        party.CharactersTeamStats.Add(slave);
                    }
                    else
                    {
                        party.CharactersBackupStats.Add(slave);
                    }

                    stock.Remove(slave.name.Substring(slave.name.IndexOf("_") + 1));

                    Destroy(GameObject.Find(slave.name.Substring(slave.name.IndexOf("_") + 1).ToUpper()));
                }
                break;
        }
    }
}
