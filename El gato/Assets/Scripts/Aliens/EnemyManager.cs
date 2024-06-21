using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyManager : MonoBehaviour
{
    #region Variables
    #region UFO Enemy
    [SerializeField]
    Transform spawnparent;

    [SerializeField]
    Transform enemyParent;

    List<Transform> spawnpoints = new();

    public Wave[] waves;

    public int currentWave;

    bool waveReady;
    #endregion

    #region Dropship Enemy

    #endregion
    #endregion


    private void Start()
    {
        #region UFO Enemy
        currentWave = -1;

        waveReady = true;

        GetSpawnpoints();

        //for (int i = 0; i < waves.Length; i++) 
        //{
        //    waves[i].enemiesAlive = waves[i].enemies.Length;
        //}
        #endregion

        #region Dropship Enemy

        #endregion
    }

    private void Update()
    {
        #region UFO Enemy
        if (currentWave >= waves.Length)
        {
            GameObject.Find("UIManager").GetComponent<UiManager>().ShowWinScreen();
            print("Game Won");

            return;
        }
        if (currentWave >= 0)
        {
            if (waves[currentWave].enemiesAlive <= 0)
            {
                waveReady = true;
            }
        }
        #endregion

        #region Dropship Enemy

        #endregion

    }

    // Starts a new wave when all enemies are gone/dead
    public void NextWave(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (waveReady)
            {
                currentWave++;
                StartCoroutine(SpawnWave());
                waveReady = false;
            }

        }
    }

    //Gets the spawnpoints where the enemies need to spawn
    void GetSpawnpoints()
    {
        for (int i = 0; i < spawnparent.childCount; i++)
        {
            spawnpoints.Add(spawnparent.GetChild(i));
        }
    }

    //Spawns the enemies with a pause between each spawn
    private IEnumerator SpawnWave()
    {
        if (currentWave < waves.Length)
        {
            for (int i = 0; i < waves[currentWave].toSpawn.Length; i++)
            {
                int spawn = UnityEngine.Random.Range(0, spawnpoints.Count);
                //GameObject enemy = Instantiate(waves[currentWave].enemies[i], spawnpoints[spawn].position, Quaternion.identity);
                GameObject g = waves[currentWave].toSpawn[i].Spawn();
                g.transform.parent = enemyParent;

                yield return new WaitForSeconds(waves[currentWave].enemyWaitTime);
            }
        }

    }
}

/// <summary>
/// This class is used to create a list of enemies and the information needed for configuration
/// </summary>
[Serializable]
public class Wave
{
    public ToSpawn[] toSpawn;
    public float enemyWaitTime = 4;

    public int enemiesAlive;
}