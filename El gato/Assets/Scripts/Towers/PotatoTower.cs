using System.Collections.Generic;
using UnityEngine;

public class PotatoTower : TowerManager
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
    GameObject bulletPotato;

    [Header("List With Targets")]
    public List<Transform> allTargets = new();

    [Header("Current Targeted Target")]
    [SerializeField]
    Transform nearestTarget;

    int damage = 1;
    #endregion

    //Assigns children and vars
    [System.Obsolete]
    private void Start()
    {
        rotateY = transform.FindChild("RotateY");
        rotateX = rotateY.FindChild("RotateX");
        bulletSpawn = rotateX.FindChild("BulletSpawn");
        bulletPotato = rotateX.FindChild("PotatoBullet").gameObject;
        rangeScale = 30;
        health = 150;
        bulletSpeed = 10000;

        fireRate = 1;

        GetComponent<SphereCollider>().radius = rangeScale;
    }

    //Updates the targeting
    private void Update()
    {
        Targeting();
    }

    //Checks when an enemy UFO has entered the towers range and adds it to the list of enemies in range
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.tag == "enemyship")
        {
            if (other.GetComponent<UfoBehaviour>().uFOState != UfoBehaviour.UFOState.MOVINGOUT || other.GetComponent<UfoBehaviour>().uFOState != UfoBehaviour.UFOState.QUEUE || other.GetComponent<UfoBehaviour>().uFOState == UfoBehaviour.UFOState.DYING)
            {
                if (!allTargets.Contains(other.transform))
                {
                    allTargets.Add(other.transform);
                }
            }
        }
    }

    //Checks when an enemy UFO has left the towers range, removes it from the list of enemies in range and sets the nearest to null
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

    /// <summary>
    /// Sets the nearest enemy UFO, traces- and shoots at its
    /// </summary>
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
            if (nearestTarget.GetComponent<UfoBehaviour>().uFOState == UfoBehaviour.UFOState.MOVINGOUT || nearestTarget.GetComponent<UfoBehaviour>().uFOState == UfoBehaviour.UFOState.DYING)
            {
                allTargets.Remove(nearestTarget);
                nearestTarget = null;
            }
            if (nearestTarget != null)
            {
                Quaternion lookTowardsY = Quaternion.LookRotation(new Vector3(nearestTarget.position.x, rotateY.transform.position.y, nearestTarget.position.z) - rotateY.transform.position);
                Quaternion lookTowardsX = Quaternion.LookRotation(nearestTarget.position - rotateX.transform.position);

                rotateY.transform.rotation = Quaternion.Slerp(rotateY.transform.rotation, lookTowardsY, 10f * Time.deltaTime);
                rotateX.transform.rotation = Quaternion.Slerp(rotateX.transform.rotation, lookTowardsX, 10f * Time.deltaTime);

                if (Time.time >= whenToFire)
                {
                    whenToFire = Time.time + 1 / fireRate;
                    Shooting();
                }
            }


        }
    }

    /// <summary>
    /// Shoots/instantiates a bullet in the direction where the tower is looking at
    /// </summary>
    void Shooting()
    {
        GameObject bullet = Instantiate(bulletPotato, bulletSpawn.position, rotateY.transform.rotation);

        bullet.SetActive(true);

        bullet.GetComponent<Rigidbody>().AddForce(rotateX.forward * bulletSpeed);
    }

    /// <summary>
    /// Applies the damage value for when an alien attacks the tower
    /// </summary>
    /// <param name="damage"></param>
    public void DoDamage(int damage)
    {
        health -= damage;

        if (health < 0)
        {
            Destroy(gameObject);
        }
    }

    public void AddDamage(int addedDamage)
    {
        damage = addedDamage;
        bulletPotato.GetComponent<PotatoBulletBehavior>().bulletDamage = bulletPotato.GetComponent<PotatoBulletBehavior>().bulletDamage * damage;

    }
}
