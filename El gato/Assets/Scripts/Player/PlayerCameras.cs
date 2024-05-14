using UnityEngine;

public class PlayerCameras : MonoBehaviour
{
    #region camera switch variables
    [SerializeField]
    GameObject droneCam, towerCam;

    public bool buildCam = false;
    #endregion

    public void SwitchCams()
    {
        #region if statement and else for functionalitie when camera switch button is pressed
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
        #endregion
    }
}
