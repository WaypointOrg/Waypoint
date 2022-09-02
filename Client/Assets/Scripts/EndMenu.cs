using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMenu : MonoBehaviour
{
    public void Continue()
    {
        ClientSend.EndGame();

        foreach (KeyValuePair<int, PlayerManager> player in GameManager.players)
        {
            player.Value.gameObject.SetActive(true);
        }

        GameManager.instance.LoadWaitingRoom();
    }
}
