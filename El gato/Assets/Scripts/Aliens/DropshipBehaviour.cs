using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropshipBehaviour : MonoBehaviour
{
    float decendSpeed;

    shipStates currentState;
    enum shipStates
    {
        DECENDING,
        DROPOFF,
        SHIPEXIT    
    }
    private void Start()
    {
        decendSpeed = 100;


    }
    private void Update()
    {
        DoState();
        StateSwitcher();
    }

    void DoState()
    {
        switch (currentState)
        {
            case shipStates.DECENDING:
                Decending();
                break;

            case shipStates.DROPOFF:
                DropOff();
                break;

            case shipStates.SHIPEXIT:
                ShipExit();
                break;
        }
    }

    void StateSwitcher()
    {

    }

    void Decending()
    {

    }

    void DropOff()
    {

    }

    void ShipExit()
    {

    }

}
