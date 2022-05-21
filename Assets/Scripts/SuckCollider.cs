using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuckCollider : MonoBehaviour
{
    Humanity humanity;
    float SUCKING_TIME = 1.5f;
    Dictionary<Collider2D, float> colliders2D = new Dictionary<Collider2D, float>();
    Collider2D suckCollider;

    void Start()
    {
        humanity = GameObject.Find("Humanity").GetComponent<Humanity>();
        suckCollider = GetComponent<Collider2D>();
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
            suckedEnemy.GetComponent<Enemy>().beingSucked = true;
            colliders2D[suckedEnemy] -= Time.deltaTime;
            if (colliders2D[suckedEnemy] <= 0)
            {
                deadEnemies.Add(suckedEnemy);
                humanity.AddHumanity(suckedEnemy.GetComponent<Enemy>().enemyHumanity);
            }
        }

        foreach (Collider2D deadEnemy in deadEnemies)
        {
            colliders2D.Remove(deadEnemy);
            Destroy(deadEnemy.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.name == "Enemy")
        {
            colliders2D.Add(col, SUCKING_TIME);
        }
    }

    void OnDestroy()
    {
        foreach (Collider2D collider2D in colliders2D.Keys)
        {
            collider2D.GetComponent<Enemy>().beingSucked = false;
        }
    }
}
