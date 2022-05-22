using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public Humanity humanity;
    public GameObject suckCollider;
    GameObject currentSuckCollider;
    NavMeshAgent agent;
    Animator animator;
    Vector3 destination;
    Vector3 direction;
    public float WALK_SPEED = 3f;
    bool isSucking = false;
    SpriteRenderer spriteRenderer;
    bool isAlive = true;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false; 
		agent.updateUpAxis = false;

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isAlive)
        {
            PlayerMovement();
            PlayerRotation();
            PlayerSuck();
        }
    }

    void PlayerMovement()
    {
        destination = transform.position;
        animator.SetBool("isWalking", false);
        if (Input.GetKey(KeyCode.W))
        {
            destination += new Vector3 (0.01f, WALK_SPEED, 0);
            animator.SetBool("isWalking", true);
        }
        if (Input.GetKey(KeyCode.S))
        {
            destination += new Vector3 (-0.01f, -WALK_SPEED, 0);
            animator.SetBool("isWalking", true);
        }
        if (Input.GetKey(KeyCode.A))
        {
            destination += new Vector3 (-WALK_SPEED, 0, 0);
            animator.SetBool("isWalking", true);
        }
        if (Input.GetKey(KeyCode.D))
        {
            destination += new Vector3 (WALK_SPEED, 0, 0);
            animator.SetBool("isWalking", true);
        }
        agent.SetDestination(destination);
    }
    
    void PlayerRotation()
    {
        if (!Input.GetKey(KeyCode.Space))
        {
            // Save last direction
            if (destination != transform.position)
            {
                direction = destination;
            }

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isSucking = true;
            animator.SetBool("isSucking", true);
            agent.SetDestination(transform.position);

            float xdif = -direction.x + transform.position.x;
            float ydif = -direction.y + transform.position.y;
            float angle = Mathf.Atan2(xdif, ydif) * Mathf.Rad2Deg;

            currentSuckCollider = GameObject.Instantiate(suckCollider, transform.position, Quaternion.Euler(0, 0, -angle));
        }
        else if (isSucking)
        {
            agent.SetDestination(transform.position);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            StartCoroutine(StopSucking());
        }
    }

    IEnumerator StopSucking()
    {
        animator.SetBool("isSucking", false);
        if (currentSuckCollider)
        {
            Destroy(currentSuckCollider.gameObject);
        }
        yield return new WaitForSeconds(0.333f);
        isSucking = false;
    }

    public void PlayerDie()
    {
        spriteRenderer.enabled = false;
        isAlive = false;
    }
}
