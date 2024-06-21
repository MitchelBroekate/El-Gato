using System.Collections.Generic;
using UnityEngine;

public class AlienBehaviour : MonoBehaviour
{
    Rigidbody rb;

    public Transform target;

    [SerializeField]
    List<Transform> checkpoints;

    Transform checkpointParent;

    public Transform nearestCheckpoint;

    float walkSpeed;

    [SerializeField]
    AlienStates currentState;

    bool collisionCheck;

    enum AlienStates
    {
        GOTOCHECKPOINT,
        GOTOTOWER,
        ATTACKTOWER
    }

    private void Start()
    {
        collisionCheck = true;

        checkpointParent = GameObject.Find("AlienCheckpoint").transform;

        for (int i = 0; i < checkpointParent.childCount; i++)
        {
                checkpoints.Add(checkpointParent.GetChild(i));
        }

        walkSpeed = 150;

        rb = GetComponent<Rigidbody>();

        rb.velocity = -transform.up * 100 * Time.deltaTime;
    }

    private void Update()
    {
        DoStates();
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
            case AlienStates.GOTOCHECKPOINT:
                GoToCheckpoint();
                break;

            case AlienStates.GOTOTOWER:
                GoToTower();
                break;

            case AlienStates.ATTACKTOWER:
                AttackTower();
                break;

            default:
                Debug.Log("No State Active!");
                break;
        }
    }

    void GoToCheckpoint()
    {
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

        transform.LookAt(new Vector3(nearestCheckpoint.position.x, transform.position.y, nearestCheckpoint.position.z));
        rb.velocity = transform.forward * walkSpeed * Time.deltaTime;

        if(Vector3.Distance(transform.position, nearestCheckpoint.position) < 3)
        {
            rb.velocity = Vector3.zero;
            currentState = AlienStates.GOTOTOWER;
        }
    }

    void GoToTower()
    {
        transform.LookAt(transform, new Vector3(target.position.x, transform.position.y, target.position.z));

        rb.velocity = transform.forward * walkSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            rb.velocity = Vector3.zero;
            currentState = AlienStates.ATTACKTOWER;
        }
    }

    void AttackTower()
    {
        // numerator that gives an attack speed or same fire rate as towers


    }
}
