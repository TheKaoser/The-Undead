using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    public Transform player;

    float MAP_WIDTH = 50f;
    float MAP_HEIGHT = 31.5f;
    float SKY_HEIGHT = 10f;

    float limitX;
    float downLimitY;
    float topLimitY;

    float playerHeight;

    void Start()
    {
        float height = GetComponent<Camera>().orthographicSize * 2f;
        float width  = height * Screen.width / Screen.height;
        limitX = (MAP_WIDTH - width) / 2f;
        downLimitY = (MAP_HEIGHT - height) / 2f;
        topLimitY = (MAP_HEIGHT + SKY_HEIGHT - height) / 2f;

        playerHeight = player.GetComponent<SpriteRenderer>().bounds.size.y / 2f;
    }

    void Update ()
    {
        transform.position = new Vector3(Mathf.Clamp(player.transform.position.x, -limitX, limitX), playerHeight + Mathf.Clamp(player.transform.position.y, -(downLimitY + playerHeight), topLimitY), transform.position.z);
    }
}