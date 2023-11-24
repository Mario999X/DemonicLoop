using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] float distance = 10f;
    [SerializeField] LayerMask layer;

    [SerializeField] bool click = false;

    MoneyPlayer playerMoney;

    public bool Click { set { this.click = value; } }

    // Start is called before the first frame update
    void Start()
    {
        playerMoney = GameObject.Find("System").GetComponent<MoneyPlayer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.right, out hit, distance, layer) && click)
        {
            Debug.Log("Click");

            Debug.DrawLine(transform.position, hit.point, Color.green);
            PlayerInventory inventory = GameObject.Find("System").GetComponent<PlayerInventory>();

            switch (hit.transform.name)
            {
                case "Chest":
                    Content content = hit.transform.GetComponent<ChestContent>().chest();

                    Debug.Log(content.Money);

                    playerMoney.Money += content.Money;

                    if (content.Count > 0)
                    {
                        Debug.Log(content.Data);
                        inventory.AddObjectToInventory(content.Data.name.Substring(4, content.Data.name.Length - 4), content.Data, content.Count);
                    }

                    Destroy(hit.transform.GetComponent<ChestContent>());

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
