using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggBulletBehavior : BulletManager
{
    private void Start()
    {
        bulletDamage = 5;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "enemyship")
        {
            collision.gameObject.GetComponent<UfoBehaviour>().DoDamage(bulletDamage);
        }

        Destroy(gameObject);

    }

    private void Update()
    {
        Destroy(gameObject, 2);
    }
}
