using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���� ��ġ â�� �ٷ�� ��ũ��Ʈ �Դϴ�.
/// </summary>
public class UI_Heros : UI_Base
{
    public Transform Content;
    public GameObject Parts;
    public List<UI_Heros_Parts> hero_parts = new List<UI_Heros_Parts>();
    private Dictionary<string, Character_Scriptable> _dict = new Dictionary<string, Character_Scriptable>();
    private Character_Scriptable Character;
    public override bool Init()
    {

        InitButtons();
        Render_Manager.instance.HERO.Init_Hero();

        Main_UI.Instance.FadeInOut(true, true, null);


        var Data = Base_Manager.Data.Data_Character_Dictionary;

        foreach (var data in Data)
        {
            _dict.Add(data.Value.Data.M_Character_Name, data.Value.Data);
        }
      

        var sort_dict = _dict.OrderByDescending(x => x.Value.Rarity);


        foreach (var data in sort_dict)
        {
            var Object = Instantiate(Parts, Content).GetComponent<UI_Heros_Parts>(); // Content�� �θ������Ʈ�� �ؼ� Parts�� ����
            hero_parts.Add(Object);
            Object.Init(data.Value, this);
        }

        return base.Init();
    }

    public override void DisableOBJ()
    {
        Main_UI.Instance.FadeInOut(false, true, () =>
        {
            Main_UI.Instance.FadeInOut(true, false, null);
            base.DisableOBJ();
        });

    }

    /// <summary>
    /// �÷��̾, ���� â���� Ư�� ������ ��ġ�������� ������ �����մϴ�.
    /// </summary>
    public void Set_Click(UI_Heros_Parts parts)
    {

        if (parts == null)
        {
            for (int i = 0; i < hero_parts.Count; i++)
            {
                hero_parts[i].Lock_OBJ.SetActive(false);
                hero_parts[i].GetComponent<Outline>().enabled = false;
            }
        }

        else
        {
            Character = parts.Character;

            for (int i = 0; i < hero_parts.Count; i++)
            {
                hero_parts[i].Lock_OBJ.SetActive(true);
                hero_parts[i].GetComponent<Outline>().enabled = false;
            }

            parts.Lock_OBJ.SetActive(false);
            parts.GetComponent<Outline>().enabled = true;

        }

    }

    /// <summary>
    /// ������ Ŭ�� ��, �÷��� ��ư�� �����Ǹ�, ������ ����ϱ� ����, �÷��� ��ư ���� ������ �ʴ� ������ ��ư�� �����մϴ�.
    /// </summary>
    public void InitButtons()
    {
        for(int i = 0; i<Render_Manager.instance.HERO.Circles.Length; i++)
        {
            int index = i;
            var go = new GameObject("Button").AddComponent<Button>();
            go.onClick.AddListener(() => Set_Character_Button(index));

            go.transform.SetParent(this.transform); // �ش� ������Ʈ�� UI_Heros �˾� �ϴܿ� �ڽĿ�����Ʈ�� ����
            go.gameObject.AddComponent<Image>();
            go.gameObject.AddComponent<RectTransform>();
            
            RectTransform rect = go.GetComponent<RectTransform>();

            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            rect.sizeDelta = new Vector2(150.0f, 150.0f);
            go.gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0.01f);

            go.transform.position = Render_Manager.instance.ReturnScreenPoint(Render_Manager.instance.HERO.Circles[i]);        
        }
        
    }

    private void Set_Character_Button(int value)
    {
        Base_Manager.Character.Get_Character(value, Character.M_Character_Name);
        Render_Manager.instance.HERO.Get_Particle(false);
        Set_Click(null);
        Render_Manager.instance.HERO.Init_Hero();

        for(int i = 0; i<hero_parts.Count; i++)
        {
            hero_parts[i].Get_Character_Check();
        }

        Main_UI.Instance.Set_Character_Data();
    }

}
