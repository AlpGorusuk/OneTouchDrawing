using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public StateMachine sceneStateMachine;
    public OnPlayState onPlayState;
    public WinState winState;
    [Header("UI Part")]
    public LevelCompletedScene levelCompletedScene;
    public GameObject RetryButtonObject;
    public Action NextButton_Callback;

    [Header("Gameplay Part")]
    public GraphControl graphControl;
    private void Awake()
    {
        sceneStateMachine = new StateMachine();
        onPlayState = new OnPlayState(this, sceneStateMachine);
        winState = new WinState(this, sceneStateMachine);
    }

    // Start is called before the first frame update
    void Start()
    {
        //set Init State
        sceneStateMachine.Initialize(onPlayState);
        //Init
        ShowRetryButton(Retrylevel);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCurrentState();
    }

    private void UpdateCurrentState()
    {
        if (sceneStateMachine.CurrentState != null)
        {
            sceneStateMachine.CurrentState.Update();
        }
    }

    //RetryButton
    private void Retrylevel()
    {
        GameControl.Instance.Load_Game();
    }
    private void ShowRetryButton(Action retry_callback)
    {
        NextButton_Callback = retry_callback;
        RetryButtonObject.SetActive(true);
    }
    public void RetryButtonClicked()
    {
        NextButton_Callback?.Invoke();
    }
}
