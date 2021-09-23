using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public enum DAMAGE_TYPE
    {
        Normal,
        Critical,
        fall,
        Block,
    }
    public struct DamageMessage
    {
        public GameObject target;           // 공격 대상
        public Vector3 contactPoint;        // 피격 지점
        public string targetName;           // 공격 대상이름
        public float amount;                // 피해량
        public DAMAGE_TYPE damageType;      // 데미지 타입

        public DamageMessage(Stateable stateable)
        {
            target = stateable.gameObject;
            targetName = stateable.name;
            amount = stateable.power;

            contactPoint = Vector3.zero;
            damageType = DAMAGE_TYPE.Block;
        }
    }

    public UnityEvent<DamageMessage> OnDamageEvent;
    public UnityEvent OnDeadEvent;

    Stateable stat;
    BlinkColor blinkColor;

    float nextDamagedTime;

    private void Start()
    {
        stat = GetComponent<Stateable>();
        blinkColor = GetComponent<BlinkColor>();
    }

    public void OnDamaged(DamageMessage message)
    {
        if (nextDamagedTime <= Time.time)
        {
            nextDamagedTime = Time.time + 1f;

            if (stat.Hp <= 0)
                return;

            float damage = Mathf.Clamp(message.amount - (stat.defense / 10), 0, 1000);
            stat.Hp -= damage;

            Debug.Log(string.Format("{0}에게 {1}의 데미지를 받음 hp:{2}",
                message.target, damage, stat.Hp));

            OnDamageEvent?.Invoke(message);

            if (blinkColor != null)
                blinkColor.Blink();

            if (stat.Hp <= 0)
                OnDead();
        }
    }
    void OnDead()
    {
        gameObject.layer = LayerMask.NameToLayer("Dead");
        OnDeadEvent?.Invoke();
    }
}
