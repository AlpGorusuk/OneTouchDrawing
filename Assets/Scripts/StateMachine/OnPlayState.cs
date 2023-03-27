using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlayState : State
{
    private Vector2 mousePosition;
    private bool isDragging;
    private Dot selectedDot;
    public OnPlayState(GameManager gameManager, StateMachine stateMachine) : base(gameManager, stateMachine)
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

                if (selectedDot == null)
                {
                    Action<Vector2> _updateLRCallback = gameManager.graphControl.UpdateSelectLineRendererCallback;
                    _updateLRCallback?.Invoke(mousePosition);
                }
                else
                {

                }
            }
        }
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
                Action _dotCallback = selectedDot.ClickedCallback;
                _dotCallback?.Invoke();
                Action<Dot> _initLRCallback = gameManager.graphControl.DotClickCallback;
                _initLRCallback?.Invoke(selectedDot);
                isDragging = true;
            }
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
        if (selectedDot == null) { return; }
        selectedDot = null;
        isDragging = false;
        mousePosition = Vector2.zero;
        Action _resetLRCallback = gameManager.graphControl.ResetCurrentLineRendererCallback;
        _resetLRCallback?.Invoke();
    }

    private void On_Drag(Vector2 delta)
    {

    }
}
