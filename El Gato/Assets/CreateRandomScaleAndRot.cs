using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CreateRandomScaleAndRot : MonoBehaviour
{
    public Vector2 randomMinMaxScale;
    public Vector2 randomMinMaxRot;

    public bool doRandom;

    // Update is called once per frame
    void Update()
    {
        if (doRandom)
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).rotation = Quaternion.identity;
                Vector3 randomRot = new Vector3(Random.Range(randomMinMaxRot.x, randomMinMaxRot.y), Random.Range(randomMinMaxRot.y, randomMinMaxRot.y),0);
                Vector3 randomScale = new Vector3(1+Random.Range(randomMinMaxScale.x, randomMinMaxScale.x), 1+ Random.Range(randomMinMaxScale.x, randomMinMaxScale.y), 1);
                transform.GetChild(i).Rotate(randomRot);
                transform.GetChild(i).localScale = randomScale;
            }
            doRandom = false;
        }
    }
}
