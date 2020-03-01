using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SoldierBaseAI : MonoBehaviour
{
    //动画控制及AI相关
    private GameObject[] TargetsUnit;
    private GameObject TargetUnit;          //获取玩家单位
    private Animator thisAnimator;          //自身动画组件
    private Vector3 initialPosition;            //初始位置
    private NavMeshAgent agent;
    public float attackRange = 2f;            //攻击距离
    public float walkSpeed = 1;          //移动速度
    public float turnSpeed = 0.1f;         //转身速度，建议0.1
    public string targetTagName = "Player";
    private enum SoldierState
    {
        Idle,      //原地
        Chase,      //追击玩家
        Attack,     //攻击
        Damage,
        Death
    }
    private SoldierState currentState = SoldierState.Idle;          //默认状态为原地    
    public float actRestTme = 5;            //更换待机指令的间隔时间
    private float lastActTime;          //最近一次指令时间
    private float distanceToTarget;         //与目标的距离
    private float distanceToInitial;         //怪物与初始位置的距离
    private Quaternion targetRotation;         //怪物的目标朝向    
    private bool is_Running = false;
    private bool _bIsHasTarget = false;
    private SoldierState CurrentState { get => currentState; set => currentState = value; }
    public SoldierType soldierType;

    private bool _isSoldierDeathPlayOnce = true;
    private SoldierBaseAI targetAI;

    //属性相关
    public int SoldierAttack = 0;
    public int SoldierDefence = 0;
    public int IntMaxHealth;   //最大的生命值
    public float FloCurrentHealth; //当前的生命数值
    public int SkillHurtPower=1;
    public int DestroyTime = 3;

    //UI相关
    public Slider HPSlider;

    private void OnEnable()
    {
        _isSoldierDeathPlayOnce = true;
    }

    void Start()
    {
        thisAnimator = GetComponent<Animator>();
        //保存初始位置信息
        initialPosition = gameObject.GetComponent<Transform>().position;
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackRange;
        agent.speed = walkSpeed;
        agent.angularSpeed = turnSpeed;

    }

    void FindTarget()
    {
        TargetsUnit = GameObject.FindGameObjectsWithTag(targetTagName);
        if (TargetsUnit.Length > 0)
        {
            _bIsHasTarget = true;

            //生成的地方士兵乱移动应该是这里造成的
            int ranTarget = Random.Range(0, TargetsUnit.Length);
            //随机目标
            TargetUnit = TargetsUnit[0];

            if (!targetAI)
            {
                targetAI = TargetUnit.GetComponent<SoldierBaseAI>();
            }
        }
        else
        {
            _bIsHasTarget = false;
        }

    }

    void Update()
    {
        FindTarget();

        //更新血条状态
        if (currentState != SoldierState.Death)
        {
            this.HPSlider.value = FloCurrentHealth / IntMaxHealth;
        }

        switch (currentState)
        {
            //待机状态，等待actRestTme后重新随机指令
            case SoldierState.Idle:
                thisAnimator.SetInteger("Run", -1);
                thisAnimator.SetBool("Attack1", false);
                thisAnimator.SetBool("Attack2", false);
                thisAnimator.SetBool("Attack3", false);
                thisAnimator.SetBool("Damage", false);
                //该状态下的检测指令
                EnemyDistanceCheck();
                break;
            //追击状态，朝着玩家跑去
            case SoldierState.Chase:
                thisAnimator.SetBool("Attack1", false);
                thisAnimator.SetBool("Attack2", false);
                thisAnimator.SetBool("Attack3", false);
                thisAnimator.SetInteger("Run", 1);
                agent.isStopped = false;
                //该状态下的检测指令
                ChaseRadiusCheck();

                break;
            case SoldierState.Attack:
                thisAnimator.SetInteger("Run", -1);
                agent.isStopped = true;
                AttackTarget();
                if (thisAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    currentState = SoldierState.Idle;
                }
                break;
            case SoldierState.Damage:
                thisAnimator.SetBool("Damage", true);
                if (thisAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    currentState = SoldierState.Idle;
                }
                break;
            case SoldierState.Death:
                thisAnimator.SetBool("Damage", false);
                if (_isSoldierDeathPlayOnce)
                {
                    thisAnimator.SetTrigger("Death");
                    _isSoldierDeathPlayOnce = false;
                }
                break;
        }
    }

    void AttackTarget()
    {
        if (targetAI.currentState != SoldierState.Death)
        {
            if (_bIsHasTarget)
            {
                UnityHelper.FaceToGo(this.transform, TargetUnit.transform, turnSpeed);

                if (soldierType == SoldierType.BatMan)
                {
                    thisAnimator.SetBool("Attack1", true);
                }else if (soldierType == SoldierType.Elite)
                {
                    thisAnimator.SetBool("Attack2", true);
                }
            }
            else
            {
                currentState = SoldierState.Idle;
            }
        }
    }

    /// <summary>
    /// 原地呼吸、观察状态的检测
    /// </summary>
    void EnemyDistanceCheck()
    {
        //检测场景中有没有目标
        if (_bIsHasTarget)
        {
            distanceToTarget = Vector3.Distance(TargetUnit.transform.position, transform.position);

            //检测到有目标在场景中，并且小于攻击距离
            if (distanceToTarget < attackRange)
            {
                currentState = SoldierState.Attack;
            }
            else
            {
                currentState = SoldierState.Chase;          //否则一直追击目标
            }
        }
        else
        {
            currentState = SoldierState.Idle;
        }

    }

    /// <summary>
    /// 追击状态检测，检测目标是否进入攻击范围
    /// </summary>
    void ChaseRadiusCheck()
    {
        if (_bIsHasTarget)
        {
            if (!is_Running)
            {
                thisAnimator.SetInteger("Run", 1);
                is_Running = true;
            }
            distanceToTarget = Vector3.Distance(TargetUnit.transform.position, transform.position);
            //transform.Translate(Vector3.forward * Time.deltaTime * runSpeed);
            UnityHelper.FaceToGo(this.transform, TargetUnit.transform, turnSpeed);
            //agent.destination = TargetUnit.transform.position;
            agent.SetDestination(TargetUnit.transform.position);    //改为用Unity的代理AI完成寻路
            if (distanceToTarget < attackRange)
            {
                currentState = SoldierState.Attack;

            }
            else
            {
                currentState = SoldierState.Chase;
            }
        }
    }

    #region 属性相关
    public void CheckLife()
    {
        //敌人当前的血量值已经耗尽 设置为死亡状态
        if (FloCurrentHealth <= IntMaxHealth * 0.01)
        {
            //更换状态
            currentState = SoldierState.Death;

            //统计剩余士兵数量
            if (this.gameObject.tag == SysDefine.SYS_TAG_PLAYER)
                BattleManager.GetInstance().AlivePlayer--;
            if (this.gameObject.tag == SysDefine.SYS_TAG_ENEMY)
                BattleManager.GetInstance().AliveEnemys--;

            Debug.Log("剩余Player数量=" + BattleManager.GetInstance().AlivePlayer);
            Debug.Log("剩余Enemy数量=" + BattleManager.GetInstance().AliveEnemys);

            //利用缓冲池的技术对敌人进行回收，而不是直接销毁掉
            StartCoroutine("RecoverEnemy");
        }
    }

    //伤害处理
    public void OnHurt(int hurtValue)
    {
        //当前的状态设置为受伤
        currentState = SoldierState.Damage;

        int hurtValus = 0;
        hurtValus = Mathf.Abs(hurtValue);   //取绝对值  伤害值不会是负数
        if (hurtValus > 0)
        {
            FloCurrentHealth -= hurtValus;
        }
        //Debug.Log("当前剩余血量:" + FloCurrentHealth);

        CheckLife();
    }

    //回收对象
    IEnumerator RecoverEnemy()
    {
        yield return new WaitForSeconds(DestroyTime);

        //敌人回收前，重置敌人的属性
        FloCurrentHealth = IntMaxHealth;
        currentState = SoldierState.Idle;
        Destroy(this.gameObject);        //延迟1秒销毁这个对象   应该放到池子 TODO...
    }
    #endregion

    /// <summary>
    /// 伤害目标 注册在动画上
    /// </summary>
    public void HurtTarget()
    {
        //伤害公式 Damage = a*ATK-b*targetDEF
        float damage = SoldierAttack * SkillHurtPower - TargetUnit.GetComponent<SoldierBaseAI>().SoldierDefence;
        if (damage <= 0)
        {
            damage = 1;
        }
        Debug.Log("造成的伤害：" + damage);
        TargetUnit.SendMessage("OnHurt", damage, SendMessageOptions.DontRequireReceiver);
    }

}
