using UnityEngine;

public class RandomMap : MonoBehaviour
{
    [Header("Map Collection")]
    [SerializeField] GameObject[] maps;

    [Header("Required maps")]
    [SerializeField] GameObject[] oblMaps;

    [Header("The last map of the list must be 100")]
    [SerializeField] int[] rarety;

    [SerializeField] int actualroom;
    [SerializeField] int boosRoom;

    bool done = false;

    int room = 0;
    int saveRoom = 0;
    int floor = 0;



    // Start is called before the first frame update
    void Update()
    {
        if (!done)
        {
            room = Data.Instance.Room;
            saveRoom = Data.Instance.SaveRoom;
            floor = Data.Instance.Floor;

            if (room == boosRoom)
            {
                // Instantiate(oblMaps[1], transform.position, Quaternion.Euler(new Vector3(0, 90, 0)));

                floor++;
                saveRoom = 0;
                room = 0;
            }
            else
            {
                if (saveRoom == 5)
                {
                    Instantiate(oblMaps[0], transform.position, Quaternion.Euler(new Vector3(0, 90, 0)));
                    saveRoom = 0;
                }
                else
                {
                    bool finish = false;

                    for (int i = 0; i < maps.Length; i++)
                    {
                        if (Random.value <= ((float)rarety[i] / 100) && !finish)
                        {
                            Instantiate(maps[i], transform.position, Quaternion.Euler(new Vector3(0, 90, 0)));
                            finish = true;
                        }
                    }

                    saveRoom++;
                }
            }

            Data.Instance.Room = room;
            Data.Instance.SaveRoom = saveRoom;
            Data.Instance.Floor = floor;

            done = true;
        }
    }
}
