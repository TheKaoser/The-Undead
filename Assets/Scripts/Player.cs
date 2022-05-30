using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public Humanity humanity;
    public GameObject suckCollider;
    public Logo logo;
    GameObject currentSuckCollider;
    SpriteRenderer spriteRenderer;
    BoxCollider2D playerCollider;
    NavMeshAgent agent;
    Animator animator;

    AudioSource audioSource;
    public AudioClip roll;
    public AudioClip death;
    public AudioClip revive;
    public AudioClip[] steps;
    public AudioClip suckStart;
    public AudioClip suckLoop;

    Vector3 destination;
    float CORRECTION_MOVEMENT = 0.01f;
    float STEP_DISTANCE = 3f;
    float INITIAL_WALK_SPEED = 5f;
    float MAX_WALK_SPEED = 10f;
    public float currentWalkSpeed;
    float STEP_COOLDOWN = 0.5f;
    float currentStepCooldown;

    float ROLL_SPEED = 15f;
    float ROLL_DISTANCE = 10f;
    
    float I_FRAME_TIME = 0.25f;
    float INITIAL_DASH_COOLDOWN = 1f;
    float MIN_DASH_COOLDOWN = 0.5f;
    public float currentDashCooldown;
    float timeForNextDash;

    int FIRST_FLOOR = 8;
    int SECOND_FLOOR = 16;
    int BOTH_FLOORS = 32;
    int FIRST_FLOOR_AND_BOTH = 40;
    int SECOND_FLOOR_AND_BOTH = 48;

    bool isRolling;
    bool isSucking;
    public bool isAlive;
    bool isReadyToRevive;
    public bool isOnFirstFloor;
    public bool isOnBothFloors;

    enum AnimationDirection
    {
        side = 0,
        up = 1,
        down = 2
    }
    AnimationDirection currentAnimationDirection; 

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false; 
		agent.updateUpAxis = false;

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();

        isRolling = false;
        isSucking = false;
        isAlive = false;
        isReadyToRevive = true;
        isOnFirstFloor = true;
    }

    void Update()
    {
        if (isAlive)
        {
            if (!isRolling)
            {
                PlayerMovement();
                PlayerRotation();
                PlayerSuck();
            }
            StartCoroutine(PlayerRoll());
            CheckPlayerFloor();
        }
        else
        {
            StartCoroutine(PlayerRevive());
        }
    }

    void CheckPlayerFloor()
    {
        if (agent.enabled)
        {
            NavMeshHit navMeshHit;
            agent.SamplePathPosition(NavMesh.AllAreas, 0f, out navMeshHit);

            if (navMeshHit.mask == BOTH_FLOORS)
            {
                isOnBothFloors = true;
            }
            else
            {
                isOnBothFloors = false;
            }
        }
    }

    IEnumerator PlayerRevive()
    {
        if (Input.anyKeyDown && isReadyToRevive)
        {
            isReadyToRevive = false;
            animator.SetBool("isAlive", true);
            PlayAudio(revive);
            playerCollider.enabled = true;
            logo.ClearLogo();
            yield return new WaitForSeconds(3f);
            agent.enabled = true;
            PlayerCorrectStats();
            isAlive = true;
        }
    }

    void PlayerMovement()
    {
        destination = transform.position;
        if (Input.GetKey(KeyCode.W))
        {
            destination += new Vector3 (CORRECTION_MOVEMENT, STEP_DISTANCE, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            destination += new Vector3 (-CORRECTION_MOVEMENT, -STEP_DISTANCE, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            destination += new Vector3 (-STEP_DISTANCE, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            destination += new Vector3 (STEP_DISTANCE, 0, 0);
        }
        if (agent.enabled)
        {
            NavMeshHit hit;
            NavMesh.Raycast(transform.position, destination, out hit, NavMesh.AllAreas);
            agent.SetDestination(hit.position);
            animator.SetBool("isWalking", true);

            currentStepCooldown -= Time.deltaTime;
            if (currentStepCooldown <= 0 && destination != transform.position)
            {
                PlayAudio(steps[Random.Range(0,4)]);
                currentStepCooldown = STEP_COOLDOWN;
            }
        }
        if (destination == transform.position)
        {
            animator.SetBool("isWalking", false);
        }
    }
    
    void PlayerRotation()
    {
        if (!Input.GetKey(KeyCode.Space))
        {
            if (Mathf.Abs(destination.x - transform.position.x) > 0.1f)
            {
                currentAnimationDirection = AnimationDirection.side;
            }
            else if (destination.y - transform.position.y > 0.1f)
            {
                currentAnimationDirection = AnimationDirection.up;
                destination.x = transform.position.x;
            }
            else if (transform.position.y - destination.y > 0.1f)
            {
                currentAnimationDirection = AnimationDirection.down;
                destination.x = transform.position.x;
            }
            animator.SetInteger("direction", ((int)currentAnimationDirection));

            if (destination.x < transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 0);
            }
            else if (destination.x > transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 0);
            }
        }
    }

    void PlayerSuck()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isSucking)
        {
            StartCoroutine(StartSucking());
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            StartCoroutine(StopSucking());
        }
    }

    IEnumerator StartSucking()
    {
        isSucking = true;
        animator.SetBool("isSucking", true);
        PlayAudio(suckStart);
        agent.SetDestination(transform.position);
        agent.enabled = false;
        
        float xdif = -destination.x + transform.position.x;
        float ydif = -destination.y + transform.position.y;
        float angle = Mathf.Atan2(xdif, ydif) * Mathf.Rad2Deg;
        
        yield return new WaitForSeconds(0.5f);
        if (isAlive && isSucking)
        {
            currentSuckCollider = GameObject.Instantiate(suckCollider, new Vector3 (transform.position.x, transform.position.y + spriteRenderer.bounds.size.y / 2 + 0.5f), Quaternion.Euler(0, 0, -angle));
            if (currentAnimationDirection == AnimationDirection.up)
            {
                currentSuckCollider.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            else
            {
                currentSuckCollider.GetComponent<SpriteRenderer>().sortingOrder = 2;
            }

            while (isSucking)
            {
                PlayAudio(suckLoop);
                yield return new WaitForSeconds(3.3f);
            }
        }
    }

    IEnumerator StopSucking()
    {
        if (currentSuckCollider)
        {
            currentSuckCollider.GetComponent<Animator>().SetBool("isFinished", true);
        }
        animator.SetBool("isSucking", false);
        audioSource.Stop();
        yield return new WaitForSeconds(0.25f);
        agent.enabled = true;
        DestroySucking();
    }

    void DestroySucking()
    {
        animator.SetBool("isSucking", false);
        isSucking = false;
        if (currentSuckCollider)
        {
            Destroy(currentSuckCollider.gameObject);
        }
    }

    public IEnumerator PlayerDie(Transform enemy)
    {
        isAlive = false;
        DestroySucking();
        agent.enabled = false;
        playerCollider.enabled = false;
        animator.SetBool("isAlive", false);
        PlayAudio(death);
        spriteRenderer.enabled = false;
        transform.position = enemy.position;
        transform.localScale = enemy.localScale;
        yield return new WaitForSeconds(1.1f);
        spriteRenderer.enabled = true;
        isReadyToRevive = true;
    }

    public void PlayerCorrectStats()
    {
        currentWalkSpeed = Mathf.Clamp(INITIAL_WALK_SPEED + Mathf.Sqrt(humanity.humanity / 4f), INITIAL_WALK_SPEED, MAX_WALK_SPEED);
        agent.speed = currentWalkSpeed;
        currentDashCooldown = Mathf.Clamp(INITIAL_DASH_COOLDOWN - Mathf.Sqrt(humanity.humanity / 400f), MIN_DASH_COOLDOWN, INITIAL_DASH_COOLDOWN);
    }

    IEnumerator PlayerRoll()
    {
        timeForNextDash -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift) && timeForNextDash <= 0 && !isRolling)
        {
            isRolling = true;
            animator.SetBool("isRolling", true);
            animator.SetBool("isWalking", false);

            PlayAudio(roll);

            DestroySucking();

            agent.enabled = true;
            destination = transform.position + Vector3.Normalize(destination - transform.position) * ROLL_DISTANCE;
            NavMeshHit hit;
            NavMesh.Raycast(transform.position, destination, out hit, NavMesh.AllAreas);
            destination = hit.position;
            if (destination.x - transform.position.x == 0)
            {
                destination = new Vector3(transform.position.x + CORRECTION_MOVEMENT, destination.y);
            }
            agent.SetDestination(destination);
            agent.speed = ROLL_SPEED;

            playerCollider.enabled = false;
            yield return new WaitForSeconds(I_FRAME_TIME);
            playerCollider.enabled = true;

            animator.SetBool("isRolling", false);
            yield return new WaitForSeconds(0.5f);
            agent.speed = currentWalkSpeed;

            timeForNextDash = currentDashCooldown;
            isRolling = false;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Link"))
        {
            agent.areaMask = NavMesh.AllAreas;
        }
    }
    
    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Link"))
        {
            NavMeshHit navMeshHit;
            agent.SamplePathPosition(NavMesh.AllAreas, 0f, out navMeshHit);
            
            if (navMeshHit.mask == FIRST_FLOOR)
            {
                agent.areaMask = FIRST_FLOOR_AND_BOTH;
                isOnFirstFloor = true;
            }
            else if (navMeshHit.mask == SECOND_FLOOR)
            {
                agent.areaMask = SECOND_FLOOR_AND_BOTH;
                isOnFirstFloor = false;
            }
        }
    }

    void PlayAudio (AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
