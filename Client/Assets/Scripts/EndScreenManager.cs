using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EndScreenManager : MonoBehaviour
{
    private List<EndScreenBar> playerBars = new List<EndScreenBar>();
    public GameObject playerBarPrefab;
    public float spaceBetweenBars;
    public float minHeight;
    public float maxHeight;

    void Start()
    {
        // PlayerManager bestPlayer = GameManager.players.OrderByDescending(x => x.Value.kills).First().Value;
        int maxScore = GameManager.players.Max(p => p.Value.kills);
        float startX = -((GameManager.players.Count - 1)*spaceBetweenBars) / 2;
        int i = 0;
        foreach (KeyValuePair<int, PlayerManager> player in GameManager.players)
        {
            float x = startX + i*spaceBetweenBars;
            GameObject _object = Instantiate(playerBarPrefab, new Vector3(x, 0, 0), Quaternion.identity);
            EndScreenBar _playerBar = _object.GetComponent<EndScreenBar>();
            _playerBar.usernameText.text = player.Value.username;
            _playerBar.scoreText.text = player.Value.kills.ToString();
            _playerBar.isBest = player.Value.kills == maxScore;
            _playerBar.targetHeight = map(player.Value.kills, 0, maxScore, minHeight, maxHeight);
            i++;
        }
    }

    static public float map(float value, float start1, float stop1, float start2, float stop2)
    {
        if (start1 == stop1)
        {
            return (start2 + stop2)/2;
        } else {
            return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
        }
    }
}
