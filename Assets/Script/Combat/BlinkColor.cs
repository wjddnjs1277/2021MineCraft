using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkColor : MonoBehaviour
{
    [SerializeField] Color blinkColor;
    [SerializeField] float blinkTime;
    [SerializeField] SkinnedMeshRenderer skinnedRenderer;
    [SerializeField] Renderer[] renderers;

    Color origin;

    void Awake()
    {
        origin = skinnedRenderer.material.color;
    }

    public void Blink()
    {
        StopCoroutine("OnBlink");
        StartCoroutine("OnBlink");
    }

    IEnumerator OnBlink()
    {
        float maxTime = Time.time + blinkTime;
        while (maxTime > Time.time)
        {
            skinnedRenderer.material.color = Color.Lerp(origin, blinkColor, maxTime / Time.time);
            for(int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.color = Color.Lerp(origin, blinkColor, maxTime / Time.time);
            }
            yield return null;
        }

        skinnedRenderer.material.color = origin;
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = origin;
        }
    }
}
