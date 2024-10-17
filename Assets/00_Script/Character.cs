using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Animator animator;

    public double HP;
    public double ATK;
    public float ATK_Speed;
    public bool isDead = false;



    protected float Attack_Range = 3.0f; // �����ϴ� ���� ����
    protected float target_Range = 5.0f; // �߰��ϴ� ����
    protected bool isATTACK = false;

    protected Transform m_target; // Ÿ�� ��ü

    [SerializeField]
    private Transform m_BulletTransform;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    protected void InitAttack() => isATTACK = false;
    //AnyState�� � ���¿��� Ʈ���Ű� �۵��Ǹ�, �ش� �ִϸ��̼����� ���� �ְԲ� �Ѵ�.
    protected void AnimatorChange(string temp)
    {
        if(temp == "isATTACK")
        {
            animator.SetTrigger("isATTACK");
            return;
        }

        animator.SetBool("isIDLE", false);
        animator.SetBool("isMOVE", false);

        animator.SetBool(temp, true);
    }

    /// <summary>
    /// ĳ���� ���ݸ�� EVENT���� ����ȴ�.
    /// </summary>
    protected virtual void Bullet()
    {
        if(m_target == null)
        {
            return;
        }

        Base_Manager.Pool.Pooling_OBJ("Attack_Helper").Get((value) =>
        {
            value.transform.position = m_BulletTransform.position;
            value.GetComponent<Bullet>().init(m_target, 10, "CH_01");
        });
    }

    protected virtual void Attack()
    {
        if(m_target == null)
        {
            return;
        }

        Base_Manager.Pool.Pooling_OBJ("Attack_Helper").Get((value) =>
        {
            value.transform.position = m_target.transform.position;
            value.GetComponent<Bullet>().Attack_Init(m_target, 10);
        });
    }

    public virtual void GetDamage(double dmg)
    {

    }

    /// <summary>
    /// ���� ����� ��ü�� �����Ѵ�. (����,�÷��̾�)
    /// </summary>
    protected void FindClosetTarget<T> (T[] targets) where T : Component
    {
        var monsters = targets;
        Transform closetTarget = null; // ���� ����� ��ü�� �����ϱ� ���� ����
        float maxDistance = target_Range;

        foreach(var monster in monsters)
        {
            float targetDistance = Vector3.Distance(transform.position, monster.transform.position);

            if(targetDistance < maxDistance)
            {
                closetTarget = monster.transform;
                maxDistance = targetDistance;
            }
        }

        m_target = closetTarget;

        if(m_target != null)
        {
            transform.LookAt(m_target.position);
        }
    }

}
