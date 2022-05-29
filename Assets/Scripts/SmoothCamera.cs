using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    public Transform target;
    float MAP_WIDTH = 50f;
    float MAP_HEIGHT = 31.5f;
    float SKY_HEIGHT = 10f;
    float limitX;
    float downLimitY;
    float topLimitY;

    void Start()
    {
        float height = GetComponent<Camera>().orthographicSize * 2f;
        float width  = height * Screen.width / Screen.height;
        limitX = (MAP_WIDTH - width) / 2f;
        downLimitY = (MAP_HEIGHT - height) / 2f;
        topLimitY = (MAP_HEIGHT + SKY_HEIGHT - height) / 2f;
    }

    void Update ()
    {
        transform.position = new Vector3(Mathf.Clamp(target.transform.position.x, -limitX, limitX), Mathf.Clamp(target.transform.position.y, -downLimitY, topLimitY), transform.position.z);
    }
}