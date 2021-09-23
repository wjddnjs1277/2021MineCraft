using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaugesBarManager : Singleton<GaugesBarManager>
{
    [SerializeField] GaugesBar gauges;

    CanvasGroup group;

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
        group.alpha = 0.0f;
    }

    public void UpdateGaugesBar(float current, float max)
    {
        gauges.UpdateBar(current, max);
    }

    public void OnSwitch(bool isOn)
    {
        group.alpha = isOn ? 1.0f : 0.0f;
    }

}
