using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnReadyEvent();
public delegate void OnPlayEvent();
public delegate void OnBossEvent();
public delegate void OnClearEvent();
public delegate void OnDeadEvent();

public class Stage_Manager
{ 
    public  static Stage_State M_State;

    public  OnReadyEvent M_ReadyEvent;
    public  OnPlayEvent M_PlayEvent;
    public  OnBossEvent M_BossEvent;
    public  OnClearEvent M_ClearEvent;
    public  OnDeadEvent M_DeadEvent;

  
    public void State_Change(Stage_State state)
    {
        M_State = state;
        switch(state)
        {
            case Stage_State.Ready:
                M_ReadyEvent?.Invoke();
                Base_Manager.instance.Coroutine_Action(2.0f, () => State_Change(Stage_State.Play));
                break;
            case Stage_State.Play:
                M_PlayEvent?.Invoke();
                break;
            case Stage_State.Boss:
                M_BossEvent?.Invoke();  
                break;
            case Stage_State.Clear:
                M_ClearEvent?.Invoke();
                break;
            case Stage_State.Dead:
                M_DeadEvent?.Invoke();
                break;
        }
    }
}
