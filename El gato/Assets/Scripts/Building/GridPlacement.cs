using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridPlacement : MonoBehaviour
{
    PlayerCameras playerCameras;
    RaycastHit hit;

    void Start()
    {
        playerCameras = GetComponent<PlayerCameras>();
    }

    private void Update()
    {
        GridBuilding();
    }

    void GridBuilding()
    {
        if (playerCameras.buildCam)
        {
            if (Physics.Raycast(Input.mousePosition, Vector3.forward, out hit, 1000))
            {

            }
        }
    }
}
