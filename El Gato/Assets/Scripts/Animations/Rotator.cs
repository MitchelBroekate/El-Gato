using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Vector3 rotateDir;
    public float rotateSpeed;
    public bool shoot = false;

    private void Start()
    {
        shoot = true;
    }

    void Update()
    {
        if (shoot)
        {
            transform.Rotate(rotateDir * Time.deltaTime*rotateSpeed);
        }
    }
}
