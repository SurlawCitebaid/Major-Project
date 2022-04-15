using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData
{
    int roomSizeX;
    int roomSizeY;
    RoomType roomType;
    int[,] roomData;


    public RoomData(int roomSizeX, int roomSizeY, RoomType roomType)
    {
        this.roomType = roomType;
        this.roomSizeX = roomSizeX;
        this.roomSizeY = roomSizeY;
        //a grid of solid and air tiles in the room
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

//Types of themed rooms
public enum RoomType
{
    CIRCUS
}
