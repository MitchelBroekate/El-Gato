using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoBehavior : MonoBehaviour
{
    public int health;
    public bool dead;

    [SerializeField]
    GameObject cowParent;

    [SerializeField]
    Transform target;

    [SerializeField]
    float moveSpeed = 1000;
    [SerializeField]
    float suckSpeed = 100;

    int check = 0;

    bool targetCheck = true;

    public bool inTurretRange;

    public bool targeted;

    RaycastHit hit;

    int layerMask;
    private void Start()
    {
        health = 100;
        cowParent = GameObject.Find("lives");

        layerMask = LayerMask.GetMask("Ignore Raycast");
    }
    private void Update()
    {
        if (health <= 0)
        {
            if (inTurretRange)
            {
                if (check < cowParent.transform.childCount)
                {
                    cowParent.transform.GetChild(check).GetComponent<CowCheck>().available = true;
                }

                dead = true;
            }
            else
            {
                if (check < cowParent.transform.childCount)
                {
                    cowParent.transform.GetChild(check).GetComponent<CowCheck>().available = true;
                }

                Destroy(gameObject);
            }
        }

        CowTargeting();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "life")
        {
            Destroy(collision.transform.gameObject);

            if (inTurretRange)
            {
                dead = true;
            }
            else
            {
                Destroy(gameObject);
            }

        }
    }

    private void CowTargeting()
    {
        if (targetCheck)
        {
            if (check >= cowParent.transform.childCount)
            {
                target = GameObject.Find("11").transform;
                check = 0;
            }
            else
            {
                if (cowParent.transform.GetChild(check).GetComponent<CowCheck>().available)
                {
                    target = null;
                    target = cowParent.transform.GetChild(check);

                    target.GetComponent<CowCheck>().available = false;

                    targetCheck = false;
                }
                else
                {
                    check++;
                }
            }

        }


        if (target != null)
        {
            Debug.Log("1");
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
            if (Physics.Raycast(transform.position, -Vector3.up, out hit, 1000, layerMask))
            {
                Debug.Log("2");
                if (hit.collider != null)
                {
                    Debug.Log("3");
                    if (hit.transform.position == target.position)
                    {
                        Debug.Log("4");
                        GetComponent<Rigidbody>().velocity = Vector3.zero;

                        target.GetComponent<Rigidbody>().velocity = transform.up * suckSpeed * Time.deltaTime;
                    }
                }
            }
            else
            {
                Debug.Log("5");
                GetComponent<Rigidbody>().velocity = transform.forward * moveSpeed * Time.deltaTime;
            }
        }

    }
}
