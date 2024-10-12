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
        HP = 5;
        StartCoroutine(Spawn_Start());
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

    public void GetDamage(double dmg)
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


            Base_Manager.Pool.m_pool_Dictionary["Monster"].Return(this.gameObject);
        }


    }

    private void Update()
    {

        //�� �������� �ٸ� �������� ���� �ӵ��� �̵��� �� �����ϰ� ���˴ϴ�.
        //MoveToWards �޼���� �������� ������ ������ ���������� �̵��ϸ�, �ӵ��� ������ �� �ֽ��ϴ�.
        
        transform.LookAt(Vector3.zero);

        if (isSpawn == false) return;


        //Vector3.Distance�� Unity���� �� ���� ���� �Ÿ��� ����ϴ� �� ���Ǵ� �޼����Դϴ�.
        //�� �޼���� 3D �������� �� ���� Vector3 ���� ��Ŭ���� �Ÿ�(Euclidean Distance)�� ��ȯ�մϴ�.
        float targetDistance = Vector3.Distance(transform.position, Vector3.zero);

        if(targetDistance <= 0.5f)
        {
            AnimatorChange("isIDLE");
        }

        else
        {
            transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, Time.deltaTime * M_Speed);
            AnimatorChange("isMOVE");
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
