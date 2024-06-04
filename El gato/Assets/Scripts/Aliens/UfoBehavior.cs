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
    GameObject towerParent;

    [SerializeField]
    Transform target;

    [SerializeField]
    float moveSpeed = 1000;
    [SerializeField]
    float suckSpeed = 100;

    int check = 0;

    bool targetCheck = true;
    RaycastHit hit;

    [SerializeField]
    int layerMask;

    List<GameObject> towerTarget = new();

    List<Transform> towers = new();

    private void Start()
    {
        health = 100;
        cowParent = GameObject.Find("lives");

        layerMask = LayerMask.GetMask("cow");

        towerParent = GameObject.Find("TowersParent");
    }
    private void Update()
    {
        if (health <= 0)
        {
            if (check < cowParent.transform.childCount)
            {
                cowParent.transform.GetChild(check).GetComponent<CowCheck>().available = true;
            }

            dead = true;
        }

        CowTargeting();
        TowerDeathCheck();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "life")
        {
            Destroy(collision.transform.gameObject);

                dead = true;
        }
    }

    private void CowTargeting()
    {
        if (targetCheck)
        {
            if (check >= cowParent.transform.childCount)
            {
                check = 0;
            }
            else
            {
                if (cowParent.transform.GetChild(check).GetComponent<CowCheck>().available)
                {
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
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));

            if (Physics.Raycast(transform.position, -Vector3.up, out hit, 1000, layerMask))
            {
                if (hit.collider != null)
                {
                    if (hit.transform.position == target.position)
                    {
                        GetComponent<Rigidbody>().velocity = Vector3.zero;

                        target.GetComponent<Rigidbody>().velocity = transform.up * suckSpeed * Time.deltaTime;
                    }
                }
            }
            else
            {
                GetComponent<Rigidbody>().velocity = transform.forward * moveSpeed * Time.deltaTime;
            }
        }

    }

    void TowerDeathCheck()
    {
        for (int i = 0; i < towerParent.transform.childCount; i++)
        {
            if (towerParent.transform.tag == "potato")
            {
                if (towerParent.transform.GetChild(i).GetComponent<PotatoTower>().allTargets.Contains(transform))
                {
                    if (!towers.Contains(towerParent.transform.GetChild(i)))
                    {
                        towers.Add(towerParent.transform.GetChild(i));
                    }

                }
            }

            if (towerParent.transform.tag == "egg")
            {
                if (towerParent.transform.GetChild(i).GetComponent<EggTower>().allTargets.Contains(transform))
                {
                    if (!towers.Contains(towerParent.transform.GetChild(i)))
                    {
                        towers.Add(towerParent.transform.GetChild(i));
                    }
                }
            }

            //if (towerParent.transform.tag == "corn")
            //{
            //    if (towerParent.transform.GetChild(i).GetComponent<CornTower>().allTargets.Contains(transform))
            //    {
            //      if (!towers.Contains(towerParent.transform.GetChild(i)))
            //      {
            //          towers.Add(towerParent.transform.GetChild(i));
            //      }
            //    }
            //}
        }

        if (dead)
        {
            for (int i = 0; i < towerParent.transform.childCount; i++)
            {
                if (towers.Contains(towerParent.transform.GetChild(i)))
                {
                    if (towerParent.transform.tag == "potato")
                    {
                        towerParent.transform.GetChild(i).GetComponent<PotatoTower>().allTargets.Remove(transform);
                    }

                    if (towerParent.transform.tag == "egg")
                    {
                        towerParent.transform.GetChild(i).GetComponent<PotatoTower>().allTargets.Remove(transform);
                    }

                    //if (towerParent.transform.tag == "corn")
                    //{
                    //    towerParent.transform.GetChild(i).GetComponent<PotatoTower>().allTargets.Remove(transform);
                    //}
                }
            }

            GameObject.Find("Scripts/PlayerInput").GetComponent<BuildingShop>().money += 50;
            Destroy(gameObject);
        }
    }
}
