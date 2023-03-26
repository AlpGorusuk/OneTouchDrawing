using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public StateMachine sceneStateMachine;
    public OnPlayState onPlayState;
    public WinState winState;
    public FailState failState;
    public GraphControl figureControl;
    private void Awake()
    {
        sceneStateMachine = new StateMachine();
        onPlayState = new OnPlayState(this, sceneStateMachine);
        winState = new WinState(this, sceneStateMachine);
        failState = new FailState(this, sceneStateMachine);

    }

    // Start is called before the first frame update
    void Start()
    {
        //set Init State
        sceneStateMachine.Initialize(onPlayState);
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
}
