using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public PlayerState[] playerStates;
    public int activeState;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        playerStates[activeState].DoUpdate();
    }

    public void SwitchCams()
    {
        playerStates[activeState].DisableState();
        activeState++;
        if (activeState >= playerStates.Length)
        {
            activeState = 0;
        }
        playerStates[activeState].EnableState();
        
    }
}
