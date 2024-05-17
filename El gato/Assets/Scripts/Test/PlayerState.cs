using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public Transform cam;

    /// <summary>
    /// Update function for States.
    /// </summary>
    public virtual void DoUpdate()
    {

    }

    /// <summary>
    /// Call this when enabling the state.
    /// </summary>
    public virtual void EnableState()
    {
        cam.gameObject.SetActive(true);
    }

    /// <summary>
    /// Call this when disabling the state.
    /// </summary>
    public virtual void DisableState()
    {
        cam.gameObject.SetActive(false);
    }
}
