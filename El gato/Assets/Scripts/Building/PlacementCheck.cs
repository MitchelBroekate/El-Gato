using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.gameObject.tag != "ground")
        {
            GetComponent<Renderer>().material.color = new Color(1, 0, 0, 0.6f);
        }
        else
        {
            GetComponent<Renderer>().material.color = new Color(0, 1, 0, 0.6f);
        }
    }
}
