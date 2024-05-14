using UnityEngine;
using UnityEngine.InputSystem;

public class GridPlacement : MonoBehaviour
{
    PlayerCameras playerCameras;

    RaycastHit hit;
    Ray ray;
    Vector3 hitPos;

    public Camera cam;
    public GameObject cube;

    void Start()
    {
        playerCameras = GetComponent<PlayerCameras>();
    }

    private void Update()
    {
        GridBuilding();
        ray = cam.ScreenPointToRay(Input.mousePosition);
    }

    void GridBuilding()
    {
        if (playerCameras.buildCam)
        {
            if (Physics.Raycast(ray, out hit, 1000))
            {
                hitPos = hit.point;
            }
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed && hit.collider != null)
        {
            if (playerCameras.buildCam)
            {
                if (hit.transform.gameObject.tag == "ground")
                {
                    Instantiate(cube, hitPos, Quaternion.identity);
                }
            }
        }
    }
}
