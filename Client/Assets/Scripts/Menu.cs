using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject endScreen;

    // Triggered when pressing play from start menu or continue from end screen
    public void ToWaitingRoom()
    {
        startMenu.SetActive(false);
        endScreen.SetActive(false);

        foreach (KeyValuePair<int, PlayerManager> player in GameManager.players)
        {
            player.Value.gameObject.SetActive(true);
        }

        if (!Client.instance.isConnected) Client.instance.ConnectToServer();

        GameManager.instance.LoadWaitingRoom();
    }

    public void Quit()
    {
        Debug.Log("Quit.");
        Application.Quit();
    }
}
