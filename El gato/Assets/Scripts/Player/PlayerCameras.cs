using UnityEngine;

public class PlayerCameras : MonoBehaviour
{
    #region camera switch variables
    [SerializeField]
    GameObject droneCam, towerCam;

    public bool buildCam = false;
    #endregion

    private void Update()
    {
        
    }

    //Function for switching between top and FPS camera
    public void SwitchCams()
    {
        towerCam.GetComponent<FPSKeybinds>().EnableInput(buildCam);
            //droneCam.SetActive(buildCam);
            //towerCam.SetActive(!buildCam);
        if (buildCam == true) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        buildCam = !buildCam;

        Debug.Log("Switch");
    }
}
