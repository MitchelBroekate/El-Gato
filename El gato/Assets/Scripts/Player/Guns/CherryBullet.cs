using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryBullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Alien")
        {
            collision.gameObject.GetComponent<AlienBehaviour>().DoDamage(100);
        }

        Destroy(gameObject);
    }

    private void Start()
    {
        Destroy(gameObject, 10);
    }
}
