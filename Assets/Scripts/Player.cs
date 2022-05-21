using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false; 
		agent.updateUpAxis = false;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 destination = transform.position;
        if (Input.GetKey(KeyCode.W))
        {
            destination += new Vector3 (0, 1f, 0);
            animator.Play("PlayerWalk");
        }
        if (Input.GetKey(KeyCode.S))
        {
            destination += new Vector3 (0, -1f, 0);
            animator.Play("PlayerWalk");
        }
        if (Input.GetKey(KeyCode.A))
        {
            destination += new Vector3 (-1f, 0, 0);
            transform.localScale = new Vector3(1, 1, 0);
            animator.Play("PlayerWalk");
        }
        if (Input.GetKey(KeyCode.D))
        {
            destination += new Vector3 (1f, 0, 0);
            transform.localScale = new Vector3(-1, 1, 0);
            animator.Play("PlayerWalk");
        }
        SetDestination(destination);        
    }

    void SetDestination(Vector3 target)
    {
        var agentDrift = 0.0001f; // minimal
        var driftPos = target + (Vector3)(agentDrift * Random.insideUnitCircle);
        agent.SetDestination(driftPos);
    }
}
