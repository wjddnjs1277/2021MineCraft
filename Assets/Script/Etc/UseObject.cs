using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseObject : MonoBehaviour
{
    enum Info
    {
        Craft,
    }

    [SerializeField] Info infoType;

    public void ShowUI()
    {
        switch (infoType)
        {
            case Info.Craft:
                PlayerState.Instance.UpdateState(false, true, true, true);
                InventoryUI.Instance.SwtichCraftingTable();
                break;        
        }
    }
}
