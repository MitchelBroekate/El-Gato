using System.Collections;
using UnityEngine;

public class CornBulletBehavior : BulletManager
{

    float moveSpeed = 40;
    Transform target;
    bool enableFlight = false;

    Quaternion lookTowards;

    Vector3 targetpos;
    private void Start()
    {
        Physics.IgnoreLayerCollision(7, 10);
        target = transform.parent.GetComponent<CornTower>().nearestTarget;
        targetpos = target.position;
        bulletDamage = 300;
        StartCoroutine(WaitForce(4f));

        Destroy(gameObject, 8);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.tag == "ground")
        {
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
                transform.rotation = Quaternion.Lerp(transform.rotation, lookTowards, 10 * Time.deltaTime);

                if (Vector3.Distance(transform.position, target.position) < 6)
                {
                    target.GetComponent<UfoBehaviour>().DoDamage(bulletDamage);
                    Destroy(gameObject);
                }
            }
            else
            {
                lookTowards = Quaternion.LookRotation(new Vector3(targetpos.x, 0, targetpos.z) - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, lookTowards, 10 * Time.deltaTime);
            }
        }
    }

    IEnumerator WaitForce(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        enableFlight = true;
    }
}
