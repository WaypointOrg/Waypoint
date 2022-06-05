using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    public Dictionary<int, LeaderboardEntry> entries = new Dictionary<int, LeaderboardEntry>();
    public GameObject entryPrefab;

    public Color32 goldColor;
    public Color32 silverColor;
    public Color32 bronzeColor;
    public Color32 defaultColor;

    public void AddPlayers(Dictionary<int, PlayerManager> players)
    {
        foreach (KeyValuePair<int, PlayerManager> pair in players)
        {
            GameObject _object = Instantiate(entryPrefab, transform);
            LeaderboardEntry entry = _object.GetComponent<LeaderboardEntry>();
            entry.username.text = pair.Value.username;
            entry.kills.text = "0";

            // float y = -entry.rectTransform.rect.height / 2 - entries.Count * entry.rectTransform.rect.height;
            // entry.targetPosition = new Vector2(0, y);
            // entry.rectTransform.anchoredPosition = new Vector2(0, y);

            entries[pair.Key] = entry;
        }
        
        SortLeaderboard(animated:false);
    }

    public void Clear()
    {
        foreach (KeyValuePair<int, LeaderboardEntry> entry in entries)
        {
            Destroy(entry.Value.gameObject);
        }
        entries.Clear();
    }

    public void IncreaseKillCount(int id)
    {
        LeaderboardEntry entry = entries[id];
        entry.kills.text = (int.Parse(entry.kills.text) + 1).ToString();

        SortLeaderboard(animated:true);
    }

    // Sets the rank and targetPositions of each LeaderboardEntry in entries according to their kills (Ascending)
    public void SortLeaderboard(bool animated)
    {
        var grouped = entries.GroupBy(p => int.Parse(p.Value.kills.text));
        var ordered = grouped.OrderByDescending(g => g.Key);

        int totalRank = 1;
        foreach (var group in ordered)
        {
            var reorderedGroup = group.OrderBy(entry => entry.Value.username.text);
            // rank is displayed, totalRank is used for positioning
            int rank = totalRank;
            foreach (KeyValuePair<int, LeaderboardEntry> entry in reorderedGroup)
            {
                entry.Value.rank.text = rank.ToString();

                entry.Value.transform.SetSiblingIndex(entries.Count - totalRank);

                switch (rank){
                    case 1:
                    entry.Value.image.color = goldColor;
                    break;
                    case 2:
                    entry.Value.image.color = silverColor;
                    break;
                    case 3:
                    entry.Value.image.color = bronzeColor;
                    break;
                    default:
                    entry.Value.image.color = defaultColor;
                    break;
                }

                float y = entry.Value.rectTransform.rect.height/2 - totalRank*entry.Value.rectTransform.rect.height;
                entry.Value.targetPosition = new Vector2(0, y);

                entry.Value.rectTransform.anchoredPosition = animated ? entry.Value.rectTransform.anchoredPosition : entry.Value.targetPosition;

                totalRank++;
            }
        }
    }
}
