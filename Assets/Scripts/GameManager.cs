using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public StateMachine sceneStateMachine;
    public OnPlayState onPlayState;
    public WinState winState;
    public FailState failState;
    private void Awake()
    {
        sceneStateMachine = new StateMachine();
        onPlayState = new OnPlayState(this, sceneStateMachine);
        winState = new WinState(this, sceneStateMachine);
        failState = new FailState(this, sceneStateMachine);
        //set Init State
        sceneStateMachine.ChangeState(onPlayState);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
