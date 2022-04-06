using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    int roomSizeX;
    int roomSizeY;
    RoomType roomType;
    int[,] roomData;

    public Room(int roomSizeX, int roomSizeY, RoomType roomType)
    {
        this.roomType = roomType;
        this.roomSizeX = roomSizeX;
        this.roomSizeY = roomSizeY;
        int[,] roomData = new int[roomSizeX,roomSizeY];
        for(int x = 0; x < roomSizeX; x++)
        {
            for(int y = 0; y < roomSizeY; y++)
            {
                roomData[x,y] = 0;
            }
        }
    }

    public int[,] getRoomData()
    {
        return roomData;
    }
}

public enum RoomType
{
    CIRCUS
}
