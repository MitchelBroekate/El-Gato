using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoBehaviour : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private int health = 100;

    public Transform target;

    [SerializeField]
    float moveSpeed = 10;
    [SerializeField]
    float levitationSpeed = 100;
    [SerializeField]
    float claimedCowDistance;

    [SerializeField]
    int layerMask;
    RaycastHit hit;

    public Transform centerObject;

    public float radius = 20;

    public float circleSpeed = 10;

    private float angle = 0;

    public float rotationSpeed = 2f;

    GameObject hover;

    EnemyManager enemyManager;

    Rigidbody rb;

    bool shipExit = false;

    int moneyAmount;

    #endregion

    public enum UFOState
    {
        QUEUE,
        GOTOCOW,
        GETTINGCOW,
        MOVINGOUT
    }

    public UFOState uFOState;

    [System.Obsolete]
    private void Start()
    {
        moneyAmount = 50;

        rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        centerObject = GameObject.Find("Queue").transform;

        enemyManager = GameObject.Find("Scripts/PlayerInput").GetComponent<EnemyManager>();

        hover = transform.FindChild("Hover").gameObject;
        hover.SetActive(false);

        layerMask = LayerMask.GetMask("cow");

        Physics.IgnoreLayerCollision(2, 2);

        GameObject.Find("Queue").GetComponent<Queue>().AddUfoToQueue(this);

        uFOState = UFOState.QUEUE;
    }

    private void Update()
    {
        DoState();
    }

    void DoState()
    {
        switch (uFOState)
        {
            case UFOState.QUEUE:
                Queue();
                break;
            case UFOState.GOTOCOW:
                GoToCow();
                break;
            case UFOState.GETTINGCOW:
                GettingCow();
                break;
            case UFOState.MOVINGOUT:
                MovingOut();
                break;
            default:
                break;
        }
    }

    void Queue()
    {
        if (target != null)
        {
            uFOState = UFOState.GOTOCOW;
        }
        else
        {
            float x = centerObject.position.x + Mathf.Cos(angle) * radius;
            float z = centerObject.position.z + Mathf.Sin(angle) * radius;

            transform.position = new Vector3(x, transform.position.y, z);

            angle += circleSpeed * Time.deltaTime;

            if (angle > Mathf.PI * 2)
            {
                angle -= Mathf.PI * 2;
            }
        }
    }

    void GoToCow()
    {
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.transform == target)
            {
                transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
                uFOState = UFOState.GETTINGCOW;
            }
        }
        else
        {

            if (target != null)
            {
                Vector3 direction = (target.position - transform.position).normalized;

                direction.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(direction);

                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                transform.GetComponent<Rigidbody>().velocity = transform.forward * moveSpeed;
            }
        }
        
    }

    void GettingCow()
    {
        if(Vector3.Distance(target.position,transform.position) < claimedCowDistance)
        {
            GameObject.Find("CowManager").GetComponent<CowManager>().RemoveCow(target);
            uFOState = UFOState.MOVINGOUT;
        }
        else
        {
            hover.SetActive(true);
            target.GetComponent<Rigidbody>().velocity = transform.up * levitationSpeed * Time.deltaTime;
        }
    }

    void MovingOut()
    {
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        hover.SetActive(false);
        transform.GetComponent<Rigidbody>().AddForce(Vector3.up * levitationSpeed * 3 * Time.deltaTime) ;
        StartCoroutine(KillWaitTime());
        if (shipExit)
        {
            DoDamage(100000);
        }
    }

    public void DoDamage(int damage)
    {
        health -= damage;
        if (health <= 0 && uFOState != UFOState.QUEUE)
        {

            if(uFOState == UFOState.GETTINGCOW || uFOState == UFOState.GOTOCOW)
            { 
                GameObject.Find("CowManager").GetComponent<CowManager>().AddFreeCow(target);
            }
            if (uFOState != UFOState.MOVINGOUT)
            {
                GameObject.Find("Scripts/PlayerInput").GetComponent<BuildingShop>().money += moneyAmount;
            }

            Destroy(gameObject);

            enemyManager.waves[enemyManager.currentWave].enemiesAlive--;
        }
    }

    public void SetHealthHerfst(int healthplus)
    {
        health += healthplus;
    }

    public void SetMoney(int extraMoney)
    {
        moneyAmount += extraMoney;
    }

    IEnumerator KillWaitTime()
    {
        yield return new WaitForSeconds(4);
        shipExit = true;
    }
}