using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public Humanity humanity;
    public GameObject suckCollider;
    GameObject currentSuckCollider;
    SpriteRenderer spriteRenderer;
    BoxCollider2D playerCollider;
    NavMeshAgent agent;
    Animator animator;

    Vector3 destination;
    Vector3 direction;
    
    float STEP_DISTANCE = 3f;
    float AGENT_SPEED = 5f;
    float sizeSuckCollider = 1f;
    float DASH_COOLDOWN = 3f;
    // float dashcurrentCooldown ;

    bool isSucking;
    bool isAlive;
    bool isReadyToRevive;

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

        isSucking = false;
        isAlive = false;
        isReadyToRevive = true;
    }

    void Update()
    {
        if (isAlive)
        {
            PlayerMovement();
            PlayerRotation();
            PlayerSuck();
            PlayerCorrectStats();
        }
        else
        {
            StartCoroutine(PlayerRevive());
        }
    }

    IEnumerator PlayerRevive()
    {
        if (Input.anyKeyDown && isReadyToRevive)
        {
            isReadyToRevive = false;
            animator.SetBool("isAlive", true);
            playerCollider.enabled = true;
            yield return new WaitForSeconds(3f);
            agent.enabled = true;
            isAlive = true;
        }
    }

    void PlayerMovement()
    {
        destination = transform.position;
        if (Input.GetKey(KeyCode.W))
        {
            destination += new Vector3 (0.01f, STEP_DISTANCE, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            destination += new Vector3 (-0.01f, -STEP_DISTANCE, 0);
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
            agent.SetDestination(destination);
            animator.SetBool("isWalking", true);
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
            // Save last direction
            if (Mathf.Abs(destination.x - transform.position.x) > 0.1f)
            {
                currentAnimationDirection = AnimationDirection.side;
                direction.y = transform.position.y;
                direction.x = destination.x;
            }
            else if (destination.y - transform.position.y > 0.1f)
            {
                currentAnimationDirection = AnimationDirection.up;
                direction.x = transform.position.x;
                direction.y = destination.y;
            }
            else if (transform.position.y - destination.y > 0.1f)
            {
                currentAnimationDirection = AnimationDirection.down;
                direction.x = transform.position.x;
                direction.y = destination.y;
            }
            animator.SetInteger("direction", ((int)currentAnimationDirection));

            if (direction.x < transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 0);
            }
            else
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
        agent.SetDestination(transform.position);
        agent.enabled = false;
        yield return new WaitForSeconds(0.5f);
        if (isAlive && isSucking)
        {
            float xdif = -direction.x + transform.position.x;
            float ydif = -direction.y + transform.position.y;
            float angle = Mathf.Atan2(xdif, ydif) * Mathf.Rad2Deg;

            currentSuckCollider = GameObject.Instantiate(suckCollider, transform.position, Quaternion.Euler(0, 0, -angle));
            currentSuckCollider.transform.localScale *= sizeSuckCollider;
        }
    }

    IEnumerator StopSucking()
    {
        if (currentSuckCollider)
        {
            currentSuckCollider.GetComponent<Animator>().SetBool("isFinished", true);
        }
        animator.SetBool("isSucking", false);
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
        DestroySucking();
        agent.enabled = false;
        playerCollider.enabled = false;
        isAlive = false;
        animator.SetBool("isAlive", false);
        spriteRenderer.enabled = false;
        transform.position = enemy.position;
        transform.localScale = enemy.localScale;
        yield return new WaitForSeconds(1f);
        spriteRenderer.enabled = true;
        isReadyToRevive = true;
    }

    void PlayerCorrectStats()
    {
        agent.speed = AGENT_SPEED + humanity.humanity * 0.1f;
        sizeSuckCollider = 1f + humanity.humanity * 0.1f;
        
    }

    void PlayerDash()
    {
        // if (Input.GetKeyDown(KeyCode.LeftShift) && )
    }
}
