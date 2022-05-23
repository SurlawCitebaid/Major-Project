using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
    [SerializeField]
    int seed;

    [Header("Room Settings")]
    [SerializeField]
    int maxRooms;
    [SerializeField]
    int gridSizeX;
    [SerializeField]
    int gridSizeY;
    [SerializeField]
    int maxRoomSize;
    [SerializeField]
    int minRoomSize;

    [Header("Platform Settings")]
    [SerializeField]
    int maxNumberOfPlatforms;
    [SerializeField]
    int platformMaxSize;
    [SerializeField]
    int platformMinSize;

    int[,] grid;
    Room[,] rooms;

    [Header("Tile Assets")]
    [SerializeField]
    List<GameObject> tiles;
    // Start is called before the first frame update
    void Start()
    {
        grid = new int[gridSizeX,gridSizeY];
        rooms = new Room[gridSizeX,gridSizeY];
        Random.seed = seed;

        populateGrid();
        placeRooms();
    }


    void populateGrid()
    {
        //Sets inital values 0 being no room 1 being a room
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
        //Stores a list of  created rooms
        int[,] createdRooms = new int[maxRooms, 2];

        int roomCountCycler = 0;
        //Loop until the number of rooms have been created
        while (roomsCreated != maxRooms)
        {
            //Checking if this cell already has a room
            if(grid[currentRoomX, currentRoomY] != 1)
            {
                //Add a room created
                createdRooms[roomsCreated, 0] = currentRoomX;
                createdRooms[roomsCreated, 1] = currentRoomY;

                roomsCreated++;
                grid[currentRoomX, currentRoomY] = 1;
            }

            //Cycle the current room of rooms that have been created
            currentRoomX = createdRooms[roomCountCycler, 0];
            currentRoomY = createdRooms[roomCountCycler, 1];

            roomCountCycler++;
            //Resets the cycler
            if (roomCountCycler > roomsCreated - 1)
            {
                roomCountCycler = 0;
            }

            //Select next room to look at
            //So that it only can move NESW
            if (Random.Range(-1, 2) > 0) { 
                //The potential next room to place if conditions are met
                int nextRoomX = currentRoomX + (int)Mathf.Round(Random.Range(-1, 2));
                //So that we don't just place them all next to each other
                if (!hasMaxNeighbours(nextRoomX, currentRoomY))
                {
                    currentRoomX = nextRoomX;
                }
            }
            else
            {
                int nextRoomY = currentRoomY + (int)Mathf.Round(Random.Range(-1, 2));
                if (!hasMaxNeighbours(currentRoomX, nextRoomY))
                {
                    currentRoomY = nextRoomY;
                }
            }

            
            

        }
    }

    void placeRooms()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                //Place room if there is a 1 in the grid
                if(grid[x,y] == 1)
                {
                    Vector2 roomPostion = new Vector2(x * maxRoomSize, y * maxRoomSize);
                    //So we can have random room sizes
                    int randomRoomSizeX = Random.Range(maxRoomSize / 2, maxRoomSize);
                    int randomRoomSizeY = Random.Range(maxRoomSize / 2, maxRoomSize);
                    Room room = new Room(roomPostion,randomRoomSizeX, randomRoomSizeY, tiles, transform, maxNumberOfPlatforms, platformMaxSize, platformMinSize, grid, x, y, this);
                    room.createRoom();
                    rooms[x, y] = room;
                }
                else if (grid[x, y] == 2)
                {

                }
            }
        }
    }

    //Checks for neighbours to the room
    bool hasMaxNeighbours(int x, int y)
    {
        //How many neighbours a room can have
        int maxNeighbours = 2;
        int neighbourCount = 0;

        //Top
        if (grid[x, y+1] == 1)
            neighbourCount += 1;
        //Bottom
        if (grid[x, y-1] == 1)
            neighbourCount += 1;
        //Right
        if (grid[x + 1, y] == 1)
            neighbourCount += 1;
        //Left
        if (grid[x - 1, y] == 1)
            neighbourCount += 1;

        if (neighbourCount >= maxNeighbours)
            return true;
        return false;
    }

    public int getRoomX(int x, int y)
    {
        return rooms[x, y].roomSizeX;
    }
    public int getRoomY(int x, int y)
    {
        return rooms[x, y].roomSizeY;
    }

    public int getMaxRoomSize()
    {
        return maxRoomSize;
    }
}
