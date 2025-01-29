using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AnimatorHelper : MonoBehaviour
{
    public Action callback;
    public Action OnStartAnimCallBack;

    public void FirstCallback()
    {
        if(OnStartAnimCallBack != null)
        {
            OnStartAnimCallBack.Invoke();
        }
    }
    public void Callback()
    {
        if(callback != null)    
        { 
            callback.Invoke();
        }
    }
   public void Destroy()
    {
        Destroy(gameObject); //distrugge l'oggetto
    }
}
