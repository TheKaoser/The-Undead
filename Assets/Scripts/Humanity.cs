using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Humanity : MonoBehaviour
{
    public Text highScore;

    AudioSource audioSource;
    public AudioClip undead;
    public AudioClip human;
    public AudioClip rascayu;

    public int humanity;

    void Start()
    {
        humanity = 0;
        GetComponent<Text>().text = "UNDEAD";

        audioSource = GetComponent<AudioSource>();

        PlayAudio(undead);
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Insert))
        {
            PlayAudio(rascayu);
        }
    }

    public void AddEnemy()
    {
        humanity++;
        GetComponent<Text>().text = "HUMANITY: " + humanity;
        if (humanity == 1)
        {
            PlayAudio(human);
        }
    }
    
    public void AddHumanity(int newHumanity)
    {
        humanity += newHumanity;
        GetComponent<Text>().text = "HUMANITY: " + humanity;
    }
    
    public void ResetHumanity()
    {
        if (humanity > int.Parse(highScore.text))
        {
            highScore.text = humanity + "";
        }

        humanity = 0;
        GetComponent<Text>().text = "UNDEAD";
        PlayAudio(undead);
    }

    void PlayAudio (AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
