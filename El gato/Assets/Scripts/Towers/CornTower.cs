using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CornTower : TowerManager
{
    #region Variables
    [Header("Children Of GameObject")]
    [SerializeField]
    Transform missileSpawn;
    [SerializeField]
    GameObject missileCorn;

    [Header("List With Targets")]
    public List<Transform> allTargets = new();

    [Header("Current Targeted Target")]
    public Transform nearestTarget;
    bool missileSpeedIncrease = true;

    GameObject missile;

    bool speedstop = true;

    #endregion

    [System.Obsolete]
    private void Start()
    {
        missileSpawn = transform.FindChild("MissileSpawn");
        missileCorn = transform.FindChild("Missile").gameObject;
        rangeScale = 30;
        health = 150;
        bulletSpeed = 10;

        fireRate = 10;

        GetComponent<SphereCollider>().radius = rangeScale;
    }

    private void Update()
    {
        Targeting();
    }

    private void FixedUpdate()
    {
        if (missile != null)
        {
            if (missileSpeedIncrease)
            {
                if (speedstop)
                {
                    StartCoroutine(BulletSpeedStop(3));
                    speedstop = false;
                }
                bulletSpeed += 0.1f;
                missile.GetComponent<Rigidbody>().AddForce(transform.up * bulletSpeed);
            }
        }
   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.tag == "enemyship")
        {
            if (!allTargets.Contains(other.transform))
            {
                allTargets.Add(other.transform);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.gameObject.tag == "enemyship")
        {
            if (allTargets.Contains(other.transform))
            {
                allTargets.Remove(other.transform);

                if (other.transform == nearestTarget)
                {
                    nearestTarget = null;
                }
            }
        }
    }

    void Targeting()
    {
        float distance;
        float nearestDistance = float.MaxValue;

        for (int i = 0; i < allTargets.Count; i++)
        {
            if (allTargets[i] != null)
            {
                distance = Vector3.Distance(transform.position, allTargets[i].position);

                if (distance < nearestDistance)
                {
                    nearestTarget = allTargets[i];
                    nearestDistance = distance;

                }
            }
        }
        if (nearestTarget != null)
        {
            if (Time.time >= whenToFire)
            {
                whenToFire = Time.time + 1 * fireRate;
                Shooting();
            }
        }
    }

    void Shooting()
    {
        missile = Instantiate(missileCorn, missileSpawn.position, quaternion.identity);
        missile.transform.parent = transform;
        speedstop = true;
        missileSpeedIncrease = true;

        missile.SetActive(true);
    }

    IEnumerator BulletSpeedStop(float time)
    {
        yield return new WaitForSeconds(time);

        missileSpeedIncrease = false;
    }
}
