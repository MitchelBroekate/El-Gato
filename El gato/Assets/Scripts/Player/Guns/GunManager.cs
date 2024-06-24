using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunManager : MonoBehaviour
{
    [SerializeField]
    BuildingShop shop;

    [SerializeField]
    GameObject cherryGun, pumpkinGun;

    public void Cherry(InputAction.CallbackContext context)
    {
        if (!cherryGun.activeInHierarchy)
        {
            if (context.performed)
            {
                pumpkinGun.SetActive(false);
                cherryGun.SetActive(true);
            }
        }
        else
        {
            print("Cherry Weapon Active Surr");
        }
    }

    public void Pumpkin(InputAction.CallbackContext context)
    {
        if (shop.weaponBought)
        {
            if (!pumpkinGun.activeInHierarchy)
            {
                if (context.performed)
                {
                    cherryGun.SetActive(false);
                    pumpkinGun.SetActive(true);
                }
            }
            else
            {
                print("Pumpkin Weapon Active Surr");
            }
        }
        else
        {
            print("No Weapon Surr");
        }
    }
}
