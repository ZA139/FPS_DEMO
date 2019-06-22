using UnityEngine;
using UnityEngine.AI;
using System.Collections;


[AddComponentMenu("Game/BadAss")]
public class BadAss : MonoBehaviour
{
    public AudioClip m_Flame;
    Transform m_transform;
    //CharacterController m_ch;
    //public Transform head;
    //public Transform right;
    //public Transform left;

    //特殊状态
    public bool Shocked = false;
    public bool OnFire = false;
    public bool first_fire = true;
    public int cntdamageFire = 0;
    public float a;

    // 动画组件
    public Animator m_ani;

    // 寻路组件
    NavMeshAgent m_agent;

    // 主角实例
    Player m_player;

    BadAssSpawn badAssSpawn;
    // 移动速度
    float m_movSpeed = 2.0f;

    // 旋转速度
    float m_rotSpeed = 5.0f;

    // 计时器
    float m_timer = 2;
    float m_fireTimer = 1;
    float m_shockedTimer = 5;

    // 生命值
    public int m_life = 1000;

    // 出生点
    protected EnemySpawn m_spawn;

    // Use this for initialization
    void Start()
    {

        m_transform = this.transform;
        // 获得动画播放器
        m_ani = this.GetComponent<Animator>();

        // 获得主角
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        m_agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        m_agent.speed = m_movSpeed;
        badAssSpawn = GameObject.FindObjectOfType<BadAssSpawn>();
    // 获得寻路组件
    m_agent.SetDestination(m_player.m_transform.position);

    }

    // 初始化
    public void Init(EnemySpawn spawn)
    {
        m_spawn = spawn;
    }


    // Update is called once per frame
    void Update()
    {
        if (m_movSpeed >(m_player.m_movSpeed+m_player.cntSpeed-1))
            ;
        else
            m_movSpeed += 2*Time.deltaTime;
        m_ani.SetFloat("Speed", m_movSpeed);
        m_agent.speed = m_movSpeed;
        // 如果主角生命为0，什么也不做
        if (m_player.m_life <= 0)
            return;
        // 更新计时器
        m_timer -= Time.deltaTime;
        m_fireTimer -= Time.deltaTime;

        // 获取当前动画状态
        AnimatorStateInfo stateInfo = m_ani.GetCurrentAnimatorStateInfo(0);

        if (this.Shocked)
        {
            m_shockedTimer -= Time.deltaTime;
            Debug.Log("Enemy Shocked!");
            //m_ani.SetBool("Shocked", true);
            this.m_agent.ResetPath();
            this.m_ani.SetBool("idle", true);
            this.m_ani.SetBool("run", false);
        }
        if (m_shockedTimer <= 0)
        {
            m_shockedTimer = 5;
            this.Shocked = false;
        }

        if (this.OnFire)
        {
            if (this.first_fire)
            {
                this.GetComponent<AudioSource>().PlayOneShot(m_Flame);
                this.first_fire = false;
            }
            if (m_fireTimer <= 0)
            {
                cntdamageFire++;
                m_fireTimer = 1;
                this.OnDamage(3);
            }
            if (cntdamageFire == 5)
            {
                this.first_fire = true;
                cntdamageFire = 0;
                this.OnFire = false;
            }
        }

        // 如果处于待机且不是过渡状态
        if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.idle")
&& !m_ani.IsInTransition(0) && !Shocked)
        {
            m_ani.SetBool("idle", false);

            // 待机一定时间

            if (m_timer > 0)
                return;

            a = Vector3.Distance(m_transform.position, m_player.m_transform.position);
            // 如果距离主角小于4米，进入攻击动画状态
            if (Vector3.Distance(m_transform.position, m_player.m_transform.position) < 4.0f)
            {
                // 停止寻路
                m_agent.ResetPath();
                if (m_movSpeed < 5)
                    m_ani.SetBool("normal_attack", true);
                else
                    m_ani.SetBool("jump_attack", true);
            }
            else
            {
                // 重置定时器
                m_timer = 1;

                // 设置寻路目标点
                m_agent.SetDestination(m_player.m_transform.position);

                // 进入跑步动画状态
                m_ani.SetBool("walk", true);
            }
        }

        // 如果处于跑步且不是过渡状态
        if ((stateInfo.fullPathHash == Animator.StringToHash("Base Layer.Walk") || stateInfo.fullPathHash==Animator.StringToHash("Base Layer.Run"))
&& !m_ani.IsInTransition(0))
        {
            m_ani.SetBool("walk", false);

            // 每隔1秒重新定位主角的位置
            if (m_timer < 0)
            {
                m_agent.SetDestination(m_player.m_transform.position);

                m_timer = 1;
            }

            // 如果距离主角小于1.5米，向主角攻击
            if (Vector3.Distance(m_transform.position, m_player.m_transform.position) < 4.0f)
            {
                // 停止寻路
                m_agent.ResetPath();
                if (m_movSpeed < 5)
                    m_ani.SetBool("normal_attack", true);
                else
                    m_ani.SetBool("jump_attack", true);
            }
        }

        // 如果处于攻击且不是过渡状态
        if ((stateInfo.fullPathHash == Animator.StringToHash("Base Layer.NormalAttack")||stateInfo.fullPathHash==Animator.StringToHash("Base Layer.JumpAttack"))
&& !m_ani.IsInTransition(0))
        {
            // 面向主角
            RotateTo();
            if (m_movSpeed < 5)
                m_ani.SetBool("normal_attack", false);
            else
                m_ani.SetBool("jump_attack", false);

            // 如果动画播完，重新进入待机状态
            if (stateInfo.normalizedTime >= 1.0f)
            {
                m_ani.SetBool("idle", true);

                // 重置计时器待机2秒
                m_timer = 2;
                if (m_movSpeed > 5)
                    m_player.OnDamage(5);
                else
                    m_player.OnDamage(2);// 攻击
            }
        }
        // 如果处于死亡且不是过渡状态
        if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.Dead") &&
 !m_ani.IsInTransition(0))
        {
            m_ani.SetBool("death", false);
            // 当播放完成死亡动画
            if (stateInfo.normalizedTime >= 1.0f)
            {
                // 加分
                GameManager.Instance.SetScore(10000);
                m_player.cntskills+=5;
                badAssSpawn.boos = false;
                // 销毁自身
                Destroy(this.gameObject);
            }
        }

    }


    // 转向目标点
    void RotateTo()
    {
        // 获取目标（Player）方向
        Vector3 targetdir = m_player.m_transform.position - m_transform.position;
        // 计算出新方向
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetdir, m_rotSpeed * Time.deltaTime, 0.0f);
        // 旋转至新方向
        m_transform.rotation = Quaternion.LookRotation(newDir);
    }

    // 向前移动
    void MoveTo()
    {
        float speed = m_movSpeed * Time.deltaTime;
        m_agent.Move(m_transform.TransformDirection((new Vector3(0, 0, speed))));

    }

    // 受到伤害
    public void OnDamage(int damage)
    {
        m_life -= damage;

        // 如果生命值为0播放死亡动画
        if (m_life <= 0)
        {
            m_ani.SetBool("death", true);
            // 停止寻路
            m_agent.ResetPath();
        }
    }
}
