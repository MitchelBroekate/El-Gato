using UnityEngine;

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

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            buildCam = false;

            Debug.Log("Switch");
        }
        else
        {
            droneCam.SetActive(true);
            towerCam.SetActive(false);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            buildCam = true;
            Debug.Log("Switch");
        }
    }
}
