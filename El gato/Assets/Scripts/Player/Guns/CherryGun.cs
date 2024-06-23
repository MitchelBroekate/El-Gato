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

    float bulletSpeed = 1000;

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

    void OnFire()
    {
        GameObject currentBullet = Instantiate(bullet, bulletspawn.position, Quaternion.identity);
        currentBullet.SetActive(true);

        currentBullet.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed * Time.deltaTime;
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

    IEnumerator FireRate()
    {
        while (true) 
        {
            OnFire();

            yield return new WaitForSeconds(1 / fireRate);
        }
    }
}
