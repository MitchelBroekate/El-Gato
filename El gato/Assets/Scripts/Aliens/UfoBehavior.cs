using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoBehavior : MonoBehaviour
{
    public int health;
    public bool dead;

    private void Start()
    {
        health = 100;
    }
    private void Update()
    {
        if (health <= 0)
        {
            dead = true;
        }
    }
}
