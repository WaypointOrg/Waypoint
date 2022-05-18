using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardEntry : MonoBehaviour
{
    public float animationSpeed = 0.05f;
    public Image image;

    public Text rank;
    public Text username;
    public Text kills;

    public RectTransform rectTransform;
    public Vector2 targetPosition;
    
    void Update()
    {
        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetPosition, animationSpeed);
    }
}
