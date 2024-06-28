using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyManager : MonoBehaviour
{
    #region Variables
    [SerializeField]
    Transform enemyParent;

    [SerializeField]
    Transform cowParent;

    int cowAmount;

    List<Transform> spawnpoints = new();

    public Wave[] waves;

    public int currentWave;

    bool waveReady;

    public bool alienEvent;

    [SerializeField]
    GameObject uFOCheck;
    #endregion

    //Sets wave and cow amount
    private void Start()
    {
        cowAmount = cowParent.childCount;

        currentWave = -1;

        waveReady = true;
    }

    //Sets wave ready if enemies are dead and checks if you won
    private void Update()
    {
        if (currentWave >= waves.Length)
        {
            if (waveReady)
            {
            GameObject.Find("UI/UXManager").GetComponent<UiManager>().ShowWinScreen();
            print("Game Won");

            return;

            }
        }
        if (currentWave >= 0)
        {
            if (waves[currentWave].enemiesAlive <= 0)
            {
                waveReady = true;
            }
        }
    }

    // Starts a new wave when all enemies are gone/dead
    public void NextWave(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (waveReady)
            {
                currentWave++;
                WaveEvent();
                StartCoroutine(SpawnWave());
                waveReady = false;
            }

        }
    }

    //Starts an event when you have all of your lives and reach wave 3
    public void WaveEvent()
    {
        if (currentWave == 2)
        {
            if (cowParent.childCount == cowAmount)
            {
                print("Wave Event");

                alienEvent = true;

                //activate TEXT
            }
        }
        else
        {
            alienEvent = false;
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
                GameObject g = waves[currentWave].toSpawn[i].Spawn();
                g.transform.parent = enemyParent;

                if (waves[currentWave].toSpawn[i].herfst)
                {
                    if (waves[currentWave].toSpawn[i].uFO)
                    {
                        g.GetComponent<UfoBehaviour>().SetHealthHerfst(75);
                    }
                    else
                    {
                        g.GetComponent<DropshipBehaviour>().SetPlusHealth(75);
                    }
                }
                

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