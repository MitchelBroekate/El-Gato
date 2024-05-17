using UnityEngine;

public class BuildingShop : MonoBehaviour
{

    public enum TowerIndicators
    {
        Tower1,
        Tower2,
        Tower3
    }

    public TowerIndicators TowerToShow;

    void TowerTypeIndicator()
    {
        switch (TowerToShow)
        {
            case TowerIndicators.Tower1:
                Debug.Log(TowerToShow.ToString());
                break;
            case TowerIndicators.Tower2:
                Debug.Log(TowerToShow.ToString());
                break;
            case TowerIndicators.Tower3:
                Debug.Log(TowerToShow.ToString());
                break;
        }
    }


    public void TowerButton1()
    {
        TowerToShow = TowerIndicators.Tower1;
    }

    public void TowerButton2()
    {
        TowerToShow = TowerIndicators.Tower2;
    }

    public void TowerButton3()
    {
        TowerToShow = TowerIndicators.Tower3;
    }
}
