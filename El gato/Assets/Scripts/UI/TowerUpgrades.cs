using UnityEngine;

public class TowerUpgrade : MonoBehaviour
{
    [SerializeField]
    GameObject potato, egg, corn, eggShow, cornShow;

    public bool pU = false, eU = false, cU = false;

    [SerializeField]
    BuildingShop buildingShop;

    public void PotatoUpgrade()
    {
        if (buildingShop.money >= 800)
        {
            pU = true;
            buildingShop.potatoT = potato;
        }
        else
        {
            buildingShop.NoMannee(2);
        }
    }

    public void EggUpgrade()
    {
        if (buildingShop.money >= 1400)
        {
            eU = true;
            buildingShop.eggT = egg;
        }
        else
        {
            buildingShop.NoMannee(2);
        }
    }

    public void CornUpgrade()
    {
        if (buildingShop.money >= 2500)
        {
            cU = true;
            buildingShop.cornT = corn;
        }
        else
        {
            buildingShop.NoMannee(2);  
        }
    }
}
