using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int itemId;
    public int type;
    private Vector2 position;

    public void Initialize(int _itemId, int _type)
    {
        itemId = _itemId;
        type = _type;
        position = transform.position;
    }
}