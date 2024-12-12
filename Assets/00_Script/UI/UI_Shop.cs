using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Shop : UI_Base
{

    public void GachaButton(int value)
    {
        Base_Canvas.instance.Get_UI("GaCha");
        var UI = Utils.UI_Holder.Peek().gameObject.GetComponent<UI_Gacha>(); // Get_UI�� ��ȯ�� Gacha ������Ʈ�� �����´�.
        UI.Get_Gacha_Hero(value);
    }
    public void GachaButton_ADS()
    {
        Base_Manager.ADS.ShowRewardedAds(() => GachaButton(1));
    }

    public override void DisableOBJ()
    {
        Main_UI.Instance.Layer_Check(-1);
        base.DisableOBJ();
    }
}
