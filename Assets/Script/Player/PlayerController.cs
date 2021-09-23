using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Info")]
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float jumpHight;

    [Header("Check")]
    [SerializeField] Transform groundChecker;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundDistance;

    Animator anim;
    Rigidbody rigid;

    bool isRun;
    bool isGround;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (PlayerState.Instance.IsStopMove)
            return;

        Jump();
        Move();        
    }
    private void FixedUpdate()
    {
        isGround = Physics.CheckSphere(groundChecker.position, groundDistance * 0.1f, groundLayer);
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        isRun = Input.GetKey(KeyCode.LeftShift);

        Vector3 direction = (transform.right * x) + (transform.forward * z);
        float moveSpeed = (isRun ? runSpeed : walkSpeed);
        direction.Normalize();
        transform.position += direction * moveSpeed * Time.deltaTime;

        anim.SetFloat("Horizontal", x);
        anim.SetFloat("Vertical", z);
    }
    void Jump()
    {
        // 내가 땅에 있으면서 점프 키를 눌렀을 때.
        if (isGround && Input.GetKeyDown(KeyCode.Space))
        {
            rigid.velocity = new Vector3(0, rigid.velocity.y, 0);
            rigid.AddForce(Vector3.up * jumpHight, ForceMode.Impulse);
        }
    }


    Coroutine knockback;
    public void OnDamaged(Damageable.DamageMessage message)
    {
        Vector3 dir = transform.position - message.target.transform.position;

        if (knockback != null)
            StopCoroutine(knockback);

        knockback = StartCoroutine(OnKnockback(dir, 3f));
    }
    IEnumerator OnKnockback(Vector3 dir, float distance)
    {
        dir = dir + Vector3.up;
        rigid.AddForce(dir * distance, ForceMode.Impulse);

        yield return new WaitForSeconds(0.5f);
    }
}
