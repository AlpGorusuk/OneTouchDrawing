using System;
using System.Collections.Generic;
using UnityEngine;

public class FigureControl : MonoBehaviour
{
    public Transform[] points;
    public LineRenderer lineRenderer;
}
[Serializable]
public class FigureProperty
{
    public Dot _selectedDot;
    public List<Dot> _contactDotList;
}