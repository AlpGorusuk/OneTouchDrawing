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
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 temp = mousePosition;
            Action<Vector2> _updateLRCallback = gameManager.figureControl.UpdateSelectLineRendererCallback;
            _updateLRCallback?.Invoke(temp);
        }

    }
    // Input ----------------------------

    private void On_Pointer_Down(Vector2 pos)
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D raycastHit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity);

        if (raycastHit.collider != null)
        {
            selectedDot = raycastHit.collider.GetComponent<Dot>();

            if (selectedDot != null)
            {
                Action _dotCallback = selectedDot.ClickedCallback;
                _dotCallback?.Invoke();
                Action<Vector2, Dot> _initLRCallback = gameManager.figureControl.DotClickCallback;
                Vector2 temp = selectedDot.GetPos();
                _initLRCallback?.Invoke(temp, selectedDot);
                isDragging = true;
            }
        }
    }

    private void On_Pointer_Up(Vector2 pos)
    {
        if (selectedDot == null) { return; }
        selectedDot = null;
        isDragging = false;
        mousePosition = Vector2.zero;
        Action _resetLRCallback = gameManager.figureControl.ResetCurrentLineRendererCallback;
        _resetLRCallback?.Invoke();
    }

    private void On_Drag(Vector2 delta)
    {

    }
}
