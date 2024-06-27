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

    public SkinnedMeshRenderer meshRenderer;
    public Color origColour;
    public float flashTime = .15f;

    GameObject particle;

    AudioSource ufoAttack;

    [SerializeField]
    List<AudioClip> audioClips = new();

    bool addMoney = true;

    #endregion

    //Enum for UFO statess
    public enum UFOState
    {
        QUEUE,
        GOTOCOW,
        GETTINGCOW,
        MOVINGOUT,
        DYING
    }

    public UFOState uFOState;

    //Sets vars and checks if the enemy event is active and applies the values
    [System.Obsolete]
    private void Start()
    {

        ufoAttack = GetComponent<AudioSource>();

        claimedCowDistance = 6;

        moneyAmount = 50;

        rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        centerObject = GameObject.Find("Queue").transform;

        enemyManager = GameObject.Find("Scripts/PlayerInput").GetComponent<EnemyManager>();

        if (enemyManager.alienEvent)
        {
            moneyAmount = 100;
            health = health / 2;
            levitationSpeed = levitationSpeed * 2;

        }

        hover = transform.FindChild("Hover").gameObject;
        hover.SetActive(false);

        layerMask = LayerMask.GetMask("cow");

        Physics.IgnoreLayerCollision(2, 2);

        GameObject.Find("Queue").GetComponent<Queue>().AddUfoToQueue(this);

        uFOState = UFOState.QUEUE;

        meshRenderer = transform.FindChild("ufo").GetComponent<SkinnedMeshRenderer>();
        origColour = meshRenderer.material.color;

        particle = transform.FindChild("UfoExplosion").gameObject;
    }

    //Updates states
    private void Update()
    {
        DoState();
    }

    //Switch case for states
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
            case UFOState.DYING:
                addMoney = false;
                break;
        }
    }

    /// <summary>
    /// Sets the UFO in a queue if there is no cow available and assigns a cow to the UFO if its available
    /// </summary>
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

    /// <summary>
    /// Moves the UFO to a cow and stops the ufo if its above the cow
    /// </summary>
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

    /// <summary>
    /// Levitates the cow and removes a life if a cow has been captured
    /// </summary>
    void GettingCow()
    {
        if(Vector3.Distance(target.position,transform.position) < claimedCowDistance)
        {
            GameObject.Find("CowManager").GetComponent<CowManager>().RemoveCow(target);
            rb.constraints = RigidbodyConstraints.None;
            ufoAttack.clip = null;
            uFOState = UFOState.MOVINGOUT;          
        }
        else 
        {
            ufoAttack.loop = true;
            ufoAttack.clip = audioClips[0];
            ufoAttack.Play();
            rb.constraints = RigidbodyConstraints.FreezeAll;
            hover.SetActive(true);
            target.GetComponent<Rigidbody>().velocity = transform.up * levitationSpeed * Time.deltaTime;
        }
    }

    /// <summary>
    /// Moves the UFO towards the sky so it can despawn
    /// </summary>
    void MovingOut()
    {
        rb.mass = 1;
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

    /// <summary>
    /// Invokes damage falsh
    /// </summary>
    void FlashStart()
    {
        meshRenderer.material.color = Color.red;
        Invoke("FlashEnd", flashTime);

    }

    /// <summary>
    /// Stops the damage flash
    /// </summary>
    void FlashEnd()
    {
        meshRenderer.material.color = origColour;
    }

    /// <summary>
    /// Applies the amount of damage a tower gives and checks if the UFO has died
    /// </summary>
    /// <param name="damage"></param>
    public void DoDamage(int damage)
    {
        health -= damage;
        FlashStart();
        if (health <= 0 && uFOState != UFOState.QUEUE)
        {


            if(uFOState == UFOState.GETTINGCOW || uFOState == UFOState.GOTOCOW)
            { 
                GameObject.Find("CowManager").GetComponent<CowManager>().AddFreeCow(target);
            }
            if (uFOState != UFOState.MOVINGOUT)
            {
                if (addMoney)
                {
                GameObject.Find("Scripts/PlayerInput").GetComponent<BuildingShop>().money += moneyAmount;

                }
            }

            particle.SetActive(true);

            ufoAttack.loop = false;
            ufoAttack.clip = audioClips[1];
            ufoAttack.Play();

            uFOState = UFOState.DYING;

            rb.velocity = Vector3.zero;

            Destroy(gameObject, 1);

            enemyManager.waves[enemyManager.currentWave].enemiesAlive--;
        }
    }

    /// <summary>
    /// Adds health for the Herfst scene
    /// </summary>
    /// <param name="healthplus"></param>
    public void SetHealthHerfst(int healthplus)
    {
        health += healthplus;
    }

    /// <summary>
    /// Waits until the UFO has gone upwards for an amount of time
    /// </summary>
    /// <returns></returns>
    IEnumerator KillWaitTime()
    {
        yield return new WaitForSeconds(4);
        shipExit = true;
    }
}