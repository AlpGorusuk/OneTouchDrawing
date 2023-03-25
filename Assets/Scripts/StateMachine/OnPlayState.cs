using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlayState : State
{
    private Vector2 mousePosition;
    public OnPlayState(GameManager gameManager, StateMachine stateMachine) : base(gameManager, stateMachine)
    {
    }
    public override void Enter()
    {
        InputControl.Instance.Add_Pointer_Down_Listener(On_Pointer_Down);
        InputControl.Instance.Add_Pointer_Up_Listener(On_Pointer_Up);
        InputControl.Instance.Add_Drag_Listener(On_Drag);
    }
    // Input ----------------------------

    private void On_Pointer_Down(Vector2 pos)
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D raycastHit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity);

        if (raycastHit.collider != null)
        {
            Dot selectedDot = raycastHit.collider.GetComponent<Dot>();

            if (selectedDot != null)
            {
                Debug.Log("TAPPED DOT");
            }
        }
    }

    private void On_Pointer_Up(Vector2 pos)
    {

    }

    private void On_Drag(Vector2 delta)
    {

    }
}
