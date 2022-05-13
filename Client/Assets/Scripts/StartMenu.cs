using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public void Play()
    {
        gameObject.SetActive(false);
        Debug.Log("Start.");
        Client.instance.ConnectToServer();
    }

    public void Quit()
    {
        Debug.Log("Quit.");
        Application.Quit();
    }
}
