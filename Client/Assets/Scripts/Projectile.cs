using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int projectileId;
    public int type;
    private Vector2 position;

    public void Initialize(int _projectileId, int _type)
    {
        projectileId = _projectileId;
        type = _type;
        position = transform.position;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}