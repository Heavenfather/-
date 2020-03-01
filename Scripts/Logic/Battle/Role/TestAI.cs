//using System.Collections;
//using System.Collections.Generic;
//using Tools;
//using UnityEngine;
//using UnityEngine.AI;
//using UnityEngine.UI;

//public class TestAI : MonoBehaviour
//{
//    //动画控制及AI相关
//    private GameObject[] playsUnit;
//    private GameObject playerUnit;          //获取玩家单位
//    private Animator thisAnimator;          //自身动画组件
//    private Vector3 initialPosition;            //初始位置
//    private NavMeshAgent agent;
//    public float attackRange = 2f;            //攻击距离
//    public float walkSpeed = 1;          //移动速度
//    public float runSpeed = 1;          //跑动速度
//    public float turnSpeed = 0.1f;         //转身速度，建议0.1
//    public string targetTagName = "Player";
//    private enum MonsterState
//    {
//        Idle,      //原地
//        Chase,      //追击玩家
//        Attack,     //攻击
//        Damage,
//        Death
//    }
//    private MonsterState currentState = MonsterState.Idle;          //默认状态为原地    
//    public float actRestTme = 5;            //更换待机指令的间隔时间
//    private float lastActTime;          //最近一次指令时间
//    private float distanceToTarget;         //与目标的距离
//    private float distanceToInitial;         //怪物与初始位置的距离
//    private Quaternion targetRotation;         //怪物的目标朝向    
//    private bool is_Running = false;
//    private bool _bIsHasTarget = false;
//    private MonsterState CurrentState { get => currentState; set => currentState = value; }

//    private bool _isSoldierDeathPlayOnce = true;
//    private TestAI targetAI;

//    //属性相关
//    public int HeroExperence = 0;
//    public int EnemyAttack = 0;
//    public int EnemyDefence = 0;
//    public int IntMaxHealth = 100;   //最大的生命值
//    private float FloCurrentHealth = 100; //当前的生命数值
//    public float FloCurrentHealth1 { get => FloCurrentHealth;}
//    public int DestroyTime = 3;

//    //UI相关
//    public Slider HPSlider;

//    private void OnEnable()
//    {
//        _isSoldierDeathPlayOnce = true;
//    }

//    void Start()
//    {
//        thisAnimator = GetComponent<Animator>();
//        //保存初始位置信息
//        initialPosition = gameObject.GetComponent<Transform>().position;
//        agent = gameObject.GetComponent<NavMeshAgent>();
        
//    }

//    void FindTarget()
//    {
//        playsUnit = GameObject.FindGameObjectsWithTag(targetTagName);
//        if (playsUnit.Length > 0)
//        {
//            _bIsHasTarget = true;

//            int ranTarget = Random.Range(0, playsUnit.Length);
//            //随机目标
//            playerUnit = playsUnit[ranTarget];

//            if (!targetAI)
//            {
//                targetAI = playerUnit.GetComponent<TestAI>();
//            }
//        }
//        else
//        {
//            _bIsHasTarget = false;
//        }
        
//    }

//    void Update()
//    {
//        FindTarget();

//        //更新血条状态
//        if(currentState!=MonsterState.Death)
//            this.HPSlider.value = FloCurrentHealth / IntMaxHealth;

//        switch (currentState)
//        {
//            //待机状态，等待actRestTme后重新随机指令
//            case MonsterState.Idle:
//                thisAnimator.SetInteger("Run", -1);
//                thisAnimator.SetBool("Attack1", false);
//                thisAnimator.SetBool("Attack2", false);
//                thisAnimator.SetBool("Attack3", false);
//                thisAnimator.SetBool("Damage", false);
//                //该状态下的检测指令
//                EnemyDistanceCheck();
//                break;
//            //追击状态，朝着玩家跑去
//            case MonsterState.Chase:
//                thisAnimator.SetBool("Attack1", false);
//                thisAnimator.SetBool("Attack2", false);
//                thisAnimator.SetBool("Attack3", false);
//                thisAnimator.SetInteger("Run", 1);
//                agent.isStopped = false;
//                //该状态下的检测指令
//                ChaseRadiusCheck();

//                break;
//            case MonsterState.Attack:
//                thisAnimator.SetInteger("Run", -1);
//                agent.isStopped = true;
//                AttackTarget();
//                if (thisAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
//                {
//                    currentState = MonsterState.Idle;
//                }
//                break;
//            case MonsterState.Damage:
//                thisAnimator.SetBool("Damage", true);
//                if (thisAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
//                {
//                    currentState = MonsterState.Idle;
//                }
//                break;
//            case MonsterState.Death:
//                thisAnimator.SetBool("Damage", false);
//                if (_isSoldierDeathPlayOnce)
//                {
//                    thisAnimator.SetTrigger("Death");
//                    _isSoldierDeathPlayOnce = false;
//                }
//                break;
//        }
//    }

//    void AttackTarget()
//    {
//        if (targetAI.currentState != MonsterState.Death)
//        {
//            if (_bIsHasTarget)
//            {
//                UnityHelper.FaceToGo(this.transform, playerUnit.transform, turnSpeed);
//                thisAnimator.SetBool("Attack1", true);

//                playerUnit.SendMessage("OnHurt", 1, SendMessageOptions.DontRequireReceiver);
//            }
//            else
//            {
//                currentState = MonsterState.Idle;
//            }
//        }
//    }

//    /// <summary>
//    /// 原地呼吸、观察状态的检测
//    /// </summary>
//    void EnemyDistanceCheck()
//    {
//        //检测场景中有没有目标
//        if (_bIsHasTarget)
//        {
//            distanceToTarget = Vector3.Distance(playerUnit.transform.position, transform.position);

//            //检测到有目标在场景中，并且小于攻击距离
//            if (distanceToTarget < attackRange)
//            {
//                currentState = MonsterState.Attack;
//            }
//            else
//            {
//                currentState = MonsterState.Chase;          //否则一直追击目标
//            }
//        }
//        else
//        {
//            currentState = MonsterState.Idle;
//        }
        
//    }

//    /// <summary>
//    /// 追击状态检测，检测目标是否进入攻击范围
//    /// </summary>
//    void ChaseRadiusCheck()
//    {
//        if (_bIsHasTarget)
//        {
//            if (!is_Running)
//            {
//                thisAnimator.SetInteger("Run", 1);
//                is_Running = true;
//            }
//            distanceToTarget = Vector3.Distance(playerUnit.transform.position, transform.position);
//            //transform.Translate(Vector3.forward * Time.deltaTime * runSpeed);
//            UnityHelper.FaceToGo(this.transform, playerUnit.transform, turnSpeed);
//            //agent.destination = playerUnit.transform.position;
//            agent.SetDestination(playerUnit.transform.position);    //改为用Unity的代理AI完成寻路
//            if (distanceToTarget < attackRange)
//            {
//                currentState = MonsterState.Attack;

//            }
//            else
//            {
//                currentState = MonsterState.Chase;
//            }
//        }
//    }

//    #region 属性相关
//    public void CheckLife()
//    {
//        //敌人当前的血量值已经耗尽 设置为死亡状态
//        if (FloCurrentHealth <= IntMaxHealth * 0.01)
//        {
//            //更换状态
//            currentState = MonsterState.Death;


//            //利用缓冲池的技术对敌人进行回收，而不是直接销毁掉
//            StartCoroutine("RecoverEnemy");
//        }
//    }

//    //伤害处理
//    public void OnHurt(int hurtValue)
//    {
//        //当前的状态设置为受伤
//        currentState = MonsterState.Damage;

//        int hurtValus = 0;
//        hurtValus = Mathf.Abs(hurtValue);   //取绝对值  伤害值不会是负数
//        if (hurtValus > 0)
//        {
//            FloCurrentHealth -= hurtValus;
//        }
//        Debug.Log("当前剩余血量:"+ FloCurrentHealth);

//        CheckLife();
//    }

//    //回收对象
//    IEnumerator RecoverEnemy()
//    {
//        yield return new WaitForSeconds(DestroyTime);
        
//        //敌人回收前，重置敌人的属性
//        FloCurrentHealth = IntMaxHealth;
//        currentState = MonsterState.Idle;
//        Destroy(this.gameObject);        //延迟1秒销毁这个对象   应该放到池子 TODO...
//    }
//    #endregion


//}
