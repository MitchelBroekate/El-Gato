using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildState : PlayerState
{
    RaycastHit hit;
    Ray ray;
    Vector3 hitPos;

    [SerializeField]
    Camera camBuild;

    public GameObject cube;

    public override void DoUpdate()
    {
        GridBuilding();
        ray = camBuild.ScreenPointToRay(Input.mousePosition);
    }

    public override void EnableState()
    {
        base.EnableState();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public override void DisableState()
    {
        base.DisableState();
    }


    void GridBuilding()
    {
            if (Physics.Raycast(ray, out hit, 1000))
            {
                hitPos = hit.point;
            }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        Debug.Log("Why cant i instantiate");

        if (context.performed && hit.collider != null)
        {
                if (hit.transform.gameObject.tag == "ground")
                {
                    Instantiate(cube, hitPos, Quaternion.identity);
                }
        }
    }
}
