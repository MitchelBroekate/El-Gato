using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornTower : TowerManager
{

    [System.Obsolete]
    private void Start()
    {
        rangeScale = 30;
        health = 300;
    }

    private void Update()
    {
        GetComponent<SphereCollider>().radius = rangeScale;
    }


}
