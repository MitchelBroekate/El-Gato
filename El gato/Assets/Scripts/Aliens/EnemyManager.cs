using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    Transform spawnparent;

    [SerializeField]
    Transform enemyParent;

    List<Transform> spawnpoints = new();

    public Wave[] waves;

    public int currentWave;

    bool waveReady;

    private void Start()
    {
        currentWave = -1;

        waveReady = true;

        GetSpawnpoints();

        for (int i = 0; i < waves.Length; i++) 
        {
            waves[i].enemiesAlive = waves[i].enemies.Length;
        }
    }

    private void Update()
    {
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

    }

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

    void GetSpawnpoints()
    {
        for (int i = 0; i < spawnparent.childCount; i++)
        {
            spawnpoints.Add(spawnparent.GetChild(i));
        }
    }

    private IEnumerator SpawnWave()
    {
        if (currentWave < waves.Length)
        {
            for (int i = 0; i < waves[currentWave].enemies.Length; i++)
            {
                int spawn = UnityEngine.Random.Range(0, spawnpoints.Count);
                GameObject enemy = Instantiate(waves[currentWave].enemies[i], spawnpoints[spawn].position, Quaternion.identity);
                enemy.transform.parent = enemyParent;

                yield return new WaitForSeconds(waves[currentWave].enemyWaitTime);
            }
        }

    }
}

[Serializable]
public class Wave
{
    public GameObject[] enemies;
    public float enemyWaitTime = 4;

    public int enemiesAlive;
}