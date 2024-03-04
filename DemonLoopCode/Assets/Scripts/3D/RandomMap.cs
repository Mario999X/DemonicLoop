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
            // Carga los datos del mapa.
            room = Data.Instance.Room;
            saveRoom = Data.Instance.SaveRoom;
            floor = Data.Instance.Floor;
            bossRoom = Data.Instance.BossRoom;

            if (room == bossRoom) // Si el numero de la habitacion coincide con la sala del jefe este pondra la sala del jefe.
            {
                Instantiate(bossRooms[floor], transform.position, Quaternion.identity);

                bossRoom += 4;

                floor++;
                saveRoom = 0;
                room = 0;
            }
            else // En caso contrario...
            {
                if (saveRoom == 4) // Si coincide con una sala de guardado pondra una.
                {
                    Instantiate(oblMaps[0], transform.position, Quaternion.identity);
                    saveRoom = 0;
                }
                else // Sino pondra una sala normal de la lista de forma aleatoria.
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

            // Guarda los datos de la sala.
            Data.Instance.SaveRoom = saveRoom;
            Data.Instance.Room = room;
            Data.Instance.Floor = floor;
            Data.Instance.BossRoom = bossRoom;

            done = true;
        }
    }
}
