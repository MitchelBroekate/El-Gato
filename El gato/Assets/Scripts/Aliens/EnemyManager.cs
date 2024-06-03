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

    int check = 0;

    GameObject cowParent;

    [SerializeField]
    bool cowStatusCheck = true;

    bool spawnCheck = false;

    [SerializeField]
    int next;

    private void Start()
    {
        GetSpawnpoints();

        cowParent = GameObject.Find("lives");
    }

    private void Update()
    {
        SpawnShuttle();
        GetCowStatus();
    }

    void GetCowStatus()
    {
        if (check >= cowParent.transform.childCount)
        {
            cowStatusCheck = false;

            check = 0;
        }
        else
        {
            if (cowParent.transform.GetChild(check).GetComponent<CowCheck>().available)
            {

                cowStatusCheck = true;

            }
            else
            {
                check++;
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
        bool stopLoop = false;
        while (cowStatusCheck)
        {
            if (!stopLoop)
            {
                if (next == 1)
                {
                    stopLoop = false;
                    for (int i = 0; i < 30; i++)
                    {
                        int spawn = Random.Range(0, spawnpoints.Count);

                        GameObject enemy = Instantiate(alienShuttle, spawnpoints[spawn].position, Quaternion.identity);
                        enemy.transform.parent = enemyParent;

                        yield return new WaitForSeconds(waitTime);
                    }
                    stopLoop = true;
                }
            }


            if (next == 2)
            {
                stopLoop = false;
                for (int i = 0; i < 15; i++)
                {
                    int spawn = Random.Range(0, spawnpoints.Count);

                    GameObject enemy = Instantiate(alienShuttle, spawnpoints[spawn].position, Quaternion.identity);
                    enemy.transform.parent = enemyParent;

                    yield return new WaitForSeconds(waitTime);
                }
                stopLoop = true;
            }

            if (next == 3)
            {
                stopLoop = false;
                for (int i = 0; i < 20; i++)
                {
                    int spawn = Random.Range(0, spawnpoints.Count);

                    GameObject enemy = Instantiate(alienShuttle, spawnpoints[spawn].position, Quaternion.identity);
                    enemy.transform.parent = enemyParent;

                    yield return new WaitForSeconds(waitTime);
                }
                stopLoop = true;
            }

            if (next == 4)
            {
                stopLoop = false;
                for (int i = 0; i < 30; i++)
                {
                    int spawn = Random.Range(0, spawnpoints.Count);

                    GameObject enemy = Instantiate(alienShuttle, spawnpoints[spawn].position, Quaternion.identity);
                    enemy.transform.parent = enemyParent;

                    yield return new WaitForSeconds(waitTime);
                }
                stopLoop = true;
            }

        }

        if (next == 5)
        {
            //enable winscreen
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
