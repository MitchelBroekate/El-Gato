
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CowManager : MonoBehaviour
{
    public List<Transform> totalCows = new();
    public List<Transform> freeCows = new();

    
    Queue queue;

    private void Start()
    {
        queue = GameObject.Find("Queue").GetComponent<Queue>();
        for (int i = 0; i < transform.childCount; i++)
        {
            totalCows.Add(transform.GetChild(i));
            freeCows.Add(transform.GetChild(i));    
        }
    }

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
        queue.AssignCow();
    }

    public void RemoveCow(Transform cow)
    {
        totalCows.Remove(cow);
        Destroy(cow.gameObject);
        if (totalCows.Count == 0)
        {
            GameObject.Find("UI/UXManager").GetComponent<UiManager>().ShowGameOverScreen();
            print("Game Over!");
        }
    }
}
