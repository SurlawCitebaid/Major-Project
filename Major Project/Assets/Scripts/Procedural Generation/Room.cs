using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    RoomData roomData;
    Vector2 spawnPosition;
    public int roomSizeX;
    public int roomSizeY;
    int[,] roomGrid;

    //Wall prefab
    public List<GameObject> tiles;

    int maxNumberOfPlatforms;
    //The widest and narrowist a platform can be
    int maxPlatformSize;
    int minPlatformSize;

    Transform parent;

    public Room(Vector2 spawnPosition, int roomSizeX, int roomSizeY, List<GameObject> tiles, Transform parent, int maxNumberOfPlatforms, int maxPlatformSize, int minPlatformSize)
    {
        this.spawnPosition = spawnPosition;
        this.roomSizeX = roomSizeX;
        this.roomSizeY = roomSizeY;
        this.tiles = tiles;
        this.parent = parent;
        this.maxNumberOfPlatforms = maxNumberOfPlatforms;
        this.maxPlatformSize = maxPlatformSize;
        this.minPlatformSize = minPlatformSize;
    }

    public void createRoom()
    {
        roomData = new RoomData(roomSizeX, roomSizeY, RoomType.CIRCUS);
        roomGrid = new int[roomSizeX, roomSizeY];
        //Populates the data with an in representing, walls, platforms objects
        fillRoomData();
        generateRoom();
    }


    void fillRoomData()
    {
        //Walls 
        placeWalls();
        //Platforms
        placePlatforms();

    }

    void generateRoom()
    {
        for (int x = 0; x < roomSizeX; x++)
        {
            for (int y = 0; y < roomSizeY; y++)
            {
                //Wall
                if (roomGrid[x, y] == 1)
                {
                   GameObject.Instantiate(tiles[0], new Vector3(spawnPosition.x+(float)x, spawnPosition.y+(float)y), Quaternion.identity, parent);
                }
                //Platform
                else if (roomGrid[x, y] == 2)
                {
                    GameObject.Instantiate(tiles[1], new Vector3(spawnPosition.x + (float)x, spawnPosition.y + (float)y), Quaternion.identity, parent);
                }
            }
        }
    }

    void placeWalls()
    {

        for (int x = 0; x < roomSizeX; x++) {
            for (int y = 0; y < roomSizeY; y++)
            {
                //Adds a border around
                if (x == 0 || x == roomSizeX - 1 || y == 0 || y == roomSizeY - 1)
                    roomGrid[x, y] = 1;
            }
        }

    }

    void placePlatforms()
    {
        int currentNumberOfPlatforms = 0;

        //Runs until all platforms are placed
        while (currentNumberOfPlatforms < maxNumberOfPlatforms)
        {
           
            //A random range out side of the borders
            int xPos = Random.Range(0 + 1, roomSizeX - 1);
            int yPos = Random.Range(0 + 1, roomSizeY - 1);

            //A random size for a platform with the max platform range
            int randomPlatformSize = Random.Range(minPlatformSize, maxPlatformSize + 1);

            //Used for the platform counter
            bool platformAdded = false;
            for(int i = 0; i < randomPlatformSize; i++)
            {
                //Makes sure we arent out of bound of the roomGrid
                if (xPos + i < roomSizeX-1)
                {
                    //Checks to see if there is an air block underneath the platform and above
                    if (roomGrid[xPos + i, yPos - 1] == 0 && roomGrid[xPos + i, yPos + 1] == 0)
                    {
                        //Place a platform
                        roomGrid[xPos + i, yPos] = 2;
                        platformAdded = true;
                    }
                }
                
            }
            //Add to the platforms count if one was added
            if (platformAdded)
            {
                platformAdded = false;
                currentNumberOfPlatforms++;
            }
              
        }
    }
}
