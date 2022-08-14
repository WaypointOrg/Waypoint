using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    //turretposition
    //turret rotation
    //turret destroyed
    public int turretId;
    public Transform gun;
    public GameObject explosionPrefab;

    public TextMesh usernameText;

    public void Initialize(int _turretId, int _owner)
    {
        turretId = _turretId;
        usernameText.text = GameManager.players[_owner].username;
    }

    public void Destroy()
    {
        Instantiate(explosionPrefab, transform.position, explosionPrefab.transform.rotation);
        Destroy(gameObject);
    }
}
