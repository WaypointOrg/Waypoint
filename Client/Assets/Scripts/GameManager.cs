using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
    public static Dictionary<int, Item> items = new Dictionary<int, Item>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;
    public List<GameObject> itemsPrefab;

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
            _player = Instantiate(localPlayerPrefab, _position, Quaternion.AngleAxis(_rotation, Vector3.forward));
        }
        else
        {
            _player = Instantiate(playerPrefab, _position, Quaternion.AngleAxis(_rotation, Vector3.forward));
        }

        _player.GetComponent<PlayerManager>().id = _id;
        _player.GetComponent<PlayerManager>().username = _username;
        players.Add(_id, _player.GetComponent<PlayerManager>());
    }

    public void ItemSpawned(int _itemId, Vector2 _position, int _type)
    {
        GameObject _itemPrefab = itemsPrefab[_type];
        GameObject _spawner = Instantiate(_itemPrefab, _position, _itemPrefab.transform.rotation);

        _spawner.GetComponent<Item>().Initialize(_itemId, _type);
        items.Add(_itemId, _spawner.GetComponent<Item>());
    }
}
