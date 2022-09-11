using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int projectileId;
    public GameObject explosionPrefab;


    public void Initialize(int _projectileId)
    {
        projectileId = _projectileId;
    }

    public void Destroy()
    {
        Instantiate(explosionPrefab, transform.position, explosionPrefab.transform.rotation);
        Destroy(gameObject);
    }
}