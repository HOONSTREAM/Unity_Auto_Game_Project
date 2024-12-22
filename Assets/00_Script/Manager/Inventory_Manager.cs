using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ȹ���� �������� �����ϴ� �Ŵ��� �Դϴ�.
/// </summary>
public class Inventory_Manager 
{
  
    public void Get_Item(Item_Scriptable item, int Drop_count = 1)
    {
        if (Base_Manager.Data.Item_Holder.ContainsKey(item.name))
        {
            Base_Manager.Data.Item_Holder[item.name].Hero_Card_Amount += Drop_count;

            return;
        }

    }
}
