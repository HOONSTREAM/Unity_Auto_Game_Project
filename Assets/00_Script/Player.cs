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

    protected override void Start()
    {
        base.Start();

        Data_Set(Resources.Load<Character_Scriptable>("Scriptable/" + CH_Name));

        Spawner.m_players.Add(this);
        Base_Manager.Stage.M_ReadyEvent += OnReady;
        startPos = transform.position;
        rotation = transform.rotation;

    }

    private void Update()
    {
        
        if(Stage_Manager.M_State != Stage_State.Play) { return; }
     
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
            if (m_target.GetComponent<Character>().isDead)
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

    private void OnReady()
    {
        transform.position = startPos;
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

        var goOBJ = Base_Manager.Pool.Pooling_OBJ("HIT_TEXT").Get((value) =>
        {
            value.GetComponent<Hit_Text>().Init(transform.position, dmg, true);

        });

        HP -= dmg;
    }

    protected override void Attack()
    {
        base.Attack();
        Trail_Object.gameObject.SetActive(true);

        Invoke("TrailDisable", 1.0f);
    }

    private void TrailDisable() => Trail_Object.gameObject.SetActive(false);
}
