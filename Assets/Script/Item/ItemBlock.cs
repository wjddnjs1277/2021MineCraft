using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBlock", menuName = "NewItem/Block", order = 0)]
public class ItemBlock : Item
{
    
    public enum BlockType
    {
        Grass,
        Rock,
        Wood,
        Leaf,
        Snow,
        CraftTable,
        Soil,
    }

    [SerializeField] BlockType type;
    [SerializeField] GameObject buildObject;

    public BlockType Type => type;
    public GameObject BuildObject => buildObject;

    protected ItemBlock(ItemBlock copy) : base(copy)
    {
        type = copy.type;
        buildObject = copy.buildObject;
    }

    public override Item GetCopy()
    {
        return new ItemBlock(this);
    }
    protected override bool IsEquals(Item item)
    {
        if (item is ItemBlock)
        {
            ItemBlock target = item as ItemBlock;
            return type == target.type;
        }

        return false;
    }
}
