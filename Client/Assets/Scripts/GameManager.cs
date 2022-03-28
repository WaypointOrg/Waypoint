using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
    public static Dictionary<int, Item> items = new Dictionary<int, Item>();
    public static Dictionary<int, Projectile> projectiles = new Dictionary<int, Projectile>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;
    public List<GameObject> itemsPrefab;
    public GameObject projectilePrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }


    public void SpawnPlayer(int _id, string _username, Vector2 _position, float _rotation)
    {
        GameObject _player;
        if (_id == Client.instance.myId)
        {
            _player = Instantiate(localPlayerPrefab, _position, localPlayerPrefab.transform.rotation);
        }
        else
        {
            _player = Instantiate(playerPrefab, _position, playerPrefab.transform.rotation);
        }

        PlayerManager _playerManager = _player.GetComponent<PlayerManager>();
        _playerManager.Initialize(_id, _username);

        players.Add(_id, _playerManager);
    }

    public void ItemSpawned(int _itemId, Vector2 _position, int _type)
    {
        GameObject _itemPrefab = itemsPrefab[_type];
        GameObject _item = Instantiate(_itemPrefab, _position, _itemPrefab.transform.rotation);

        _item.GetComponent<Item>().Initialize(_itemId, _type);
        items.Add(_itemId, _item.GetComponent<Item>());
    }

    public void ProjectileSpawned(int _projectileId, Vector2 _position, int _type)
    {
        GameObject _projectile = Instantiate(projectilePrefab, _position, projectilePrefab.transform.rotation);
        _projectile.GetComponent<Projectile>().Initialize(_projectileId, _type);
        projectiles.Add(_projectileId, _projectile.GetComponent<Projectile>());
    }
}
