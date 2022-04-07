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

    public static void StartGame(Packet _packet)
    {
        Debug.Log("Starting game...");
        GameManager.instance.StartGame();
    }

    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector2 _position = _packet.ReadVector2();

        GameManager.players[_id].transform.position = _position;
    }

    // Only received from other players.
    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        float _rotation = _packet.ReadFloat();

        Debug.Log(_rotation);
        GameManager.players[_id].gun.rotation = Quaternion.Euler(0f, 0f, _rotation);
    }

    public static void ItemSpawned(Packet _packet)
    {
        int itemId = _packet.ReadInt();
        Vector2 _position = _packet.ReadVector2();
        int _type = _packet.ReadInt();

        GameManager.instance.ItemSpawned(itemId, _position, _type);
    }

    public static void ItemPickedUp(Packet _packet)
    {
        int _itemId = _packet.ReadInt();
        int _byPlayer = _packet.ReadInt();

        // TODO: Add item to player.
        GameManager.items[_itemId].ItemPickedUp();
        GameManager.items.Remove(_itemId);
    }

    public static void ProjectileSpawned(Packet _packet)
    {
        int _projectileId = _packet.ReadInt();
        Vector2 _position = _packet.ReadVector2();
        int _type = _packet.ReadInt();

        GameManager.instance.ProjectileSpawned(_projectileId, _position, _type);
    }

    public static void ProjectilePosition(Packet _packet)
    {
        int _projectileId = _packet.ReadInt();
        Vector2 _position = _packet.ReadVector2();

        GameManager.projectiles[_projectileId].transform.position = _position;
    }

    public static void ProjectileDestroy(Packet _packet)
    {
        int _projectileId = _packet.ReadInt();
        GameManager.projectiles[_projectileId].Destroy();
        GameManager.projectiles.Remove(_projectileId);
    }
}
