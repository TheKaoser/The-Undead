using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public Humanity humanity;
    public bool beingSucked = false;
    public int enemyHumanity = 20;
    NavMeshAgent agent;
    Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; 
		agent.updateUpAxis = false;

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        EnemyMovement();
    }

    void EnemyMovement()
    {
        if (humanity.humanity > 0 && !beingSucked)
        {
            animator.SetBool("Agressive", true);
            agent.SetDestination(player.transform.position);
            if (player.transform.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 0);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 0);
            }
        }
        else if (beingSucked)
        {
            agent.SetDestination(transform.position);
        }
        else
        {
            animator.SetBool("Agressive", false);
        }
    }
}
