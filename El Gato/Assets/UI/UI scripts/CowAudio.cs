using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{

    public AudioClip moo1,moo2,moo3;
    public AudioSource source;
    public bool mooWaitCheck = true;

    void Start()
    {
        
    }

    
    void Update()
    {
        PlayMooSFX();
    }

    public void PlayMooSFX()
    {
        

        if (mooWaitCheck)
        {

            float randomWait = Random.Range(5,15);
            int randomAudioClip = Random.Range(0,3);

            if (randomAudioClip == 0)
            {
                source.clip = moo1;
            }

            if (randomAudioClip == 1)
            {
                source.clip = moo2;
            }

            if (randomAudioClip == 2)
            {
                source.clip = moo3;
            }

            StartCoroutine(MooWait(randomWait));

            
        }
        
    }

    IEnumerator MooWait(float waitTime)
    {
        mooWaitCheck = false;
        source.Play();
        yield return new WaitForSeconds(waitTime);
        mooWaitCheck = true;
    }

}
