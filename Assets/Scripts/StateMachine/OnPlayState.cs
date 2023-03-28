using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlayState : State
{
    private Vector2 mousePosition;
    private bool isDragging;
    Action<Vector2> _updateLineCallback;
    Action _dotCallback;
    public OnPlayState(GameplayManager gameManager, StateMachine stateMachine) : base(gameManager, stateMachine)
    {
    }
    public override void Enter()
    {
        InputControl.Instance.Add_Pointer_Down_Listener(On_Pointer_Down);
        InputControl.Instance.Add_Pointer_Up_Listener(On_Pointer_Up);
        InputControl.Instance.Add_Drag_Listener(On_Drag);
    }
    public override void Update()
    {
        base.Update();
        if (isDragging)
        {
            RaycastHit2D raycastHit = SendRaycast();
            Vector2 temp = mousePosition;
            if (raycastHit.collider != null)
            {
                Dot selectedDot = raycastHit.collider.GetComponent<Dot>();
                bool isAvailableEdge = gameManager.graphControl.isAvailableEdge(selectedDot.GetIndex());
                if (isAvailableEdge)
                {
                    _dotCallback = selectedDot.ClickedCallback;
                    _dotCallback?.Invoke();
                    Action<Dot> _startLineCallback = gameManager.graphControl.vertexClickedCallback;
                    _startLineCallback?.Invoke(selectedDot);
                    bool isAllEdgesVisited = gameManager.graphControl.IsAllEdgesVisited();
                    Debug.Log(isAllEdgesVisited);
                    if (isAllEdgesVisited) { stateMachine.ChangeState(gameManager.winState); }
                }
                else
                {
                    UpdateLineCallback(gameManager.graphControl.updateLineCallback, temp);
                }
            }
            else
            {
                UpdateLineCallback(gameManager.graphControl.updateLineCallback, temp);
            }
        }
    }
    public override void Exit()
    {
        base.Exit();
        InputControl.Instance.Remove_Pointer_Down_Listener(On_Pointer_Down);
        InputControl.Instance.Remove_Pointer_Up_Listener(On_Pointer_Up);
        InputControl.Instance.Remove_Drag_Listener(On_Drag);
    }

    private void UpdateLineCallback(Action<Vector2> updateLineCallback, Vector2 temp)
    {
        Action<Vector2> _updateLineCallback = updateLineCallback;
        _updateLineCallback?.Invoke(temp);
    }

    // Input

    private void On_Pointer_Down(Vector2 pos)
    {
        RaycastHit2D raycastHit = SendRaycast();

        if (raycastHit.collider != null)
        {
            Dot selectedDot = raycastHit.collider.GetComponent<Dot>();

            if (selectedDot != null)
            {
                _dotCallback = selectedDot.ClickedCallback;
                _dotCallback?.Invoke();
                Action<Dot> _startLineCallback = gameManager.graphControl.vertexClickedCallback;
                _startLineCallback?.Invoke(selectedDot);
                isDragging = true;
            }
        }
        else
        {
            bool isLineNull = gameManager.graphControl.isCurrentLineNull();
            if (!isLineNull)
            {
                isDragging = true;
            }
            _updateLineCallback = gameManager.graphControl.updateLineCallback;
            _updateLineCallback?.Invoke(mousePosition);
        }
    }

    private RaycastHit2D SendRaycast()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D raycastHit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity);
        return raycastHit;
    }

    private void On_Pointer_Up(Vector2 pos)
    {
        isDragging = false;
        _updateLineCallback = gameManager.graphControl.updateLineCallback;
        _updateLineCallback?.Invoke(mousePosition);
    }

    private void On_Drag(Vector2 delta)
    {

    }
}
