using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Logo : MonoBehaviour
{
    Image logo;

    bool isDisappearing;

    float timeElapsed;
    float lerpDuration = 1;
    float transparent = 0;
    float opaque = 1;
    float currentColor;

    void Start()
    {
        logo = GetComponent<Image>();
    }

    void Update()
    {
        if (isDisappearing)
        {
            timeElapsed += Time.deltaTime;
            currentColor = Mathf.Lerp(opaque, transparent, timeElapsed / lerpDuration);
            logo.color = new Color(1f, 1f, 1f, currentColor);
        }
    }

    public void ClearLogo()
    {
        isDisappearing = true;
    }
}
