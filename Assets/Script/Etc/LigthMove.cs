using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LigthMove : MonoBehaviour
{
    [SerializeField] float SecondParRealTimeSecond;
    [SerializeField] float nightFogDensity;
    [SerializeField] float fogDensityScale;

    float dayFogDensity;
    float currentFogDensity;

    bool isNight;

   private void Start()
    {
        isNight = false;
        dayFogDensity = RenderSettings.fogDensity;
    }

    void FixedUpdate()
    {
        transform.Rotate(Vector3.right, 0.1f * SecondParRealTimeSecond * Time.deltaTime);

        if (transform.eulerAngles.x >= 170)
            isNight = true;
        else if (transform.eulerAngles.x <= 10)
            isNight = false;

        if(isNight)
        {
            if(currentFogDensity <= nightFogDensity)
            {
                currentFogDensity += 0 / 1f * fogDensityScale * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
        else
        {
            if(currentFogDensity>=dayFogDensity)
            {
                currentFogDensity -= 0.1f * fogDensityScale * Time.deltaTime;
                RenderSettings.fogDensity = currentFogDensity;
            }
        }
    }
}
