using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    int enemies;
    int enemyPenalty = 10;
    public Humanity humanity;
    int humanityPenalty = 1/2;

    public GameObject player;
    public GameObject zombie;

    public SpriteRenderer map;
    int MAP_HEIGHT = 1431/100;
    int MAP_WEIGHT = 2048/100;

    float timeForNextSpawn;

    void Update()
    {
        timeForNextSpawn -= Time.deltaTime;
        if (timeForNextSpawn <= 0)
        {
            Vector3 enemyLocation;
            do
            {
                enemyLocation = new Vector3(Random.Range(MAP_WEIGHT/2, -MAP_WEIGHT/2), Random.Range(MAP_HEIGHT/2, -MAP_HEIGHT/2), 0);
            }
            while (Vector3.Distance(enemyLocation, player.transform.position) < 12f);

            GameObject.Instantiate(zombie, enemyLocation, Quaternion.identity);
            enemies++;

            print (enemies + ", " + humanity.humanity);

            int seconds = Mathf.Clamp(enemies * enemyPenalty - humanity.humanity * humanityPenalty, 0, 100);
            timeForNextSpawn = seconds;
        }  
    }

    public void NotifyEnemyDead()
    {
        timeForNextSpawn -= enemies * enemyPenalty;
        enemies--;
    }
}
