using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Main_UI : MonoBehaviour
{
    public static Main_UI Instance = null;
    [SerializeField]
    private TextMeshProUGUI _level_Text;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        Level_Text_Check();
    }

    public void Level_Text_Check()
    {
        _level_Text.text = "LV." + (Base_Manager.Player.Level + 1).ToString();
    }

}
