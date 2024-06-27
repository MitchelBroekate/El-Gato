using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Devtools : MonoBehaviour
{

    public Transform enemyParent;
    public Transform cowParent;

    public CowManager cowManager;
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

    public void KillSelf(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            for (int i = 0; i < cowParent.childCount; i++)
            {
                cowManager.RemoveCow(cowParent.GetChild(i));
                Destroy(cowParent.GetChild(i).gameObject);
            }
        }
    }

    public void KillEnemies(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            for (int i = 0; i < enemyParent.childCount; i++)
            {
                Destroy(enemyParent.GetChild(i).gameObject);
            }
        }
    }
}
