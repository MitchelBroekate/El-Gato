using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PumpkinLauncher : FpsState
{


    RaycastHit hit;

    [SerializeField]
    Transform bulletspawn;
    [SerializeField]
    GameObject bullet;

    float bulletSpeed = 4000;

    GameObject currentBullet;

    bool canFire = true;

    GameObject rocketShow;

    bool rocketGrow = false;

    float t;

    float shrinkDuration = 3.5f;

    

    [System.Obsolete]
    private void Start()
    {
        rocketShow = transform.FindChild("BulletShow").gameObject;
        
    }

    private void Update()
    {
        if (Physics.Raycast(cam.position, transform.forward, out hit, Mathf.Infinity))
        {

        }

        RocketGrowing();

    }


    public void FireManager(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            print("Perform");
            if (gameObject.activeInHierarchy)
            {
                if (canFire)
                {
                    FireRate();
                    currentBullet.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed * Time.deltaTime;
                    canFire = false;
                }

            }

        }
    }

    void FireRate()
    {
        rocketShow.SetActive(false);
        currentBullet = Instantiate(bullet, bulletspawn.position, transform.rotation);
        currentBullet.SetActive(true);
        source.clip = clip;
        source.Play();

        rocketShow.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        rocketShow.SetActive(true);
        rocketGrow = true;
    }

    void RocketGrowing()
    {
        if (rocketGrow)
        {
            t += Time.deltaTime / shrinkDuration;

            rocketShow.transform.localScale = Vector3.Lerp(new Vector3(0.1f, 0.1f, 0.1f), Vector3.one, t);

            if (rocketShow.transform.localScale == Vector3.one)
            {
                rocketGrow = false;
                t = 0;

                canFire = true;
            }
        }
    }
}
