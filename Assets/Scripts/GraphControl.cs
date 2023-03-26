using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class GraphControl : MonoBehaviour
{
    //List of dots
    public List<Dot> dotList;

    // List of vertices
    private List<Vertex> vertexList = new List<Vertex>();

    // List of edges
    public List<Edge> edgeList;

    //Select Line
    GameObject currLineRendererObject;
    private LineRenderer currLineRenderer;

    //Selected Dot
    private Dot currSelectedDot;

    //Callback
    public Action<Vector2, Dot> DotClickCallback;
    public Action<Vector2> UpdateSelectLineRendererCallback;
    public Action ResetCurrentLineRendererCallback;
    private void OnEnable()
    {
        DotClickCallback += InitSelectLineRenderPos;
        UpdateSelectLineRendererCallback += UpdateCurrentLineRendererPos;
        ResetCurrentLineRendererCallback += ResetCurrentLineRenderer;
    }
    private void OnDisable()
    {
        DotClickCallback -= InitSelectLineRenderPos;
    }
    private void Awake()
    {
        SetVertexList();
    }

    //Vertex adn Edges
    [ContextMenu("Set AllPossible Edges")]
    public void SetAllPossibleEdges()
    {
        // int initListCount = edgeList.Count;
        // for (int i = 0; i < initListCount; i++)
        // {
        //     int temp1 = edgeList[i].indexOfVertex1;
        //     int temp2 = edgeList[i].indexOfVertex2;

        //     Edge shiftEdge = new Edge(temp2, temp1);
        //     edgeList.Add(shiftEdge);
        // }
    }
    private void SetVertexList()
    {
        for (int i = 0; i < dotList.Count; i++)
        {
            Dot _dot = dotList[i];
            Vertex _vertex = new Vertex();
            _vertex.InitVertex(_dot);
            vertexList.Add(_vertex);
        }
    }
    private void CheckEdgesForCurrentDot()
    {
        if (currSelectedDot == null) return;
        int selectedIndex = currSelectedDot.Index;

    }

    //Select LRenderer Prcocess
    private void InitSelectLineRenderPos(Vector2 pos, Dot currDot)
    {
        currSelectedDot = currDot;
        currLineRendererObject = ObjectPooler.Generate("lineRenderer", transform.position, Quaternion.identity);
        currLineRenderer = currLineRendererObject.GetComponent<LineRenderer>();
        int temp = 2;
        currLineRenderer.positionCount = temp;
        currLineRenderer.SetPosition(0, pos);
    }
    private void UpdateCurrentLineRendererPos(Vector2 pos)
    {
        currLineRenderer.SetPosition(1, pos);
        Vector3 tempPos = currLineRenderer.GetPosition(1);
    }
    private void ResetCurrentLineRenderer()
    {
        Vector3 tempPos = currLineRenderer.GetPosition(0);
        currLineRenderer.SetPosition(1, tempPos);
        ObjectPooler.Destroy(currLineRendererObject);
        currLineRenderer = null;
    }
}
[Serializable]
public class Vertex
{
    public int vertexIndex;
    public Vector2 _pos;
    public Dot vertexDot;
    public List<Dot> nonEdgeDotList;
    public void InitVertex(Dot _dot)
    {
        vertexDot = _dot;
        this.vertexIndex = vertexDot.Index;
        this._pos = vertexDot.transform.position;
    }
}