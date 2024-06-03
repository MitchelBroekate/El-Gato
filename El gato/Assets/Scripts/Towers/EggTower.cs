using System.Collections.Generic;
using UnityEngine;

public class EggTower : TowerManager
{
    #region Variables
    [Header("Children Of GameObject")]
    [SerializeField]
    Transform rotateY;
    [SerializeField]
    Transform rotateX;
    [SerializeField]
    Transform bulletSpawn;
    [SerializeField]
    GameObject bulletEgg;

    [Header("List With Targets")]
    [SerializeField]
    List<Transform> allTargets = new();

    [Header("Current Targeted Target")]
    [SerializeField]
    Transform nearestTarget;

    #endregion

    [System.Obsolete]
    private void Start()
    {
        rotateY = transform.FindChild("RotateY");
        rotateX = rotateY.FindChild("RotateX");
        bulletSpawn = rotateX.FindChild("BulletSpawn");
        bulletEgg = rotateX.FindChild("BulletEgg").gameObject;
        rangeScale = 30;
        health = 150;
        bulletSpeed = 10000;

        fireRate = 15;

        GetComponent<SphereCollider>().radius = rangeScale;
    }

    private void Update()
    {
        Targeting();

        DeathCheck();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.tag == "enemyship")
        {
            other.GetComponent<UfoBehavior>().inTurretRange = true;

            if (!other.GetComponent<UfoBehavior>().targeted)
            {
                if (!allTargets.Contains(other.transform))
                {
                    allTargets.Add(other.transform);

                    other.GetComponent<UfoBehavior>().targeted = true;
                }
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.gameObject.tag == "enemyship")
        {

            other.GetComponent<UfoBehavior>().inTurretRange = false;

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

                    for (int b = 0; b < allTargets.Count; b++)
                    {
                        if (allTargets[b] != null)
                        {
                            if (allTargets[b].transform != nearestTarget)
                            {
                                allTargets[b].GetComponent<UfoBehavior>().targeted = false;

                                if (allTargets[b].GetComponent<UfoBehavior>().dead)
                                {
                                    Destroy(allTargets[b].gameObject);
                                    allTargets.Remove(allTargets[b]);

                                    GameObject.Find("Scripts/PlayerInput").GetComponent<BuildingShop>().money += 50;
                                }
                            }
                        }

                    }

                }
            }



        }
        if (nearestTarget != null)
        {
            rotateY.transform.LookAt(new Vector3(nearestTarget.position.x, rotateY.transform.position.y, nearestTarget.position.z));
            rotateX.transform.LookAt(nearestTarget.position);

            if (Time.time >= whenToFire)
            {
                whenToFire = Time.time + 1 / fireRate;
                Shooting();
            }

        }

    }

    void Shooting()
    {
        GameObject bullet = Instantiate(bulletEgg, bulletSpawn.position, rotateY.transform.rotation);

        bullet.SetActive(true);

        bullet.GetComponent<Rigidbody>().AddForce(rotateX.forward * bulletSpeed);
    }

    void DeathCheck()
    {
        if (nearestTarget != null)
        {
            if (nearestTarget.GetComponent<UfoBehavior>().dead)
            {
                allTargets.Remove(nearestTarget);
                Destroy(nearestTarget.gameObject);

                GameObject.Find("Scripts/PlayerInput").GetComponent<BuildingShop>().money += 50;
            }
        }

    }
}