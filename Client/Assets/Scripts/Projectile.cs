using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int projectileId;
    public int type;
    public GameObject explosionPrefab;


    public void Initialize(int _projectileId, int _type)
    {
        projectileId = _projectileId;
        type = _type;
    }

    public void Destroy()
    {
        Instantiate(explosionPrefab, transform.position, explosionPrefab.transform.rotation);
        Destroy(gameObject);
    }
}