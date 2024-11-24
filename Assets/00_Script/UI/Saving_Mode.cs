using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Saving_Mode : UI_Base
{
    [SerializeField]
    private TextMeshProUGUI Battery_Text;
    [SerializeField]
    private TextMeshProUGUI Time_Text;
    [SerializeField]
    private Image Battery_Fill_Image;
    [SerializeField]
    private Transform Content;
    [SerializeField]
    private UI_Inventory_Parts item_parts;

    private void Update()
    {
        Battery_Text.text = (SystemInfo.batteryLevel * 100.0f).ToString() + "%";
        Battery_Fill_Image.fillAmount = SystemInfo.batteryLevel;

        Time_Text.text = System.DateTime.Now.ToString("HH:mm:ss"); //�ڵ����ð�����, �������κ��� ����ϸ� ���׷� �ǿ�� �� �ִ�.
    }
}
