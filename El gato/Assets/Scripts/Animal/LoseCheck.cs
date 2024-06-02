using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseCheck : MonoBehaviour
{
    void CheckIfLost()
    {
        if (transform.childCount <= 0)
        {
            //loseScreen active
        }
    }
}
