using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private int M_Count; // ������ ��
    private float M_SpawnTime; // �� �ʸ��� ������ �� ������ ����.
    // 1. ���ʹ� ���������� �� �� ���� ���÷� ������ ���� �Ǿ�� �Ѵ�.

    //Spawner �� �ս��� �����ϱ� ����, static���� ����
    public static List<Monster> m_monsters = new List<Monster>();
    public static List<Player> m_players = new List<Player>();

    private Coroutine coroutine;

    private void Start()
    {
        Base_Manager.Stage.M_ReadyEvent += OnReady;
        Base_Manager.Stage.M_PlayEvent += OnPlay;
        Base_Manager.Stage.M_BossEvent += OnBoss;
    }

    public void OnReady()
    {
        M_Count = int.Parse(CSV_Importer.Spawn_Design[Data_Manager.Main_Players_Data.Player_Stage]["Spawn_Count"].ToString());
        M_SpawnTime = float.Parse(CSV_Importer.Spawn_Design[Data_Manager.Main_Players_Data.Player_Stage]["Spawn_Timer"].ToString());
    }
    public void OnPlay()
    {
        coroutine = StartCoroutine(SpawnCoroutine());
    }
    public void OnBoss()
    {
        if(coroutine != null)
        {            
            StopAllCoroutines();
        }

        for(int i = 0; i<m_monsters.Count; i++)
        {
            if (m_monsters[i].isDead != true)
            {
                m_monsters[i].isDead = true;
                Base_Manager.Pool.m_pool_Dictionary["Monster"].Return(m_monsters[i].gameObject);
            }
            
        }
        m_monsters.Clear();
        

        StartCoroutine(BossSetCoroutine());
      
    }    

    IEnumerator BossSetCoroutine()
    {
        yield return new WaitForSeconds(2.0f);
        var monster = Instantiate(Resources.Load<Monster>("Boss"), Vector3.zero, Quaternion.Euler(0, 180, 0)); // ���� ����
        monster.Init();

        Vector3 Pos = monster.transform.position; // ���� ������ ����� ����, �� ������ ��� ����ϸ� �޸� ������ ��. (�ߺ�������)


        // ���� ��ȯ�Ÿ� ���ο� �÷��̾ �����ϸ�, ���� ��ȯ ��, �˹��� �մϴ�.
        for(int i = 0; i<m_players.Count; i++)
        {
            if(Vector3.Distance(Pos, m_players[i].transform.position) <= 3.0f)
            {
                m_players[i].transform.LookAt(monster.transform.position);
                m_players[i].Knock_Back();
            }
         
        }

        yield return new WaitForSeconds(1.5f);

        m_monsters.Add(monster);

        Base_Manager.Stage.State_Change(Stage_State.BossPlay);
    }
    //Random.insideUnitSphere = Vector3(x,y,z)
    //Random.insideUnitCircle = Vector3(x,y)
    IEnumerator SpawnCoroutine()
    {
        
        Vector3 pos;

        int Monster_Spawn_Value = M_Count - m_monsters.Count;

        for(int i = 0; i < Monster_Spawn_Value; i++)
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
