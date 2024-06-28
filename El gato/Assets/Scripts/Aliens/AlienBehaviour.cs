using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AlienBehaviour : MonoBehaviour
{
    #region Variables
    Rigidbody rb;

    public SkinnedMeshRenderer meshRenderer;
    public Color origColour;
    public float flashTime = .15f;

    [SerializeField]
    List<Transform> checkpoints;

    Transform checkpointParent;
    public Transform towerParent;

    public Transform nearestCheckpoint;
    public Transform nearestTower;

    float walkSpeed;

    public AlienStates currentState;

    bool collisionCheck;

    [SerializeField]
    int health = 300;

    int damage = 100;

    Animator animator;

    bool allowAttack;
    bool allowDamage;

    EnemyManager enemyManager;
    BuildingShop buildingShop;

    int giveMoneyAmount;

    bool addMoney;

    AudioSource source;

    [SerializeField]
    List<AudioClip> audioClips = new();

    AudioSource alienChild;

    #endregion

    //Enum for Alien States
    public enum AlienStates
    {
        DECEND,
        GOTOCHECKPOINT,
        GOTOTOWER,
        ATTACKTOWER,
        DYING,
        DEAD
    }


    //Start for Var assigning and checks if the enemy event is active and applies the values
    [System.Obsolete]
    private void Start()
    {
        giveMoneyAmount = 50;

        alienChild = transform.FindChild("Alien").GetComponent<AudioSource>();
        source = GetComponent<AudioSource>();

        Physics.IgnoreLayerCollision(9, 8);

        currentState = AlienStates.DECEND;

        animator = GetComponent<Animator>();

        collisionCheck = true;

        checkpointParent = GameObject.Find("AlienCheckpoint").transform;
        enemyManager = GameObject.Find("Scripts/PlayerInput").GetComponent<EnemyManager>();
        buildingShop = GameObject.Find("Scripts/PlayerInput").GetComponent<BuildingShop>();

        if (enemyManager.alienEvent)
        {
            giveMoneyAmount = 100;
            health = health / 2;
            damage = damage * 2;
        }

        for (int i = 0; i < checkpointParent.childCount; i++)
        {
            checkpoints.Add(checkpointParent.GetChild(i));
        }

        walkSpeed = 150;

        rb = GetComponent<Rigidbody>();

        meshRenderer = transform.FindChild("Alien").GetComponent<SkinnedMeshRenderer>();

        origColour = meshRenderer.material.color;

        addMoney = true;

        int randomAudio = Random.Range(0, 3);

        if (randomAudio == 0)
        {

            alienChild.clip = audioClips[0];
            alienChild.Play();
        }


        if (randomAudio == 1)
        {
            alienChild.clip = audioClips[1];
            alienChild.Play();
        }

        if (randomAudio == 2)
        {
            alienChild.clip = audioClips[2];
            alienChild.Play();
        }
    }

    //Updates the States and forces the alien to the ground
    private void Update()
    {
        DoStates();

        if (collisionCheck)
        {
            rb.AddForce(-transform.up * 100 * Time.deltaTime, ForceMode.Force);
        }
    }


    //Checks when the alien hits the ground and sets a new state
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "ground")
        {
            if (collisionCheck)
            {
                rb.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;

                currentState = AlienStates.GOTOCHECKPOINT;

                collisionCheck = false;
            }

        }
    }

    //Switch case for states and animations
    void DoStates()
    {
        switch (currentState)
        {
            case AlienStates.DECEND:
                animator.GetBool("Idle");
                animator.SetBool("Idle", true);
                rb.velocity = -transform.up * 1000 * Time.deltaTime;
                break;

            case AlienStates.GOTOCHECKPOINT:
                animator.GetBool("Walking");
                animator.GetBool("Idle");
                animator.GetBool("Attacking");

                animator.SetBool("Walking", true);
                animator.SetBool("Idle", false);
                animator.SetBool("Attacking", false);

                GoToCheckpoint();
                break;

            case AlienStates.GOTOTOWER:
                animator.GetBool("Walking");
                animator.GetBool("Idle");
                animator.GetBool("Attacking");

                animator.SetBool("Walking", true);
                animator.SetBool("Idle", false);
                animator.SetBool("Attacking", false);

                GoToTower();
                break;

            case AlienStates.ATTACKTOWER:
                animator.GetBool("Attacking");
                animator.GetBool("Idle");
                animator.GetBool("Walking");

                animator.SetBool("Attacking", true);
                animator.SetBool("Idle", false);
                animator.SetBool("Walking", false);

                AttackTower();
                break;

            case AlienStates.DYING:
                if (addMoney)
                {
                    buildingShop.money += giveMoneyAmount;
                    addMoney = false;
                }

                rb.constraints = RigidbodyConstraints.FreezeAll;
                animator.GetBool("Dying");
                animator.GetBool("Attacking");
                animator.GetBool("Walking");
                animator.GetBool("Idle");

                animator.SetBool("Dying", true);
                animator.SetBool("Attacking", false);
                animator.SetBool("Walking", false);
                animator.SetBool("Idle", false);
                StartCoroutine(DeathWait());
                currentState = AlienStates.DEAD;
                break;

            default:
                Debug.Log("No State Active!");
                break;
        }
    }

    /// <summary>
    /// Goes through the checkpoint list and gets the closest one to go towards
    /// </summary>
    void GoToCheckpoint()
    {
        allowAttack = true;

        float distance;
        float nearestDistance = float.MaxValue;

        for (int i = 0; i < checkpoints.Count; i++)
        {
            if (checkpoints[i] != null)
            {
                distance = Vector3.Distance(transform.position, checkpoints[i].position);

                if (distance < nearestDistance)
                {
                    nearestCheckpoint = checkpoints[i];
                    nearestDistance = distance;

                }
            }
        }
        Quaternion lookTowards = Quaternion.LookRotation(new Vector3(nearestCheckpoint.position.x, transform.position.y, nearestCheckpoint.position.z) - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookTowards, 2 * Time.deltaTime);
        rb.velocity = transform.forward * walkSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, nearestCheckpoint.position) < 3f)
        {
            rb.velocity = Vector3.zero;

            animator.GetBool("Idle");
            animator.GetBool("Walking");

            animator.SetBool("Idle", true);
            animator.SetBool("Walking", false);

            if (towerParent.childCount > 0)
            {
                currentState = AlienStates.GOTOTOWER;
            }

        }
    }

    /// <summary>
    /// Goes through the tower parent and gets the closest tower and goes towards it
    /// </summary>
    void GoToTower()
    {
        allowAttack = true;
        float distance;
        float nearestDistance = float.MaxValue;

        for (int i = 0; i < towerParent.childCount; i++)
        {
            if (towerParent.childCount > 0)
            {
                distance = Vector3.Distance(transform.position, towerParent.GetChild(i).position);

                if (distance < nearestDistance)
                {
                    nearestTower = towerParent.GetChild(i);
                    nearestDistance = distance;

                }
            }
        }
        if (nearestTower != null)
        {
            Quaternion lookTowards = Quaternion.LookRotation(new Vector3(nearestTower.position.x, transform.position.y, nearestTower.position.z) - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookTowards, 10 * Time.deltaTime);
        }
        else
        {
            currentState = AlienStates.GOTOCHECKPOINT;
        }

        rb.velocity = transform.forward * walkSpeed * Time.deltaTime;

        if (nearestTower != null)
        {
            if (Vector3.Distance(transform.position, nearestTower.position) < 3.5f)
            {
                rb.velocity = Vector3.zero;
                currentState = AlienStates.ATTACKTOWER;
            }
        }
        else
        {
            currentState = AlienStates.GOTOCHECKPOINT;
        }


    }

    /// <summary>
    /// Attacks the tower when its near it. If the tower is destroyed it goes to the next one or goes it back to the checkpoint when there are no more towers left
    /// </summary>
    void AttackTower()
    {
        if (currentState == AlienStates.ATTACKTOWER)
        {
            if (nearestTower != null)
            {
                allowDamage = true;
                Quaternion lookTowards = Quaternion.LookRotation(new Vector3(nearestTower.position.x, transform.position.y, nearestTower.position.z) - transform.position);

                transform.rotation = Quaternion.Slerp(transform.rotation, lookTowards, 2 * Time.deltaTime);
                if (allowAttack)
                {
                    StartCoroutine(AttackDamage());
                    allowAttack = false;
                }

            }

            if (nearestTower == null && towerParent.childCount <= 0)
            {
                allowDamage = false;
                StopAllCoroutines();
                currentState = AlienStates.GOTOCHECKPOINT;
            }
            else if (nearestTower == null && towerParent.childCount > 0)
            {
                allowDamage = false;
                StopAllCoroutines();
                currentState = AlienStates.GOTOTOWER;
            }


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
    /// Applies damage value that the gun gives
    /// </summary>
    /// <param name="damage"></param>
    public void DoDamage(int damage)
    {
        if (currentState != AlienStates.DYING)
        {
            health -= damage;
            //FlashStart();

            if (health <= 0)
            {
                Destroy(gameObject);
                //currentState = AlienStates.DYING;
            }
        }

    }

    /// <summary>
    /// Applies health for the Herfst scene
    /// </summary>
    /// <param name="healthplus"></param>
    public void SetHealthHerfst(int healthplus)
    {
        health += healthplus;
    }

    /// <summary>
    /// Gives an attack cooldown
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackDamage()
    {
        while (allowDamage)
        {
            if (currentState == AlienStates.ATTACKTOWER)
            {
                if (source.clip != null)
                {
                    source.clip = null;
                }

                yield return new WaitForSeconds(2f);

                source.clip = audioClips[4];
                source.Play();

                nearestTower.GetComponent<Health>().DoDamage(damage);
            }
            else
            {
                StopCoroutine(AttackDamage());
            }



        }

    }

    IEnumerator DeathWait()
    {
        print("Waiting for death.");
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }
}
