using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Offline_Reward : UI_Base
{
    [SerializeField]
    private TextMeshProUGUI Offline_Time;
    [SerializeField]
    private TextMeshProUGUI money_reward_value;

    private double _money_reward_value;

    public override bool Init()
    {
        return base.Init();
    }

    /// <summary>
    /// ������ �Ǽ��� �������� ����â�� �����ص�, �ڵ����� �⺻ ������ ȹ���ϵ��� �մϴ�.
    /// </summary>
    public override void DisableOBJ()
    {
        base.DisableOBJ();
    }
}
