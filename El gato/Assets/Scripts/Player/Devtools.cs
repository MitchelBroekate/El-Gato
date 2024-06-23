using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Devtools : MonoBehaviour
{
    public void AddMoney(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GetComponent<BuildingShop>().money += 100;
        }

    }

    public void NextWave(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GetComponent<EnemyManager>().currentWave++;
        }

    }
}
