using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Humanity humanity;
    public GameObject player;
    public GameObject zombie;
    public SpriteRenderer map;

    int MAP_HEIGHT = 50;
    int MAP_WEIGHT = 30;
    
    float RESPAWN_TIME = 10f;
    int enemies;
    float timeForNextSpawn;

    void Update()
    {
        timeForNextSpawn -= Time.deltaTime;
        if (timeForNextSpawn <= 0)
        {
            while (enemies < 3)
            {
                Vector3 enemyLocation;
                do
                {
                    enemyLocation = new Vector3(Random.Range(MAP_WEIGHT/2, -MAP_WEIGHT/2), Random.Range(MAP_HEIGHT/2, -MAP_HEIGHT/2), 0);
                }
                while (Vector3.Distance(enemyLocation, player.transform.position) < 12f);

                GameObject.Instantiate(zombie, enemyLocation, Quaternion.identity);
                enemies++;
            }

            float rateHumanity = 1f - Mathf.Sqrt(humanity.humanity / 50f);
            float rateEnemies = Mathf.Sqrt(enemies / 20f);
            float totalRate = rateHumanity * 0.2f + rateEnemies * 0.8f;
            print (rateHumanity * 0.2f + ", " + rateEnemies * 0.8f + ": " + totalRate);

            timeForNextSpawn = totalRate * RESPAWN_TIME;

        }  
    }

    public void NotifyEnemyDead()
    {
        // timeForNextSpawn -= enemies * enemyPenalty;
        enemies--;
    }
}
