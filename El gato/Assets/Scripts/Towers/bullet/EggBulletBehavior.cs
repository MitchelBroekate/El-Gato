using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggBulletBehavior : BulletManager
{
    private void Start()
    {
        bulletDamage = 10;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "enemyship")
        {
            collision.gameObject.GetComponent<UfoBehavior>().health -= bulletDamage;
        }

        Destroy(gameObject);

    }

    private void Update()
    {
        Destroy(gameObject, 5);
    }
}
