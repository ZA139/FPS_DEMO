using UnityEngine;
using System.Collections;

[AddComponentMenu("Game/Player")]
public class Player : MonoBehaviour
{
    //各种特殊弹药的数量
    public int ShockedAmmo = 5;
    public int OnFireAmmo = 5;
    public int DamAmmo = 5;
    public int ElectricAmmo = 5;
    public int NormalAmmo = 5;

    //技能模块
    public bool SkillsMenu = false;
    public int cntskills = 200;
    public int cntAtk = 0;
    public float cntSpeed = 0.0f;
    public int cntLife = 0;
    public long cnt_kills = 0;
    //创建特殊弹药类型
    public Stack SpecialMag = new Stack();
    //枚举弹药类型
    enum SpecialAmmo
    {
        ShcokedAmmo,
        OnFireAmmo,
        DamAmmo,
        ElectricAmmo,
        NormalAmmo
    };
    public Transform m_transform;

    public bool SpecialMagOn = false;

    // 角色控制器组件
    CharacterController m_ch;

    // 角色移动速度
    public float m_movSpeed = 10.0f;

    // 重力
    float m_gravity = 2.0f;

    // 摄像机Transform
    Transform m_camTransform;

    // 摄像机旋转角度
    Vector3 m_camRot;

    // 摄像机高度(即表示主角的身高)
    float m_camHeight = 1.4f;

    // 生命值
    public int m_life = 5;

    // 枪口transform
    Transform m_muzzlepoint;

    // 射击时，射线能射到的碰撞层
    public LayerMask m_layer;
    public LayerMask m_layer_ass;

    // 射中目标后的粒子效果
    public Transform m_fx;

    // 射击音效
    public AudioClip m_audio;

    public AudioClip m_special_audio;

    // 射击间隔时间计时器
    float m_shootTimer = 0;
    float m_special_shootTimer = 0;


    // Use this for initialization
    void Start()
    {

        m_transform = this.transform;
        // 获取角色控制器组件
        m_ch = this.GetComponent<CharacterController>();

        // 获取摄像机
        m_camTransform = Camera.main.transform;

        // 设置摄像机初始位置
        //Vector3 pos = m_transform.position;
        //pos.y += m_camHeight;
        //m_camTransform.position = pos;
        m_camTransform.position = m_transform.TransformPoint(0, m_camHeight, 0);

        // 设置摄像机的旋转方向与主角一致
        m_camTransform.rotation = m_transform.rotation;
        m_camRot = m_camTransform.eulerAngles;
        //锁定鼠标
        Screen.lockCursor = true;

        // 查找射击点
        m_muzzlepoint = m_camTransform.Find("M16/weapon/muzzlepoint").transform;

        SpecialMag.Clear();

    }

    // Update is called once per frame
    void Update()
    {
        m_movSpeed = 3.0f + cntSpeed;
        // 生命值为0，GG
        if (m_life <= 0)
            return;

        Control();
    }

    void Control()
    {
        if (cnt_kills / 50 > 0)
        {
            cntskills++;
            cnt_kills -= 50;
            ShockedAmmo += 5;
            OnFireAmmo += 5;
            ElectricAmmo += 5;
            DamAmmo += 5;
        }
        //获取鼠标移动距离
        float rh = Input.GetAxis("Mouse X");
        float rv = Input.GetAxis("Mouse Y");

        // 旋转摄像机
        m_camRot.x -= rv;
        m_camRot.y += rh;
        m_camTransform.eulerAngles = m_camRot;

        // 使主角的面向方向与摄像机一致
        Vector3 camrot = m_camTransform.eulerAngles;
        camrot.x = 0; camrot.z = 0;
        m_transform.eulerAngles = camrot;

        // 定义3个值控制移动
        float xm = 0, ym = 0, zm = 0;

        // 重力运动
        ym -= m_gravity * Time.deltaTime;

        // 上下左右运动
        if (Input.GetKey(KeyCode.W))
        {
            zm += m_movSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            zm -= m_movSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            xm -= m_movSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            xm += m_movSpeed * Time.deltaTime;
        }

        // 更新射击间隔时间
        m_shootTimer -= Time.deltaTime;
        m_special_shootTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.K) && cntskills != 0)
        {
            if (SkillsMenu)
                SkillsMenu = false;
            else
                SkillsMenu = true;
        }
        if (SkillsMenu)
        {
            skillsUpdate();
        }
        else
        {
            //特殊弹药压入
            if (Input.GetKeyDown(KeyCode.Alpha1) && (ShockedAmmo != 0))
            {
                SpecialMag.Push(SpecialAmmo.ShcokedAmmo);//麻痹弹药
                //GameManager.Instance.SetShocked(1);
                ShockedAmmo--;
                GameManager.Instance.SetAmmo(0);
                Debug.Log("ShockedAMMO");
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && (OnFireAmmo != 0))
            {
                SpecialMag.Push(SpecialAmmo.OnFireAmmo);//火炎弹药
                OnFireAmmo--;
                GameManager.Instance.SetAmmo(0);
                Debug.Log("OnFireAMMO");
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && (DamAmmo != 0))
            {
                SpecialMag.Push(SpecialAmmo.DamAmmo);//达姆弹
                DamAmmo--;
                GameManager.Instance.SetAmmo(0);
                Debug.Log("DamAMMO");
            }
            if (Input.GetKeyDown(KeyCode.Alpha4) && (ElectricAmmo != 0))
            {
                SpecialMag.Push(SpecialAmmo.ElectricAmmo);//电击弹药
                ElectricAmmo--;
                GameManager.Instance.SetAmmo(0);
                Debug.Log("ElectricAMMO");
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                SpecialMag.Push(SpecialAmmo.NormalAmmo);
                Debug.Log("NormalAMMO");
                GameManager.Instance.SetAmmo(0);
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!SpecialMagOn)
                SpecialMagOn = true;
            else
                SpecialMagOn = false;
            GameManager.Instance.SetAmmo(0);

        }
        // 鼠标左键射击
        if (Input.GetMouseButton(0) && m_shootTimer <= 0 && !SpecialMagOn)
        {
            this.GetComponent<AudioSource>().PlayOneShot(m_audio);
            shoot();
        }
        else if (Input.GetMouseButton(0) && m_special_shootTimer <= 0 && SpecialMagOn)
        {
            if (SpecialMagOn)
            {
                if (SpecialMag.Count != 0)
                {
                    this.GetComponent<AudioSource>().PlayOneShot(m_special_audio);
                    shoot();
                }
            }
        }


        // 使用角色控制器提供的Move函数进行移动 它会自动检测碰撞
        m_ch.Move(m_transform.TransformDirection(new Vector3(xm, ym, zm)));

        // 更新摄像机位置（始终与Player一致）
        //Vector3 pos = m_transform.position;
        //pos.y += m_camHeight;
        //m_camTransform.position = pos;
        m_camTransform.position = m_transform.TransformPoint(0, m_camHeight, 0);
    }

    // 在编辑器中为主角显示一个图标
    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(this.transform.position, "Spawn.tif");
    }


    // 受到伤害
    public void OnDamage(int damage)
    {
        m_life -= damage;

        // 更新UI
        GameManager.Instance.SetLife(m_life);

        // 取消锁定鼠标光标
        if (m_life <= 0)
            Screen.lockCursor = false;
    }

    void shoot()
    {
        //if (SpecialMagOn)
        //{
        //    //SpecialAmmoShooted = SpecialMag.Pop();
        //}
        m_shootTimer = 0.1f;
        m_special_shootTimer = 0.9f;
        // 减少弹药，更新弹药UI

        // RaycastHit用来保存射线的探测结果
        RaycastHit info;
        RaycastHit info_ass;

        // 从muzzlepoint的位置，向摄像机面向的正方向射出一根射线
        // 射线只能与m_layer所指定的层碰撞
        bool hit_ass = Physics.Raycast(m_muzzlepoint.position, m_camTransform.TransformDirection(Vector3.forward), out info_ass, 100, m_layer_ass);
        bool hit = Physics.Raycast(m_muzzlepoint.position, m_camTransform.TransformDirection(Vector3.forward), out info, 100, m_layer);
        //Debug.Log("info:"+info.transform.tag);
        if (hit)
        {
            //Debug.Log("info_ass:"+info_ass.transform.tag);
            // 如果射中了Tag为enemy的游戏体
            if (SpecialMagOn)
            {
                if (info_ass.collider != null)
                {
                    //Debug.Log(info_ass.transform.tag);
                    if (info_ass.transform.tag.CompareTo("Left") == 0)
                    {
                        BadAss badAss = info.transform.GetComponent<BadAss>();
                        badAss.m_ani.SetTrigger("left");
                    }
                    else if (info_ass.transform.tag.CompareTo("Right") == 0)
                    {
                        BadAss badAss = info.transform.GetComponent<BadAss>();
                        badAss.m_ani.SetTrigger("right");
                    }
                    else if (info_ass.transform.tag.CompareTo("head") == 0)
                    {
                        BadAss badAss = info.transform.GetComponent<BadAss>();
                        badAss.m_ani.SetTrigger("head");
                    }
                    else if (info_ass.transform.tag.CompareTo("body") == 0)
                    {
                        BadAss badAss = info.transform.GetComponent<BadAss>();
                        badAss.m_ani.SetTrigger("body");
                    }
                }
            }
            if (info.transform.tag.CompareTo("enemy") == 0)
            {
                Enemy enemy = info.transform.GetComponent<Enemy>();
                BadAss badAss = info.transform.GetComponent<BadAss>();
                if (!SpecialMagOn)
                {
                    if (enemy)
                        enemy.OnDamage(1 + cntAtk);
                    else
                        badAss.OnDamage(1 + cntAtk);
                }
                else
                {
                    if (SpecialMag.Count != 0)
                    {
                        switch (SpecialMag.Pop())
                        {
                            case SpecialAmmo.ShcokedAmmo:
                                if (enemy)
                                {
                                    enemy.Shocked = true;
                                    enemy.OnDamage(3 + cntAtk);
                                }
                                else
                                {
                                    badAss.m_ani.SetBool("shocked", true);
                                    badAss.Shocked = true;
                                    badAss.OnDamage(3 + cntAtk);
                                }
                                break;
                            case SpecialAmmo.OnFireAmmo:
                                if (enemy)
                                {
                                    enemy.OnFire = true;
                                    enemy.OnDamage(1 + cntAtk);
                                }
                                else
                                {
                                    badAss.OnFire = true;
                                    badAss.OnDamage(1 + cntAtk);
                                }
                                break;
                            case SpecialAmmo.ElectricAmmo://连锁功能，可通过rigibody组件来实现
                                if (enemy)
                                    enemy.OnDamage(10 + cntAtk);
                                else
                                    badAss.OnDamage(10 + cntAtk);
                                break;
                            case SpecialAmmo.DamAmmo:
                                if (enemy)
                                    enemy.OnDamage(100 + cntAtk);
                                else
                                    badAss.OnDamage(100 + cntAtk);
                                break;
                            case SpecialAmmo.NormalAmmo:
                                if (enemy)
                                    enemy.OnDamage(1 + cntAtk);
                                else
                                    badAss.OnDamage(1 + cntAtk);
                                break;
                        }
                    }
                }
                GameManager.Instance.SetAmmo(1);
            }
            else
            {
                if (SpecialMagOn)
                {
                    SpecialMag.Pop();
                }
                GameManager.Instance.SetAmmo(1);
            }
            // 在射中的地方释放一个粒子效果
            Instantiate(m_fx, info.point, info.transform.rotation);
        }


    }
    void skillsUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && cntskills != 0)
        {
            cntAtk++;
            cntskills--;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && cntskills != 0 && cntSpeed <= 20)
        {
            cntSpeed = cntSpeed + 0.5f;
            cntskills--;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && cntskills != 0)
        {
            cntLife++;
            //int peach = (5 + cntLife) - m_life;
            //GameManager.Instance.SetLife(peach);
            m_life = 5 + cntLife;
            GameManager.Instance.SetLife(m_life);
            cntskills--;
        }
    }
}
