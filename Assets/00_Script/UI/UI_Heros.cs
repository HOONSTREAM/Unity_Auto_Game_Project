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

    public override bool Init()
    {

        InitButtons();
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
        for(int i = 0; i < hero_parts.Count; i++)
        {
            hero_parts[i].Lock_OBJ.SetActive(true);
            hero_parts[i].GetComponent<Outline>().enabled = false;
        }

        parts.Lock_OBJ.SetActive(false);
        parts.GetComponent<Outline>().enabled = true;

    }

    /// <summary>
    /// ������ Ŭ�� ��, �÷��� ��ư�� �����Ǹ�, ������ ����ϱ� ����, �÷��� ��ư ���� ������ �ʴ� ������ ��ư�� �����մϴ�.
    /// </summary>
    public void InitButtons()
    {
        for(int i = 0; i<Render_Manager.instance.HERO.Circles.Length; i++)
        {
            var go = new GameObject("Button").AddComponent<Button>();

            go.transform.SetParent(this.transform); // �ش� ������Ʈ�� UI_Heros �˾� �ϴܿ� �ڽĿ�����Ʈ�� ����
            go.gameObject.AddComponent<Image>();
            go.gameObject.AddComponent<RectTransform>();
            
            RectTransform rect = go.GetComponent<RectTransform>();

            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            go.transform.position = Render_Manager.instance.ReturnScreenPoint(Render_Manager.instance.HERO.Circles[i]);        
        }
        
    }

}
