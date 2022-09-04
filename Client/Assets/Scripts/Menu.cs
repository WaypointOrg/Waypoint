using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject optionMenu;
    public GameObject waitingRoomMenu;
    public GameObject pauseMenu;

    public GameObject invalidIpText;
    public GameObject invalidPortText;

    public InputField ipField;
    public InputField portField;

    bool validIp;
    bool validPort;

    public Animator transition;

    void Start()
    {
        ipField.text = PlayerPrefs.GetString("Ip", "127.0.0.1");
        portField.text = PlayerPrefs.GetInt("Port", 26950).ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown("escape") && !startMenu.activeSelf)
        {
            pauseMenu.SetActive(true);
        }
    }

    // Triggered when pressing play from start menu or continue from end screen
    public void ToWaitingRoom()
    {
        transition.SetBool("open", false);

        if (!validIp && !validPort)
        {
            optionMenu.SetActive(true);
            return;
        }

        StartCoroutine(ToWR());
    }

    IEnumerator ToWR()
    {
        yield return new WaitForSeconds(2);

        transition.SetBool("open", true);

        startMenu.SetActive(false);
        Client.instance.ConnectToServer();

        GameManager.instance.LoadWaitingRoom();
        waitingRoomMenu.SetActive(true);
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

    public void QuitGame()
    {
        startMenu.SetActive(true);
        GameManager.instance.Disconnect();
    }

    public void Continue()
    {
        ClientSend.EndGame();

        foreach (KeyValuePair<int, PlayerManager> player in GameManager.players)
        {
            player.Value.gameObject.SetActive(true);
            player.Value.kills = 0;
        }

        GameManager.instance.LoadWaitingRoom();
    }
}
