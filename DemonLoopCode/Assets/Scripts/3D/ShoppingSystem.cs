using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShoppingSystem : MonoBehaviour
{
    Canvas canvas;

    Scene scene;

    bool done = false;

    Dictionary<string, ObjectData> stock = new();

    // Start is called before the first frame update
    void Start()
    {
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

    void OpenCloseShop()
    {
        if (canvas != null)
            canvas.enabled = !canvas.enabled;
    }


}
