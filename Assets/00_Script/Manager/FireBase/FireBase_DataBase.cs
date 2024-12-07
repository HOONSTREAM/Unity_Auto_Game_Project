using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class User
{
    public string userName;
    public int Stage;
}
public partial class FireBase_Manager 
{
    public void WriteData()
    {

        

        //DB_reference.Child("USER").Child(user.userName).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
        //{
        //    if (task.IsCompleted)
        //    {
        //        Debug.Log("������ ���� �����Ͽ����ϴ�.");
        //    }
        //    else
        //    {
        //        Debug.LogError("������ ���� ���� : " + task.Exception.ToString());
        //    }

        //});
    }

    public void ReadData()
    {
        DB_reference.Child("USER").Child(currentUser.UserId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if(task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                User user = JsonUtility.FromJson<User>(snapshot.GetRawJsonValue());
                Debug.Log("����� �̸� :" + user.userName + ", ���� �������� : " + user.Stage);
                Loading_Scene.instance.LoadingMain();
            }

            else
            {
                Debug.LogError("������ �б� ���� : " + task.Exception.ToString());
            }
        });

    } 

    
}
