using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject optionMenu;
    public GameObject waitingRoomMenu;

    public GameObject invalidIpText;
    public GameObject invalidPortText;

    public InputField ipField;
    public InputField portField;

    bool validIp;
    bool validPort;

    void Start()
    {
        ipField.text = PlayerPrefs.GetString("Ip", "127.0.0.1");
        portField.text = PlayerPrefs.GetInt("Port", 26950).ToString();
    }

    // Triggered when pressing play from start menu or continue from end screen
    public void ToWaitingRoom()
    {
        startMenu.SetActive(false);

        if (!validIp && !validPort)
        {
            optionMenu.SetActive(true);
            return;
        }

        Client.instance.ConnectToServer();

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
            validIp = true;
        } else {
            invalidIpText.gameObject.SetActive(true);
            validIp = false;
        }
    }

    public void TryChangePort(string _port)
    {
        if (Client.instance.TryChangePort(_port))
        {
            invalidPortText.gameObject.SetActive(false);
            validPort = true;
        } else {
            invalidPortText.gameObject.SetActive(true);
            validPort = false;
        }
    }

    public void ChangeName(string _name)
    {
        ClientSend.NameChanged(_name);
    }
}
