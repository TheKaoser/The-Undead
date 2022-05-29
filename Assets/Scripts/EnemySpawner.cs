using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public Humanity humanity;
    public GameObject player;
    public GameObject zombie;
    public SpriteRenderer map;

    float MAP_HEIGHT = 50f;
    float MAP_WEIGHT = 31.5f;
    
    float RESPAWN_TIME = 10f;
    int enemies;
    float timeForNextSpawn;

    void Update()
    {
        timeForNextSpawn -= Time.deltaTime;
        if (timeForNextSpawn <= 0)
        {
            do
            {
                Vector3 enemyLocation;
                NavMeshHit hit = new NavMeshHit();
                do
                {
                    enemyLocation = new Vector3(Random.Range(MAP_WEIGHT / 2f, -MAP_WEIGHT / 2f), Random.Range(MAP_HEIGHT / 2f, -MAP_HEIGHT / 2f), 0);
                }
                while (Vector3.Distance(enemyLocation, player.transform.position) < 50f && NavMesh.SamplePosition(enemyLocation, out hit, 1.0f, NavMesh.AllAreas));
                print (Vector3.Distance(enemyLocation, player.transform.position));
                print (enemyLocation);

                GameObject.Instantiate(zombie, enemyLocation, Quaternion.identity);
                enemies++;
            }
            while (enemies < 3);

            float rateHumanity = 1f - Mathf.Sqrt(humanity.humanity / 50f);
            float rateEnemies = Mathf.Sqrt(enemies / 20f);
            float totalRate = rateHumanity * 0.2f + rateEnemies * 0.8f;
            // print (rateHumanity * 0.2f + ", " + rateEnemies * 0.8f + ": " + totalRate);

            timeForNextSpawn = totalRate * RESPAWN_TIME;

        }  
    }

    public void NotifyEnemyDead()
    {
        // timeForNextSpawn -= enemies * enemyPenalty;
        enemies--;
    }
}
