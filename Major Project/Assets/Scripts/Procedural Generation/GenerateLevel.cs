using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
    [SerializeField]
    int maxRooms;
    [SerializeField]
    int gridSizeX;
    [SerializeField]
    int gridSizeY;
    [SerializeField]
    int maxRoomSize;
    [SerializeField]
    int seed;
    int[,] grid;
    GenerateRoom[,] rooms;
    [SerializeField]
    GameObject tiles;
    // Start is called before the first frame update
    void Start()
    {
        grid = new int[gridSizeX,gridSizeY];
        rooms = new GenerateRoom[gridSizeX, gridSizeY];
        Random.seed = seed;

        populateGrid();
        placeRooms();
    }


    void populateGrid()
    {
        //Sets inital values 0 being no room
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                grid[x, y] = 0;

            }
        }

        int roomsCreated = 0;
        int currentRoomX = gridSizeX/2;
        int currentRoomY = gridSizeY/2;
        //Loop until the number of rooms have been created
        while (roomsCreated != maxRooms)
        {
            grid[currentRoomX, currentRoomY] = 1;
            //Select next room to look at
            //So that it only can move NESW
            if (Random.Range(-1, 2) > 0) { 
                
                currentRoomX = currentRoomX + (int)Mathf.Round(Random.Range(-1, 1));
            }
            else
                currentRoomY = currentRoomY + (int)Mathf.Round(Random.Range(-1, 1));

            roomsCreated++;
            Debug.Log(currentRoomX + " " + currentRoomY);
        }
    }

    void placeRooms()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                //Place room
                if(grid[x,y] == 1)
                {
                    GenerateRoom generateRoom = new GenerateRoom(new Vector2(x * maxRoomSize, y * maxRoomSize),maxRoomSize, maxRoomSize, tiles);
                    generateRoom.createRoom();
                }
            }
        }
    }
}
