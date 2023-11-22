using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] float distance = 10f;
    [SerializeField] LayerMask layer;

    bool click = false;

    public bool Click { set { this.click = value; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.right, out hit, distance, layer) && click)
        {
            Debug.DrawLine(transform.position, hit.point, Color.green);
            PlayerInventory inventory = GameObject.Find("System").GetComponent<PlayerInventory>();

            switch (hit.transform.name)
            {
                case "Chest":
                    Content content = hit.transform.GetComponent<ChestContent>().chest();
                    inventory.AddObjectToInventory(content.Data.name, content.Data, content.Count);
                    break;
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.right * distance, Color.red);
        }
    }
}
