using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Character
{
    public float M_Speed;
    

    bool isSpawn = false;

   
    public void Init()
    {
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

   
}
