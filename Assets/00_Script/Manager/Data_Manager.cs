using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������� ���� �� DB�� �����ϴ� �뵵�� ����ϴ� �Ŵ��� �Դϴ�.
/// </summary>
/// 
public class Character_Holder
{
    public Character_Scriptable Data;
    public int Hero_Level;
    public int Hero_Card_Amount;
}

public class Data
{
    public double Player_Money;
    public int Player_Level;
    public double EXP;
    public int Player_Stage = default;
    public float[] Buff_Timers = { 0.0f, 0.0f, 0.0f };
    public float buff_x2_speed = 0.0f;
    public int Buff_Level, Buff_Level_Count;
}

public class Data_Manager
{

    public static Data Main_Players_Data;
    /// <summary>
    /// �÷��̾ ���� �������� �������� �����մϴ�.
    /// </summary>
    public Dictionary<string, Character_Holder> Data_Character_Dictionary = new Dictionary<string, Character_Holder>(); 


    public void Init()
    {
        Set_Character();
    }
    private void Set_Character()
    {
        var datas = Resources.LoadAll<Character_Scriptable>("Scriptable/Character");

        foreach(var data in datas)
        {
            var character = new Character_Holder();

            character.Data = data;
            character.Hero_Level = 0;
            character.Hero_Card_Amount = 0;

            Data_Character_Dictionary.Add(data.M_Character_Name, character);
        }
    }
}
