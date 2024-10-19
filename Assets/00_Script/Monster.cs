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
    /// 원하는 시점에 계속 Init을 시킬수 있다.
    /// </summary>
    public void Init()
    {
        isDead = false;
        ATK = 10;
        HP = 5;
        Attack_Range = 0.5f;
        target_Range = Mathf.Infinity;
        StartCoroutine(Spawn_Start());
    }

    private void Update()
    {
        if(isSpawn == false)
        {
            return;
        }
        if(m_target == null)
        {
            FindClosetTarget(Spawner.m_players.ToArray());
        }
       
        if(m_target != null)
        {
            if (m_target.GetComponent<Character>().isDead)
            {
                FindClosetTarget(Spawner.m_players.ToArray());
            }

            float targetDistance = Vector3.Distance(transform.position, m_target.position);

            if (targetDistance > Attack_Range && isATTACK == false) // 현재 타겟이 추적 범위 안에는 있지만, 공격범위 안에는 없을 때
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

            //한 지점에서 다른 지점으로 일정 속도로 이동할 때 유용하게 사용됩니다.
            //MoveToWards 메서드는 목적지에 도달할 때까지 선형적으로 이동하며, 속도를 조절할 수 있습니다.


            //Vector3.Distance는 Unity에서 두 지점 간의 거리를 계산하는 데 사용되는 메서드입니다.
            //이 메서드는 3D 공간에서 두 개의 Vector3 간의 유클리드 거리(Euclidean Distance)를 반환합니다.

        }

    }

    /// <summary>
    /// 몬스터가 스폰이 될 때, 스케일의 크기변화를 줍니다.
    /// </summary>
    /// <returns></returns>
    IEnumerator Spawn_Start()
    {
        float current = 0.0f;
        float percent = 0.0f;
        float start = 0.0f;
        float end = transform.localScale.x; // 몬스터의 로컬스케일 

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / 0.3f;
            float LerpPos = Mathf.Lerp(start,end, percent); // 선형보간 (시작값,끝값,시간)
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
                    value.GetComponent<Item_OBJ>().Init(transform.position); // 몬스터 위치 삽입
                });
            }


            Base_Manager.Pool.m_pool_Dictionary["Monster"].Return(this.gameObject);
        }


    }

    /// <summary>
    /// 여러가지 이펙트들을 코루틴으로 리턴시켜주는 메서드 입니다.
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
