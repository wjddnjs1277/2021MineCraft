using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlot : Slot
{
    [SerializeField] Image selectedImage;

    public static int QuickIndex = 0;
    public static int MaxQuickIndex = 0;

    protected override void Init()
    {
        base.Init();
        Index += 100;

        if (MaxQuickIndex < 100)
        {
            QuickIndex = 100;
            MaxQuickIndex = 100;
        }

        MaxQuickIndex += 1;
    }
    public override void Clear()
    {
        base.Clear();

        selectedImage.enabled = (Index == QuickIndex);
    }
    public override void Setup(Item item)
    {
        base.Setup(item);

        selectedImage.enabled = (Index == QuickIndex);
    }
    public override void OnCursorEnter()
    {
        base.OnCursorEnter();
        SelectedSlot += 100;
    }
}
