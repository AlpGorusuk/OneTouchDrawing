using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected GameManager gameManager;
    protected StateMachine stateMachine;

    protected State(GameManager gameManager, StateMachine stateMachine)
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