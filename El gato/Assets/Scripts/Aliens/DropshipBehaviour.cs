using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropshipBehaviour : MonoBehaviour
{
    float decendSpeedShip;
    float decendSpeedAlien;

    float ascendSpeedShip;

    RaycastHit hit;

    shipStates currentState;

    GameObject alien;
    GameObject currentAlien;

    Transform spawnLocation;

    List<Transform> spawnpoints = new();

    bool doRoutine = true;

    enum shipStates
    {
        DECENDING,
        DROPOFF,
        SHIPEXIT    
    }
    [System.Obsolete]
    private void Start()
    {
        GetShipChildInfo();

        decendSpeedShip = 1000;
        decendSpeedAlien = 10;

        ascendSpeedShip = 10;

        currentState = shipStates.DECENDING;

    }
    private void Update()
    {
        DoState();
        StateSwitcher();
    }

    [System.Obsolete]
    void GetShipChildInfo()
    {
        alien = transform.FindChild("Alien").gameObject;
        spawnLocation = transform.FindChild("Spawns").transform;

        for (int i = 0; 0 < spawnLocation.childCount -1; i++) 
        {
            if (!spawnpoints.Contains(spawnLocation.GetChild(i)))
            {
                spawnpoints.Add(spawnLocation.GetChild(i));
            }
        }
    }

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

    void StateSwitcher()
    {

    }

    void Decending()
    {
        GetComponent<Rigidbody>().velocity = -transform.up * decendSpeedShip * Time.deltaTime;

        if (Physics.Raycast(transform.position, -transform.up, out hit, Mathf.Infinity))
        {

            if (Vector3.Distance(transform.position, hit.point) <= 60)
            {
                GetComponent<Rigidbody>().AddForce(transform.up * ascendSpeedShip * Time.deltaTime);
            }

            if (Vector3.Distance(transform.position, hit.point) >= 200)
            {
                Destroy(gameObject);
            }
        }

    }

    void DropOff()
    {
        if (doRoutine)
        {
            StartCoroutine(AlienSpawn());
        }
    }

    void ShipExit()
    {
        GetComponent<Rigidbody>().velocity = transform.up * decendSpeedShip * Time.deltaTime;

        if (Physics.Raycast(transform.position, -transform.up, out hit, Mathf.Infinity))
        {
            if (Vector3.Distance(transform.position, hit.point) <= 60)
            {
                if (decendSpeedShip > 0)
                {
                    decendSpeedShip -= 5;
                }

            }

            if (Vector3.Distance(transform.position, hit.point) <= 30)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }

    IEnumerator AlienSpawn()
    {
        doRoutine = false;

        for (int i = 0; 0 < Random.Range(2, 6); i++)
        {
            int randomWaitTime = Random.Range(4, 6);
            int randomSpawn = Random.Range(0, 4);

            currentAlien = Instantiate(alien, spawnpoints[randomSpawn].position, Quaternion.identity);
            currentAlien.SetActive(true);
            currentAlien.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            currentAlien.GetComponent<Rigidbody>().velocity = -currentAlien.transform.up * decendSpeedAlien * Time.deltaTime;

            yield return new WaitForSeconds(randomWaitTime);
        }

        currentState = shipStates.SHIPEXIT;
    }

}
