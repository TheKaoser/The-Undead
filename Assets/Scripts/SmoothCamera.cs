using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    public Transform target;
    float mapWidth = 40f;
    float mapHeight = 28f;
    float limitX;
    float limitY;

    void Start()
    {
        float height = GetComponent<Camera>().orthographicSize * 2f;
        float width  = height * Screen.width / Screen.height;
        limitX = (mapWidth - width) / 2f;
        limitY = (mapHeight - height) / 2f;
    }

    void Update ()
    {
        transform.position = new Vector3(Mathf.Clamp(target.transform.position.x, -limitX, limitX), Mathf.Clamp(target.transform.position.y, -limitY, limitY), transform.position.z);
    }
}