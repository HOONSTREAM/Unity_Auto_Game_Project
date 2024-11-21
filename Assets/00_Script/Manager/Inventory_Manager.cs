using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ȹ���� �������� �����ϴ� �Ŵ��� �Դϴ�.
/// </summary>
public class Inventory_Manager 
{
    public Dictionary<string, Item> Items_Dict = new Dictionary<string, Item>(); 
   
    public void Get_Item(Item_Scriptable item)
    {
        if (Items_Dict.ContainsKey(item.name))
        {
            Items_Dict[item.name].Count++;

            return;
        }

        Items_Dict.Add(item.name, new Item { data = item, Count = 1 });
       
    }
}
