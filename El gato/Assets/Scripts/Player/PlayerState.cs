using UnityEngine;

public class PlayerState : MonoBehaviour
{
    //Transform for activating the correct camera for th corresponding state 
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
