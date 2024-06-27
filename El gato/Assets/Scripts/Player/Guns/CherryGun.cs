using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CherryGun : FpsState
{

    RaycastHit hit;

    [SerializeField]
    Transform bulletspawn;
    [SerializeField]
    GameObject bullet;

    float bulletSpeed = 3000;

    float fireRate = 5;

    [SerializeField]
    Animator animator;

    Coroutine fireCoroutine;

    

    private void Update()
    {
        if (Physics.Raycast(cam.position, transform.forward, out hit, Mathf.Infinity))
        {

        }
    }


    public void FireManager(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (gameObject.activeInHierarchy)
            {
                animator.GetBool("Shoot");
                animator.SetBool("Shoot", true);
                fireCoroutine = StartCoroutine(FireRate());
            }

        }

        if (context.canceled)
        {
            if (fireCoroutine != null)
            {
                animator.GetBool("Shoot");
                animator.SetBool("Shoot", false);
                StopCoroutine(fireCoroutine);
            }
        }

    }
    void OnFire()
    {
        source.clip = clip;
        source.Play();
        GameObject currentBullet = Instantiate(bullet, bulletspawn.position, transform.rotation);
        currentBullet.SetActive(true);

        currentBullet.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed * Time.deltaTime;
    }

    IEnumerator FireRate()
    {
        while (true) 
        {
            OnFire();

            yield return new WaitForSeconds(1 / fireRate);
        }
    }
}
