using UnityEngine;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;

    public TextMesh usernameText;
    public Transform gun;

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        usernameText.text = _username;
    }
}