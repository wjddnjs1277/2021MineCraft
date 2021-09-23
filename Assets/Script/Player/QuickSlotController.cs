using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotController : Singleton<QuickSlotController>
{
    [SerializeField] Transform handPivot;
    [SerializeField] GameObject rightHand;
    [SerializeField] float rayDistance;
    [SerializeField] LayerMask layer;
    [SerializeField] Transform attackPivot;
    [SerializeField] float attackDelay;

    GameObject quickSlotItmeObject;
    GameObject target;
    Item currentItem;

    Attackable attackable;
    PlayerState player;
    Animator anim;

    float currentTime = 0;

    private void Start()
    {
        player = PlayerState.Instance;
        anim = GetComponentInChildren<Animator>();
        attackable = GetComponent<Attackable>();
    }

    private void Update()
    {
        if (player.IsStopMove)
        {
            anim.SetBool("isAttack", false);
            return;
        }

        if(Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            anim.SetBool("isAttack", true);

            RaycastHit hitPoint;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitPoint, rayDistance, layer))
            {
                switch (hitPoint.collider.tag)
                {
                    case "Harvest":
                            Harvest(hitPoint);
                        break;
                    case "Enemy":
                            Attack(hitPoint);
                        break;
                }
            }
            else
            {
                currentTime = 0;
                GaugesBarManager.Instance.OnSwitch(false);
            }

        }
        else if( Input.GetMouseButtonDown(1))
        {            
            RaycastHit hitPoint;

            if(currentItem is UsedItem)
            {
                UseItem();
            }
            else
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitPoint, rayDistance, layer))
                {
                    switch (hitPoint.collider.tag)
                    {
                        case "Harvest":
                            UseObject obj = hitPoint.collider.GetComponent<UseObject>();
                            if (obj != null)
                                obj.ShowUI();
                            else
                                Build(hitPoint);
                            break;
                    }
                }
            }
        }
        else
        {
            currentTime = 0;
            GaugesBarManager.Instance.OnSwitch(false);
            anim.SetBool("isAttack", false);
        }  
    }

    void Attack(RaycastHit hit)
    {
        anim.SetBool("isAttack", true);
        attackable.OnAttack(attackPivot.position, 1);
    }
    void Harvest(RaycastHit hit)
    {
        if(target == null)
        {
            target = hit.collider.gameObject;
        }
        else if(target != hit.collider.gameObject)
        {
            target = null;
            currentTime = 0;
            GaugesBarManager.Instance.OnSwitch(false);
        }

        Harvest harvest = hit.collider.GetComponent<Harvest>();
        float farmingTime = harvest.FarmingTime;

        if(currentItem is Weapon)
        {
            Weapon currentWeapon = currentItem as Weapon;
            farmingTime = Mathf.Clamp(harvest.FarmingTime - currentWeapon.HarvestTimeDcrease,
                0.1f, harvest.FarmingTime);
        }

        if (harvest != null && currentTime <= farmingTime)
        {                        
            currentTime += Time.deltaTime;
            GaugesBarManager.Instance.OnSwitch(true);
            GaugesBarManager.Instance.UpdateGaugesBar(currentTime, farmingTime);

            if(currentTime >= farmingTime)
            {
                harvest.OnHarvesting();
                GaugesBarManager.Instance.OnSwitch(false);
                currentTime = 0;
            }
        }
    }
    void Build(RaycastHit hit)
    {
        if (currentItem == null)
            return;

        Vector3 surfaceVec = hit.normal;
        Vector3 hitCubePos = hit.transform.position;
        Vector3 buildPos = surfaceVec + hitCubePos;

        if (currentItem is ItemBlock)
        {
            ItemBlock block = currentItem as ItemBlock;
            GameObject obj = Instantiate(block.BuildObject, buildPos, Quaternion.identity);
            obj.layer = LayerMask.NameToLayer("Ground");

            player.Inven.ItemDecrease();
        }
    }
    void UseItem()
    {
        if(currentItem is UsedItem)
        {
            UsedItem item = currentItem as UsedItem;
            UsedItem.UsedType type = item.Type;

            switch(type)
            {
                case UsedItem.UsedType.Cure:
                    player.Hp += item.Amount;
                    break;
                case UsedItem.UsedType.Food:
                    player.Hungry += item.Amount;
                    break;
                case UsedItem.UsedType.Water:
                    player.Thirst += item.Amount;
                    break;
            }
            player.Inven.ItemDecrease();
        }
    }
    // ================ ƒ¸ΩΩ∑‘ æ∆¿Ã≈€ ¿Â¬¯ =====================
    public int AddAttack()
    {
        int addPoint = 0;
        if (currentItem is Weapon)
        {
            Weapon currentWeapon = currentItem as Weapon;
            addPoint = currentWeapon.AddAttack;
        }
        return addPoint;
    }
    public void OnChangeWeapon(Item item)
    {
        bool isBeforeEmpty = (currentItem == null);
        currentItem = item;

        if(isBeforeEmpty)       
        {
            if (currentItem != null)
                Equiped();
        }
        else
        {
            if (currentItem != null)
                Changed();
            else
                UnEquiped();
        }
    }
    private void Changed()
    {
        UnEquiped();
        Equiped();
    }
    private void Equiped()
    {
        quickSlotItmeObject = currentItem.GetObject();
        quickSlotItmeObject.GetComponent<Rigidbody>().isKinematic = true;
        quickSlotItmeObject.GetComponent<Collider>().enabled = false;
        quickSlotItmeObject.transform.SetParent(handPivot);
        quickSlotItmeObject.transform.localPosition = Vector3.zero;
        quickSlotItmeObject.transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);
        quickSlotItmeObject.transform.localEulerAngles = Vector3.zero;
        rightHand.SetActive(false);
    }
    private void UnEquiped()
    {
        Destroy(quickSlotItmeObject.gameObject);
        rightHand.SetActive(true);
    }

    private void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.color = Color.red;
        Gizmos.DrawWireSphere(attackPivot.position, 1f);
    }
}
