using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public PlayerState[] playerStates;
    public int activeState;

    //this start only hides and locks the cursor
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    //this update runs the updates from one of the camera scripts at a time
    private void Update()
    {
        playerStates[activeState].DoUpdate();
    }

    /// <summary>
    /// Function that switches the camera modes when switch key is pressed
    /// </summary>
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
