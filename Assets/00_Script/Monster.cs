using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Character
{
    public float M_Speed;
    

    bool isSpawn = false;

    protected override void Start()
    {
        base.Start();

    }


    /// <summary>
    /// ���ϴ� ������ ��� Init�� ��ų�� �ִ�.
    /// </summary>
    public void Init()
    {
        isDead = false;
        HP = 5123123;
        Attack_Range = 0.5f;
        StartCoroutine(Spawn_Start());
    }

    private void Update()
    {

        FindClosetTarget(Spawner.m_players.ToArray());

        if (m_target.GetComponent<Character>().isDead)
        {
            FindClosetTarget(Spawner.m_players.ToArray());
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

        //�� �������� �ٸ� �������� ���� �ӵ��� �̵��� �� �����ϰ� ���˴ϴ�.
        //MoveToWards �޼���� �������� ������ ������ ���������� �̵��ϸ�, �ӵ��� ������ �� �ֽ��ϴ�.

       
        //Vector3.Distance�� Unity���� �� ���� ���� �Ÿ��� ����ϴ� �� ���Ǵ� �޼����Դϴ�.
        //�� �޼���� 3D �������� �� ���� Vector3 ���� ��Ŭ���� �Ÿ�(Euclidean Distance)�� ��ȯ�մϴ�.
        
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
            percent = current / 0.3f;
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

        Base_Manager.Pool.Pooling_OBJ("HIT_TEXT").Get((value) =>
        {
            value.GetComponent<Hit_Text>().Init(transform.position, dmg, false);
        });

        HP -= dmg;

        if(HP <= 0)
        {
            isDead = true;
            Spawner.m_monsters.Remove(this);

            Base_Manager.Pool.Pooling_OBJ("Smoke").Get((value) =>
            {
                value.transform.position = new Vector3(transform.position.x,0.5f,transform.position.z);
                Base_Manager.instance.Return_Pool(value.GetComponent<ParticleSystem>().duration, value, "Smoke");
                
            });



            Base_Manager.Pool.Pooling_OBJ("COIN_PARENT").Get((value) =>
            {
                value.GetComponent<Coin_Parent>().Init(transform.position);
            });

            for(int i = 0; i < 3; i++)
            {
                Base_Manager.Pool.Pooling_OBJ("Item_OBJ").Get((value) =>
                {
                    value.GetComponent<Item_OBJ>().Init(transform.position); // ���� ��ġ ����
                });
            }


            Base_Manager.Pool.m_pool_Dictionary["Monster"].Return(this.gameObject);
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
   
}
