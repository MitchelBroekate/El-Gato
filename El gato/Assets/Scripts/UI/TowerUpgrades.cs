using UnityEngine;

public class TowerUpgrade : MonoBehaviour
{
    [SerializeField]
    GameObject potato, egg, corn, eggShow, cornShow;

    public bool pU = false, eU = false, cU = false;

    bool canBuyP = true, canBuyE = true, canBuyC = true;

    [SerializeField]
    BuildingShop buildingShop;

    public void PotatoUpgrade()
    {
        if (canBuyP)
        {
            if (buildingShop.money >= 800)
            {
                pU = true;
                buildingShop.potatoT = potato;

                buildingShop.money -= 800;

                canBuyP = false;
            }
            else
            {
                buildingShop.NoMannee(2);
            }
        }

    }

    public void EggUpgrade()
    {
        if (canBuyE)
        {
            if (buildingShop.money >= 1400)
            {
                eU = true;
                buildingShop.eggT = egg;
                buildingShop.showE = eggShow;

                buildingShop.money -= 1400;

                canBuyE = false;
            }
            else
            {
                buildingShop.NoMannee(2);
            }
        }

    }

    public void CornUpgrade()
    {
        if (canBuyC)
        {
            if (buildingShop.money >= 2500)
            {
                cU = true;
                buildingShop.cornT = corn;
                buildingShop.showC = cornShow;

                buildingShop.money -= 2500;

                canBuyC = false;
            }
            else
            {
                buildingShop.NoMannee(2);
            }
        }

    }
}
