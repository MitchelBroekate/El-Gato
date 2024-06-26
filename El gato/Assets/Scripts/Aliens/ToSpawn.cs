using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "ToSpawn", menuName = "EnemySO/Spawn", order = 1)]
public class ToSpawn : ScriptableObject
{
    #region Variables
    public Transform[] spawnPoints;
    public GameObject toSpawn;
    public bool herfst = false;
    public bool uFO = false;
    #endregion

    /// <summary>
    /// Spawns the Scriptable Object on the given location
    /// </summary>
    /// <returns></returns>
    public GameObject Spawn()
    {
        int randomPos = Random.Range(0, spawnPoints.Length);
        GameObject g = Instantiate(toSpawn, spawnPoints[randomPos].position, Quaternion.identity);
        return g;
    }
}
