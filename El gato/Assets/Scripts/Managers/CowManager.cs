
using System.Collections.Generic;
using UnityEngine;

public class CowManager : MonoBehaviour
{
    public List<Transform> totalCows = new();
    public List<Transform> freeCows = new();

    public Transform GetCow()
    {
        if(freeCows.Count > 0)
        {
            Transform cowToReturn = freeCows[0];
            freeCows.RemoveAt(0);
            return cowToReturn;
        }
        else
        {
            return null;
        }
    }

    public void AddFreeCow(Transform cow)
    {
        freeCows.Add(cow);
    }

    public void RemoveCow(Transform cow)
    {
        totalCows.Remove(cow);
        Destroy(cow);
        if (totalCows.Count == 0)
        {
            // game over.
        }
    }
}
