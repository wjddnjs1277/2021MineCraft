using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : ScriptableObject
{
    const int MAX_ITEM_COUNT = 99;

    [SerializeField] GameObject itemPrefab;     // 아이템 오브젝트
    [SerializeField] Sprite itemSprite;          // 아이템 이미지
    [SerializeField] string itemName;           // 아이템 이름

    [Range(1, MAX_ITEM_COUNT)]
    int count = 1;

    public Sprite ItemSprite => itemSprite;
    public string ItemName => itemName;
    public int Count => count;

    public Item(Item copy)
    {
        itemPrefab = copy.itemPrefab;
        itemName = copy.itemName;
        itemSprite = copy.itemSprite;
        count = Mathf.Clamp(copy.count, 1, MAX_ITEM_COUNT);
    }

    public void MergeItem(Item target)
    {
        // 만약 다른 아이템을 병합하려는 경우.
        if (IsEquals(target) == false)
            return;

        int freeCount = MAX_ITEM_COUNT - count; // 여유공간.
        if (target.count <= freeCount)
        {
            count += target.count;
            target.count = 0;
        }
        else
        {
            target.count -= freeCount;
            count = MAX_ITEM_COUNT;
        }
    }
    public Item Split()
    {
        Item copy = GetCopy();

        int splitCount = Mathf.CeilToInt(Count / 2f);  // 나누기 한 값을 '올림' 하겠다.

        count -= splitCount;
        copy.count = splitCount;

        return copy;
    }
    public void ItemCountDecrease(int amount)
    {
        count -= amount;
    }
    public ItemObject GetDropObject()
    {
        GameObject obj = Instantiate(itemPrefab);
        ItemObject itemObject = obj.GetComponent<ItemObject>();
        itemObject.SetItem(GetCopy());        

        return itemObject;
    }
    public virtual GameObject GetObject()
    {
        return Instantiate(itemPrefab);
    }

    public abstract Item GetCopy();
    protected abstract bool IsEquals(Item item);

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    public override bool Equals(object other)
    {
        if (other is Item)
            return IsEquals(other as Item);
        else
            return false;
    }

    public override string ToString()
    {
        return itemName;
    }


    public static bool operator ==(Item a, Item b)
    {
        bool isEquals = false;

        try
        {
            isEquals = a.IsEquals(b);
        }
        catch
        {
            // null일 경우
            isEquals = true;
        }

        return isEquals;
    }
    public static bool operator !=(Item a, Item b)
    {
        bool isEquals = false;

        try
        {
            isEquals = a.IsEquals(b);
        }
        catch
        {
            // null일 경우
            isEquals = true;
        }

        return isEquals == false;
    }
    public static Item operator +(Item a, int value)
    {
        a.count += value;
        return a.GetCopy();
    }
    public static Item operator -(Item a, int value)
    {
        Debug.Log(a.ToString());

        a.count -= value;
        a.count = Mathf.Clamp(a.count, 1, MAX_ITEM_COUNT + 1);
        return a.GetCopy();
    }
}
