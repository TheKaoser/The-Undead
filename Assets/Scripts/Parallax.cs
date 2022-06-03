using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform player;
    public SmoothCamera smoothCamera;

    void Update()
    {
        if(smoothCamera.isMoving)
        {
            transform.position = new Vector3(player.transform.position.x / 6, transform.position.y, transform.position.z);
        }
    }
}
