using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggTower : TowerManager
{


    [System.Obsolete]
    private void Start()
    {
        rangeScale = 20;
        health = 200;
    }

    private void Update()
    {
        GetComponent<SphereCollider>().radius = rangeScale;
    }
}
