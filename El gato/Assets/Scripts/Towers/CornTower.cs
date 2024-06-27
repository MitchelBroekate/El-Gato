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
    bool missileSpeedIncrease = false;

    GameObject missile;

    bool speedstop = true;

    Animator animator;

    bool bulletNumerator = true;

    int damage = 1;
    #endregion

    //Assigns children and vars
    [System.Obsolete]
    private void Start()
    {
        missileSpawn = transform.FindChild("MissileSpawn");
        missileCorn = transform.FindChild("Missile").gameObject;
        rangeScale = 30;
        health = 300;
        bulletSpeed = 10;

        fireRate = 12;

        GetComponent<SphereCollider>().radius = rangeScale;
        animator = GetComponent<Animator>();

        Physics.IgnoreLayerCollision(7, 3);

        
    }

    //Updates the tower/missile Targeting
    private void Update()
    {
        Targeting();
    }

    //Updates the missile launch
    private void FixedUpdate()
    {
        if (missile != null)
        {
            if (missileSpeedIncrease)
            {
                if (speedstop)
                {
                    StartCoroutine(BulletSpeedStop(4.5f));
                    speedstop = false;
                }
                bulletSpeed += 0.2f;
                missile.GetComponent<Rigidbody>().AddForce(transform.up * bulletSpeed);
            }
        }
   
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
    /// Sets the nearest enemy UFO and shoots a missile at its
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
            }

            if (Time.time >= whenToFire)
            {
                whenToFire = Time.time + 1 * fireRate;
                animator.GetBool("RocketLift");
                animator.SetBool("RocketLift", true);
                Shooting();
            }
        }
    }

    /// <summary>
    /// Launches a missile with a wait time
    /// </summary>
    void Shooting()
    {
        bulletNumerator = true;
        bulletSpeed = 10;
        missile = Instantiate(missileCorn, missileSpawn.position, missileCorn.transform.rotation);
        missile.transform.parent = transform;
        missile.SetActive(true);
        speedstop = true;


        if (bulletNumerator)
        {
            StartCoroutine(BulletWait());
        }

    }

    /// <summary>
    /// Checks for the launch time
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator BulletSpeedStop(float time)
    {
        animator.GetBool("RocketLift");
        animator.SetBool("RocketLift", false);

        yield return new WaitForSeconds(time);

        missileSpeedIncrease = false;
    }

    /// <summary>
    /// Cooldown for the missile launch
    /// </summary>
    /// <returns></returns>
    IEnumerator BulletWait()
    {
        bulletNumerator = false;
        yield return new WaitForSeconds(2);

        missileSpeedIncrease = true;

    }

    public void DoDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void AddDamage(int addedDamage)
    {
        damage = addedDamage;
        missileCorn.GetComponent<CornBulletBehavior>().bulletDamage = missileCorn.GetComponent<CornBulletBehavior>().bulletDamage * damage;
    }
}
