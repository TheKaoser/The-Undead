using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Humanity : MonoBehaviour
{
    public int humanity;

    void Start()
    {
        humanity = 0;
        GetComponent<Text>().text = "UNDEAD";
    }

    public void AddHumanity()
    {
        humanity++;
        GetComponent<Text>().text = "HUMANITY: " + humanity;
    }
    
    public void ResetHumanity()
    {
        humanity = 0;
        GetComponent<Text>().text = "UNDEAD";
    }
}
