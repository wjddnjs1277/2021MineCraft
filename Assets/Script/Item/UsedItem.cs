using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUsedItem", menuName = "NewItem/UsedItem")]
public class UsedItem : Item
{
    public enum UsedType
    {
        Cure,
        Food,
        Water,
    }

    [SerializeField] UsedType type;
    [SerializeField] float amount;

    public UsedType Type => type;
    public float Amount => amount;

    public UsedItem(UsedItem copy) : base(copy)
    {
        type = copy.type;
        amount = copy.amount;
    }

    public override Item GetCopy()
    {
        return new UsedItem(this);
    }

    protected override bool IsEquals(Item item)
    {
        if (item is UsedItem)
        {
            UsedItem target = item as UsedItem;
            return type == target.type;
        }

        return false;
    }

}
