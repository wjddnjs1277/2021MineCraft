using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stateable : MonoBehaviour
{
    public float maxHp;       // 최대 체력
    public float power;       // 공격력
    public float defense;     // 방어력

    public event System.Action OnDeadEvent;

    new Collider collider;

    float hp;

    public virtual float Hp
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
        }
    }
    public float Defense => defense;

    protected void Awake()
    {
        collider = GetComponent<Collider>();

        hp = maxHp;
    }

    public void OnDead()
    {
        collider.enabled = false;
        OnDeadEvent?.Invoke();
    }
}
