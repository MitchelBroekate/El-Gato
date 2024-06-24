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
    public List<Transform> allTargets = new();

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
            Quaternion lookTowardsY = Quaternion.LookRotation(new Vector3(nearestTarget.position.x, rotateY.transform.position.y, nearestTarget.position.z) - rotateY.transform.position);
            Quaternion lookTowardsX = Quaternion.LookRotation(nearestTarget.position - rotateX.transform.position);

            rotateY.transform.rotation = Quaternion.Slerp(rotateY.transform.rotation, lookTowardsY, 1 * Time.deltaTime);
            rotateX.transform.rotation = Quaternion.Slerp(rotateX.transform.rotation, lookTowardsX, 1 * Time.deltaTime);

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
        bullet.transform.parent = null;

        bullet.SetActive(true);

        bullet.GetComponent<Rigidbody>().AddForce(rotateX.forward * bulletSpeed);
    }

    public void DoDamage(int damage)
    {
        health -= damage;

        if (health < 0)
        {
            Destroy(gameObject);
        }
    }
}
