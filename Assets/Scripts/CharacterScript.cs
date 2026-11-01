using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Scriptable Object/Character")]
public class CharacterScript : ScriptableObject
{
    [Header("Animation")]
    public AnimatorOverrideController overrideController;

    [Header("Info")]
    public new string name;
    public int id;

    [TextArea]
    public string description;

    [Header("Stats")]
    public float health;
    public float damage;
    public float defense;

    [Header("Visual")]
    public Sprite portrait;

    [Header("Textures and Body Parts")]
    [Header("Head")]
    public Sprite headTexture;
    [Header("Position")]
    public float headPosX;
    public float headPosY;
    [Header("Rotation")]
    public float headRotation;
    [Header("Size")]
    public float headSizeX;
    public float headSizeY;

    [Header("Body")]
    public Sprite bodyTexture;
    [Header("Body Position")]
    public float bodyPosX;
    public float bodyPosY;
    [Header("Body Rotation")]
    public float bodyRotation;
    [Header("Body Size")]
    public float bodySizeX;
    public float bodySizeY;

    [Header("Hand")]
    public Sprite handTexture;
    [Header("Hand Position")]
    public float handPosX;
    public float handPosY;
    [Header("Hand Rotation")]
    public float handRotation;
    [Header("Hand Size")]
    public float handSizeX;
    public float handSizeY;

    [Header("Options")]
    public bool useHands = true;
    public float leftFeetHeight;
    public float rightFeetHeight;
    public float potraitSize = 500;
}
