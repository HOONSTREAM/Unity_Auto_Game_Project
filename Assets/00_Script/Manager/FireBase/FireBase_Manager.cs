using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using UnityEngine;

public partial class FireBase_Manager
{
    private FirebaseAuth auth;
    private FirebaseUser currentUser;
    public void Init()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if(task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                currentUser = auth.CurrentUser;
                GuestLogin();
                Debug.Log("Firebase �ʱ�ȭ�� �����Ͽ����ϴ�.");
            }
            else
            {
                Debug.Log("Firebase �ʱ�ȭ�� �����Ͽ����ϴ�.");
            }

        });

    }
  
}
