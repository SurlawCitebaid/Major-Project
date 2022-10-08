using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    int roomX;
    int roomY;
    doorType type;
    //Unlock door transition
    bool unlock;
    bool unlockedOnce = false;
    //Lock door only for boss
    public static bool lockD = false;
    public static bool lockOnce = true;

    //For animated door
    public float doorOpenTime = 3f;
    GameObject childStoneSlab;
    Vector2 originalPosition;
    //Time since start of door animation
    float currentTime;
    float currentTime2;
    float lerpTime;

    private void Start()
    {
        //Lock doors at begining
        gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        childStoneSlab = transform.Find("stoneSlabDoor").gameObject;
        //Arange child stone slab to door type
        switch (type)
        {
            case doorType.Up:
                childStoneSlab.transform.localPosition = new Vector2(0.5f, 0.157f);
                break;
            case doorType.Down:
                childStoneSlab.transform.localPosition = new Vector2(0.5f, 1.136f);
                break;
            case doorType.Left:
                childStoneSlab.transform.localPosition = new Vector2(0.7f, 0.5f);
                childStoneSlab.transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case doorType.Right:
                childStoneSlab.transform.localPosition = new Vector2(0.23f, 0.5f);
                childStoneSlab.transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
        }
        originalPosition = childStoneSlab.transform.position;
    }

    private void Update()
    {
        if (unlock && !unlockedOnce)
        {
            currentTime += Time.deltaTime;
            lerpTime = currentTime / doorOpenTime;
            //Door no longer needs to be unlocked
            if (lerpTime > 1f)
            {
                Debug.Log("Door unlocked");
                unlock = false;
                unlockedOnce = true;
                currentTime = 0;
                lerpTime = 0;
            }
            else
            {
                //Depending on door type lerp it and it will open
                switch (type)
                {
                    case doorType.Up:
                        childStoneSlab.transform.position = new Vector2(Mathf.Lerp(originalPosition.x, originalPosition.x + 4, lerpTime), childStoneSlab.transform.position.y);
                        break;
                    case doorType.Down:
                        childStoneSlab.transform.position = new Vector2(Mathf.Lerp(originalPosition.x, originalPosition.x + 4, lerpTime), childStoneSlab.transform.position.y);
                        break;
                    case doorType.Left:
                        childStoneSlab.transform.position = new Vector2(childStoneSlab.transform.position.x, Mathf.Lerp(originalPosition.y, originalPosition.y + 4, lerpTime));
                        break;
                    case doorType.Right:
                        childStoneSlab.transform.position = new Vector2(childStoneSlab.transform.position.x, Mathf.Lerp(originalPosition.y, originalPosition.y + 4, lerpTime));
                        break;
                }
            }
        }

        //Lock door only for boss
        if (lockD && !lockOnce)
        {
            currentTime2 += Time.deltaTime;
            lerpTime = currentTime2 / doorOpenTime;

            //Door no longer needs to be unlocked
            if (lerpTime > 1f)
            {
                lockD = false;
                lockOnce = true;
                currentTime2 = 0;
                lerpTime = 0;
            }
            else
            {
                //Depending on door type lerp it and it will open
                switch (type)
                {
                    case doorType.Up:
                        childStoneSlab.transform.position = new Vector2(Mathf.Lerp(originalPosition.x + 4, originalPosition.x, lerpTime), childStoneSlab.transform.position.y);
                        break;
                    case doorType.Down:
                        childStoneSlab.transform.position = new Vector2(Mathf.Lerp(originalPosition.x + 4, originalPosition.x, lerpTime), childStoneSlab.transform.position.y);
                        break;
                    case doorType.Left:
                        childStoneSlab.transform.position = new Vector2(childStoneSlab.transform.position.x, Mathf.Lerp(originalPosition.y + 4, originalPosition.y, lerpTime));
                        break;
                    case doorType.Right:
                        childStoneSlab.transform.position = new Vector2(childStoneSlab.transform.position.x, Mathf.Lerp(originalPosition.y + 4, originalPosition.y, lerpTime));
                        break;
                }
            }
        }
    }

    //Unlocks doors of current room
    public static void unlockDoors()
    {
        Debug.Log("Doors Unlocking");
        Room room = GenerateLevel.rooms[(int)GenerateLevel.currentPlayerRoom.x, (int)GenerateLevel.currentPlayerRoom.y];
        foreach(Door door in room.getDoors())
        {
            door.unlockDoor();
        }
    }

    public static void lockDoors()
    {
        EnemySpawner.unlockDoorsOnce = true;
        Debug.Log("Doors Locking");
        Room room = GenerateLevel.rooms[(int)GenerateLevel.currentPlayerRoom.x, (int)GenerateLevel.currentPlayerRoom.y];
        foreach (Door door in room.getDoors())
        {
            door.lockDoor();
        }
    }

    public void unlockDoor()
    {
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        unlock = true;
        unlockedOnce = false;
    }

    public void lockDoor()
    {
        gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        lockD = true;
        lockOnce = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.position = getDoorToTravelTo();
            lockDoors();
            EnemySpawner.enemiesAlive = true;
            CameraFollow.updateRoomCamera = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.position = getDoorToTravelTo();
            lockDoors();
            EnemySpawner.enemiesAlive = true;
            CameraFollow.updateRoomCamera = true;
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
