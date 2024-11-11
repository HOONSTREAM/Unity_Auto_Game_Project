using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


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
        M_Rarity_Image.sprite = Utils.Get_Atlas(data.Rarity.ToString());
        M_character_Image.sprite = Utils.Get_Atlas(data.M_Character_Name);
        M_character_Image.SetNativeSize();
        
        RectTransform rect = M_character_Image.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector3(rect.sizeDelta.x / 2.5f, rect.sizeDelta.y / 2.5f);

        Get_Character_Check();
    }

    /// <summary>
    /// �������� ������ ��ġ���� ���� ����� �����մϴ�.
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
