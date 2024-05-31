using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoTower : TowerManager
{
    #region Variables
    [SerializeField]
    Transform rotateY;
    [SerializeField]
    Transform rotateX;

    [SerializeField]
    List<Transform> allTargets = new();
    [SerializeField]
    Transform nearestTarget;

    #endregion

    [System.Obsolete]
    private void Start()
    {
        rotateY = transform.FindChild("RotateY");
        rotateX = rotateY.FindChild("RotateX");
        rangeScale = 12;
        health = 150;
    }

    private void Update()
    {
        //For testing/balancing
        GetComponent<SphereCollider>().radius = rangeScale;

        Targeting();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.tag == "enemyship")
        {
            if (!allTargets.Contains(other.transform))
            {
                allTargets.Add(other.transform);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.gameObject.tag == "enemyship")
        {
            if (allTargets.Contains(other.transform))
            {
                allTargets.Remove(other.transform);
            }
        }
    }

    void Targeting()
    {
        float distance;
        float nearestDistance = float.MaxValue;

        for (int i = 0; i < allTargets.Count; i++)
        {
            distance = Vector3.Distance(transform.position, allTargets[i].position);
            if(distance < nearestDistance)
            {
                nearestTarget = allTargets[i];
                nearestDistance = distance; 
            }
        }
        if (nearestTarget != null)
        {
            rotateY.transform.LookAt(new Vector3(nearestTarget.position.x, rotateY.transform.position.y, nearestTarget.position.z));
            rotateX.transform.LookAt(nearestTarget.position);
        }

    }
}
