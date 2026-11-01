using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Theme", menuName = "Scriptable Object/Theme")]
public class ThemeScript : ScriptableObject
{
    [Header("Animation")]
    public new string name;
    public Sprite display;
    public int id;

    public Vector3 position;
    public Vector3 size;
}
