using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();

        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector2();
        float _rotation = _packet.ReadFloat();

        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);
    }

    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector2 _position = _packet.ReadVector2();

        GameManager.players[_id].transform.position = _position;
    }

    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        float _rotation = _packet.ReadFloat();

        GameManager.players[_id].transform.eulerAngles = new Vector3(0, 0, _rotation);
    }

    public static void ItemSpawned(Packet _packet)
    {
        int _itemID = _packet.ReadInt();
        Vector2 _position = _packet.ReadVector2();
        int _type = _packet.ReadInt();

        GameManager.instance.ItemSpawned(_itemID, _position, _type);
    }

    // public static void ItemPickedUp(Packet _packet)
    // {
    //     int _spawnerId = _packet.ReadInt();
    //     int _byPlayer = _packet.ReadInt();

    //     GameManager.items[_spawnerId].ItemPickedUp();
    //     GameManager.players[_byPlayer].itemCount++;
    // }
}
