using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyManager : MonoBehaviour
{
    List<Transform> spawnpoints = new();

    [SerializeField]
    Transform spawnparent;
    [SerializeField]
    GameObject alienShuttle;
    [SerializeField]
    Transform enemyParent;

    bool spawnCheck = false;

    public int next;



    private void Start()
    {
        GetSpawnpoints();
    }

    private void Update()
    {
        SpawnShuttle();
    }

    void GetSpawnpoints()
    {
        for (int i = 0; i < spawnparent.childCount; i++)
        {
            spawnpoints.Add(spawnparent.GetChild(i));
        }
    }

    void SpawnShuttle()
    {
        switch (next)
        {
            case 1:

                if (spawnCheck)
                {
                    StartCoroutine(WaveWaitTime(1));
                    spawnCheck = false;
                }

                break;

            case 2:

                if (spawnCheck)
                {
                    StartCoroutine(WaveWaitTime(3));
                    spawnCheck = false;
                }

                break;

            case 3:

                if (spawnCheck)
                {
                    StartCoroutine(WaveWaitTime(2));
                    spawnCheck = false;
                }

                break;

            case 4:

                if (spawnCheck)
                {
                    StartCoroutine(WaveWaitTime(1));
                    spawnCheck = false;
                }

                break;

            case 5:

                if (spawnCheck)
                {
                    StartCoroutine(WaveWaitTime(1));
                    spawnCheck = false;
                }
                break;
        }
    }



    IEnumerator WaveWaitTime(int waitTime)
    {
        if (next == 1)
        {
            for (int i = 0; i < 20; i++)
            {
                int spawn = Random.Range(0, spawnpoints.Count);

                GameObject enemy = Instantiate(alienShuttle, spawnpoints[spawn].position, Quaternion.identity);
                enemy.transform.parent = enemyParent;

                yield return new WaitForSeconds(waitTime);
            }
            StopAllCoroutines();
        }
        if (next == 2)
        {
            for (int i = 0; i < 15; i++)
            {
                int spawn = Random.Range(0, spawnpoints.Count);

                GameObject enemy = Instantiate(alienShuttle, spawnpoints[spawn].position, Quaternion.identity);
                enemy.transform.parent = enemyParent;

                yield return new WaitForSeconds(waitTime);
            }
            StopAllCoroutines();
        }

        if (next == 3)
        {
            for (int i = 0; i <20; i++)
            {
                int spawn = Random.Range(0, spawnpoints.Count);

                GameObject enemy = Instantiate(alienShuttle, spawnpoints[spawn].position, Quaternion.identity);
                enemy.transform.parent = enemyParent;

                yield return new WaitForSeconds(waitTime);
            }
                StopAllCoroutines();
        }

        if (next == 4)
        {
            for (int i = 0; i < 30; i++)
            {
                int spawn = Random.Range(0, spawnpoints.Count);

                GameObject enemy = Instantiate(alienShuttle, spawnpoints[spawn].position, Quaternion.identity);
                enemy.transform.parent = enemyParent;

                yield return new WaitForSeconds(waitTime);
            }
            StopAllCoroutines();
        }

        if (next == 5)
        {
            GameObject.Find("UIManager").GetComponent<UiManager>().ShowWinScreen();
        }
    }

    public void NextWave(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (enemyParent.childCount <= 0)
            {
                next++;
                spawnCheck = true;
            }

        }


    }
}
