using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "ToSpawn", menuName = "Eldoradothingmijig/Spawn", order = 1)]
public class ToSpawn : ScriptableObject
{
    public Transform[] spawnPoints;
    public GameObject toSpawn;

    public GameObject Spawn()
    {
        int randomPos = Random.Range(0, spawnPoints.Length);
        GameObject g = Instantiate(toSpawn, spawnPoints[randomPos].position, Quaternion.identity);
        return g;
    }
}
