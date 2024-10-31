using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{

    private Vector3 startPos;
    private Quaternion rotation;
    public Character_Scriptable CH_Data;
    public string CH_Name;
    public GameObject Trail_Object;
    [SerializeField]
    private ParticleSystem Provocation_Effect; // ������ ������ ��, �Ӹ� ��ܿ� ����ǥ�� ��Ÿ���ϴ�.
    

    protected override void Start()
    {
        base.Start();

        Data_Set(Resources.Load<Character_Scriptable>("Scriptable/" + CH_Name));

        Spawner.m_players.Add(this);
        Base_Manager.Stage.M_ReadyEvent += OnReady;
        Base_Manager.Stage.M_BossEvent += OnBoss;
        Base_Manager.Stage.M_ClearEvent += OnClear;
        Base_Manager.Stage.M_DeadEvent += OnDead;
        startPos = transform.position;
        rotation = transform.rotation;

    }

    private void Update()
    {
       
        if (isDead)
        {
            return;
        }

        if (Stage_Manager.M_State == Stage_State.Play || Stage_Manager.M_State == Stage_State.BossPlay)
        {
            FindClosetTarget(Spawner.m_monsters.ToArray()); // ����Ʈ�� �迭�� ����ȯ

            if (m_target == null)
            {
                float targetPos = Vector3.Distance(transform.position, startPos);

                if (targetPos > 0.1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, startPos, Time.deltaTime); // time.deltatime�� speed�� �����ָ� �ӵ��� ������
                    transform.LookAt(startPos);
                    AnimatorChange("isMOVE");
                }

                else
                {
                    transform.rotation = rotation;
                    AnimatorChange("isIDLE");
                }

            }

            else
            {
                if (m_target.GetComponent<Character>().isDead) // ���Ͱ� ������ϸ� ���ο� Ÿ���� ã��
                {                   
                    FindClosetTarget(Spawner.m_monsters.ToArray());
                }

                float targetDistance = Vector3.Distance(transform.position, m_target.position);

                if (targetDistance <= target_Range && targetDistance > Attack_Range && isATTACK == false) // ���� Ÿ���� ���� ���� �ȿ��� ������, ���ݹ��� �ȿ��� ���� ��
                {
                    AnimatorChange("isMOVE");
                    transform.LookAt(m_target.position);
                    transform.position = Vector3.MoveTowards(transform.position, m_target.transform.position, Time.deltaTime);
                }

                else if (targetDistance <= Attack_Range && isATTACK == false)
                {
                    isATTACK = true;
                    AnimatorChange("isATTACK");
                    Invoke("InitAttack", 1.0f);
                }
            }
        }
         
    }
    private void OnReady()
    {
        isDead = false;
        AnimatorChange("isIDLE");
        Spawner.m_players.Add(this);
        Set_ATK_HP();
        transform.position = startPos;
        transform.rotation = rotation;
    }
    private void OnBoss()
    {
        AnimatorChange("isIDLE");
        Provocation_Effect.Play();
    }
    private void OnClear()
    {
        AnimatorChange("isVICTORY");
    }
    private void OnDead()
    {
        Spawner.m_players.Add(this);
    }
    private void Data_Set(Character_Scriptable datas)
    {
        CH_Data = datas;
        Attack_Range = datas.M_Attack_Range;

        Set_ATK_HP();
    }
    public void Set_ATK_HP()
    {
        ATK = Base_Manager.Player.Get_ATK(CH_Data.Rarity);
        HP = Base_Manager.Player.Get_HP(CH_Data.Rarity);
    }
    public override void GetDamage(double dmg)
    {
        base.GetDamage(dmg);

        if (Stage_Manager.isDead)
        {
            return;
        }

        var goOBJ = Base_Manager.Pool.Pooling_OBJ("HIT_TEXT").Get((value) =>
        {
            value.GetComponent<Hit_Text>().Init(transform.position, dmg, true);

        });

        HP -= dmg;

        if(HP <= 0)
        {
            isDead = true;
            DeadEvent();
        }
    }
    private void DeadEvent()
    {
        Spawner.m_players.Remove(this); // �ڽ��� ���̻� �������� ���ϵ��� ����Ʈ���� ����

        if(Spawner.m_players.Count <= 0 && Stage_Manager.isDead == false)
        {
            Base_Manager.Stage.State_Change(Stage_State.Dead);
        }

        AnimatorChange("isDEAD");
        m_target = null;

    }

    /// <summary>
    /// ����ĳ������ �˱⸦ Ȱ��ȭ �մϴ�.
    /// </summary>
    protected override void Attack()
    {
        base.Attack();
        Trail_Object.gameObject.SetActive(true);

        Invoke("TrailDisable", 1.0f);
    }

    /// <summary>
    /// Ȱ��ȭ �� �˱⸦ ��Ȱ��ȭ �մϴ�. 
    /// </summary>
    private void TrailDisable() => Trail_Object.gameObject.SetActive(false);

    /// <summary>
    /// ������ ������ ��, ĳ���͸� �˹��Ű�� �޼��� �Դϴ�.
    /// </summary>
    public void Knock_Back()
    {
        StartCoroutine(Knock_Back_Coroutine(5.0f, 0.3f));
    }

    IEnumerator Knock_Back_Coroutine(float power, float duration)
    {
        float t = duration;
        Vector3 force = this.transform.forward * -power;
        force.y = 0f;

        while (t > 0f)
        {
            t -= Time.deltaTime;
            transform.position += force * Time.deltaTime;
            yield return null;
        }
    }
}
