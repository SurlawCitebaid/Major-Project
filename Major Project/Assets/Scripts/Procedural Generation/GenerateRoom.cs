using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRoom
{
    Room room;
    Vector2 spawnPosition;
    public int roomSizeX;
    public int roomSizeY;
    int[,] roomData;
    //Wall prefab
    public GameObject wall;

    public GenerateRoom(Vector2 spawnPosition, int roomSizeX, int roomSizeY, GameObject wall)
    {
        this.spawnPosition = spawnPosition;
        this.roomSizeX = roomSizeX;
        this.roomSizeY = roomSizeY;
        this.wall = wall;
    }

    public void createRoom()
    {
        room = new Room(roomSizeX, roomSizeY, RoomType.CIRCUS);
        roomData = new int[roomSizeX, roomSizeY];
        //Populates the data with an in representing, walls, platforms objects
        fillRoomData();
        generateRoom();
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
                   GameObject.Instantiate(wall, new Vector3(spawnPosition.x+(float)x, spawnPosition.y+(float)y), Quaternion.identity);
                }
            }
        }
    }
}
