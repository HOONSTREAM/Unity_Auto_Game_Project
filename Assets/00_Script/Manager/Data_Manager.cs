using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 서버에서 관리 할 DB를 저장하는 용도로 사용하는 매니저 입니다.
/// </summary>
/// 
public class Character_Holder
{
    public Character_Scriptable Data;
    public Holder holder;
}

public class Holder
{
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

    public static Data Main_Players_Data = new Data();
    /// <summary>
    /// 플레이어가 현재 소유중인 영웅들을 관리합니다.
    /// </summary>
    public Dictionary<string, Character_Holder> Data_Character_Dictionary = new Dictionary<string, Character_Holder>();
    public Dictionary<string, Holder> character_Holder = new Dictionary<string, Holder>();

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
            Holder s_holder = new Holder();
            if (character_Holder.ContainsKey(data.M_Character_Name))
            {
                s_holder = character_Holder[data.M_Character_Name];
            }
            else
            {
                character_Holder.Add(data.M_Character_Name, s_holder);
            }
            
            character.holder = s_holder;

            Data_Character_Dictionary.Add(data.M_Character_Name, character);
        }
    }
}
