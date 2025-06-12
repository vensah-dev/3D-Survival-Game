using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Folliage Profile", menuName = "Folliage/Profile")]
public class Folliage : ScriptableObject
{
    [Space]
    [Header("Foliage")]
    public int id = 0;
    public GameObject Prefab;

    [Space]
    [Header("Spawn Settings")]
    public float MinHeight = 30f;
    public float MaxHeight = 100f;
    public int Offset;
    public Vector3 MinSize;
    public Vector3 MaxSize;


    [Space]
    [Header("Random Spawns")]
    public bool randomSpawns;
    public int Rarity;


    [Space]
    [Header("Group Spawns")]
    public float Density = 0.3f;

    [Header("Noise Settings")]
    public float Lacunarity = 2f;

    [Range(0, 3)]
    public float Persistance = 0.5f;

    public float NoiseScale = 24.2f;

    public int Octaves = 3;

    [HideInInspector]
    public int Seed;

}
