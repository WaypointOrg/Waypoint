using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
    public static Dictionary<int, Item> items = new Dictionary<int, Item>();
    public static Dictionary<int, Projectile> projectiles = new Dictionary<int, Projectile>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;
    public List<GameObject> itemsPrefab;

    public GameObject friendlyProjectilePrefab;
    public GameObject ennemyProjectilePrefab;

    public Leaderboard leaderboard;
    public Text ammoText;

    public bool gameStarted = false;
    public float gameTime;
    public Text gameTimeText;

    public GameObject gameUI;
    public GameObject waitingRoomUI;
    public GameObject endUI;
    public InputField usernameField;


    public Animator transition;

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
            gameTimeText.text = Mathf.FloorToInt(gameTime).ToString();
        }
    }

    public void LoadEmpty()
    {
        gameUI.SetActive(false);
        SceneManager.LoadScene("Empty");
    }

    public void LoadWaitingRoom()
    {
        SceneManager.LoadScene("WaitingRoom");
        waitingRoomUI.SetActive(true);
        transition.SetBool("open", true);
    }

    public void LoadEndScreen()
    {
        transition.SetBool("open", false);
        StartCoroutine(LoadES());
    }

    IEnumerator LoadES()
    {
        yield return new WaitForSeconds(2);

        endUI.SetActive(true);
        SceneManager.LoadScene("End Screen");
        gameUI.SetActive(false);

        transition.SetBool("open", true);
    }

    public void LoadMap(int _mapId)
    {
        SceneManager.LoadScene("Map" + _mapId.ToString());
        waitingRoomUI.SetActive(false);
        gameUI.SetActive(true);
    }

    public void StartGame(float _duration, int _mapId)
    {
        transition.SetBool("open", false);
        StartCoroutine(StartG(_duration, _mapId));
    }

    IEnumerator StartG(float _duration, int _mapId)
    {
        yield return new WaitForSeconds(2);

        LoadMap(_mapId);

        leaderboard.AddPlayers(players);

        gameStarted = true;
        gameTime = _duration;
        gameTimeText.gameObject.SetActive(true);
        Camera.main.GetComponent<CameraSC>().SetMainPlayer(players[Client.instance.myId].gameObject.transform);

        yield return new WaitForSeconds(1);

        transition.SetBool("open", true);
    }

    public void EndGame()
    {
        transition.SetBool("open", false);
        StartCoroutine(EndG());
    }

    IEnumerator EndG()
    {
        LoadEndScreen();
        Camera.main.GetComponent<CameraSC>().ResetMP();

        leaderboard.Clear();

        foreach (KeyValuePair<int, PlayerManager> player in players)
        {
            player.Value.gameObject.SetActive(false);
        }

        foreach (KeyValuePair<int, Item> item in items)
        {
            Destroy(item.Value.gameObject);
        }
        items.Clear();

        foreach (KeyValuePair<int, Projectile> projectile in projectiles)
        {
            Destroy(projectile.Value.gameObject);
        }
        projectiles.Clear();

        gameStarted = false;
        gameTimeText.gameObject.SetActive(false);

        yield return new WaitForSeconds(2);

        transition.SetBool("open", true);
    }

    public void Disconnect()
    {
        Client.instance.Disconnect();

        LoadEmpty();
        waitingRoomUI.SetActive(false);
        
        Camera.main.GetComponent<CameraSC>().ResetMP();

        leaderboard.Clear();

        foreach (KeyValuePair<int, PlayerManager> player in players)
        {
            Destroy(player.Value.gameObject);
        }
        players.Clear();

        foreach (KeyValuePair<int, Item> item in items)
        {
            Destroy(item.Value.gameObject);
        }
        items.Clear();

        foreach (KeyValuePair<int, Projectile> projectile in projectiles)
        {
            Destroy(projectile.Value.gameObject);
        }
        projectiles.Clear();

        gameStarted = false;
        gameTimeText.gameObject.SetActive(false);
    }

    public void UpdateAmmo(int ammo_)
    {
        ammoText.text = ammo_.ToString();
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
        usernameField.text = _username;
    }

    public void ItemSpawned(int _itemId, Vector2 _position, int _type)
    {
        GameObject _itemPrefab = itemsPrefab[_type];
        GameObject _item = Instantiate(_itemPrefab, _position, _itemPrefab.transform.rotation);

        _item.GetComponent<Item>().Initialize(_itemId, _type);
        items.Add(_itemId, _item.GetComponent<Item>());
    }

    public void ProjectileSpawned(int _projectileId, Vector2 _position, int _ownerId)
    {
        GameObject _projectile;
        if (_ownerId == Client.instance.myId)
        {
            _projectile = Instantiate(friendlyProjectilePrefab, _position, friendlyProjectilePrefab.transform.rotation);
        } 
        else 
        {
            _projectile = Instantiate(ennemyProjectilePrefab, _position, ennemyProjectilePrefab.transform.rotation);
        }

        _projectile.GetComponent<Projectile>().Initialize(_projectileId);
        projectiles.Add(_projectileId, _projectile.GetComponent<Projectile>());
    }
}
