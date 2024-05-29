using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoTower : TowerManager
{
    #region Variables
    Transform rotateY;
    Transform rotateX;

    [SerializeField]
    GameObject target;

    bool firstTarget = false;
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

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Trigger");
        if (other.transform.gameObject.tag == "enemyship" && !firstTarget)
        {
            target = other.transform.gameObject;
            firstTarget = true;
        }
    }

    void Targeting()
    {
        if (firstTarget)
        {
            rotateY.transform.LookAt(new Vector3(target.transform.position.x, rotateY.transform.position.y, target.transform.position.z));
            rotateX.transform.LookAt(target.transform.position);
        }

        if (target = null)
        {
            firstTarget = false;
        }

    }
}
