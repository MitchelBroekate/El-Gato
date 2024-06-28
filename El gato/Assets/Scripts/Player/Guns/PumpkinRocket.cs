using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinRocket : MonoBehaviour
{
    bool enableDamage = false;

    private void Start()
    {
        Physics.IgnoreLayerCollision(10, 8);
        Physics.IgnoreLayerCollision(10, 2);

        Destroy(gameObject, 4);
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
                if (other.gameObject.GetComponent<AlienBehaviour>().currentState != AlienBehaviour.AlienStates.DYING )
                {
                other.gameObject.GetComponent<AlienBehaviour>().DoDamage(500);

                }
            }

            Destroy(gameObject);

        }
    }

}
