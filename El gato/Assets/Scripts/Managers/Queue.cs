
using System.Collections.Generic;
using UnityEngine;

public class Queue : MonoBehaviour
{
    public List<UfoBehaviour> ufoInQueue = new();
    public CowManager cowManager;

    public void AddUfoToQueue(UfoBehaviour ufoToAdd)
    {
        if (!ufoInQueue.Contains(ufoToAdd))
        {
            ufoInQueue.Add(ufoToAdd);
        }
        AssignCow();
    }
    public void RemoveUfoFromQueue(UfoBehaviour ufoToRemove)
    {
        for (int i = 0; i < ufoInQueue.Count; i++)
        {
            if (ufoInQueue[i] == ufoToRemove)
            {
                ufoInQueue.RemoveAt(i);
            }
        }
        AssignCow();
    }

    public void AssignCow()
    {
        if(ufoInQueue.Count > 0)
        {
            Transform newTarget = cowManager.GetCow();
            if(newTarget != null)
            {
                ufoInQueue[0].target = newTarget;
                RemoveUfoFromQueue(ufoInQueue[0]);
            }
        }
    }
}
