using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
    Humanity humanity;
    SoulSpawner soulSpawner;

    public int spawn;
    int SOUL_HUMANITY = 2;

    void Start()
    {
        humanity = GameObject.Find("Humanity").GetComponent<Humanity>();
        soulSpawner = GameObject.Find("SoulSpawner").GetComponent<SoulSpawner>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            humanity.AddHumanity(SOUL_HUMANITY);
            soulSpawner.NotifySoulTaken(spawn);
            Destroy(this.gameObject);
        }
    }
}