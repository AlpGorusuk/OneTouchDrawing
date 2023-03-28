using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected GameplayManager gameManager;
    protected StateMachine stateMachine;

    protected State(GameplayManager gameManager, StateMachine stateMachine)
    {
        this.gameManager = gameManager;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter()
    {

    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void Exit()
    {

    }

    protected void DisplayOnUI()
    {

    }
}