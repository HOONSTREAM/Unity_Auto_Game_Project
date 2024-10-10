using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private Vector3 startPos;
    private Quaternion rotation;

    protected override void Start()
    {
        base.Start();

        startPos = transform.position;
        rotation = transform.rotation;

    }

    private void Update()
    {
        FindClosetTarget(Spawner.m_monsters.ToArray()); // ����Ʈ�� �迭�� ����ȯ

        if (m_target == null)
        {
            
            float targetPos = Vector3.Distance(transform.position, startPos);

            if(targetPos > 0.1f)
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

            return;
        }

        if (m_target.GetComponent<Character>().isDead)
        {
            FindClosetTarget(Spawner.m_monsters.ToArray());
        }

        float targetDistance = Vector3.Distance(transform.position, m_target.position);

        if(targetDistance <= target_Range && targetDistance > Attack_Range && isATTACK == false) // ���� Ÿ���� ���� ���� �ȿ��� ������, ���ݹ��� �ȿ��� ���� ��
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
