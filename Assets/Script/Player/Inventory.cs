using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    enum MOVE_RESULT
    {
        None = -1,
        Drop,       // 버리기
        Swap,       // 교환
        Move,       // 이동
        Merge,      // 병합
        Wear,       // 착용
        Cancel,     // 취소
    }

    Item[] itemArray;   // 아이템 배열.
    Item[] quickArray;  // 퀵 슬롯의 배열.
    Equip[] equipArray;
    Item dragItem;      // 옮겨지는 아이템.

    public event System.Action<Item[], Item[], Equip[], Item> OnUpdateInven;

    public Item DrageItem => dragItem;
    public Item CurrentItem => quickArray[QuickSlot.QuickIndex - 100];

    public Inventory()
    {
        itemArray = new Item[24];
        for (int i = 0; i < itemArray.Length; i++)
            itemArray[i] = null;

        quickArray = new Item[8];
        for (int i = 0; i < quickArray.Length; i++)
            quickArray[i] = null;

        equipArray = new Equip[4];
        for (int i = 0; i < equipArray.Length; i++)
            equipArray[i] = null;


    }

    public void Push(Item origin)
    {
        bool isUpdate = false;
        bool isQuickPush = false;
        Item newItem = origin.GetCopy();

        for(int i = 0; i < quickArray.Length; i++)
        {
            if (newItem.Count <= 0)
            {
                break;
            }

            Item quickItem = quickArray[i];

            if (quickItem == null || quickItem != newItem)
                continue;

            quickItem.MergeItem(newItem);
            isUpdate = true;
        }

        for (int i = 0; i < itemArray.Length; i++)
        {
            if (newItem.Count <= 0)
            {
                break;
            }

            Item invenItem = itemArray[i];

            if (invenItem == null || invenItem != newItem)
                continue;

            invenItem.MergeItem(newItem);
            isUpdate = true;
        }

        if (newItem.Count > 0)
        {
            for (int i = 0; i < quickArray.Length; i++)
            {
                if (quickArray[i] == null)
                {
                    quickArray[i] = newItem;
                    isQuickPush = true;
                    isUpdate = true;
                    break;
                }
            }
            if(isQuickPush == false)
            {
                for (int i = 0; i < itemArray.Length; i++)
                {
                    if (itemArray[i] == null)
                    {
                        itemArray[i] = newItem;
                        isUpdate = true;
                        break;
                    }
                }
            }
        }

        if (isUpdate)
        {
            UpdateInven();
        }
    }

    public void Drop(int index)
    {
        DropToObject(itemArray[index]);
        itemArray[index] = null;
    }
    private void DropToObject(Item dropItem)
    {
        ItemObject itemObj = dropItem.GetDropObject();
        Transform dropPivot = PlayerState.Instance.DropPivot;

        itemObj.transform.position = dropPivot.position;
        itemObj.GetComponent<Rigidbody>().AddForce(dropPivot.forward * 5f, ForceMode.Impulse);
    }
    public void ItemDecrease()
    {
        quickArray[QuickSlot.QuickIndex - 100].ItemCountDecrease(1);

        if (quickArray[QuickSlot.QuickIndex - 100].Count <= 0)
        {
            quickArray[QuickSlot.QuickIndex - 100] = null;
            QuickSlotController.Instance.OnChangeWeapon(null);
        }

        UpdateInven();
    }

    public void ChangeQuickSlot()
    {
        UpdateInven();

        Item item = quickArray[QuickSlot.QuickIndex - 100];

        if (item == null)
            QuickSlotController.Instance.OnChangeWeapon(null);
        else
            QuickSlotController.Instance.OnChangeWeapon(item);
    }

    public bool UseMaterials(Recipe recipe)
    {
        Recipe.Material[] materialArray = recipe.MaterialArray;

        for (int i = 0; i < materialArray.Length; i++)
        {
            int haveCount = GetItemCount(materialArray[i].item);
            int needCount = materialArray[i].count;
            if (haveCount < needCount)
                return false;
        }

        for (int i = 0; i < materialArray.Length; i++)
            Remove(materialArray[i].item, materialArray[i].count);

        return true;
    }
    private bool Remove(Item targetItem, int decrease)
    {
        if (GetItemCount(targetItem) < decrease)
            return false;

        for (int i = 0; i < itemArray.Length; i++)
        {
            if (itemArray[i] == null)
                continue;

            if (itemArray[i] != targetItem)
                continue;

            int itemCount = itemArray[i].Count;

            if (itemCount <= decrease)
            {
                itemArray[i] = null;
                decrease -= itemCount;
            }
            else
            {
                itemArray[i] -= decrease;
                decrease = 0;
                break;
            }
        }
        if(decrease >= 1)
        {
            for (int i = 0; i < quickArray.Length; i++)
            {
                if (quickArray[i] == null)
                    continue;

                if (quickArray[i] != targetItem)
                    continue;

                int itemCount = quickArray[i].Count;

                if (itemCount <= decrease)
                {
                    quickArray[i] = null;
                    decrease -= itemCount;
                }
                else
                {
                    quickArray[i] -= decrease;
                    decrease = 0;
                    break;
                }
            }
        }

        UpdateInven();
        return true;
    }
    public int GetItemCount(Item item)
    {
        int total = 0;
        // 인벤토리 검사
        for(int i=0; i< itemArray.Length; i++)
        {
            if(itemArray[i] != null)
            {
                if (itemArray[i] == item)
                    total += itemArray[i].Count;
            }
        }
        // 퀵슬롯 검사
        for (int i = 0; i < quickArray.Length; i++)
        {
            if (quickArray[i] != null)
            {
                if (quickArray[i] == item)
                    total += quickArray[i].Count;
            }
        }

        return total;
    }

    public int AddDefense()
    {
        int addPoint = 0;

        for(int i= 0; i < equipArray.Length; i++)
        {
            if(equipArray[i] != null)
            {
                addPoint += equipArray[i].DefensePoint;
            }
        }

        return addPoint;
    }

    public float AddHp()
    {
        float addPoint = 0;

        for (int i = 0; i < equipArray.Length; i++)
        {
            if (equipArray[i] != null)
            {
                addPoint += equipArray[i].HpPoint;
            }
        }

        return addPoint;
    }

    private void UpdateInven()
    {
        OnUpdateInven(itemArray, quickArray, equipArray, dragItem);
    }
    // ============ DRAG EVENT =============================================
    private Item[] GetInven(int index)
    {
        Item[] inven = null;

        if (index < 100)
            inven = itemArray;
        else if (index >= 100 && index < 1000)
            inven = quickArray;
        else if (index >= 1000 && index < 10000)
            inven = equipArray;

        return inven;
    }
    private int GetIndex(int index)
    {
        int getIndex = 0;

        if (index < 100)
            getIndex = 0;
        else if (index >= 100 && index < 1000)
            getIndex -= 100;
        else if (index >= 1000 && index < 10000)
            getIndex -= 1000;

        return getIndex;
    }

    private MOVE_RESULT GetMoveResult(int beginIndex, int targetIndex, bool isDrop)
    {
        if (isDrop)
        {
            return MOVE_RESULT.Drop;
        }
        else
        {
            Item[] beginInven = GetInven(beginIndex);
            Item[] targetInven = GetInven(targetIndex);

            beginIndex += GetIndex(beginIndex);
            targetIndex += GetIndex(targetIndex);

            if (dragItem is Equip && targetInven == equipArray)
                return MOVE_RESULT.Wear;

            if (targetInven == equipArray)
                return MOVE_RESULT.Cancel;

            if ((beginIndex == targetIndex) || (targetIndex < 0))
                return MOVE_RESULT.Cancel;

            else if (targetInven[targetIndex] == null)
                return MOVE_RESULT.Move;

            else if (targetInven[targetIndex] != null)
            {
                if (targetInven[targetIndex] == dragItem)
                    return MOVE_RESULT.Merge;
                else if (beginInven[beginIndex] == null)
                    return MOVE_RESULT.Swap;
                else
                    return MOVE_RESULT.Cancel;
            }
        }

        return MOVE_RESULT.None;
    }

    public void OnDragBegin(int index)
    {
        Item[] inven = GetInven(index);
        index += GetIndex(index);

        if (inven[index] == null)
            return;

        if (Input.GetMouseButton(1))
        {
            dragItem = inven[index].Split();
            inven[index] = (inven[index].Count <= 0) ? null : inven[index];         
        }
        else
        {
            dragItem = inven[index];
            inven[index] = null;
        }

        UpdateInven();
    }
    public void OnDragEnd(int beginIndex, int targetIndex, bool isDrop)
    {
        MOVE_RESULT result = GetMoveResult(beginIndex, targetIndex, isDrop);

        Item[] beginInven = GetInven(beginIndex);
        Item[] targetInven = GetInven(targetIndex);

        beginIndex += GetIndex(beginIndex);
        targetIndex += GetIndex(targetIndex);

        switch (result)
        {
            case MOVE_RESULT.Swap:
                beginInven[beginIndex] = targetInven[targetIndex];
                targetInven[targetIndex] = dragItem;
                break;
            case MOVE_RESULT.Move:
                {
                    targetInven[targetIndex] = dragItem;
                }
                break;
            case MOVE_RESULT.Merge:
                {
                    targetInven[targetIndex].MergeItem(dragItem); // 옮기려는 칸에 병합.

                    if (dragItem.Count > 0 && beginInven[beginIndex] == null)
                        beginInven[beginIndex] = dragItem;
                    else if (dragItem.Count > 0 && beginInven[beginIndex] != null)
                        beginInven[beginIndex].MergeItem(dragItem);
                }
                break;
            case MOVE_RESULT.Wear:
                {
                    if(dragItem is Equip)
                    {
                        Equip item = dragItem as Equip;
                        if(targetIndex == (int)item.Type)
                        {
                            Debug.Log(targetIndex);
                            targetInven[targetIndex] = item;
                        }
                        else if( targetIndex != (int)item.Type)
                        {
                            Push(dragItem);
                        }
                        else
                        {
                            beginInven[beginIndex].MergeItem(dragItem);
                        }
                    }
                }
                break;
            case MOVE_RESULT.Cancel:
                {
                    if (beginInven[beginIndex] == null)
                        beginInven[beginIndex] = dragItem;
                    else
                        beginInven[beginIndex].MergeItem(dragItem);
                }
                break;
            case MOVE_RESULT.Drop:
                {
                    DropToObject(dragItem);
                }
                break;
        }

        dragItem = null;
        UpdateInven();
    }
}