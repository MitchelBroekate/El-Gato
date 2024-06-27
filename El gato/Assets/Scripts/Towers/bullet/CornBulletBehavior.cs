using System.Collections;
using UnityEngine;

public class CornBulletBehavior : BulletManager
{

    float moveSpeed = 40;
    Transform target;

    bool enableDamage = false;
    bool enableFlight = false;

    Quaternion lookTowards;
    private void Start()
    {
        Physics.IgnoreLayerCollision(7, 3);
        target = transform.parent.GetComponent<CornTower>().nearestTarget;
        bulletDamage = 300;
        StartCoroutine(WaitForce(4.5f));

        Destroy(gameObject, 8);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "enemyship")
        {
            if (enableDamage)
            {

                if (other.gameObject.GetComponent<UfoBehaviour>().uFOState != UfoBehaviour.UFOState.MOVINGOUT || other.gameObject.GetComponent<UfoBehaviour>().uFOState != UfoBehaviour.UFOState.DYING)
                {
                    other.gameObject.GetComponent<UfoBehaviour>().DoDamage(bulletDamage);
                }
                

            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if(collision.transform.tag == "enemyship")
        {
            enableDamage = true;
            Destroy(gameObject);
        }


    }

    private void FixedUpdate()
    {
        if (enableFlight)
        {
            transform.GetComponent<Rigidbody>().velocity = transform.forward * moveSpeed;
        }

    }
    private void Update()
    {
        if (enableFlight)
        {
            if (target != null)
            {
                lookTowards = Quaternion.LookRotation(target.position - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, lookTowards, 5 * Time.deltaTime);
            }
        }
    }

    IEnumerator WaitForce(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        enableFlight = true;
    }
}
