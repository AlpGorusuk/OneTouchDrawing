using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelCompletedScene : MonoBehaviour
{
    Action Next_Callback;
    public GameObject next_Button;
    public void Show(Action next_callback)
    {
        Next_Callback = next_callback;
        gameObject.SetActive(true);
        next_Button.SetActive(true);
    }
    public void Hide()
    {

    }

    public void Next_Clicked()
    {
        Next_Callback?.Invoke();
    }
}
