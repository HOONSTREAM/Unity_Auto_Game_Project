using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Character
{
    public float M_Speed;
    public double R_ATK, R_HP;
    public float  R_ATTACK_RANGE;
    private bool isSpawn = false;
    public bool isBoss = false;
    private Vector3 Original_Scale;


    private void Awake()
    {
        Original_Scale = transform.localScale;
    }
    protected override void Start()
    {
        base.Start();

        Base_Manager.Stage.M_DeadEvent += OnDead;
    }

    /// <summary>
    /// ���ϴ� ������ ��� Init�� ��ų�� �ִ�.
    /// </summary>
    public void Init()
    {
        isDead = false;
        ATK = Utils.CalculateValue(Utils.Data.stageData.Base_ATK, Stage_Manager.Stage, Utils.Data.stageData.MONSTER_ATK);
        HP = Utils.CalculateValue(Utils.Data.stageData.Base_HP, Stage_Manager.Stage, Utils.Data.stageData.MONSTER_HP);
        Attack_Range = R_ATTACK_RANGE;
        target_Range = Mathf.Infinity;
        transform.localScale = Original_Scale;

        if (isBoss)
        {
            StartCoroutine(Skill_Coroutine());
        }
        
        StartCoroutine(Spawn_Start());

    }

    IEnumerator Skill_Coroutine()
    {
        yield return new WaitForSeconds(3.0f);
        GetComponent<Skill_Base>().Set_Skill();

        StartCoroutine(Skill_Coroutine());
    }

    private void Update()
    {
        if(isSpawn == false)
        {
            return;
        }
        if (Stage_Manager.M_State == Stage_State.Play || Stage_Manager.M_State == Stage_State.BossPlay)
        {
            if (m_target == null)
            {
                FindClosetTarget(Spawner.m_players.ToArray());
            }

            if (m_target != null)
            {
                if (m_target.GetComponent<Character>().isDead)
                {
                    FindClosetTarget(Spawner.m_players.ToArray());
                }

                float targetDistance = Vector3.Distance(transform.position, m_target.position);

                if (targetDistance > Attack_Range && isATTACK == false) // ���� Ÿ���� ���� ���� �ȿ��� ������, ���ݹ��� �ȿ��� ���� ��
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
               
            //�� �������� �ٸ� �������� ���� �ӵ��� �̵��� �� �����ϰ� ���˴ϴ�.
            //MoveToWards �޼���� �������� ������ ������ ���������� �̵��ϸ�, �ӵ��� ������ �� �ֽ��ϴ�.


            //Vector3.Distance�� Unity���� �� ���� ���� �Ÿ��� ����ϴ� �� ���Ǵ� �޼����Դϴ�.
            //�� �޼���� 3D �������� �� ���� Vector3 ���� ��Ŭ���� �Ÿ�(Euclidean Distance)�� ��ȯ�մϴ�.

        }

    }

    /// <summary>
    /// ���Ͱ� ������ �� ��, �������� ũ�⺯ȭ�� �ݴϴ�.
    /// </summary>
    /// <returns></returns>
    IEnumerator Spawn_Start()
    {
        float current = 0.0f;
        float percent = 0.0f;
        float start = 0.0f;
        float end = transform.localScale.x; // ������ ���ý����� 

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / 0.2f;
            float LerpPos = Mathf.Lerp(start,end, percent); // �������� (���۰�,����,�ð�)
            transform.localScale = new Vector3(LerpPos, LerpPos, LerpPos);
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);
        isSpawn = true;
    }
    public override void GetDamage(double dmg)
    {
        if(isDead)
        {
            return;
        }

        bool critical = Critical_Damage(ref dmg);

        Base_Manager.Pool.Pooling_OBJ("HIT_TEXT").Get((value) =>
        {
            value.GetComponent<Hit_Text>().Init(transform.position, dmg, false, critical);
        });

        HP -= dmg;

        if (isBoss)
        {
            Main_UI.Instance.Boss_Slider_Count(HP, 500); //TODO
        }

        if(HP <= 0)
        {
            isDead = true;
            Dead_Event();
        }

    }
    private void OnDead()
    {
        StopAllCoroutines();
        AnimatorChange("isIDLE");
    }
    private void Dead_Event()
    {
        if (!isBoss)
        {
            if (!Stage_Manager.isDead)
            {
                Stage_Manager.Count++;
                Main_UI.Instance.Monster_Slider_Count();
            }
           
        }
        else
        {
            Base_Manager.Stage.State_Change(Stage_State.Clear);
        }

        Spawner.m_monsters.Remove(this);

        Base_Manager.Pool.Pooling_OBJ("Smoke").Get((value) =>
        {
            value.transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
            Base_Manager.instance.Return_Pool(value.GetComponent<ParticleSystem>().duration, value, "Smoke");

        });

        Base_Manager.Pool.Pooling_OBJ("COIN_PARENT").Get((value) =>
        {
            value.GetComponent<Coin_Parent>().Init(transform.position);
        });

        for (int i = 0; i < 3; i++)
        {
            Base_Manager.Pool.Pooling_OBJ("Item_OBJ").Get((value) =>
            {
                value.GetComponent<Item_OBJ>().Init(transform.position); // ���� ��ġ ����
            });
        }

        if (!isBoss)
        {
            Base_Manager.Pool.m_pool_Dictionary["Monster"].Return(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject); // �������ʹ� Ǯ�������ʰ� �ı��Ѵ�.
        }
        
    }

    /// <summary>
    /// �������� ����Ʈ���� �ڷ�ƾ���� ���Ͻ����ִ� �޼��� �Դϴ�.
    /// </summary>
    /// <param name="timer"></param>
    /// <param name="obj"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    IEnumerator ReturnCoroutine(float timer, GameObject obj, string path)
    {
        yield return new WaitForSeconds(timer);
        Base_Manager.Pool.m_pool_Dictionary[path].Return(obj);
    }

    protected virtual bool Critical_Damage(ref double dmg)
    {
        float critical_percentage = Random.Range(0.0f, 100.0f);

        if (critical_percentage <= Base_Manager.Player.Critical_Percentage)
        {
            dmg *= (Base_Manager.Player.Critical_Damage / 100);
            return true;
        }

        else
        {
            return false;
        }
    }


}
