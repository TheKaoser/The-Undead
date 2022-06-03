using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public Humanity humanity;
    public GameObject zombie;
    public SpriteRenderer map;
    public Transform[] spawns;
    
    float RESPAWN_TIME = 6.5f;

    int enemies;
    float timeForNextSpawn;

    void Update()
    {
        timeForNextSpawn -= Time.deltaTime;
        if (timeForNextSpawn <= 0 && (humanity.humanity > 0 || enemies < 3))
        {
            do
            {
                GameObject.Instantiate(zombie, spawns[Random.Range(0, spawns.Length)].transform.position, Quaternion.identity);
                enemies++;
            }
            while (enemies < 3);

            timeForNextSpawn = CalculateNextSpawn();
        }
    }

    float CalculateNextSpawn()
    {
        float rateHumanity = 1f - Mathf.Sqrt(humanity.humanity / 50f);
        float rateEnemies = Mathf.Sqrt(enemies / 20f);
        float totalRate = rateHumanity * 0.35f + rateEnemies * 0.65f;
        return totalRate * RESPAWN_TIME;
    }

    public void NotifyEnemyDead()
    {
        timeForNextSpawn = CalculateNextSpawn() - timeForNextSpawn;
        enemies--;
    }
}
