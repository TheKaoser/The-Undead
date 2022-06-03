using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulSpawner : MonoBehaviour
{
    public Humanity humanity;
    public GameObject soul;
    public Transform[] spawns;

    float RESPAWN_TIME = 20f;
    int MAX_SOULS = 4;

    int currentSouls;
    float timeForNextSpawn;
    List <int> spawnsTaken = new List<int>();

    void Update()
    {
        timeForNextSpawn -= Time.deltaTime;
        if (timeForNextSpawn <= 0 && currentSouls < MAX_SOULS && humanity.humanity > 0)
        {
            int random; 
            do
            {
                random = Random.Range(0, spawns.Length);
            } while (spawnsTaken.Contains(random));
            spawnsTaken.Add(random);

            Soul newSoul = GameObject.Instantiate(soul, spawns[random].transform.position, Quaternion.identity).GetComponent<Soul>();
            newSoul.spawn = random;
            currentSouls++;

            timeForNextSpawn = RESPAWN_TIME;
        }
        else if (currentSouls >= MAX_SOULS || humanity.humanity == 0)
        {
            timeForNextSpawn = RESPAWN_TIME;
        }
    }

    public void NotifySoulTaken(int spawn)
    {
        spawnsTaken.Remove(spawn);
        currentSouls--;
    }
}
