using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Leaderboard leaderboard;

    public bool gameStarted = false;
    public float gameTime;
    public Text gameTimeText;

    public List<Transform> cameraTargets;

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

    void Update()
    {
        if (gameStarted)
        {
            gameTime -= Time.deltaTime;
            gameTimeText.text = Mathf.CeilToInt(gameTime).ToString();
        }
    }

    // Map 0 is waiting room, then it goes counterclockwise from the bottom left
    public void MoveToMap(int _mapId)
    {
        if (_mapId == 0)
        {
            Camera.main.transform.position = new Vector3(-23 - CameraSC.width/2, 0f, -10f);
        } 
        else 
        {
            Camera.main.transform.position = cameraTargets[_mapId - 1].position + new Vector3(0, 0, -10);
        }
    }
    
    public void StartGame(float _duration, int _mapId)
    {
        MoveToMap(_mapId);

        leaderboard.AddPlayers(players);

        gameStarted = true;
        gameTime = _duration;
        gameTimeText.gameObject.SetActive(true);
    }

    public void EndGame()
    {
        MoveToMap(0);

        leaderboard.Clear();

        foreach (KeyValuePair<int, Item> item in items)
        {
            Destroy(item.Value.gameObject);
        }
        items.Clear();

        gameStarted = false;
        gameTimeText.gameObject.SetActive(false);
    }

    public void NameChanged(InputField input)
    {
        ClientSend.NameChanged(input.text);
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
