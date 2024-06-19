using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBehaviour : MonoBehaviour
{
    Rigidbody rb;

    public Transform target;

    List<Transform> checkpoints;

    public Transform nearestCheckpoint;

    float walkSpeed;

    AlienStates currentState;

    enum AlienStates
    {
        GOTOCHECKPOINT,
        GOTOTOWER,
        ATTACKTOWER
    }

    private void Start()
    {
        walkSpeed = 15;

        rb = GetComponent<Rigidbody>();

        GoToCheckpoint();
    }

    private void Update()
    {
        DoStates();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "ground")
        {
            rb.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;

            currentState = AlienStates.GOTOCHECKPOINT;
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

        if (Vector3.Distance(transform.position, nearestCheckpoint.position) < 0.5f)
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
