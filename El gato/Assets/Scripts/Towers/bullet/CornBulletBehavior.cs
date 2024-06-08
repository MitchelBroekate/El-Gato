using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class CornBulletBehavior : BulletManager
{

    float moveSpeed = 40;
    Transform target;
    Quaternion targetRotation;
    bool enableDamage = false;
    bool enableFilght = false;
    private void Start()
    {
        target = transform.parent.GetComponent<CornTower>().nearestTarget;
        bulletDamage = 300;
        targetRotation = Quaternion.Euler(target.position.x, 0, target.position.z);
        StartCoroutine(WaitForce(4.5f));
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "enemyship")
        {
            if (enableDamage)
            {
                other.gameObject.GetComponent<UfoBehavior>().DoDamage(bulletDamage);

            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "ground")
        {
            enableDamage = true;
            Destroy(gameObject);
        }

        if(collision.transform.tag == "enemyship")
        {
            enabled = true;
            Destroy(gameObject);
        }


    }

    private void FixedUpdate()
    {
        if (enableFilght)
        {
            transform.GetComponent<Rigidbody>().velocity = transform.forward * moveSpeed;

        }

    }
    private void Update()
    {
        if (enableDamage)
        {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 3f);
        }
    }

    IEnumerator WaitForce(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        enableFilght = true;
    }
}
