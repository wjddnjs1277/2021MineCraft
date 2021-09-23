using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] Transform groundChecker;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundDistance;

    Item item;
    Rigidbody rigid;

    float scale = 1;
    float heightScale = 0.3f;
    float x;
    float y;
    float z;

    bool isGround;
    bool isNearPlayer;

    public bool IsNearPlayer
    {
        get
        {
            return isNearPlayer;
        }
        set
        {
            isNearPlayer = value;
        }
    }

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();

        gameObject.layer = 0;

        isGround = false;
        isNearPlayer = false;
    }

    private void FixedUpdate()
    {
        if(Physics.CheckSphere(groundChecker.position, groundDistance * 0.5f, groundLayer) == false)
        {
            rigid.useGravity = true;
            isGround = false;
        }

        ItemWaveMove();
    }

    void ItemWaveMove()
    {
        if (isGround == false)
            return;

        float moveY = heightScale * Mathf.PerlinNoise(Time.time + (x * scale),
                                            Time.time + (z * scale));
        transform.position = new Vector3(x, moveY + y, z);
    }

    void ChangeLayer()
    {
        gameObject.layer = LayerMask.NameToLayer("Item");
    }

    public Item GetItem()
    {
        return item;
    }
    public void SetItem(Item item)
    {
        this.item = item;
    }  
    public void GainItem()
    {
        Item copy = item.GetCopy();
        PlayerState.Instance.Inven.Push(copy);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGround = true;
            rigid.useGravity = false;
            x = transform.position.x;
            y = transform.position.y;
            z = transform.position.z;
            Invoke("ChangeLayer", 0.5f);
        }
    }
}
