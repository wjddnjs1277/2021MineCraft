using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equip", menuName = "NewItem/Equip")]
public class Equip : Item
{
    public enum Equip_Type
    {
        Head,
        Chest,
        Pants,
        Feet,
    }

    [SerializeField] Equip_Type type;
    [SerializeField] float addHpPoint; 
    [SerializeField] int defensePoint;

    public Equip_Type Type => type;
    public int DefensePoint => defensePoint;
    public float HpPoint => addHpPoint;

    protected Equip(Equip copy) : base(copy)
    {
        type = copy.type;
        defensePoint = copy.defensePoint;
        addHpPoint = copy.addHpPoint;
    }   

    public override Item GetCopy()
    {
        return new Equip(this);
    }
    protected override bool IsEquals(Item item)
    {
        if (item is Equip)
        {
            Equip target = item as Equip;
            return type == target.type;
        }

        return false;
    }
}
