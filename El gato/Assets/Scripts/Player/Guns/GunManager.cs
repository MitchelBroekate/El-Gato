using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunManager : MonoBehaviour
{
    BuildingShop shop;

    [SerializeField]
    GameObject cherryGun, pumpkinGun;

    public void Cherry(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!cherryGun.activeInHierarchy)
            {
                pumpkinGun.SetActive(false);
                cherryGun.SetActive(true);
            }
        }
    }

    public void Pumpkin(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            if (shop.weaponBought)
            {
                if (!pumpkinGun.activeInHierarchy)
                {
                    cherryGun.SetActive(false);
                    pumpkinGun.SetActive(true);
                }
            }
        }
    }
}
