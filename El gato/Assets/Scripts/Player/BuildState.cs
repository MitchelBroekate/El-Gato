using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildState : PlayerState
{
    #region Variables
    public RaycastHit hit;
    Ray ray;
    public Vector3 hitPos;

    [SerializeField]
    Camera camBuild;

    [SerializeField]
    GameObject canvasShop;

    public GameObject CurrentTowerToPlace;

    [SerializeField]
    BuildingShop BuildingShop;

    Color checkgreen;
    Color checkRed;
    #endregion

    private void Start()
    {
        checkgreen = new Color(0, 1, 0, 0.6f);
        checkRed = new Color(1, 0, 0, 0.6f);
    }

    public override void DoUpdate()
    {
        GridBuilding();
        ray = camBuild.ScreenPointToRay(Input.mousePosition);
        ShowWhatToPlace();
    }

    public override void EnableState()
    {
        base.EnableState();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        canvasShop.SetActive(true);
    }

    public override void DisableState()
    {
        base.DisableState();

        canvasShop.SetActive(false);
    }


    void GridBuilding()
    {
            if (Physics.Raycast(ray, out hit, 1000))
            {
                hitPos = hit.point;
            }
    }
    /// <summary>
    /// Function that checks when to show the towers for the placement check
    /// </summary>
    void ShowWhatToPlace()
    {
        if (BuildingShop.showP || BuildingShop.showC || BuildingShop.showE)
        {
            BuildingShop.showObject.transform.position = hitPos;
        }
        else
        {
            if (BuildingShop.showObject != null)
            {
                Destroy(BuildingShop.showObject);
            }

        }
        if (hit.collider != null && BuildingShop.showObject != null)
        {
            if (hit.transform.gameObject.tag == "ground")
            {
                BuildingShop.showObject.GetComponent<Renderer>().material.color = checkgreen;
            }
            else
            {
                BuildingShop.showObject.GetComponent<Renderer>().material.color = checkRed;
            }
        }

    }

    public void OnFire(InputAction.CallbackContext context)
    {

        if (context.performed && hit.collider != null && CurrentTowerToPlace != null)
        {
            if (hit.transform.gameObject.tag == "ground")
            {
                if (BuildingShop.showObject != null)
                {
                    BuildingShop.showP = false;
                    BuildingShop.showC = false;
                    BuildingShop.showE = false;
                }

                Instantiate(CurrentTowerToPlace, hitPos, Quaternion.identity);

                CurrentTowerToPlace = null;
            }
        }

        if (context.performed && hit.collider != null)
        {
            if (BuildingShop.SellModeSwitch)
            {
                if (hit.transform.gameObject.tag == "potato")
                {
                    Destroy(hit.transform.gameObject);
                }

                if (hit.transform.gameObject.tag == "egg")
                {
                    Destroy(hit.transform.gameObject);
                }

                if (hit.transform.gameObject.tag == "corn")
                {
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }
}
