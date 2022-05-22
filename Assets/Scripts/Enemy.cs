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
    Vector3 randomDestination;
    Vector3 currentDestination;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; 
		agent.updateUpAxis = false;

        animator = GetComponent<Animator>();

        StartCoroutine(GenerateRandomDestinations());
    }

    void Update()
    {
        EnemyMovement();
        EnemyRotation();
    }

    void EnemyMovement()
    {
        if (humanity.humanity > 0 && !beingSucked)
        {
            agent.speed = 3f;
            animator.SetBool("Agressive", true);
            currentDestination = player.transform.position;
        }
        else if (beingSucked)
        {
            currentDestination = transform.position;
        }
        else
        {
            agent.speed = 1f;
            animator.SetBool("Agressive", false);
            currentDestination = transform.position + randomDestination;
        }
        agent.SetDestination(currentDestination);        
    }

    void EnemyRotation()
    {
        if (currentDestination.x < transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 0);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 0);
        }
    }

    IEnumerator GenerateRandomDestinations()
    {
        while(true)
        {
            randomDestination = new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), transform.position.z);
            yield return new WaitForSeconds(Random.Range(0f, 2f));
        }
    }
}
