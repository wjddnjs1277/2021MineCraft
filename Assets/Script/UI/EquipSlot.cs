using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipSlot : Slot
{
    public static int EquipIndex = 0;
    public static int MaxEquipIndex = 0;

    protected override void Init()
    {
        base.Init();
        Index += 1000;

        if (MaxEquipIndex < 1000)
        {
            EquipIndex = 1000;
            MaxEquipIndex = 1000;
        }

        MaxEquipIndex += 1;
    }
    public override void Clear()
    {
        base.Clear();
    }

    public override void OnCursorEnter()
    {
        base.OnCursorEnter();
        SelectedSlot += 1000;
    }
}
