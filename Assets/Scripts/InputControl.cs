using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InputControl : Singleton<InputControl>
{
    private bool Input_Enabled = true;

    private List<Action<Vector2>> Pointer_Down_Listeners;
    private List<Action<Vector2>> Pointer_Up_Listeners;
    private List<Action<Vector2>> Drag_Listeners;

    private Vector2 Pointer_Position;

    private bool Pointer_Down = false;

    // Init =====================================================================

    public override void Awake()
    {
        base.Awake();
        Pointer_Down_Listeners = new List<Action<Vector2>>();
        Pointer_Up_Listeners = new List<Action<Vector2>>();
        Drag_Listeners = new List<Action<Vector2>>();
    }

    // Enable/Disable ===========================================================

    public void Enable_Input()
    {
        Input_Enabled = true;
    }

    public void Disable_Input()
    {
        if (Pointer_Down)
        {
            On_Pointer_Up(Pointer_Position);
        }
        Input_Enabled = false;
    }
    // Add/Remove Listeners =====================================================

    // Add

    public void Add_Pointer_Down_Listener(Action<Vector2> callback)
    {
        Pointer_Down_Listeners.Add(callback);
    }

    public void Add_Pointer_Up_Listener(Action<Vector2> callback)
    {
        Pointer_Up_Listeners.Add(callback);
    }

    public void Add_Drag_Listener(Action<Vector2> callback)
    {
        Drag_Listeners.Add(callback);
    }

    // Remove 

    public void Remove_Pointer_Down_Listener(Action<Vector2> callback)
    {
        Pointer_Down_Listeners.Remove(callback);
    }

    public void Remove_Pointer_Up_Listener(Action<Vector2> callback)
    {
        Pointer_Up_Listeners.Remove(callback);
    }

    public void Remove_Drag_Listener(Action<Vector2> callback)
    {
        Drag_Listeners.Remove(callback);
    }

    // Input Actions ===========================================================

    public void On_Pointer_Down(Vector2 pos)
    {
        if (!Input_Enabled) { return; }
        Pointer_Position = pos;
        for (int i = 0; i < Pointer_Down_Listeners.Count; i++)
        {
            Pointer_Down_Listeners[i].Invoke(pos);
        }
        Pointer_Down = true;
    }

    public void On_Pointer_Up(Vector2 pos)
    {
        if (!Input_Enabled) { return; }
        for (int i = 0; i < Pointer_Up_Listeners.Count; i++)
        {
            Pointer_Up_Listeners[i].Invoke(pos);
        }
        Pointer_Down = false;
    }

    public void On_Drag(Vector2 delta)
    {
        if (!Input_Enabled) { return; }
        Pointer_Position += delta;
        for (int i = 0; i < Drag_Listeners.Count; i++)
        {
            Drag_Listeners[i].Invoke(delta);
        }
    }
}
