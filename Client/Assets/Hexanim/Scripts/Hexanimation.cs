using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Clip")]
public class Hexanimation : ScriptableObject
{
    public string _name;

    public Sprite[] sprites;

    public bool loop;
}
