
using System.Collections.Generic;
using UnityEngine;

public class Queue : MonoBehaviour
{
    public List<UfoBehavior> ufoInQueue = new();
    public CowManager cowManager;

    public void AddUfoToQueue(UfoBehavior ufoToAdd)
    {
        if (!ufoInQueue.Contains(ufoToAdd))
        {
            ufoInQueue.Add(ufoToAdd);
        }
        AssignCow();
    }

    public void AssignCow()
    {
        Transform newTarget = cowManager.GetCow();
        if(newTarget != null)
        {
            ufoInQueue[0].target = newTarget;
        }
    }
}
