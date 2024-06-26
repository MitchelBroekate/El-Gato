using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public Color origColour;
    public float flashTime = .15f;


    public void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        origColour = meshRenderer.material.color;
    }

    [System.Obsolete]

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FlashStart();
        }


    }
     void FlashStart()
    {
        meshRenderer.material.color = Color.red;
        Invoke("FlashEnd", flashTime);

    }

    void FlashEnd()
    {
        meshRenderer.material.color = origColour;
    }
}
