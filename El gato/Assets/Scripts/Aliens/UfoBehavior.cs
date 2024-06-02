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

    RaycastHit hit;
    private void Start()
    {
        health = 100;
        cowParent = GameObject.Find("lives");
        
    }
    private void Update()
    {
        if (health <= 0)
        {
            if (inTurretRange)
            {
                cowParent.transform.GetChild(check).GetComponent<CowCheck>().available = true;

                dead = true;
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
            if (cowParent.transform.GetChild(check).GetComponent<CowCheck>().available)
            {
                target = cowParent.transform.GetChild(check);

                cowParent.transform.GetChild(check).GetComponent<CowCheck>().available = false;

                targetCheck = false;
            }
            else
            {
                check++;
            }
        }


        if (target != null)
        {
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
            if (Physics.Raycast(transform.position, -Vector3.up, out hit, 1000))
            {
                if (hit.collider != null)
                {
                    if (hit.transform.position == target.position)
                    {
                        GetComponent<Rigidbody>().velocity = Vector3.zero;

                        target.GetComponent<Rigidbody>().velocity = transform.up * suckSpeed * Time.deltaTime;
                    }
                    else
                    {
                        GetComponent<Rigidbody>().velocity = transform.forward * moveSpeed * Time.deltaTime;
                    }
                }

            }
        }

    }
}
