using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoBehavior : MonoBehaviour
{
    #region Variables
    private int health = 100;

    public Transform target;

    [SerializeField]
    float moveSpeed = 1000;
    [SerializeField]
    float levitationSpeed = 100;
    [SerializeField]
    float claimedCowDistance = 0.25f;

    [SerializeField]
    int layerMask;

    Quaternion targetRotation;
    RaycastHit hit;

    public Transform centerObject;

    public float radius = 60;

    public float circleSpeed = 1;

    private float angle = 0;

    #endregion

    public enum UFOState
    {
        QUEUE,
        GOTOCOW,
        GETTINGCOW,
        MOVINGOUT
    }

    public UFOState uFOState;

    private void Start()
    {
        centerObject = GameObject.Find("Queue").transform;

        layerMask = LayerMask.GetMask("cow");

        Physics.IgnoreLayerCollision(2, 2);

        uFOState = UFOState.QUEUE;
    }

    private void Update()
    {
        DoState();
    }

    void DoState()
    {
        switch (uFOState)
        {
            case UFOState.QUEUE:
                Queue();
                break;
            case UFOState.GOTOCOW:
                GoToCow();
                break;
            case UFOState.GETTINGCOW:
                GettingCow();
                break;
            case UFOState.MOVINGOUT:
                MovingOut();
                break;
            default:
                break;
        }
    }

    void Queue()
    {
        GameObject.Find("Queue").GetComponent<Queue>().AddUfoToQueue(this);

        if (target != null)
        {
            uFOState = UFOState.GOTOCOW;
        }
        else
        {
            float x = centerObject.position.x + Mathf.Cos(angle) * radius;
            float z = centerObject.position.z + Mathf.Sin(angle) * radius;

            transform.position = new Vector3(x, transform.position.y, z);

            angle += circleSpeed * Time.deltaTime;

            if (angle > Mathf.PI * 2)
            {
                angle -= Mathf.PI * 2;
            }
        }

    }

    void GoToCow()
    {
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.transform == target)
            {
                transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
                uFOState = UFOState.GETTINGCOW;
            }
        }
        else
        {
            targetRotation = Quaternion.Euler(target.position.x, transform.rotation.y, target.position.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 1);
            transform.GetComponent<Rigidbody>().velocity = transform.forward * moveSpeed * Time.deltaTime;
        }
    }

    void GettingCow()
    {
        if(Vector3.Distance(target.position,transform.position) < claimedCowDistance)
        {
            GameObject.Find("CowManager").GetComponent<CowManager>().RemoveCow(target);
            uFOState = UFOState.MOVINGOUT;
        }
        else
        {
            target.GetComponent<Rigidbody>().velocity = transform.up * levitationSpeed * Time.deltaTime;
        }
    }

    void MovingOut()
    {
        transform.GetComponent<Rigidbody>().AddForce(Vector3.up * levitationSpeed * Time.deltaTime) ;

        Destroy(gameObject, 3);
    }

    public void DoDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            if(uFOState == UFOState.GETTINGCOW)
            {
                GameObject.Find("Queue").GetComponent<Queue>().AssignCow();
            }

            GameObject.Find("Scripts/PlayerInput").GetComponent<BuildingShop>().money += 50;
        }
    }
}