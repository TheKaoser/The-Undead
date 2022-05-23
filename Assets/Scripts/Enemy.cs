using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    GameObject player;
    Humanity humanity;
    public bool beingSucked = false;
    NavMeshAgent agent;
    Animator animator;
    Vector3 randomDestination;
    Vector3 currentDestination;

    bool isAttacking = false;
    bool doesDamage = false;
    bool hasGrabbedPlayer = false;
    Vector3 attackDirection;

    EnemySpawner enemySpawner;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        humanity = GameObject.Find("Humanity").GetComponent<Humanity>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; 
		agent.updateUpAxis = false;

        animator = GetComponent<Animator>();

        enemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();

        StartCoroutine(GenerateRandomDestinations());
    }

    void Update()
    {
        EnemyMovement();
        EnemyRotation();
        AttackPlayer();
        CheckIfBeingSucked();
    }

    void EnemyMovement()
    {
        if (!isAttacking)
        {
            if (humanity.humanity > 0 && !beingSucked)
            {
                agent.speed = 3f;
                animator.SetBool("isAgressive", true);
                currentDestination = player.transform.position;
            }
            else if (beingSucked)
            {
                currentDestination = transform.position;
            }
            else
            {
                agent.speed = 1f;
                animator.SetBool("isAgressive", false);
                currentDestination = randomDestination;
            }
            if (agent.enabled)
            {
                agent.SetDestination(currentDestination);
            }
        }
    }

    void AttackPlayer()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < 7.5f && !isAttacking && humanity.humanity > 0 && !beingSucked && agent.enabled)
        {
            isAttacking = true;
            animator.SetBool("isAttacking", true);
            agent.SetDestination(transform.position);
            StartCoroutine(Rush());
        }
    }

    IEnumerator Rush()
    {
        Vector3 destination = transform.position + Vector3.Normalize(player.transform.position - transform.position) * 15f;
        yield return new WaitForSeconds(0.5f);
        if (!CheckIfBeingSucked())
        {
            doesDamage = true;
            agent.SetDestination(destination);
            agent.speed = 15f;
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;

            yield return new WaitForSeconds(1f);
            doesDamage = false;
            if (!CheckIfBeingSucked() && !hasGrabbedPlayer)
            {
                animator.SetBool("isRecovering", true);

                yield return new WaitForSeconds(0.5f);

                isAttacking = false;
                animator.SetBool("isRecovering", false);
                animator.SetBool("isAttacking", false);
                agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
            }
        }        
    }

    bool CheckIfBeingSucked()
    {
        if (beingSucked)
        {
            animator.SetBool("isAttacking", false);
            animator.SetBool("isRecovering", false);
            isAttacking = false;
            animator.SetBool("isBeingSucked", true);
            
            return true;
        }
        return false;
    }

    void EnemyRotation()
    {
        if (!isAttacking && agent.enabled && !beingSucked)
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
    }

    IEnumerator GenerateRandomDestinations()
    {
        while(true)
        {
            randomDestination = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), transform.position.z);
            yield return new WaitForSeconds(Random.Range(1f, 2f));
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player") && doesDamage)
        {
            agent.SetDestination(transform.position);
            hasGrabbedPlayer = true;
            animator.SetBool("isGrabbing", true);
            animator.SetBool("isAttacking", false);
            isAttacking = false;
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
            humanity.ResetHumanity();
            player.GetComponent<Player>().PlayerDie();
            StartCoroutine(ResetEnemy());
        }
    }

    IEnumerator ResetEnemy()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("isGrabbing", false);
        hasGrabbedPlayer = false;
    }

    public void KillEnemy()
    {
        StartCoroutine(EnemyDeath());
    }

    IEnumerator EnemyDeath()
    {
        animator.SetBool("isDead", true);
        yield return new WaitForSeconds(0.75f);
        enemySpawner.NotifyEnemyDead();
        Destroy(gameObject);
    }

    public void ReleaseEnemy()
    {
        StartCoroutine(EnemyRelease());
    }
    
    IEnumerator EnemyRelease()
    {
        beingSucked = false;
        animator.SetBool("isBeingSucked", false);
        yield return new WaitForSeconds(0.75f);
        agent.enabled = true;
    }
}
