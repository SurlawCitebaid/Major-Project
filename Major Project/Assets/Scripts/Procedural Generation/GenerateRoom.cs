using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRoom : MonoBehaviour
{
    Room room;
    public int roomSizeX;
    public int roomSizeY;
    int[,] roomData;
    //Wall prefab
    public GameObject wall;
    // Start is called before the first frame update
    void Start()
    {
        room = new Room(roomSizeX, roomSizeY, RoomType.CIRCUS);
        roomData = new int[roomSizeX, roomSizeY];
        //Populates the data with an in representing, walls, platforms objcts
        fillRoomData();
        generateRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void fillRoomData()
    {
        for (int x = 0; x < roomSizeX; x++)
        {
            for (int y = 0; y < roomSizeY; y++)
            {
                //Border 
                if (x == 0 || x == roomSizeX-1 || y == 0 || y == roomSizeY - 1)
                {
                    roomData[x, y] = 1;
                }
                //Air Blocks
                else
                {
                    roomData[x, y] = 0;
                }
            }
        }
    }

    void generateRoom()
    {
        for (int x = 0; x < roomSizeX; x++)
        {
            for (int y = 0; y < roomSizeY; y++)
            {
                //Border and a border
                if (roomData[x, y] == 1)
                {
                   Instantiate(wall, new Vector3((float)x, (float)y), Quaternion.identity);
                }
            }
        }
    }
}
