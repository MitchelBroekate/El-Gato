using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildState : PlayerState
{
    #region Variables

    [Header("Raycast")]
    public RaycastHit hit;
    public Vector3 hitPos;
    Ray ray;


    [Header("Camera")]
    [SerializeField]
    Camera camBuild;

    [Header("Canvas")]
    [SerializeField]
    GameObject canvasShop;

    [Header("Tower To Place")]
    public GameObject CurrentTowerToPlace;

    [Header("Build Scripts")]
    [SerializeField]
    BuildingShop BuildingShop;

    [Header("Grid")]
    [SerializeField]
    Grid grid;

    [SerializeField]
    GameObject gridIndicator;

    [SerializeField]
    GameObject gridPlane;


    Vector3Int gridposition;
    Vector3Int placementList;

    [Header("Placement Location")]
    public Vector3 placementLocation;

    [Header("Tower Parent")]
    [SerializeField]
    GameObject towerParent;

    GameObject tower;

    List<Vector3Int> towersInGrid = new List<Vector3Int>();
    #endregion

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
        //places the chosen tower on a chosen grid location if there isn't already a tower in that grid
        if (context.performed && hit.collider != null && CurrentTowerToPlace != null && !towersInGrid.Contains(placementList) && !BuildingShop.sellModeSwitch)
        {

            if (BuildingShop.showObject != null)
            {
                BuildingShop.showP = false;
                BuildingShop.showC = false;
                BuildingShop.showE = false;
            }

            tower = Instantiate(CurrentTowerToPlace, placementLocation, Quaternion.identity);
            tower.transform.parent = towerParent.transform;

            CurrentTowerToPlace = null;

            towersInGrid.Add(placementList);
        }

        //if the grid contains a tower it disables the current build mode
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

        //Sells, destroys and makes the grid available for a new tower
        if (context.performed && hit.collider != null)
        {
            if (BuildingShop.sellModeSwitch)
            {
                if (towersInGrid.Contains(placementList))
                {
                    if (hit.transform.gameObject.tag == "potato")
                    {
                        towersInGrid.Remove(placementList);
                        Destroy(hit.transform.gameObject);
                    }

                }

                if (towersInGrid.Contains(placementList))
                {
                    if (hit.transform.gameObject.tag == "egg")
                    {
                        towersInGrid.Remove(placementList);
                        Destroy(hit.transform.gameObject);
                    }
                }

                if (towersInGrid.Contains(placementList))
                {
                    if (hit.transform.gameObject.tag == "corn")
                    {
                        towersInGrid.Remove(placementList);
                        Destroy(hit.transform.gameObject);
                    }
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
