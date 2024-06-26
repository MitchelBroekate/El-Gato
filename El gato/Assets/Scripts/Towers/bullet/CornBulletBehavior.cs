using System.Collections;
using UnityEngine;

public class CornBulletBehavior : BulletManager
{

    float moveSpeed = 40;
    Transform target;
    Quaternion targetRotation;
    bool enableDamage = false;
    bool enableFlight = false;

    Quaternion lookTowards;
    private void Start()
    {
        target = transform.parent.GetComponent<CornTower>().nearestTarget;
        bulletDamage = 300;
        StartCoroutine(WaitForce(4.5f));

        Physics.IgnoreLayerCollision(7, 3);
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
        if (collision.transform.tag == "ground")
        {
            enableDamage = true;
            Destroy(gameObject);
        }

        if(collision.transform.tag == "enemyship")
        {
            enabled = true;
            Destroy(gameObject);
        }


    }

    private void FixedUpdate()
    {
        if (enableFlight)
        {
            transform.GetComponent<Rigidbody>().velocity = transform.up * moveSpeed;

        }

    }
    private void Update()
    {
        if (enableFlight)
        {
            lookTowards = Quaternion.LookRotation(new Vector3(target.position.x, 0, target.position.z) - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookTowards, 1 * Time.deltaTime);
        }
    }

    IEnumerator WaitForce(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        enableFlight = true;
    }
}
