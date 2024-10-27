using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int M_Count; // ������ ��
    public float M_SpawnTime; // �� �ʸ��� ������ �� ������ ����.
    // 1. ���ʹ� ���������� �� �� ���� ���÷� ������ ���� �Ǿ�� �Ѵ�.

    //Spawner �� �ս��� �����ϱ� ����, static���� ����
    public static List<Monster> m_monsters = new List<Monster>();
    public static List<Player> m_players = new List<Player>();

    private Coroutine coroutine;

    private void Start()
    {
        Base_Manager.Stage.M_PlayEvent += OnPlay;
        Base_Manager.Stage.M_BossEvent += OnBoss;
    }
    public void OnPlay()
    {
        coroutine = StartCoroutine(SpawnCoroutine());
    }
    public void OnBoss()
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        for(int i = 0; i<m_monsters.Count; i++)
        {
            Base_Manager.Pool.m_pool_Dictionary["Monster"].Return(m_monsters[i].gameObject);
        }
        m_monsters.Clear();
    }
    //Random.insideUnitSphere = Vector3(x,y,z)
    //Random.insideUnitCircle = Vector3(x,y)
    IEnumerator SpawnCoroutine()
    {
        Vector3 pos;

        for(int i = 0; i < M_Count; i++)
        {
            pos = Vector3.zero + Random.insideUnitSphere * 5.0f;
            pos.y = 0.0f;
            Vector3 returnPos = Vector3.zero;

            while (Vector3.Distance(pos, Vector3.zero) <= 3.0f)
            {
                pos = Vector3.zero + Random.insideUnitSphere * 5.0f;
                pos.y = 0.0f;
            }

            //���� ����
            var go = Base_Manager.Pool.Pooling_OBJ("Monster").Get((value) => 
            {
                // Ǯ���� �����ɶ��� ����� �����Ѵ�.

                value.GetComponent<Monster>().Init();
                value.transform.position = pos;
                value.transform.LookAt(Vector3.zero);
                m_monsters.Add(value.GetComponent<Monster>());

            });

        }

        yield return new WaitForSeconds(M_SpawnTime);

        coroutine = StartCoroutine(SpawnCoroutine());
    }
   
}
