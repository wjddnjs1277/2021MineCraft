using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackable : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] Stateable owner;

    void OnContact(Damageable target, Vector3 contactPoint)
    {
        if (owner == null)
            return;

        bool isCritical = Random.value < 0.1f;
        float damage = Random.Range(owner.power * 0.9f, owner.power * 1.1f);
        Damageable.DAMAGE_TYPE type = Damageable.DAMAGE_TYPE.Normal;

        if(isCritical)
        {
            damage *= 2f;
            type = Damageable.DAMAGE_TYPE.Critical;
        }

        Damageable.DamageMessage message;

        message.target = owner.gameObject;
        message.targetName = owner.name;
        message.contactPoint = contactPoint;
        message.amount = damage;

        message.damageType = type;

        target.OnDamaged(message);
    }
    public  void OnAttack(Vector3 pivot, float radius)
    {
        Collider[] hits = Physics.OverlapSphere(pivot, radius, layerMask);
        foreach(Collider hit in hits)
        {
            Damageable target = hit.GetComponent<Damageable>();
            if(target != null)
            {
                OnContact(target, hit.ClosestPointOnBounds(pivot));
            }
        }
    }
}
