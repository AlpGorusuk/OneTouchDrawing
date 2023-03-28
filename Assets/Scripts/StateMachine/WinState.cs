using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinState : State
{
    public WinState(GameplayManager gameManager, StateMachine stateMachine) : base(gameManager, stateMachine)
    {
    }
    public override void Enter()
    {
        gameManager.levelCompletedScene.Show(NextButtonClicked);
        ObjectPooler.Generate("levelCompleteParticle");
        GameControl.Instance.Finish_Current_Level();
    }
    private void NextButtonClicked()
    {
        GameControl.Instance.Load_Game();
    }
}
