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

    [SerializeField]
    Grid grid;
    [SerializeField]
    GameObject gridIndicator, gridPlane;

    Vector3Int gridposition;
    Vector3Int placementList;

    public Vector3 placementLocation;



    List<Vector3Int> towersInGrid = new List<Vector3Int>();
    #endregion

    //Start to set the colors for placeable check
    private void Start()
    {
        checkgreen = new Color(0, 1, 0, 0.6f);
        checkRed = new Color(1, 0, 0, 0.6f);
    }

    //Updates the corresponding functions and sets the var ray to the mouse position
    public override void DoUpdate()
    {
        GridBuilding();
        ray = camBuild.ScreenPointToRay(Input.mousePosition);
        ShowWhatToPlace();
        GridSelection();
    }

    //Sets the building mode active with indicators, the grid and the cursor state
    public override void EnableState()
    {
        base.EnableState();

        gridIndicator.SetActive(true);
        gridPlane.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        canvasShop.SetActive(true);
    }

    //Sets the building mode inactive with indicators, the grid and the cursor state
    public override void DisableState()
    {
        base.DisableState();

        gridIndicator.SetActive(false);
        gridPlane.SetActive(false);

        canvasShop.SetActive(false);
    }

    /// <summary>
    /// Makes a raycast with a position for the placement of the towers
    /// </summary>
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
            BuildingShop.showObject.transform.position = placementLocation;
        }
        else
        {
            if (BuildingShop.showObject != null)
            {
                Destroy(BuildingShop.showObject);
            }

        }
    }

    /// <summary>
    /// When the corresponding button is pressed this function will try to instantiate a tower or in sell mode, deletes the tower
    /// </summary>
    /// <param name="context"></param>
    public void OnFire(InputAction.CallbackContext context)
    {

        if (context.performed && hit.collider != null && CurrentTowerToPlace != null && !towersInGrid.Contains(placementList))
        {

            if (BuildingShop.showObject != null)
            {
                BuildingShop.showP = false;
                BuildingShop.showC = false;
                BuildingShop.showE = false;
            }

            Instantiate(CurrentTowerToPlace, placementLocation, Quaternion.identity);

            CurrentTowerToPlace = null;

            towersInGrid.Add(placementList);
        }

        if(towersInGrid.Contains(placementList))
        {
            if (BuildingShop.showObject != null && CurrentTowerToPlace != null)
            {
                BuildingShop.showP = false;
                BuildingShop.showC = false;
                BuildingShop.showE = false;
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

                    BuildingShop.SellModeSwitch = false;
                }

                if (hit.transform.gameObject.tag == "egg")
                {
                    Destroy(hit.transform.gameObject);

                    BuildingShop.SellModeSwitch = false;
                }

                if (hit.transform.gameObject.tag == "corn")
                {
                    Destroy(hit.transform.gameObject);

                    BuildingShop.SellModeSwitch = false;
                }
            }
        }
    }

    /// <summary>
    /// Sets the tower indicator and the grid indicator to the correct grid for placement
    /// </summary>
    void GridSelection()
    {
        gridposition = grid.WorldToCell(hitPos);
        gridIndicator.transform.position = grid.CellToWorld(gridposition);

        placementList = Vector3Int.FloorToInt(grid.CellToWorld(gridposition));

        placementLocation = grid.CellToWorld(gridposition);

        placementLocation.x += 2.5f;
        placementLocation.z += 2.5f;
    }
}
