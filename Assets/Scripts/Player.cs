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

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false; 
		agent.updateUpAxis = false;

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        MovePlayer();
        Suck();
    }

    void MovePlayer()
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
            transform.localScale = new Vector3(1, 1, 0);
            animator.SetBool("isWalking", true);
        }
        if (Input.GetKey(KeyCode.D))
        {
            destination += new Vector3 (WALK_SPEED, 0, 0);
            transform.localScale = new Vector3(-1, 1, 0);
            animator.SetBool("isWalking", true);
        }
        agent.SetDestination(destination);
    }

    void Suck()
    {
        if (destination != transform.position)
        {
            direction = destination;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("isSucking", true);
            agent.SetDestination(transform.position);

            float xdif = -direction.x + transform.position.x;
            float ydif = -direction.y + transform.position.y;
            float angle = Mathf.Atan2(xdif, ydif) * Mathf.Rad2Deg;

            currentSuckCollider = GameObject.Instantiate(suckCollider, transform.position, Quaternion.Euler(0, 0, -angle));
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            agent.SetDestination(transform.position);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            animator.SetBool("isSucking", false);
            if (currentSuckCollider)
            {
                Destroy(currentSuckCollider.gameObject);
            }
        }
    }
}
