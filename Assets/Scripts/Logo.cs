using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Logo : MonoBehaviour
{
    Image logo;

    bool isAppearing;
    bool isDisappearing;

    float timeElapsed;
    float lerpDuration = 1;
    float transparent = 0;
    float opaque = 1;
    float currentColor;
    bool isBlack;

    void Start()
    {
        logo = GetComponent<Image>();
    }

    void Update()
    {
        if (isDisappearing)
        {
            timeElapsed += Time.deltaTime;
            logo.color = new Color(1f, 1f, 1f, currentColor);
            currentColor = Mathf.Lerp(opaque, transparent, timeElapsed / lerpDuration);
            if (timeElapsed >= lerpDuration)
            {
                timeElapsed = 0;
                isDisappearing = false;
            }
        }
        else if (isAppearing)
        {
            timeElapsed += Time.deltaTime;
            logo.color = new Color(1f, 1f, 1f, currentColor);
            currentColor = Mathf.Lerp(transparent, opaque, timeElapsed / lerpDuration);
            if (timeElapsed >= lerpDuration)
            {
                timeElapsed = 0;
                isAppearing = false;
            }
        }
    }

    public void ClearLogo()
    {
        isDisappearing = true;
    }

    public void AddLogo()
    {
        isAppearing = true;
    }
}
