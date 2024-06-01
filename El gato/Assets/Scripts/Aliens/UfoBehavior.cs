using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoBehavior : MonoBehaviour
{
    public int health;
    public bool dead;
    GameObject cowParent;
    Transform nearestTarget;

    private void Start()
    {
        health = 100;
        cowParent = GameObject.Find("lives");
        
    }
    private void Update()
    {
        if (health <= 0)
        {
            dead = true;
        }

        CowTargeting();
    }

    private void CowTargeting()
    {
        float distance;
        float nearestDistance = float.MaxValue;

        for (int i = 0; i < cowParent.transform.childCount; i++)
        {
            distance = Vector3.Distance(transform.position, cowParent.transform.GetChild(i).position);
            if (distance < nearestDistance)
            {
                nearestTarget = cowParent.transform.GetChild(i);
                nearestDistance = distance;
            }
        }
    }
}
