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

    public static void DisconnectPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();

        Debug.Log($"Player with id {_id} has disconnected.");

        Destroy(GameManager.players[_id].gameObject);
        GameManager.players.Remove(_id);
    }

    public static void SetName(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _name = _packet.ReadString();

        GameManager.players[_id].SetName(_name);
    }

    public static void StartGame(Packet _packet)
    {
        float _duration = _packet.ReadFloat();
        int _mapId = _packet.ReadInt();
        GameManager.instance.StartGame(_duration, _mapId);
    }

    public static void EndGame(Packet _packet)
    {
        Debug.Log("Ending game...");
        GameManager.instance.EndGame();
    }

    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector2 _position = _packet.ReadVector2();

        if (GameManager.players.ContainsKey(_id))
        {
            GameManager.players[_id].transform.position = _position;
        }
    }

    // Only received from other players.
    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        float _rotation = _packet.ReadFloat();

        if (GameManager.players.ContainsKey(_id))
        {
            GameManager.players[_id].gun.rotation = Quaternion.Euler(0f, 0f, _rotation);
        }
    }

    public static void PlayerRespawned(Packet _packet)
    {
        int _id = _packet.ReadInt();

        GameManager.players[_id].Respawn();
    }

    public static void PlayerHit(Packet _packet)
    {
        int _id = _packet.ReadInt();
        int _by = _packet.ReadInt();

        GameManager.players[_by].kills += 1;
        GameManager.players[_id].Hit();
        GameManager.instance.leaderboard.IncreaseKillCount(_by);
    }

    public static void PlayerAmmo(Packet _packet)
    {
        int _ammo = _packet.ReadInt();
        GameManager.instance.UpdateAmmo(_ammo);
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
        if(GameManager.items.ContainsKey(_itemId))
        {
            Destroy(GameManager.items[_itemId].gameObject);
            GameManager.items.Remove(_itemId);
        }
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

        if (GameManager.projectiles.ContainsKey(_projectileId))
        {
            GameManager.projectiles[_projectileId].transform.position = _position;
        }
    }

    public static void ProjectileDestroy(Packet _packet)
    {
        int _projectileId = _packet.ReadInt();
        GameManager.projectiles[_projectileId].Destroy();
        GameManager.projectiles.Remove(_projectileId);
    }
}
