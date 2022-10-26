using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    Transform player;
    public static bool updateRoomCamera = true;
    float xCentre, yCentre;
    Vector3 roomCentre;
    static Camera MainCamera;
    private Vector2 currentRes;
    float height, width, maxHeight, maxWidth;
    public static Vector2 centre;
    private void Start() {
        currentRes = new Vector2(Screen.width, Screen.height);
        MainCamera = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update() {
        if (currentRes.x != Screen.width || currentRes.y != Screen.height)
        {
            updateRoomCamera = true;
            currentRes = new Vector2(Screen.width, Screen.height);
        }
            
        if(updateRoomCamera)
        {
            height = MainCamera.orthographicSize + 1.5f;
            width = (((height * 2) * MainCamera.aspect) / 2) - 1.5f;
            xCentre = (Room.getTopRightRoomPosition().x - Room.getBottomLeftRoomPosition().x) / 2;
            yCentre = (Room.getTopRightRoomPosition().y - Room.getBottomLeftRoomPosition().y) / 2;
            roomCentre = new Vector3(Room.getTopRightRoomPosition().x - xCentre, Room.getTopRightRoomPosition().y - yCentre, -10);
            maxHeight = (Mathf.Abs(yCentre) - height);
            maxWidth = (Mathf.Abs(xCentre) - width);
            xCentre = roomCentre.x;
            yCentre = roomCentre.y;
            transform.position = roomCentre;

            updateRoomCamera = false;
        }
        else
        {
            centre = new(xCentre, yCentre);
            float xChecker = (new Vector2(player.position.x, 0) - new Vector2(xCentre, 0)).x;
            float yChecker = (new Vector2(0, player.position.y) - new Vector2(0, yCentre)).y;
            float playerPosFromCentX = (new Vector2(xCentre, 0) - new Vector2(player.position.x, 0)).magnitude;
            float playerPosFromCentY = (new Vector2(0, yCentre) - new Vector2(0, player.position.y)).magnitude;
            float scalingPercentageX = (playerPosFromCentX / maxWidth);
            if (scalingPercentageX > 1)
            {
                scalingPercentageX = 1;
            }
            float scalingPercentageY = (playerPosFromCentY / maxHeight);
            if (scalingPercentageY > 1)
            {
                scalingPercentageY = 1;
            }
            float distanceFromCentX = maxWidth * scalingPercentageX;
            float distanceFromCentY = maxHeight * scalingPercentageY;
            if (xChecker >= 0)
            {
                transform.position = new Vector3(roomCentre.x + distanceFromCentX, transform.position.y, -10);
            }
            else if (xChecker < 0)
            {
                transform.position = new Vector3(roomCentre.x - distanceFromCentX, transform.position.y, -10);
            }
            if (yChecker >= 0)
            {
                transform.position = new Vector3(transform.position.x, roomCentre.y + distanceFromCentY, -10);
            }
            else if (yChecker < 0)
            {
                transform.position = new Vector3(transform.position.x, roomCentre.y - distanceFromCentY, -10);
            }

        }
    }
}
