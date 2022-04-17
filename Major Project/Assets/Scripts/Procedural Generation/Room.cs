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

    //Chance of an item spawning in a room
    float itemSpawnChance = 50f;

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
        //Add item pedestals
        placeItem();

    }

    void generateRoom()
    {
        for (int x = 0; x < roomSizeX; x++)
        {
            for (int y = 0; y < roomSizeY; y++)
            {
                //Air/BackGround
                if (roomGrid[x, y] == 0)
                {
                    GameObject.Instantiate(tiles[0], new Vector3(spawnPosition.x + (float)x, spawnPosition.y + (float)y), Quaternion.identity, parent);
                }
                //Wall
                else if (roomGrid[x, y] == 1)
                {
                   GameObject.Instantiate(tiles[1], new Vector3(spawnPosition.x+(float)x, spawnPosition.y+(float)y), Quaternion.identity, parent);
                }
                //Platform
                else if (roomGrid[x, y] == 2)
                {
                    GameObject.Instantiate(tiles[2], new Vector3(spawnPosition.x + (float)x, spawnPosition.y + (float)y), Quaternion.identity, parent);
                }
                //Item
                else if(roomGrid[x, y] == 3)
                {
                    GameObject.Instantiate(tiles[3], new Vector3(spawnPosition.x + (float)x, spawnPosition.y + (float)y), Quaternion.identity, parent);
                }
            }
        }
    }

    void placeWalls()
    {

        for (int x = 0; x < roomSizeX; x++) {
            for (int y = 0; y < roomSizeY; y++)
            {
                //Adds a border around the room
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
            int xPos = Random.Range(0 + 2, roomSizeX - 1);
            int yPos = Random.Range(0 + 2, roomSizeY - 1);

            //A random size for a platform with the max platform range
            int randomPlatformSize = Random.Range(minPlatformSize, maxPlatformSize + 1);

            //Used for the platform counter
            bool platformAdded = false;
            for(int i = 0; i < randomPlatformSize; i++)
            {
                //Makes sure we arent out of bound of the roomGrid
                if (xPos + i < roomSizeX-1)
                {
                    //Checks to see if there is an air block underneath the platform and above also looking for diagonal platforms
                    if (roomGrid[xPos + i, yPos - 1] == 0 && roomGrid[xPos + i, yPos + 1] == 0 
                        && roomGrid[xPos + i, yPos - 2] == 0 && roomGrid[xPos + i, yPos + 2] == 0
                        && roomGrid[xPos + i - 1, yPos + 1] == 0 && roomGrid[xPos + i + 1, yPos + 1] == 0)
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

    void placeItem()
    {
        bool itemPlaced = false;
        //chance for item to spawn
        if(Random.Range(0,100) < itemSpawnChance)
        {
            //loop until a suitible postion is found
            while (!itemPlaced)
            {
                //Random postion with in the box
                int positionX = Random.Range(2, roomSizeX - 2);
                int positionY = Random.Range(2, roomSizeY - 2);
                //checks if there is an airblock at its postion and above and also that there is solid ground below
                if(roomGrid[positionX,positionY] == 0 && roomGrid[positionX, positionY+1] == 0 && roomGrid[positionX, positionY - 1] != 0)
                {
                    //add item location to grid
                    roomGrid[positionX, positionY] = 3;
                    itemPlaced = true;
                }
                
            }
        }
        
        

        
    }
}
