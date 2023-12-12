using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    static Data instance;
    public static Data Instance { get { return instance; } }

    int saveRoom;
    int room;
    int floor;

    public int SaveRoom { get { return saveRoom; } set { saveRoom = value; } }
    public int Room { get { return room; } set { room = value; } }
    public int Floor { get { return floor; } set { floor = value; } }

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(this); }
    }
}
