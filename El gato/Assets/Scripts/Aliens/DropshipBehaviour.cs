using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DropshipBehaviour : MonoBehaviour
{
    #region Variables
    float decendSpeedShip;

    float ascendSpeedShip;

    RaycastHit hit;

    shipStates currentState;

    GameObject alien;
    GameObject currentAlien;

    Transform spawnLocation;

    [SerializeField]
    List<Transform> spawnpoints = new();

    Transform towerParent;
    Transform enemyParent;

    bool doRoutine = true;

    GameObject particleParent;
    #endregion

    /// <summary>
    /// Enum for ship states
    /// </summary>
    enum shipStates
    {
        DECENDING,
        DROPOFF,
        SHIPEXIT
    }
    [System.Obsolete]

    //Sets default values, starting state and gets children/spawnpoints
    private void Start()
    {
        GetShipChildInfo();

        decendSpeedShip = 1000;

        ascendSpeedShip = 20;

        currentState = shipStates.DECENDING;

        transform.Rotate(0, UnityEngine.Random.Range(0, 360), 0);

        towerParent = GameObject.Find("TowersParent").transform;
        enemyParent = GameObject.Find("EnemyParent").transform;

        particleParent = GameObject.Find("Ufo Coils");

    }

    //Updates the voids
    private void Update()
    {
        DoState();
    }

    /// <summary>
    /// Gets children and spawnpoints
    /// </summary>
    [System.Obsolete]
    void GetShipChildInfo()
    {
        alien = transform.FindChild("Alien").gameObject;
        spawnLocation = transform.FindChild("Spawns").transform;

        for (int i = 0; i < spawnLocation.childCount; i++)
        {
            if (!spawnpoints.Contains(spawnLocation.GetChild(i)))
            {
                spawnpoints.Add(spawnLocation.GetChild(i));
            }
        }
    }

    //Switch case for the ships states
    void DoState()
    {
        switch (currentState)
        {
            case shipStates.DECENDING:
                Decending();
                break;

            case shipStates.DROPOFF:
                DropOff();
                break;

            case shipStates.SHIPEXIT:
                ShipExit();
                break;
        }
    }

    /// <summary>
    /// Starts the ship with a decend 
    /// </summary>
    void Decending()
    {
        GetComponent<Rigidbody>().velocity = -transform.up * decendSpeedShip * Time.deltaTime;

        if (Physics.Raycast(transform.position, -transform.up, out hit, Mathf.Infinity))
        {

            if (Vector3.Distance(transform.position, hit.point) <= 60)
            {
                if (decendSpeedShip > 0)
                {
                    decendSpeedShip -= 200 * Time.deltaTime;
                }
            }

            if (Vector3.Distance(transform.position, hit.point) <= 30)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;

                currentState = shipStates.DROPOFF;
            }
        }

    }

    /// <summary>
    /// Activates the alien spawner
    /// </summary>
    void DropOff()
    {
        if (doRoutine)
        {
            StartCoroutine(AlienSpawn());
        }
    }

    /// <summary>
    /// Ascends the ship when its done and destroys it
    /// </summary>
    void ShipExit()
    {
        doRoutine = true;

        GetComponent<Rigidbody>().velocity = transform.up * ascendSpeedShip * Time.deltaTime;

        if (Physics.Raycast(transform.position, -transform.up, out hit, Mathf.Infinity))
        {
            if (Vector3.Distance(transform.position, hit.point) <= 60)
            {
                ascendSpeedShip += 500 * Time.deltaTime;
            }

            if (Vector3.Distance(transform.position, hit.point) >= 200)
            {
                ascendSpeedShip = 50000;

                GetComponent<Rigidbody>().velocity = transform.forward * ascendSpeedShip * Time.deltaTime;

                if (doRoutine)
                {
                    StartCoroutine(KillShip());
                }
            }
        }
    }

    /// <summary>
    /// Spawns aliens at random spawnpoints
    /// </summary>
    /// <returns></returns>
    IEnumerator AlienSpawn()
    {
        doRoutine = false;

        if (towerParent.childCount > 0)
        {
            particleParent.SetActive(true);

            for (int i = 0; i < towerParent.childCount; i++)
            {
                int randomSpawn = UnityEngine.Random.Range(0, 3);

                currentAlien = Instantiate(alien, spawnpoints[randomSpawn].position, Quaternion.identity);
                currentAlien.transform.parent = enemyParent;
                currentAlien.GetComponent<AlienBehaviour>().towerParent = towerParent;
                currentAlien.SetActive(true);

                yield return new WaitForSeconds(4);


            }

            currentState = shipStates.SHIPEXIT;

            particleParent.SetActive(false);

            StopCoroutine(AlienSpawn());
        }

    }

    /// <summary>
    /// Destroys the ship after takeoff
    /// </summary>
    /// <returns></returns>
    IEnumerator KillShip()
    {
        doRoutine = false;

        yield return new WaitForSeconds (3);

        Destroy(gameObject);
    }
}
