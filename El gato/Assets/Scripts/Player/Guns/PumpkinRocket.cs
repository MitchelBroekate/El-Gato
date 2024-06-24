using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinRocket : MonoBehaviour
{
    bool enableDamage = false;

    private void Start()
    {
        Physics.IgnoreLayerCollision(10, 8);

        Destroy(gameObject, 10);
    }

    private void OnCollisionEnter(Collision collision)
    {
        enableDamage = true;
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (enableDamage)
        {
            if (other.transform.tag == "Alien")
            {
                other.gameObject.GetComponent<AlienBehaviour>().DoDamage(500);
            }

            Destroy(gameObject);

        }
    }

}
