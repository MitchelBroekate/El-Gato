using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBehaviour : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField]
    List<Transform> checkpoints;

    Transform checkpointParent;
    public Transform towerParent;

    public Transform nearestCheckpoint;
    public Transform nearestTower;

    float walkSpeed;

    [SerializeField]
    AlienStates currentState;

    bool collisionCheck;

    [SerializeField]
    int health = 300;

    int damage = 100;

    Animator animator;

    bool allowAttack;
    bool allowDamage;

    enum AlienStates
    {
        DECEND,
        GOTOCHECKPOINT,
        GOTOTOWER,
        ATTACKTOWER,
        DYING   
    }

    private void Start()
    {
        Physics.IgnoreLayerCollision(9, 8);

        currentState = AlienStates.DECEND;

        animator = GetComponent<Animator>();

        collisionCheck = true;

        checkpointParent = GameObject.Find("AlienCheckpoint").transform;

        for (int i = 0; i < checkpointParent.childCount; i++)
        {
            checkpoints.Add(checkpointParent.GetChild(i));
        }

        walkSpeed = 150;

        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        DoStates();

        if (collisionCheck)
        {
            rb.AddForce(-transform.up * 100 * Time.deltaTime, ForceMode.Force);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "ground")
        {
            if (collisionCheck)
            {
                rb.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;

                currentState = AlienStates.GOTOCHECKPOINT;

                collisionCheck = false;
            }

        }
    }

    void DoStates()
    {
        switch (currentState)
        {
            case AlienStates.DECEND:
                animator.GetBool("Idle");
                animator.SetBool("Idle", true);
                rb.velocity = -transform.up * 100 * Time.deltaTime;
                break;

            case AlienStates.GOTOCHECKPOINT:
                animator.GetBool("Walking");
                animator.GetBool("Idle");
                animator.GetBool("Attacking");

                animator.SetBool("Walking", true);
                animator.SetBool("Idle", false);
                animator.SetBool("Attacking", false);

                GoToCheckpoint();
                break;

            case AlienStates.GOTOTOWER:
                animator.GetBool("Walking");
                animator.GetBool("Idle");
                animator.GetBool("Attacking");

                animator.SetBool("Walking", true);
                animator.SetBool("Idle", false);
                animator.SetBool("Attacking", false);

                GoToTower();
                break;

            case AlienStates.ATTACKTOWER:
                animator.GetBool("Attacking");
                animator.GetBool("Idle");
                animator.GetBool("Walking");

                animator.SetBool("Attacking", true);
                animator.SetBool("Idle", false);
                animator.SetBool("Walking", false);

                AttackTower();
                break;

            case AlienStates.DYING:

                rb.constraints = RigidbodyConstraints.FreezeAll;
                animator.GetBool("Dying");
                animator.GetBool("Attacking");
                animator.GetBool("Walking");
                animator.GetBool("Idle");

                animator.SetBool("Dying", true);
                animator.SetBool("Attacking", false);
                animator.SetBool("Walking", false);
                animator.SetBool("Idle", false);

                Destroy(gameObject, 2.5f);
                break;

            default:
                Debug.Log("No State Active!");
                break;
        }
    }

    /// <summary>
    /// Goes through the checkpoint list and gets the closest one to go towards
    /// </summary>
    void GoToCheckpoint()
    {
        allowAttack = true;

        float distance;
        float nearestDistance = float.MaxValue;

        for (int i = 0; i < checkpoints.Count; i++)
        {
            if (checkpoints[i] != null)
            {
                distance = Vector3.Distance(transform.position, checkpoints[i].position);

                if (distance < nearestDistance)
                {
                    nearestCheckpoint = checkpoints[i];
                    nearestDistance = distance;

                }
            }
        }
        Quaternion lookTowards = Quaternion.LookRotation(new Vector3(nearestCheckpoint.position.x, transform.position.y, nearestCheckpoint.position.z) - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookTowards, 2 * Time.deltaTime);
        rb.velocity = transform.forward * walkSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, nearestCheckpoint.position) < 3f)
        {
            rb.velocity = Vector3.zero;

            animator.GetBool("Idle");
            animator.GetBool("Walking");

            animator.SetBool("Idle", true);
            animator.SetBool("Walking", false);

            if (towerParent.childCount > 0)
            {
                currentState = AlienStates.GOTOTOWER;
            }

        }
    }

    /// <summary>
    /// Goes through the tower parent and gets the closest tower and goes towards it
    /// </summary>
    void GoToTower()
    {
        allowAttack = true;
        float distance;
        float nearestDistance = float.MaxValue;

        for (int i = 0; i < towerParent.childCount; i++)
        {
            if (towerParent.childCount > 0)
            {
                distance = Vector3.Distance(transform.position, towerParent.GetChild(i).position);

                if (distance < nearestDistance)
                {
                    nearestTower = towerParent.GetChild(i);
                    nearestDistance = distance;

                }
            }
        }

        Quaternion lookTowards = Quaternion.LookRotation(new Vector3(nearestTower.position.x, transform.position.y, nearestTower.position.z) - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookTowards, 2 * Time.deltaTime);

        rb.velocity = transform.forward * walkSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, nearestTower.position) < 3.5f)
        {
            rb.velocity = Vector3.zero;
            currentState = AlienStates.ATTACKTOWER;
        }

    }

    /// <summary>
    /// Attacks the tower when its near it. If the tower is destroyed it goes to the next one or goes it back to the checkpoint when there are no more towers left
    /// </summary>
    void AttackTower()
    {
        if (nearestTower != null)
        {
            allowDamage = true;
            Quaternion lookTowards = Quaternion.LookRotation(new Vector3(nearestTower.position.x, transform.position.y, nearestTower.position.z) - transform.position);

            transform.rotation = Quaternion.Slerp(transform.rotation, lookTowards, 2 * Time.deltaTime);
            if (allowAttack)
            {
                StartCoroutine(AttackDamage());

                allowAttack = false;
            }
           
        }

        if (nearestTower == null && towerParent.childCount <= 0)
        {
            allowDamage = false;
            StopAllCoroutines();
            currentState = AlienStates.GOTOCHECKPOINT;
        }
        else if (nearestTower == null && towerParent.childCount > 0)
        {
            allowDamage = false;
            StopAllCoroutines();
            currentState = AlienStates.GOTOTOWER;
        }


    }

    public void DoDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            currentState = AlienStates.DYING;

        }
    }

    IEnumerator AttackDamage()
    {
        while (allowDamage)
        {
            yield return new WaitForSeconds(2f);

            nearestTower.GetComponent<Health>().DoDamage(damage);
        }

    }
}
