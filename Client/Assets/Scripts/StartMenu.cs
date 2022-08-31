using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject invalidIpText;
    public GameObject invalidPortText;


    // Triggered when pressing play from start menu or continue from end screen
    public void ToWaitingRoom()
    {
        startMenu.SetActive(false);

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

    public void TryChangeIp(string _ip)
    {
        if (Client.instance.TryChangeIp(_ip))
        {
            invalidIpText.gameObject.SetActive(false);
        } else {
            invalidIpText.gameObject.SetActive(true);
        }
    }

    public void TryChangePort(string _port)
    {
        if (Client.instance.TryChangePort(_port))
        {
            invalidPortText.gameObject.SetActive(false);
        } else {
            invalidPortText.gameObject.SetActive(true);
        }
    }
}
