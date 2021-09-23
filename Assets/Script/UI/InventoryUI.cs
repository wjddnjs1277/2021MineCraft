using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : Singleton<InventoryUI>
{
    [SerializeField] Transform slotParent;
    [SerializeField] Transform quickSlotParent;
    [SerializeField] Transform equipSlotParent;
    [SerializeField] Slot dummySlot;
    [SerializeField] Image infoBar;
    [SerializeField] Text infoText;
    [SerializeField] Text invenText;

    [Header("Rect")]
    [SerializeField] Image invenBack;
    [SerializeField] Image quickBack;

    [Header("Function")]
    [SerializeField] GameObject CreaftTabel;

    Slot[] allSlots;
    QuickSlot[] allQuickSlots;
    EquipSlot[] allEquipSlots;


    PlayerState player;
    Inventory inven;
    

    private void Start()
    {
        player = PlayerState.Instance;
        inven = PlayerState.Instance.Inven;

        PlayerState.Instance.OnRegestedEvent(KeyCode.E, SwitchInventory);

        Clear();

        infoBar.enabled = false;
        infoText.text = string.Empty;
        invenText.text = player.IsCraft ? "CRAFTING" : "INVENTORY";

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        // 초기화.
        if (allSlots == null && allQuickSlots == null)
        {
            allSlots = slotParent.GetComponentsInChildren<Slot>(true);
            allQuickSlots = quickSlotParent.GetComponentsInChildren<QuickSlot>(true);
            allEquipSlots = equipSlotParent.GetComponentsInChildren<EquipSlot>(true);

            PlayerState.Instance.Inven.OnUpdateInven += OnUpdateInventory;

            foreach(Slot slot in allSlots)
            {
                slot.ShowItemInfo += OnShowInfo;
                slot.CloseItemInfo += OnCloseInfo;
                slot.OnDragBegin += OnDragBegin;
                slot.OnDragging += OnDragging;
                slot.OnDragEnd += OnDragEnd;
            }

            foreach(QuickSlot slot in allQuickSlots)
            {
                slot.ShowItemInfo += OnShowInfo;
                slot.CloseItemInfo += OnCloseInfo;
                slot.OnDragBegin += OnDragBegin;
                slot.OnDragging += OnDragging;
                slot.OnDragEnd += OnDragEnd;
            }

            foreach (EquipSlot slot in allEquipSlots)
            {
                slot.ShowItemInfo += OnShowInfo;
                slot.CloseItemInfo += OnCloseInfo;
                slot.OnDragBegin += OnDragBegin;
                slot.OnDragging += OnDragging;
                slot.OnDragEnd += OnDragEnd;
            }
        }

        Cursor.lockState = CursorLockMode.None;     // 커서 락 해제.
    }
    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;   // 커서 락.
        CreaftTabel.SetActive(false);
    }

    public void SwitchInventory()
    {
        gameObject.SetActive(!gameObject.activeSelf);

        if (gameObject.activeSelf)
            PlayerState.Instance.UpdateState(false, true, true, false);
        else
            PlayerState.Instance.UpdateState();

        invenText.text = player.IsCraft ? "CRAFTING" : "INVENTORY";
    }
    public void SwtichCraftingTable()
    {
        CreaftTabel.SetActive(!gameObject.activeSelf);

        PlayerState.Instance.UpdateState(false, true, true, true);

        invenText.text = player.IsCraft ? "CRAFTING" : "INVENTORY";

        gameObject.SetActive(!gameObject.activeSelf);
    }

    void Clear()
    {
        foreach (Slot slot in allSlots)
        {
            slot.Clear();
        }
        foreach (QuickSlot slot in allQuickSlots)
        {
            slot.Clear();
        }
        foreach (EquipSlot slot in allEquipSlots)
        {
            slot.Clear();
        }
    }
    void OnUpdateInventory(Item[] itemArray, Item[] quickArray, Equip[] equipArray, Item dragItem)
    {
        if (player.IsCraft)
            RecipeUIManager.Instance.Clear();

        Clear();

        for (int i = 0; i < itemArray.Length; i++)
            allSlots[i].Setup(itemArray[i]);

        for (int i = 0; i < quickArray.Length; i++)
            allQuickSlots[i].Setup(quickArray[i]);

        for (int i = 0; i < equipArray.Length; i++)
            allEquipSlots[i].Setup(equipArray[i]);

        dummySlot.Setup(dragItem);
    }


    private void OnShowInfo(Slot slot)
    {
        if (slot.InItem == null)
            return;

        infoBar.enabled = true;
        infoBar.transform.position = slot.transform.position;
        infoText.text = slot.InItem.ItemName;
    }
    private void OnCloseInfo()
    {
        infoText.text = string.Empty;
        infoBar.enabled = false;
    }

    private void OnDragBegin(Slot slot)
    {
        if (slot.InItem == null)
            return;

        inven.OnDragBegin(slot.Index);
        dummySlot.gameObject.SetActive(dummySlot.InItem != null);
    }
    private void OnDragging()
    {
        if (dummySlot.InItem == null)
            return;

        // 드래그 중...
        dummySlot.transform.position = Input.mousePosition;
    }
    private void OnDragEnd(Slot slot)
    {
        if (dummySlot.InItem == null)
            return;

        dummySlot.gameObject.SetActive(false);

        int beginIndex = slot.transform.GetSiblingIndex();
        int lastIndex = Slot.SelectedSlot;

        bool isInInventory = (invenBack.enabled && IsCursorInRect(invenBack)) || IsCursorInRect(quickBack);

        inven.OnDragEnd(beginIndex, lastIndex, !isInInventory);


        //if (lastIndex < 100)
        //    allSlots[lastIndex].OnCursorEnter();
        //else if (lastIndex >= 100 && lastIndex < 1000)
        //    allQuickSlots[lastIndex - 100].OnCursorEnter();
        //else if (lastIndex >= 1000 && lastIndex < 10000)
        //    allCraftSlots[lastIndex - 1000].OnCursorEnter();
        //else
        //    return;
    }

    private bool IsCursorInRect(Image image)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(image.rectTransform, Input.mousePosition);
    }

}
