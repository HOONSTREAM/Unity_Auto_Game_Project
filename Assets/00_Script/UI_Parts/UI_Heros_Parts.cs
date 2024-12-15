using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 영웅 창에서 확인 할 수 있는 각각의 영웅 카드를 제어합니다.
/// </summary>
public class UI_Heros_Parts : MonoBehaviour
{
    [SerializeField]
    private Image M_Silder, M_character_Image, M_Rarity_Image;
    [SerializeField]
    private TextMeshProUGUI M_Level, M_Count;
    [SerializeField]
    private GameObject Eqiup_Hero_Image;

  
    public Character_Scriptable Character;
    private UI_Heros parent;
    public GameObject Lock_OBJ;
    

    public void Init(Character_Scriptable data, UI_Heros parentsBASE)
    {
        parent = parentsBASE; 
        Character = data;

        //int LevelCount = (Base_Manager.Data.character_Holder[data.name].Hero_Level) * 5;

        int Card_Level_Count = Utils.Data.heroCardData.Get_LEVELUP_Card_Amount(data.name);

        M_Silder.fillAmount = (float)Base_Manager.Data.character_Holder[data.name].Hero_Card_Amount /(float)Card_Level_Count;
        M_Count.text = Base_Manager.Data.character_Holder[data.name].Hero_Card_Amount.ToString() + "/" + Card_Level_Count.ToString();
        M_Level.text = "LV." + (Base_Manager.Data.character_Holder[data.name].Hero_Level + 1).ToString();
        M_Rarity_Image.sprite = Utils.Get_Atlas(data.Rarity.ToString());
        M_character_Image.sprite = Utils.Get_Atlas(data.M_Character_Name);
        M_character_Image.SetNativeSize();
        
        RectTransform rect = M_character_Image.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector3(rect.sizeDelta.x / 2.5f, rect.sizeDelta.y / 2.5f);

        Get_Character_Check();
    }

    /// <summary>
    /// 보유중인 영웅을 터치했을 때의 기능을 구현합니다.
    /// </summary>
    public void Click_My_Hero()
    {
        parent.Set_Click(this);
        Render_Manager.instance.HERO.Get_Particle(true);
    }

    public void Get_Character_Check()
    {
        bool Equip = false;

        for(int i = 0; i<Base_Manager.Character.Set_Character.Length; i++)
        {
            if(Base_Manager.Character.Set_Character[i] != null)
            {
                if (Base_Manager.Character.Set_Character[i].Data == Character)
                {
                    Equip = true;
                }
            }
            
        }

        Eqiup_Hero_Image.gameObject.SetActive(Equip);
    }
}
