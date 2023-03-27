using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dot : MonoBehaviour
{
    [SerializeField]
    private GameObject _clickedBG;
    public Action ClickedCallback, ResetCallback;
    public int Index;
    private void OnEnable()
    {
        ClickedCallback += ShowClickedBG;
        ResetCallback += Reset;
    }
    private void OnDisable()
    {
        ClickedCallback -= ShowClickedBG;
        ResetCallback -= Reset;
    }
    private void ShowClickedBG()
    {
        _clickedBG.SetActive(true);
    }

    public void Reset() { _clickedBG.SetActive(false); }
    public Vector2 GetPos() { return transform.position; }
    public int GetIndex() { return Index; }
}
