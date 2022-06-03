using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SuckCollider : MonoBehaviour
{
    Player player;
    Humanity humanity;
    Animator animator;
    Collider2D suckCollider;

    AudioSource audioSource;
    public AudioClip suckSucced;

    Dictionary<Collider2D, float> colliders2D = new Dictionary<Collider2D, float>();
    
    float START_SUCK_ANIMATION = 0.2f;
    float SUCKING_EFFECT_DURATION = 0.3f;
    float SUCKING_TIME = 0.3f;
    
    bool isAbleToSuckNewEnemies;

    void Awake()
    {
        humanity = GameObject.Find("Humanity").GetComponent<Humanity>();
        player = GameObject.Find("Player").GetComponent<Player>();
        suckCollider = GetComponent<Collider2D>();

        animator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();

        StartCoroutine(EnableObstacle());
    }

    IEnumerator EnableObstacle()
    {
        suckCollider.enabled = false;
        yield return new WaitForSeconds(START_SUCK_ANIMATION);
        animator.SetBool("isStarted", true);
        isAbleToSuckNewEnemies = true;
        suckCollider.enabled = true;
        yield return new WaitForSeconds(SUCKING_EFFECT_DURATION);
        isAbleToSuckNewEnemies = false;

        GetComponent<NavMeshObstacle>().enabled = true;
    }

    void Update()
    {
        List<Collider2D> suckedEnemies = new List<Collider2D>();
        List<Collider2D> notSuckedEnemies = new List<Collider2D>();
        List<Collider2D> deadEnemies = new List<Collider2D>();
        foreach (Collider2D collider2D in colliders2D.Keys)
        {
            if (collider2D.IsTouching(suckCollider))
            {
                suckedEnemies.Add(collider2D);
            }
        }

        foreach (Collider2D suckedEnemy in suckedEnemies)
        {
            colliders2D[suckedEnemy] -= Time.deltaTime;
            if (colliders2D[suckedEnemy] <= 0)
            {
                deadEnemies.Add(suckedEnemy);
                humanity.AddEnemy();
            }
        }

        foreach (Collider2D deadEnemy in deadEnemies)
        {
            colliders2D.Remove(deadEnemy);
            PlayAudio(suckSucced);
            deadEnemy.GetComponent<Enemy>().KillEnemy();
            player.PlayerCorrectStats();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Enemy") && isAbleToSuckNewEnemies)
        {
            if (colliders2D.ContainsKey(col))
            {
                colliders2D.Remove(col);
            }
            colliders2D.Add(col, SUCKING_TIME);
            col.GetComponent<Enemy>().EnemyBeSucked();
        }
    }

    void OnDestroy()
    {
        foreach (Collider2D collider2D in colliders2D.Keys)
        {
            collider2D.GetComponent<Enemy>().ReleaseEnemy();
        }
    }

    void PlayAudio (AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
