using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;

public partial class FireBase_Manager 
{
    public void WriteData()
    {
        #region DEFAULT DATA

        Data data = new Data();

        if (Data_Manager.Main_Players_Data != null)
        {
            data = Data_Manager.Main_Players_Data;
            data.EndDate = DateTime.Now.ToString();
            Debug.Log("����ð� : " + data.EndDate);
        }

        string DefalutJson = JsonUtility.ToJson(data);

        DB_reference.Child("USER").Child(currentUser.UserId).Child("DATA").SetRawJsonValueAsync(DefalutJson).ContinueWithOnMainThread(task => 
        {
            if (task.IsCompleted)
            {
                Debug.Log("Child.DATA ������ ���� �����Ͽ����ϴ�.");
            }
            else
            {
                Debug.LogError("������ ���� ���� : " + task.Exception.ToString());
            }

        });
        #endregion

        #region CHARACTER DATA

        string Character_Json = JsonConvert.SerializeObject(Base_Manager.Data.character_Holder);

        DB_reference.Child("USER").Child(currentUser.UserId).Child("CHARACTER").SetRawJsonValueAsync(Character_Json).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log(" Child.character ������ ���� �����Ͽ����ϴ�.");              
            }
            else
            {
                Debug.LogError("������ ���� ���� : " + task.Exception.ToString());
            }

        });
        #endregion

        #region ITEM_DATA
        string Item_Json = JsonConvert.SerializeObject(Base_Manager.Data.Item_Holder);

        DB_reference.Child("USER").Child(currentUser.UserId).Child("ITEM").SetRawJsonValueAsync(Item_Json).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log(" Child.Item ������ ���� �����Ͽ����ϴ�.");
            }
            else
            {
                Debug.LogError("������ ���� ���� : " + task.Exception.ToString());
            }

        });

        #endregion

    }

    public void ReadData()
    {
        #region Default_Data
        DB_reference.Child("USER").Child(currentUser.UserId).Child("DATA").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if(task.IsCompleted)
            {
                Debug.Log("USER�� DATA if�� task.iscompleted true ����");
                DataSnapshot snapshot = task.Result;

                var Default_Data = JsonUtility.FromJson<Data>(snapshot.GetRawJsonValue());
                Data data = new Data();
                if(Default_Data != null)
                {
                    data = Default_Data;                     
                }
                data.StartDate = DateTime.Now.ToString();
                Data_Manager.Main_Players_Data = data;
                Debug.Log("���۽ð� : " + data.StartDate);
                Loading_Scene.instance.LoadingMain();
            }

            else
            {
                Debug.LogError("������ �б� ���� : " + task.Exception.ToString());
            }
        });
        #endregion

        #region Character_Data

        DB_reference.Child("USER").Child(currentUser.UserId).Child("CHARACTER").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("USER�� CHARACTER if�� task.iscompleted true ����");
                DataSnapshot snapshot = task.Result;

                var data = JsonConvert.DeserializeObject<Dictionary<string, Holder>>(snapshot.GetRawJsonValue());
               
                Base_Manager.Data.character_Holder = data;
                

                Debug.Log("�ε�� ������: " + JsonConvert.SerializeObject(Base_Manager.Data.character_Holder));


                Base_Manager.Data.Init(); // TODO
                
                           
            }

            else
            {
                Debug.LogError("������ �б� ���� : " + task.Exception.ToString());
            }
        });

        #endregion

        #region Item_Data

        DB_reference.Child("USER").Child(currentUser.UserId).Child("ITEM").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("USER�� ITEM if�� task.iscompleted true ����");
                DataSnapshot snapshot = task.Result;

                var data = JsonConvert.DeserializeObject<Dictionary<string, Holder>>(snapshot.GetRawJsonValue());

                Base_Manager.Data.Item_Holder = data;

                Debug.Log("�ε�� ������: " + JsonConvert.SerializeObject(Base_Manager.Data.Item_Holder));


                Base_Manager.Data.Init(); // TODO


            }

            else
            {
                Debug.LogError("������ �б� ���� : " + task.Exception.ToString());
            }
        });

        #endregion

    }


}
