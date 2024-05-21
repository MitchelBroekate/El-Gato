using UnityEngine;

public class BuildingShop : MonoBehaviour
{
    #region Variables
    [SerializeField]
    BuildState buildState;

    [SerializeField]
    GameObject potatoT, cornT, eggT;

    public bool SellModeSwitch = false;

    public Transform towerHidePlace;

    public GameObject towershowP, towershowC, towershowE;

    public bool showP, showC, showE;

    public GameObject showObject;

    #endregion

    /// <summary>
    /// Enum for states of shop building
    /// </summary>
    public enum TowerIndicators
    {
        Tower1,
        Tower2,
        Tower3
    }

    public TowerIndicators TowerToShow;

    /// <summary>
    /// Swtich case for GameObject assignment for tower placement
    /// </summary>
    void TowerTypeIndicator()
    {
        switch (TowerToShow)
        {
            case TowerIndicators.Tower1:
                Debug.Log("Tower1");

                buildState.CurrentTowerToPlace = potatoT;

                break;
            case TowerIndicators.Tower2:
                Debug.Log("Tower2");

                buildState.CurrentTowerToPlace = eggT;

                break;
            case TowerIndicators.Tower3:
                Debug.Log("Tower3");

                buildState.CurrentTowerToPlace = cornT;

                break;
        }
    }

    /// <summary>
    /// assigns the "Potato Tower" for placement
    /// </summary>
    public void TowerButton1()
    {
        if (showObject != null)
        {
            Destroy(showObject);
        }
        TowerToShow = TowerIndicators.Tower1;
        showP = true;
        showObject = Instantiate(towershowP, buildState.hitPos, Quaternion.identity);

        TowerTypeIndicator();
    }

    /// <summary>
    /// assigns the "Egg Tower" for placement
    /// </summary>
    public void TowerButton2()
    {
        if (showObject != null)
        {
            Destroy(showObject);
        }
        TowerToShow = TowerIndicators.Tower2;
        showE = true;
        showObject = Instantiate(towershowE, buildState.hitPos, Quaternion.identity);

        TowerTypeIndicator();
    }

    /// <summary>
    /// assigns the "Corn Tower" for placement
    /// </summary>
    public void TowerButton3()
    {
        if (showObject != null)
        {
            Destroy(showObject);
        }
        TowerToShow = TowerIndicators.Tower3;
        showC = true;
        showObject = Instantiate(towershowC, buildState.hitPos, Quaternion.identity);

        TowerTypeIndicator();
    }

    public void TowerSellMode()
    {
        buildState.CurrentTowerToPlace = null;

        SellModeSwitch = !SellModeSwitch;
    }
}
