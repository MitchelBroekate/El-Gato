using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildState : PlayerState
{
    #region Variables

    [Header("Raycast Variables")]
    public RaycastHit hit;
    public Vector3 hitPos;
    Ray ray;


    [Header("Camera")]
    [SerializeField]
    Camera camBuild;

    [Header("Canvas")]
    [SerializeField]
    GameObject shop;

    [Header("Tower To Place")]
    public GameObject CurrentTowerToPlace;

    [Header("Shop Scripts")]
    [SerializeField]
    BuildingShop BuildingShop;

    [Header("Grid Objects")]
    [SerializeField]
    Grid grid;

    [SerializeField]
    GameObject gridIndicator;

    [SerializeField]
    GameObject gridPlane;


    Vector3Int gridposition;
    Vector3Int placementList;

    [Header("Tower Placement Location")]
    public Vector3 placementLocation;

    [Header("Tower Instantiate Parent")]
    [SerializeField]
    GameObject towerParent;

    GameObject tower;

    List<Vector3Int> towersInGrid = new List<Vector3Int>();

    [Header("Raycast Ignore")]
    [SerializeField]
    LayerMask maskLayer;

    [SerializeField]
    TowerUpgrade towerUpgrade;
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

        shop.SetActive(true);
    }

    //Sets the building mode inactive with indicators, the grid and the cursor state
    public override void DisableState()
    {
        base.DisableState();

        gridIndicator.SetActive(false);
        gridPlane.SetActive(false);

        shop.SetActive(false);

        if (BuildingShop.showP || BuildingShop.showC || BuildingShop.showE)
        {
            Destroy(BuildingShop.showObject);

            BuildingShop.showP = false;
            BuildingShop.showC = false;
            BuildingShop.showE = false;

            BuildingShop.showObject = null;
            CurrentTowerToPlace = null;
        }

        if (BuildingShop.sellModeSwitch)
        {
            BuildingShop.sellModeSwitch = false;
        }
    }

    /// <summary>
    /// Makes a raycast with a position for the placement of the towers
    /// </summary>
    void GridBuilding()
    {
            if (Physics.Raycast(ray, out hit, 1000, maskLayer))
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
            if (BuildingShop.showObject != null)
            {
                BuildingShop.showObject.transform.position = placementLocation;
            }
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
    [System.Obsolete]
    public void OnFire(InputAction.CallbackContext context)
    {
        //places the chosen tower on a chosen grid location if there isn't already a tower in that grid
        if (context.performed && hit.collider != null && CurrentTowerToPlace != null && !towersInGrid.Contains(placementList) && !BuildingShop.sellModeSwitch)
        {
            if (BuildingShop.showP)
            {
                BuildingShop.money -= 200;
            }

            if (BuildingShop.showC)
            {
                BuildingShop.money -= 500;
            }

            if (BuildingShop.showE)
            {
                BuildingShop.money -= 350;
            }




            tower = Instantiate(CurrentTowerToPlace, placementLocation, Quaternion.identity);
            tower.transform.parent = towerParent.transform;

            if (towerUpgrade.pU)
            {
                if (tower != null)
                {
                    if (BuildingShop.showP)
                    {
                        tower.transform.FindChild("Base").GetComponent<PotatoTower>().AddDamage(2);
                    }
                    
                }
            }
            if (towerUpgrade.cU)
            {
                if (BuildingShop.showC)
                {
                    if (tower != null)
                    {
                        tower.transform.FindChild("Base").GetComponent<CornTower>().AddDamage(2);
                    }
                }

            }
            if (towerUpgrade.eU)
            {
                if (BuildingShop.showE)
                {
                    if (tower != null)
                    {
                        Transform towerChild = tower.transform.FindChild("GatllingGun");
                        towerChild.FindChild("Base").GetComponent<EggTower>().AddDamage(2);
                    }
                }

            }

            if (BuildingShop.showObject != null)
            {
                BuildingShop.showP = false;
                BuildingShop.showC = false;
                BuildingShop.showE = false;
            }

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
                        BuildingShop.money += 100;
                    }

                }

                if (towersInGrid.Contains(placementList))
                {
                    if (hit.transform.gameObject.tag == "egg")
                    {
                        towersInGrid.Remove(placementList);
                        Destroy(hit.transform.gameObject);
                        BuildingShop.money += 175;
                    }
                }

                if (towersInGrid.Contains(placementList))
                {
                    if (hit.transform.gameObject.tag == "corn")
                    {
                        towersInGrid.Remove(placementList);
                        Destroy(hit.transform.gameObject);
                        BuildingShop.money += 250;
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
