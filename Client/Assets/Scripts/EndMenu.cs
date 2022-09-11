using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMenu : MonoBehaviour
{
    public void Continue()
    {
        ClientSend.EndGame();

        foreach (PlayerManager player in GameManager.players.Values)
        {
            player.gameObject.SetActive(true);
        }

        GameManager.instance.LoadWaitingRoom();
    }
}
