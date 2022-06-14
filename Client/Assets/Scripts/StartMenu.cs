using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public RectTransform waitingRoom;

    public void ToWaitingRoom()
    {
        gameObject.SetActive(false);
        Client.instance.ConnectToServer();

        GameManager.instance.MoveToMap(0);

        float waitingRoomWidth = CameraSC.width - 23;
        waitingRoom.sizeDelta = new Vector2(waitingRoomWidth, 20);
        waitingRoom.anchoredPosition = new Vector2(-23 - 23 - waitingRoomWidth/2, 0);
    }

    public void Quit()
    {
        Debug.Log("Quit.");
        Application.Quit();
    }
}
