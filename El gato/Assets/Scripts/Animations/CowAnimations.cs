using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowAnimations : MonoBehaviour
{
    Animator animator;
    RaycastHit hitUp;
    bool grounded;
    bool isCycling = true;
    bool PauseRoutine = false;
    [SerializeField]
    LayerMask maskLayer;


    //missing: walking and grazing
    public enum animateState
    {
        IDLE,
        LEVITATING,
        FALLING,
        GRAZE,
        WALK

    }

    animateState currentState = animateState.IDLE;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckStates();
        StateSwitch();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "ground")
        {
            if (!grounded)
            {
                StartCoroutine(FallWait());
            }
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "ground")
        {
            grounded = false;
        }
    }

    void StateSwitch()
    {
        switch (currentState)
        {
            case animateState.IDLE:
                AnimateIdle();
                break;

            case animateState.LEVITATING:
                AnimateLevitating();
                break;

            case animateState.FALLING:
                AnimateFallling();
                break;

            case animateState.GRAZE:
                AnimateGraze();
                break;

            case animateState.WALK:
                AnimateWalk();
                break;
        }
    }

    void CheckStates()
    {
        if (Physics.Raycast(transform.position, transform.up, out hitUp, Mathf.Infinity, maskLayer))
        {
            if (hitUp.transform != null)
            {
                if (hitUp.transform.GetComponent<UfoBehaviour>().uFOState == UfoBehaviour.UFOState.GETTINGCOW)
                {
                        currentState = animateState.LEVITATING;
                }

            }
        }

        if (grounded)
        {
            if (!PauseRoutine)
            {
                StartCoroutine(CycleSwitchCases());
            }

        }
    }

    void AnimateIdle()
    {
        animator.GetBool("Idle");
        animator.GetBool("Walk");
        animator.GetBool("AlienCapturing");
        animator.GetBool("Falling");
        animator.GetBool("Graze");

        animator.SetBool("Idle", true);
        animator.SetBool("Walk", false);
        animator.SetBool("AlienCapturing", false);
        animator.SetBool("Falling", false);
        animator.SetBool("Graze", false);

    }

    void AnimateLevitating()
    {
        animator.GetBool("AlienCapturing");
        animator.GetBool("Idle");
        animator.GetBool("Walk");
        animator.GetBool("Falling");
        animator.GetBool("Graze");

        animator.SetBool("AlienCapturing", true);
        animator.SetBool("Idle", false);
        animator.SetBool("Walk", false);
        animator.SetBool("Falling", false);
        animator.SetBool("Graze", false);
    }

    void AnimateFallling()
    {
        animator.GetBool("Falling");
        animator.GetBool("Idle");
        animator.GetBool("Walk");
        animator.GetBool("AlienCapturing");
        animator.GetBool("Graze");

        animator.SetBool("Falling", true);
        animator.SetBool("Idle", false);
        animator.SetBool("Walk", false);
        animator.SetBool("AlienCapturing", false);
        animator.SetBool("Graze", false);
    }

    void AnimateGraze()
    {
        animator.GetBool("Graze");
        animator.GetBool("Falling");
        animator.GetBool("Idle");
        animator.GetBool("Walk");
        animator.GetBool("AlienCapturing");

        animator.SetBool("Graze", true);
        animator.SetBool("Falling", false);
        animator.SetBool("Idle", false);
        animator.SetBool("Walk", false);
        animator.SetBool("AlienCapturing", false);
    }

    void AnimateWalk()
    {
        animator.GetBool("Walk");
        animator.GetBool("Graze");
        animator.GetBool("Falling");
        animator.GetBool("Idle");
        animator.GetBool("AlienCapturing");

        animator.SetBool("Walk", true);
        animator.SetBool("Graze", false);
        animator.SetBool("Falling", false);
        animator.SetBool("Idle", false);
        animator.SetBool("AlienCapturing", false);
    }

    IEnumerator CycleSwitchCases()
    {
        while (true)
        {
            PauseRoutine = true;

            if (isCycling)
            {
                int randomCase = Random.Range(0, 4);

                if (randomCase == 1)
                {
                    currentState = animateState.IDLE;
                }
                if (randomCase == 2)
                {
                    currentState = animateState.GRAZE;
                }
                if (randomCase == 3)
                {
                    currentState = animateState.WALK;
                }
            }

            float randomWaitTime = Random.Range(2, 5f);

            yield return new WaitForSeconds(randomWaitTime);

            PauseRoutine = false;
        }

    }

    IEnumerator FallWait()
    {
        currentState = animateState.FALLING;
        yield return new WaitForSeconds(1);
        grounded = true;
    }


}