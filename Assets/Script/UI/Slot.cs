using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] Image Back;
    [SerializeField] Image itemIcon;
    [SerializeField] Text itemCount;
    [SerializeField] Image Cilck;

    public event System.Action<Slot> OnDragBegin;
    public event System.Action<Slot> OnDragEnd;
    public event System.Action OnDragging;
    public event System.Action<int> OnEnterSlot;

    public event System.Action<Slot> ShowItemInfo;
    public event System.Action CloseItemInfo;

    protected Item item;

    public static int SelectedSlot
    {
        get;
        set;
    }
    
    public int Index
    {
        get;
        protected set;
    }

    public Item InItem => item;

    private void Start()
    {
        Init();
    }
    protected virtual void Init()
    {
        Index = transform.GetSiblingIndex();
    }

    public virtual void Setup(Item item)
    {
        this.item = item;

        if (item == null)
        {
            Clear();
        }
        else
        {
            itemIcon.enabled = true;
            itemIcon.sprite = item.ItemSprite;
            itemCount.text = (item.Count > 1) ? item.Count.ToString() : string.Empty;
            Cilck.enabled = false;
        }
    }
    public void Setup(Slot slot)
    {
        Setup(slot.item);
    }

    public virtual void Clear()
    {
        item = null;
        itemIcon.enabled = false;
        itemCount.text = string.Empty;
    }

    private void OnEnable()
    {
        Cilck.enabled = false;
    }

    // 접촉 함수
    public virtual void OnCursorEnter()
    {
        Cilck.enabled = true;
        SelectedSlot = transform.GetSiblingIndex();

        ShowItemInfo?.Invoke(this);
    }
    public virtual void OnCursorExit()
    {
        Cilck.enabled = false;
        SelectedSlot = -1;
        CloseItemInfo?.Invoke();
    }

    // 클릭 함수
    public virtual void OnCursorClick()
    {
        
    }

    // 드래그 함수
    public virtual void OnCursorBeginDrag()
    {
        OnDragBegin?.Invoke(this);
    }
    public virtual void OnCursorEndDrag()
    {
        OnDragEnd?.Invoke(this);
    }
    public virtual void OnCursorDraging()
    {
        OnDragging?.Invoke();
    }

}
