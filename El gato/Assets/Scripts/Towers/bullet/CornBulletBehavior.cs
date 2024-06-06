using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class CornBulletBehavior : BulletManager
{

    float moveSpeed = 1500;
    Transform target;
    Quaternion targetRotation;
    private void Start()
    {
        bulletDamage = 300;

        target = transform.parent.GetComponent<CornTower>().nearestTarget;
        targetRotation = Quaternion.Euler(target.position.x, target.position.y, target.position.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
            if (collision.transform.tag == "enemyship")
            {
                collision.gameObject.GetComponent<UfoBehavior>().health -= bulletDamage;
            }

            Destroy(gameObject);
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.transform.tag == "enemyship")
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    private void Update()
    {
            Destroy(gameObject, 50);

    }

    //private void FixedUpdate()
    //{
    //    if (start)
    //    {
    //        transform.GetComponent<Rigidbody>().velocity = transform.forward * moveSpeed;

    //        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 1f);
    //    }
    //}
}
