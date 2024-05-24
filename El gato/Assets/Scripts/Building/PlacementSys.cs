using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSys : MonoBehaviour
{
    public bool cantplace;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            if(collision.gameObject.tag == "potato" || collision.gameObject.tag == "egg" || collision.gameObject.tag == "corn")
            {
                cantplace = true;
            }
            else
            {
                cantplace= false;
            }

            cantplace = false;
        }
    }
}
