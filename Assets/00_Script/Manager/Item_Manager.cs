using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������� �����ϴ� �Ŵ��� �Դϴ�.
/// </summary>
public class Item_Manager 
{
    /// <summary>
    /// ��� ������ �����Ͱ� ����ִ� ��ųʸ��� ��ȸ�ϸ鼭 ���� Ȯ���� �ӽ� ����Ʈ�� ��� �������� ��ȯ�մϴ�.
    /// </summary>
    /// <returns></returns>
    public List<Item_Scriptable> Get_Drop_Set()
    {
        List<Item_Scriptable> objs = new List<Item_Scriptable>();

        foreach(var data in Base_Manager.Data.Data_Item_Dictionary)
        {
            if(data.Value.ItemType == ItemType.Consumable) // ������ ������� �ʵ��� ó��
            {
                float ValueCount = Random.Range(0.0f, 100.0f);
                if (ValueCount <= data.Value.Item_Chance)
                {
                    objs.Add(data.Value);
                }
            }
          
        }

        return objs;
    }
   
}
