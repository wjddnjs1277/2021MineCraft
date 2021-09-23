using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct KeyEvent
{
    public KeyCode key;
    public Action OnCallback;

    public KeyEvent(KeyCode key, Action OnCallback)
    {
        this.key = key;
        this.OnCallback = OnCallback;
    }
}

public class PlayerState : Stateable
{
    static PlayerState instance;

    public static PlayerState Instance => instance;

    [SerializeField] Transform dropPivot;
    [SerializeField] bool isStopControl;
    [SerializeField] bool isStopMove;
    [SerializeField] bool isStopRotate;
    [SerializeField] bool isCraft;

    [Header("Item")]
    [SerializeField] float getItemRange;
    [SerializeField] LayerMask itemLayer;

    public float maxThirst;      // 목마름
    public float maxHungry;      // 배고픔

    float thirst;
    float hungry;

    Dictionary<KeyCode, Action> eventList;
    Inventory inven;

    int addDefense;
    int addAttack;

    public Transform DropPivot => dropPivot;
    public Inventory Inven => inven;
    public bool IsStopMove => isStopMove;
    public bool IsStopRotate => isStopRotate;
    public bool IsCraft => isCraft;

    public override float Hp
    {
        get
        {
            return base.Hp;
        }
        set
        {
            base.Hp = Mathf.Clamp(value, 0, maxHp);
            StatusManager.Instance.UpdateGaugesBar(StatusManager.StatusType.Hp, base.Hp, maxHp);
        }
    }
    public float Hungry
    {
        get
        {
            return hungry;
        }
        set
        {
            hungry = Mathf.Clamp(value, 0, maxHungry);
            StatusManager.Instance.UpdateGaugesBar(StatusManager.StatusType.Hungry, hungry, maxHungry);
        }
    }
    public float Thirst
    {
        get
        {
            return thirst;
        }
        set
        {
            thirst = Mathf.Clamp(value, 0, maxThirst);
            StatusManager.Instance.UpdateGaugesBar(StatusManager.StatusType.Thirst, thirst, maxThirst);
        }
    }
    public int AddDefense => addDefense;
    public int AddAttack => addAttack;

    protected new void Awake()
    {
        base.Awake();
        instance = this;

        inven = new Inventory();
        eventList = new Dictionary<KeyCode, Action>();
    }
    private void Start()
    {
        thirst = maxThirst;
        hungry = maxHungry;

        addDefense = 0;
        addAttack = 0;

        StartCoroutine(FindItem());
        StartCoroutine(DecreaseStatus());
    }
    private void Update()
    {
        if (isStopControl)
            return;


        if (Input.anyKeyDown)
        {
            for (KeyCode type = 0; type < KeyCode.Mouse6; type++)
            {
                if (Input.GetKeyDown(type))
                {
                    if (eventList.ContainsKey(type))
                    {
                        eventList[type]?.Invoke();
                    }
                    break;
                }
            }
        }

        if(isStopMove != true && isStopRotate != true)
        {
            float mouseWheelY = Input.mouseScrollDelta.y;
            if (mouseWheelY > 0)
            {
                QuickSlot.QuickIndex = Mathf.Clamp(QuickSlot.QuickIndex - 1, 100, QuickSlot.MaxQuickIndex - 1);
                inven.ChangeQuickSlot();
            }
            else if (mouseWheelY < 0)
            {
                QuickSlot.QuickIndex = Mathf.Clamp(QuickSlot.QuickIndex + 1, 100, QuickSlot.MaxQuickIndex - 1);
                inven.ChangeQuickSlot();
            }
        }

        UpdateAbility();
    }

    public void UpdateAbility()
    {
        addAttack = QuickSlotController.Instance.AddAttack();
        addDefense = inven.AddDefense();
    }
    public void UpdateStatus()
    { 
    }

    public void OnRegestedEvent(KeyCode key, Action OnCallback)
    {
        if (!eventList.ContainsKey(key))
            eventList.Add(key, null);

        eventList[key] += OnCallback;
    }

    public void UpdateState()
    {
        UpdateState(false, false, false, false);
    }
    public void UpdateState(bool isStopControl, bool isStopMove, bool isStopRotate, bool isCraft)
    {
        this.isStopControl = isStopControl;
        this.isStopMove = isStopMove;
        this.isStopRotate = isStopRotate;
        this.isCraft = isCraft;
    }

    IEnumerator FindItem()
    {
        while (true)
        {
            ItemObject itemObj = null;
            Collider[] hits = Physics.OverlapSphere(transform.position, getItemRange, itemLayer);

            foreach (Collider hit in hits)
            {
                itemObj = hit.GetComponent<ItemObject>();

                if(itemObj != null)
                {
                    //itemObj.GetComponent<Collider>().enabled = false;
                    Vector3 dir = transform.position - itemObj.transform.position;
                    itemObj.transform.rotation = Quaternion.LookRotation(dir);

                    itemObj.GetComponent<Rigidbody>().AddForce(dir.normalized * 10f, ForceMode.Impulse);


                    if (Vector3.Distance(transform.position, itemObj.transform.position) <= 0.7f)
                        itemObj.GainItem();
                }
            }

            yield return null;
        }
    }
    IEnumerator DecreaseStatus()
    {
        while(Hp > 0)
        {
            if(Hp < 100 && Hungry >= 50 && Thirst >= 50)
            {
                Hp += 1f;
                Hungry -= 2f;
                Thirst -= 2f;
            }
            else
            {
                Hungry -= 1f;
                Thirst -= 1f;
            }

            yield return new WaitForSeconds(1);
        }
    }
}
