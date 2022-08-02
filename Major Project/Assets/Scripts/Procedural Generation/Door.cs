using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    int roomX;
    int roomY;
    doorType type;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.position = getDoorToTravelTo();
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
                break;
            case doorType.Down:
                doorPosition = getDoorByType(doorType.Up, GenerateLevel.rooms[roomX, roomY - 1].getDoors()).transform.position;
                doorPosition = new Vector2(doorPosition.x, doorPosition.y - 2);
                break;
            case doorType.Left:
                doorPosition = getDoorByType(doorType.Right, GenerateLevel.rooms[roomX - 1, roomY].getDoors()).transform.position;
                doorPosition = new Vector2(doorPosition.x - 2, doorPosition.y);
                break;
            case doorType.Right:
                doorPosition = getDoorByType(doorType.Left, GenerateLevel.rooms[roomX + 1, roomY].getDoors()).transform.position;
                doorPosition = new Vector2(doorPosition.x + 2, doorPosition.y);
                break;
            default:
                return new Vector2();
        }
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
