using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementShow : MonoBehaviour
{
    public bool cantPlace;

    private void OnCollisionEnter(Collision collision)
    {
        cantPlace = true;
        Debug.Log("Can't place");
    }
    private void OnCollisionExit(Collision collision)
    {
        cantPlace = false;
        Debug.Log("Can place");
    }
}
