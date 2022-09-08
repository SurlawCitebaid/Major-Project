using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    int roomX;
    int roomY;
    doorType type;

    private void Start()
    {
        //Lock doors at begining
        gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
    }

    //Unlocks doors of current room
    public static void unlockDoors()
    {
        Room room = GenerateLevel.rooms[(int)GenerateLevel.currentPlayerRoom.x, (int)GenerateLevel.currentPlayerRoom.y];
        foreach(Door door in room.getDoors())
        {
            door.unlockDoor();
        }
    }

    public static void lockDoors()
    {
        Room room = GenerateLevel.rooms[(int)GenerateLevel.currentPlayerRoom.x, (int)GenerateLevel.currentPlayerRoom.y];
        foreach (Door door in room.getDoors())
        {
            door.lockDoor();
        }
    }

    public void unlockDoor()
    {
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public void lockDoor()
    {
        gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.position = getDoorToTravelTo();
            lockDoors();
            EnemySpawner.enemiesAlive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.position = getDoorToTravelTo();
            lockDoors();
            EnemySpawner.enemiesAlive = true;
        }
    }


    private Vector2 getDoorToTravelTo()
    {
        Vector2 doorPosition;
        switch (type)
        {
            case doorType.Up:
                doorPosition = getDoorByType(doorType.Down, GenerateLevel.rooms[roomX, roomY + 1].getDoors()).transform.position;
                doorPosition = new Vector2(doorPosition.x+2, doorPosition.y+2);
                GenerateLevel.currentPlayerRoom = new Vector2(roomX, roomY + 1);
                break;
            case doorType.Down:
                doorPosition = getDoorByType(doorType.Up, GenerateLevel.rooms[roomX, roomY - 1].getDoors()).transform.position;
                doorPosition = new Vector2(doorPosition.x, doorPosition.y - 2);
                GenerateLevel.currentPlayerRoom = new Vector2(roomX, roomY - 1);
                break;
            case doorType.Left:
                doorPosition = getDoorByType(doorType.Right, GenerateLevel.rooms[roomX - 1, roomY].getDoors()).transform.position;
                doorPosition = new Vector2(doorPosition.x - 2, doorPosition.y);
                GenerateLevel.currentPlayerRoom = new Vector2(roomX - 1, roomY);
                break;
            case doorType.Right:
                doorPosition = getDoorByType(doorType.Left, GenerateLevel.rooms[roomX + 1, roomY].getDoors()).transform.position;
                doorPosition = new Vector2(doorPosition.x + 2, doorPosition.y);
                GenerateLevel.currentPlayerRoom = new Vector2(roomX + 1, roomY);
                break;
            default:
                return new Vector2();
        }
        Debug.Log(GenerateLevel.currentPlayerRoom);
        return doorPosition;
    }
    
    //The door to go to
    GameObject getDoorByType(doorType type, List<Door> doors)
    {
        foreach (Door door in doors)
        {
            if (type == door.getDoorType())
            {
                return door.getDoorGameObject();
            }
        }
        //No Door was found
        return null;
    }
    
    public doorType getDoorType()
    {
        return type;
    }

    public GameObject getDoorGameObject()
    {
        return gameObject;
    }

    //sets information for door traversal
    public void setDoor(doorType type, int x, int y)
    {
        setDoorType(type);
        setRoomX(x);
        setRoomY(y);
    }


    public void setDoorType(doorType type)
    {
        this.type = type;
    }

    public void setRoomX(int x)
    {
        roomX = x;
    }

    public void setRoomY(int y)
    {
        roomY = y;
    }
}

public enum doorType{
    Up,
    Down,
    Left,
    Right
}
