using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stuff", menuName = "NewItem/Stuff")]
public class Stuff : Item
{
    public enum Stuff_Type
    {
        Bone,
    }

    [SerializeField] Stuff_Type type;

    public Stuff_Type Type => type;

    public Stuff(Stuff copy) : base(copy)
    {
        type = copy.type;
    }

    public override Item GetCopy()
    {
        return new Stuff(this);
    }

    protected override bool IsEquals(Item item)
    {
        if (item is Stuff)
        {
            Stuff target = item as Stuff;
            return type == target.type;
        }

        return false;
    }
}
