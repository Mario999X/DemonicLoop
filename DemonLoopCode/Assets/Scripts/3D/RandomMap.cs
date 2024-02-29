using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomMap : MonoBehaviour
{
    [Header("Map Collection")]
    [SerializeField] GameObject[] maps;
    [SerializeField] List<GameObject> bossRooms;

    [Header("Required maps")]
    [SerializeField] GameObject[] oblMaps;

    [Header("The last map of the list must be 100")]
    [SerializeField] int[] rarity;

    [SerializeField] int actualroom;

    bool done = false;

    int room = 0;
    int saveRoom = 0;
    int floor = 0;
    int bossRoom;

    // Start is called before the first frame update
    void Update()
    {
        if (!done)
        {
            room = Data.Instance.Room;
            saveRoom = Data.Instance.SaveRoom;
            floor = Data.Instance.Floor;
            bossRoom = Data.Instance.BossRoom;

            actualroom = room;

            Debug.Log(bossRoom);

            if (room == bossRoom)
            {
                Instantiate(bossRooms[floor], transform.position, Quaternion.identity);

                bossRoom += 4;

                floor++;
                saveRoom = 0;
                room = 0;
            }
            else
            {
                if (saveRoom == 4)
                {
                    Instantiate(oblMaps[0], transform.position, Quaternion.Euler(new Vector3(0, 90, 0)));
                    saveRoom = 0;
                }
                else
                {
                    bool finish = false;

                    for (int i = 0; i < maps.Length; i++)
                    {
                        if (Random.value <= ((float)rarity[i] / 100) && !finish)
                        {
                            Instantiate(maps[i], transform.position, Quaternion.identity);
                            finish = true;
                        }
                    }

                    saveRoom++;
                }

                room++;
            }

            Data.Instance.Room = room;
            Data.Instance.SaveRoom = saveRoom;
            Data.Instance.Floor = floor;
            Data.Instance.BossRoom = bossRoom;

            done = true;
        }
    }
}
