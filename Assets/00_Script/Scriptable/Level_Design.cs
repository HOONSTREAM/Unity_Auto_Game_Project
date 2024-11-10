using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ���������� ���� �� ĳ������ �ɷ�ġ�� ���� �����������Դϴ�.
/// �������������� �̿��մϴ�.
/// </summary>
[CreateAssetMenu(fileName = "Level_Design", menuName = "Level Design/Level Design Data")]
public class Level_Design : ScriptableObject
{
    public LevelData levelData; // Ÿ�԰�ü����
    [Space(20f)]
    public StageData stageData;

}

[System.Serializable]
public class LevelData
{
    public int Current_Level;
    [Range(0.0f, 10.0f)]
    public float ATK, HP, EXP, MAX_EXP, LEVELUP_MONEY;

    [Space(20f)]
    [Header("BASE_VALUE")]
    [Space(10f)]
    public int Base_ATK;
    public int Base_HP;
    public int Base_EXP;
    public int Base_MAX_EXP;
    public int Base_LEVELUP_MONEY;

    public double Get_ATK() => Utils.CalculateValue(Base_ATK, Base_Manager.Data.Player_Level, ATK);
    public double Get_HP() => Utils.CalculateValue(Base_HP, Base_Manager.Data.Player_Level, HP);
    public double Get_EXP() => Utils.CalculateValue(Base_EXP, Base_Manager.Data.Player_Level, EXP);
    public double Get_MAXEXP() => Utils.CalculateValue(Base_MAX_EXP, Base_Manager.Data.Player_Level, MAX_EXP);
    public double Get_LEVELUP_MONEY() => Utils.CalculateValue(Base_LEVELUP_MONEY, Base_Manager.Data.Player_Level, LEVELUP_MONEY);


}

[System.Serializable]
public class StageData
{
    public int Current_Stage;
    [Range(0.0f, 10.0f)]
    public float MONSTER_ATK, MONSTER_HP, DROP_MONEY;

    [Space(20f)]
    [Header("BASE_VALUE")]
    [Space(10f)]
    public int Base_MONSTER_ATK;
    public int Base_MONSTER_HP;
    public int Base_DROP_MONEY;

    public double Get_ATK() => Utils.CalculateValue(Base_MONSTER_ATK, Base_Manager.Data.Player_Stage, MONSTER_ATK);
    public double Get_HP() => Utils.CalculateValue(Base_MONSTER_HP, Base_Manager.Data.Player_Stage, MONSTER_HP);
    public double Get_DROP_MONEY() => Utils.CalculateValue(Base_DROP_MONEY, Base_Manager.Data.Player_Stage, DROP_MONEY);
}
