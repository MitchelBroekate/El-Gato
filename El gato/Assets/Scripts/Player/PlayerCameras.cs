using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameras : MonoBehaviour
{
    [SerializeField]
    public GameObject droneCam, towerCam;

    bool buildCam = false;
    public void SwitchCams()
    {
        if (buildCam == true)
        {
            towerCam.SetActive(true);
            droneCam.SetActive(false);
            buildCam = false;
        }
        else
        {
            droneCam.SetActive(true);
            towerCam.SetActive(false);
            buildCam = true;
        }
    }
}
