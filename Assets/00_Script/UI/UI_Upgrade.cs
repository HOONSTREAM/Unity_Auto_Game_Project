using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class UI_Upgrade : UI_Base
{
    [SerializeField]
    private UI_Upgrade_Parts UpGrade_Panel;
    [SerializeField]
    private Transform Content;

    private UI_Base parentBase;

    public void Initialize(UI_Base parent)
    {
        parentBase = parent;
        StartCoroutine(All_Upgrade_Coroutine());      
    }

    IEnumerator All_Upgrade_Coroutine()
    {

        var Data = Base_Manager.Data.Data_Character_Dictionary;
        
        foreach (var Character_Data in Data)
        {
            if (Can_Level_Up(Character_Data.Value))
            {
                var go = Instantiate(UpGrade_Panel, Content);
                go.gameObject.SetActive(true);

                int now_level = Character_Data.Value.holder.Hero_Level + 1; //TODO
                int value = 0;
                Calculate_Upgrade_Level(Character_Data.Value, ref value);
                go.Init(Character_Data.Value, now_level, value + 1);
            }

            yield return new WaitForSecondsRealtime(0.1f);
        }

        var heroes = parentBase.GetComponent<UI_Heros>();

        for (int i = 0; i < heroes.hero_parts.Count; i++)
        {
            heroes.hero_parts[i].Initialize();
        }

        Base_Manager.FireBase.WriteData();

    }

    private void Calculate_Upgrade_Level(Character_Holder holder, ref int value)
    {
        while (holder.holder.Hero_Card_Amount >= Utils.Data.heroCardData.Get_LEVELUP_Card_Amount(holder.Data.name))
        {
            holder.holder.Hero_Card_Amount -= Utils.Data.heroCardData.Get_LEVELUP_Card_Amount(holder.Data.name);
            holder.holder.Hero_Level++;
        }

        value = holder.holder.Hero_Level;
    }

    private bool Can_Level_Up(Character_Holder holder)
    {
        int value = Utils.Data.heroCardData.Get_LEVELUP_Card_Amount(holder.Data.name);
        if(holder.holder.Hero_Card_Amount >= value)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    
}
