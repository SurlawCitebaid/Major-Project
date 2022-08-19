using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room
{
    RoomData roomData;
    Vector2 spawnPosition;
    public int roomSizeX;
    public int roomSizeY;
    int[,] roomGrid;

    //The tile map for tiles
    Tilemap tileMap;
    //Is a custom scripatable tile for the tile map
    public List<TileBase> tiles;

    int maxNumberOfPlatforms;
    //The widest and narrowist a platform can be
    int maxPlatformSize;
    int minPlatformSize;

    Transform parent;

    //Chance of an item spawning in a room
    float itemSpawnChance = 50f;

    //Reference where the room is compare to all the other rooms
    int[,] levelLayoutGrid;
    int levelGridX;
    int levelGridY;

    //Get the script for level gen and finding other rooms
    GenerateLevel generateLevelScript;

    int maxRoomSize;

    List<Door> doors;

    float detailChance;

    public Room(Vector2 spawnPosition, int roomSizeX, int roomSizeY, List<TileBase> tiles, Transform parent, 
        int maxNumberOfPlatforms, int maxPlatformSize, int minPlatformSize, int[,] levelLayoutGrid, int gridX, 
        int gridY, GenerateLevel generateLevelScript, Tilemap tileMap, float detailChance)
    {
        this.spawnPosition = spawnPosition;
        this.roomSizeX = roomSizeX;
        this.roomSizeY = roomSizeY;

        this.tiles = tiles;
        this.tileMap = tileMap;

        this.parent = parent;
        this.maxNumberOfPlatforms = maxNumberOfPlatforms;
        this.maxPlatformSize = maxPlatformSize;
        this.minPlatformSize = minPlatformSize;

        this.levelLayoutGrid = levelLayoutGrid;
        this.levelGridX = gridX;
        this.levelGridY = gridY;

        this.generateLevelScript = generateLevelScript;
        maxRoomSize = generateLevelScript.getMaxRoomSize();
        doors = new List<Door>();

        this.detailChance = detailChance;
    }

    public void createRoom()
    {
        roomData = new RoomData(roomSizeX, roomSizeY, RoomType.CIRCUS);
        roomGrid = new int[generateLevelScript.getMaxRoomSize(), generateLevelScript.getMaxRoomSize()];
        for (int x = 0; x < maxRoomSize; x++)
        {
            for (int y = 0; y < maxRoomSize; y++) {
                roomGrid[x, y] = 7;//This danger block out of level
            }
        }
    }


    public void fillRoomData()
    {
        //Back ground
        placeBackGround();
        //Walls 
        placeWalls();
        //Platforms
        placePlatforms();
        //Add item pedestals
        placeItem();
        //Add Enemy Spawners
        placeSpawners();
    }

    //Has to be done after as it requires all the rooms to function
    public void fillCorridorData()
    {
        //Add Doors
        placeCorridors();
        //Place Details
        placeDetails();
    }

    public void generateRoom()
    {
        for (int x = 0; x < maxRoomSize; x++)
        {
            for (int y = 0; y < maxRoomSize; y++)
            {
                //Air/BackGround
                if (roomGrid[x, y] == 0)
                {
                    Vector3Int cell = tileMap.WorldToCell(new Vector3(spawnPosition.x + (float)x, spawnPosition.y + (float)y));
                    tileMap.SetTile(cell, tiles[0]);
                }
                //Wall
                else if (roomGrid[x, y] == 1)
                {
                    //Convert it to tilemap position
                    Vector3Int cell = tileMap.WorldToCell(new Vector3(spawnPosition.x + (float)x, spawnPosition.y + (float)y));
                    tileMap.SetTile(cell, tiles[1]);
                    //GameObject.Instantiate(tiles[1], new Vector3(spawnPosition.x + (float)x, spawnPosition.y + (float)y), Quaternion.identity, parent);
                }
                //Platform
                else if (roomGrid[x, y] == 2)
                {
                    Vector3Int cell = tileMap.WorldToCell(new Vector3(spawnPosition.x + (float)x, spawnPosition.y + (float)y));
                    tileMap.SetTile(cell, tiles[2]);
                }
                //Item
                else if (roomGrid[x, y] == 3)
                {
                    Vector3Int cell = tileMap.WorldToCell(new Vector3(spawnPosition.x + (float)x, spawnPosition.y + (float)y));
                    tileMap.SetTile(cell, tiles[3]);
                }
                ////Door
                //else if (roomGrid[x, y] == 4)
                //{
                //    Vector3Int cell = tileMap.WorldToCell(new Vector3(spawnPosition.x + (float)x, spawnPosition.y + (float)y));
                //    tileMap.SetTile(cell, tiles[4]);
                //}
                //EnemySpawners
                else if (roomGrid[x, y] == 5)
                {
                    Vector3Int cell = tileMap.WorldToCell(new Vector3(spawnPosition.x + (float)x, spawnPosition.y + (float)y));
                    tileMap.SetTile(cell, tiles[5]);
                }
                //Details
                else if(roomGrid[x, y] == 6)
                {
                    Vector3Int cell = tileMap.WorldToCell(new Vector3(spawnPosition.x + (float)x, spawnPosition.y + (float)y));
                    tileMap.SetTile(cell, tiles[6]);
                }
            }
        }
    }

    void placeBackGround()
    {
        for (int x = 0; x < roomSizeX; x++)
        {
            for (int y = 0; y < roomSizeY; y++) {
                roomGrid[x, y] = 0;
            }
        }
    }

    void placeWalls()
    {

        for (int x = 0; x < roomSizeX; x++) {
            for (int y = 0; y < roomSizeY; y++)
            {
                //Adds a border around the room
                if (x == 0 || x == roomSizeX - 1 || y == 0 || y == roomSizeY - 1 ||
                    x == 1 || x == roomSizeX - 2 || y == 1 || y == roomSizeY - 2)
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
            for (int i = 0; i < randomPlatformSize; i++)
            {
                //Makes sure we arent out of bound of the roomGrid
                if (xPos + i < roomSizeX - 1)
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
        if (Random.Range(0, 100) < itemSpawnChance)
        {
            //loop until a suitible postion is found
            while (!itemPlaced)
            {
                //Random postion with in the box
                int positionX = Random.Range(2, roomSizeX - 2);
                int positionY = Random.Range(2, roomSizeY - 2);
                //checks if there is an airblock at its postion and above and also that there is solid ground below
                if (roomGrid[positionX, positionY] == 0 && roomGrid[positionX, positionY + 1] == 0 && roomGrid[positionX, positionY - 1] != 0)
                {
                    //add item location to grid
                    roomGrid[positionX, positionY] = 3;
                    itemPlaced = true;
                }

            }
        }




    }


    void placeCorridors()
    {
        //DO it based of the fact that the bottonm left hand corner is the actually postion of the room in the grid
        //So you can determine the room with smallest x or y roomsize to determine which room centre can be chosen to have to corridor follow to the other rooom
        //Up 
        if (levelLayoutGrid[levelGridX, levelGridY + 1] == 1 || levelLayoutGrid[levelGridX, levelGridY + 1] == 2)
        {
            int neighbourRoomXSize = generateLevelScript.getRoomXSize(levelGridX, levelGridY + 1);
            //This room is Wider
            if (roomSizeX > neighbourRoomXSize)
            {
                //Centre of neighbouir room X
                int neighbourRoomCentreX = neighbourRoomXSize / 2;
                for (int yCentre = roomSizeY - 2; yCentre < roomSizeY; yCentre++)
                {
                    roomGrid[neighbourRoomCentreX - 1, yCentre] = 1;
                    roomGrid[neighbourRoomCentreX, yCentre] = 0;
                    roomGrid[neighbourRoomCentreX + 1, yCentre] = 0;
                    roomGrid[neighbourRoomCentreX + 2, yCentre] = 1;

                    
                }

                //Door
                createDoor(neighbourRoomCentreX, roomSizeY - 2, doorType.Up);
            }
            //Neighbor room is Wider
            else
            {
                for (int yCentre = roomSizeY - 2; yCentre < roomSizeY; yCentre++)
                {
                    roomGrid[roomSizeX / 2 - 1, yCentre] = 1;
                    roomGrid[roomSizeX / 2, yCentre] = 0;
                    roomGrid[roomSizeX / 2 + 1, yCentre] = 0;
                    roomGrid[roomSizeX / 2 + 2, yCentre] = 1;
                }
                //Door
                createDoor(roomSizeX / 2, roomSizeY - 2, doorType.Up);
            }
        }
        //Down 
        if (levelLayoutGrid[levelGridX, levelGridY - 1] == 1 || levelLayoutGrid[levelGridX, levelGridY - 1] == 2)
        {
            int neighbourRoomXSize = generateLevelScript.getRoomXSize(levelGridX, levelGridY - 1);
            //This room is Wider
            if (roomSizeX > neighbourRoomXSize)
            {
                //Centre of neighbouir room X
                int neighbourRoomCentreX = neighbourRoomXSize / 2;
                roomGrid[neighbourRoomCentreX - 1, 0] = 1;
                roomGrid[neighbourRoomCentreX, 0] = 0;
                roomGrid[neighbourRoomCentreX + 1, 0] = 0;
                roomGrid[neighbourRoomCentreX + 2, 0] = 1;

                //4 is doors
                roomGrid[neighbourRoomCentreX - 1, 1] = 1;
                //Door
                createDoor(neighbourRoomCentreX, 0, doorType.Down);
                roomGrid[neighbourRoomCentreX, 1] = 0;
                roomGrid[neighbourRoomCentreX + 1, 1] = 0;
                roomGrid[neighbourRoomCentreX + 2, 1] = 1;
            }
            //Neighbor room is Wider
            else
            {
                //Do this twice as the wall is 2 thick
                roomGrid[roomSizeX / 2 - 1, 0] = 1;
                //Door
                createDoor(roomSizeX / 2, 0, doorType.Down);
                roomGrid[roomSizeX / 2 + 1, 0] = 0;
                roomGrid[roomSizeX / 2 + 2, 0] = 1;

                roomGrid[roomSizeX / 2 - 1, 1] = 1;
                roomGrid[roomSizeX / 2, 1] = 0;
                roomGrid[roomSizeX / 2 + 1, 1] = 0;
                roomGrid[roomSizeX / 2 + 2, 1] = 1;
            }
        }
        //Left 
        if (levelLayoutGrid[levelGridX - 1, levelGridY] == 1 || levelLayoutGrid[levelGridX - 1, levelGridY] == 2)
        {
            int neighbourRoomYSize = generateLevelScript.getRoomYSize(levelGridX - 1, levelGridY);
            //This room is Wider
            if (roomSizeY > neighbourRoomYSize)
            {
                //Centre of neighbouir room X
                int neighbourRoomCentreY = neighbourRoomYSize / 2;
                roomGrid[0, neighbourRoomCentreY - 1] = 1;
                //Door
                createDoor(0, neighbourRoomCentreY, doorType.Left);
                roomGrid[0, neighbourRoomCentreY + 1] = 0;
                roomGrid[0, neighbourRoomCentreY + 2] = 1;

                roomGrid[1, neighbourRoomCentreY - 1] = 1;
                roomGrid[1, neighbourRoomCentreY] = 0;
                roomGrid[1, neighbourRoomCentreY + 1] = 0;
                roomGrid[1, neighbourRoomCentreY + 2] = 1;
            }
            //Neighbor room is Wider
            else
            {
                roomGrid[0, roomSizeY / 2 - 1] = 1;
                //Door
                createDoor(0, roomSizeY / 2, doorType.Left);
                roomGrid[0, roomSizeY / 2] = 4;
                roomGrid[0, roomSizeY / 2 + 1] = 0;
                roomGrid[0, roomSizeY / 2 + 2] = 1;

                roomGrid[1, roomSizeY / 2 - 1] = 1;           
                roomGrid[1, roomSizeY / 2] = 0;
                roomGrid[1, roomSizeY / 2 + 1] = 0;
                roomGrid[1, roomSizeY / 2 + 2] = 1;
            }
        }
        //Right 
        if (levelLayoutGrid[levelGridX + 1, levelGridY] == 1 || levelLayoutGrid[levelGridX + 1, levelGridY] == 2)
        {
            int neighbourRoomYSize = generateLevelScript.getRoomYSize(levelGridX+1, levelGridY);
            //This room is Wider
            if (roomSizeY > neighbourRoomYSize)
            {
                //Centre of neighbouir room X
                int neighbourRoomCentreY = neighbourRoomYSize / 2;
                for (int xCentre = roomSizeX - 2; xCentre < roomSizeX; xCentre++)
                {
                    roomGrid[xCentre, neighbourRoomCentreY - 1] = 1;
                    roomGrid[xCentre, neighbourRoomCentreY] = 0;
                    roomGrid[xCentre, neighbourRoomCentreY + 1] = 0;
                    roomGrid[xCentre, neighbourRoomCentreY + 2] = 1;

                }
                //Door
                createDoor(roomSizeX - 2, neighbourRoomCentreY, doorType.Right);
            }
            //Neighbor room is Wider
            else
            {
                int neighbourRoomCentreY = neighbourRoomYSize / 2;
                for (int xCentre = roomSizeX - 2; xCentre < roomSizeX; xCentre++)
                {
                    roomGrid[xCentre, roomSizeY/2 - 1] = 1;
                    roomGrid[xCentre, roomSizeY / 2] = 0;
                    roomGrid[xCentre, roomSizeY / 2 + 1] = 0;
                    roomGrid[xCentre, roomSizeY / 2 + 2] = 1;

                }
                //Door
                createDoor(roomSizeX - 2, roomSizeY / 2, doorType.Right);
            }
        }
    }

    //Helper function for creating doors
    void createDoor(int xPos, int yPos, doorType direction)
    {
        roomGrid[xPos, yPos] = 4;
        Vector3Int cell = tileMap.WorldToCell(new Vector3(spawnPosition.x + (float)xPos, spawnPosition.y + (float)yPos));
        tileMap.SetTile(cell, tiles[4]);
        //Get the game object attached to the tile that is created
        Door door = tileMap.GetInstantiatedObject(cell).GetComponent<Door>();
        //Gets the grid value
        door.setDoor(direction, (int)spawnPosition.x/maxRoomSize, (int)spawnPosition.y/ maxRoomSize);
        doors.Add(door);
    }

    void placeSpawners()
    {
        //Add a inspector field for max spawwners
        int numberOfSpawners = 0;
        int maxNumberOfSpawners = 2;

        while(numberOfSpawners < maxNumberOfSpawners)
        {
            //Random postion with in the box
            int positionX = Random.Range(2, roomSizeX - 2);
            int positionY = Random.Range(2, roomSizeY - 2);
            //checks if there is an airblock at its postion and above and also that there is solid ground below
            if (roomGrid[positionX, positionY] == 0)
            {
                //add item location to grid
                roomGrid[positionX, positionY] = 5;
                numberOfSpawners++;
            }
        }
    }

    //Rocks, Bushes, Flowers ID 6
    void placeDetails()
    {
        int aboveGroundHeight = 2;
        //Ground Details
        for(int x = 0; x < roomSizeX-1; x++)
        {
            //If the chance is met and there is an empty air block above the ground
            if (Random.Range(0f,1f) < detailChance && roomGrid[x,aboveGroundHeight] == 0 && roomGrid[x, aboveGroundHeight-1] == 1)
            {
                roomGrid[x, aboveGroundHeight] = 6;
            }
        }
        //Ceiling Details
    }

    public int getRoomSizeX()
    {
        return roomSizeX;
    }
    public int getRoomSizeY()
    {
        return roomSizeY;
    }

    public List<Door> getDoors()
    {
        return doors;
    }

    public int getMaxRoomSize()
    {
        return maxRoomSize;
    }

    public GenerateLevel getGenerateLevel()
    {
        return generateLevelScript;
    }

    //Checks if an enemy can spawn here or be here
    public static bool enemyLocationValid(Vector2 enemyPosition)
    {
        //Get current room and compare positions
        Room room = GenerateLevel.rooms[(int)GenerateLevel.currentPlayerRoom.x, (int)GenerateLevel.currentPlayerRoom.y];
        //0,0 of this room
        Vector2 roomPosition = room.spawnPosition;
        
        roomPosition = new Vector2(roomPosition.x + (-(room.generateLevelScript.getGridX() * room.maxRoomSize) / 2), roomPosition.y + (-(room.generateLevelScript.getGridX() * room.maxRoomSize) / 2));
        if (enemyPosition.x >= roomPosition.x + 2 && enemyPosition.x <= (room.roomSizeX + roomPosition.x - 2))
        {
            if (enemyPosition.y >= roomPosition.y + 2 && enemyPosition.y <= (room.roomSizeY + roomPosition.y - 2))
            {
                return true;
            }
        }
        return false;
    }
}
