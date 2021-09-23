using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : Singleton<StatusManager>
{
    public enum StatusType
    {
        Hp,
        Hungry,
        Thirst,
    }

    [SerializeField] GaugesBar[] gauges;

    public void UpdateGaugesBar(StatusType type ,float current, float max)
    {
        gauges[(int)type].UpdateBar(current, max);
    }

}
