using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public RectTransform waitingRoom;

    // Triggered when pressing play
    public void ToWaitingRoom()
    {
        gameObject.SetActive(false);
        Client.instance.ConnectToServer();

        GameManager.instance.LoadWaitingRoom();

        // float waitingRoomWidth = CameraSC.width - 23;
        // waitingRoom.sizeDelta = new Vector2(waitingRoomWidth, 20);
        // waitingRoom.anchoredPosition = new Vector2(-23 - 23 - waitingRoomWidth/2, 0);
    }

    public void Quit()
    {
        Debug.Log("Quit.");
        Application.Quit();
    }
}
