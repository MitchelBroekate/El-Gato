using UnityEngine;

public class BuildingShop : MonoBehaviour
{
    #region Variables
    [SerializeField]
    BuildState buildState;

    [SerializeField]
    GameObject potatoT, CornT, eggT;

    public bool SellModeSwitch = false;
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

                buildState.CurrentTowerToPlace = CornT;

                break;
        }
    }

    /// <summary>
    /// assigns the "Potato Tower" for placement
    /// </summary>
    public void TowerButton1()
    {
        TowerToShow = TowerIndicators.Tower1;

        TowerTypeIndicator();
    }

    /// <summary>
    /// assigns the "Egg Tower" for placement
    /// </summary>
    public void TowerButton2()
    {
        TowerToShow = TowerIndicators.Tower2;

        TowerTypeIndicator();
    }

    /// <summary>
    /// assigns the "Corn Tower" for placement
    /// </summary>
    public void TowerButton3()
    {
        TowerToShow = TowerIndicators.Tower3;

        TowerTypeIndicator();
    }

    public void TowerSellMode()
    {
        buildState.CurrentTowerToPlace = null;

        SellModeSwitch = !SellModeSwitch;
    }
}
