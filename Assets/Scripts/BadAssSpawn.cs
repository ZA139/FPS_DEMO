using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadAssSpawn : EnemySpawn
{
    // 敌人的Prefab
    public Transform m_enemy;

    protected Transform m_transform;

    float m_timer = 46.0f;

    public bool boos = false;

    // Use this for initialization
    void Start()
    {

        m_transform = this.transform;

    }

    // Update is called once per frame
    void Update()
    {
        if(!boos)
        {
            m_timer -= Time.deltaTime;
        }
        if (m_timer <= 0.0)
        {
            // 生成敌人
            Transform obj = (Transform)Instantiate(m_enemy, m_transform.position, Quaternion.identity);

            // 获取敌人的脚本
            BadAss badAss = obj.GetComponent<BadAss>();

            // 初始化敌人
            badAss.Init(this);
            m_timer = 20.0f;
            boos = true;
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "item.png", true);
    }

}
