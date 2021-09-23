using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    enum STATE
    {
        Idle,           // 기본 자세
        Walk,           // 패트롤
        Run,            // 추격
        Attack,         // 공격
        AttackWait,     // 공격 후
        Damaged,
        Dead,
    }

    // 생성후 일정범위를 탐색
    // 플레이어가 탐색되면 계속 추격
    // 일정 거리가 되면 공격 아니면 추격
    // 플레이어와의 거리가 너무 멀어지면 되돌아감
    // 이동 중 1칸 높이의 블럭은 올라갈수잇다.

    [SerializeField] LayerMask playerMask, groundMask;

    [Header("Patrol")]
    [SerializeField] float walkSpeed;       // 속도
    [SerializeField] float patrolRange;     // 패트롤 거리(범위)
    [SerializeField] float patrolRate;      // 패트롤 주기
    [SerializeField] float sight;           // 시야
    [SerializeField] float runSpeed;        // 추격 속도

    [Header("Attack")]
    [SerializeField] float attackRange;     // 공격 거리
    [SerializeField] float attackRate;      // 공격 주기
    [SerializeField] Transform attackPivot;

    [SerializeField] Item item;

    public event System.Action<int> OnDeadEnemy;

    Transform player;
    Animator anim;
    Attackable attackable;
    Stateable stat;
    Rigidbody rigid;

    Vector3 patrolPos;
    STATE state = STATE.Idle;

    float nextAttackTime;
    float nextPatrolTime;

    int spawnIndex;

    bool isFindTarget;
    bool isDamaged;

    private void Start()
    {
        player = PlayerState.Instance.transform;
        anim = GetComponentInChildren<Animator>();
        stat = GetComponent<Stateable>();
        attackable = GetComponent<Attackable>();
        rigid = GetComponent<Rigidbody>();

        nextAttackTime = 0.0f;
        nextPatrolTime = 0.0f;

        StartCoroutine(TargetInSight());
        StartCoroutine(Patrol());
    }

    private void Update()
    {
        anim.SetBool("isAlive", stat.Hp > 0);
        anim.SetBool("isDamaged", isDamaged);

        Jump();
    }

    public void Setup(int index)
    {
        spawnIndex = index;
    }
    public void Respawn()
    {
        stat.Hp = stat.maxHp;
        gameObject.SetActive(true);
    }

    void OnChangeState(STATE state)
    {
        this.state = state;
        
        anim.SetBool("isWalk", state == STATE.Walk);
        anim.SetBool("isRun", state == STATE.Run);
        anim.SetBool("isAttack", state == STATE.Attack || state == STATE.AttackWait);
    }

    void Jump()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.forward, out hit, 1f, groundMask))
        {
            rigid.AddForce(Vector3.up * Time.deltaTime, ForceMode.Impulse);
        }

        // 몬스터 앞에 블록이 있을경우
        // 블록이 한칸이면 점프 아니면 x
    }

    IEnumerator Patrol()
    {
        while(true)
        {
            if(isFindTarget == false)
            {
                if (nextPatrolTime <= Time.time)
                {
                    float x = Random.Range(transform.position.x -patrolRange,
                                            transform.position.x + patrolRange);
                    float z = Random.Range(transform.position.z - patrolRange,
                                            transform.position.z + patrolRange);

                    patrolPos = new Vector3(x, transform.position.y, z);

                    Vector3 dir = patrolPos - transform.position;
                    transform.rotation = Quaternion.LookRotation(dir);
                    transform.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y, 0.0f);

                    yield return null;

                    nextPatrolTime = Time.time + patrolRate;
                }
                else if(Vector3.Distance(patrolPos, transform.position) <= 0.7f)
                {
                    OnChangeState(STATE.Idle);
                }
                else
                {
                    OnChangeState(STATE.Walk);
                    transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
                }
            }
            yield return null;
        }
    }

    IEnumerator TargetInSight()
    {
        while(true)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if(distance <= sight)
            {
                isFindTarget = true;

                Vector3 dir = player.position - transform.position;
                transform.rotation = Quaternion.LookRotation(dir);
                transform.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y, 0.0f);

                if (isDamaged == false && distance <= attackRange)
                {
                    if(nextAttackTime <= Time.time)
                    {
                        attackable.OnAttack(attackPivot.position, attackRange * 0.5f);
                        anim.SetTrigger("onAttack");
                        OnChangeState(STATE.Attack);

                        yield return null;

                        nextAttackTime = Time.time + attackRate;
                    }
                    else
                    {
                        OnChangeState(STATE.AttackWait);
                        yield return new WaitForSeconds(attackRate);
                    }
                }
                else
                {
                    if(stat.Hp > 0)
                    {
                        OnChangeState(STATE.Run);
                        transform.Translate(Vector3.forward * runSpeed * Time.deltaTime);
                    }                  
                }
            }
            else
                isFindTarget = false;

            yield return null;
        }
    }

    void Destroy()
    {
        gameObject.SetActive(false);
        OnDeadEnemy?.Invoke(spawnIndex);
        ItemObject itemObj = item.GetDropObject();
        itemObj.transform.position = transform.position;
        itemObj.GetComponent<Rigidbody>().AddForce(Vector3.up * 1.5f, ForceMode.Impulse);
    }

    public void OnDead()
    {
        gameObject.layer = LayerMask.NameToLayer("Dead");
        anim.SetBool("isAlive", false);
        anim.SetTrigger("onDead");
        Invoke("Destroy", 1f);
        enabled = false;
    }
    Coroutine knockback;
    public void OnDamaged(Damageable.DamageMessage message)
    {
        Vector3 dir = transform.position - message.target.transform.position;
        anim.SetTrigger("onDamaged");

        if (knockback != null)
            StopCoroutine(knockback);

        knockback = StartCoroutine(OnKnockback(dir, 2f));
    }
    IEnumerator OnKnockback(Vector3 dir, float distance)
    {
        STATE beforState = state;
        state = STATE.Damaged;

        isDamaged = true;
        dir = dir + Vector3.up;
        rigid.AddForce(dir * distance, ForceMode.Impulse);

        yield return new WaitForSeconds(0.5f);

        isDamaged = false;
        state = beforState;
    }
}
