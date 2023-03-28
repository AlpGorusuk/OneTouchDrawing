using System;
using System.Collections.Generic;
using UnityEngine;

public class GraphControl : MonoBehaviour
{
    public int startIndexForCheckEdgesVisited = 0;
    public List<Dot> dotList;
    public List<Edge> edgeList;
    public List<Edge> allPossibleEdgeList;
    Dictionary<int, Vector2> Vertices = new Dictionary<int, Vector2>();
    List<int> eulerPath = new List<int>();
    public LineRenderer BGLineRenderer;
    //OnGameplay variables
    private LineRenderer currGameplayLineRenderer;
    private const int LINE_RENDERER_POS_COUNT = 2;
    private bool[] visitedEdge;
    private List<int> availableEdgeIndexList = new List<int>();
    private List<int> visitableEdgeIndices;
    //Gameplay Callbacks
    public Action<Dot> vertexClickedCallback;
    public Action<Vector2> updateLineCallback;
    public Dot startVertex;
    private void OnEnable()
    {
        vertexClickedCallback += VisitVertex;
        updateLineCallback += UpdateLine;
    }
    private void OnDisable()
    {
        vertexClickedCallback -= VisitVertex;
        updateLineCallback -= UpdateLine;
    }
    private void Awake()
    {
        BGLineRenderer = GetComponent<LineRenderer>();
        visitedEdge = new bool[allPossibleEdgeList.Count];
        for (int i = 0; i < visitedEdge.Length; i++)
            visitedEdge[i] = false;
        visitableEdgeIndices = new List<int>();
    }
    //OnEditor Updates
    [ContextMenu("Reset Graph")]
    public void ResetGraph()
    {
        BGLineRenderer.positionCount = 0;
        ResetVertices();
    }
    [ContextMenu("Set Graph")]
    public void SetGraph()
    {
        SetVertices();
        SetEulerPath();
        DrawEulerPath(eulerPath);
    }
    [ContextMenu("Set All Possible Edges")]
    public void SetAllPossibleEdges()
    {
        int tempCount = edgeList.Count;
        for (int i = 0; i < tempCount; i++)
        {
            int from = edgeList[i].FromVertex;
            int to = edgeList[i].ToVertex;
            Edge _edge = new Edge(to, from);
            allPossibleEdgeList.Add(_edge);
        }
        allPossibleEdgeList.AddRange(edgeList);
    }
    [ContextMenu("reset All Possible Edges")]
    public void ResetAllPossibleEdges()
    {
        allPossibleEdgeList.Clear();
    }

    private void SetEulerPath()
    {
        Graph g = new Graph(edgeList.Count);
        for (int i = 0; i < edgeList.Count; i++)
        {
            int fv = edgeList[i].FromVertex;
            int tv = edgeList[i].ToVertex;
            g.AddEdge(fv, tv);
        }
        eulerPath = g.FindEulerPath(startIndexForCheckEdgesVisited);
        eulerPath.Reverse();
    }

    private void SetVertices()
    {
        for (int i = 0; i < dotList.Count; i++)
        {
            int key = i;
            Vector2 value = dotList[key].GetPos();
            Vertices.Add(key, value);
        }
    }
    private void ResetVertices()
    {
        Vertices.Clear();
    }

    // Draw Eulerpath with using DFS
    public void DrawEulerPath(List<int> eulerPath)
    {
        // create vector3 list for euler path
        List<Vector3> pathPoints = new List<Vector3>();

        foreach (int vertex in eulerPath)
        {
            Vector2 position = Vertices[vertex];
            pathPoints.Add(new Vector3(position.x, position.y, 0f));
        }

        // Set LineRenderer positions
        BGLineRenderer.positionCount = pathPoints.Count;
        BGLineRenderer.SetPositions(pathPoints.ToArray());
    }

    // Visit vertex
    private void VisitVertex(Dot vertex)
    {
        if (startVertex != null)
        {
            bool isVisited = isVisitedEdge(startVertex.GetIndex(), vertex.GetIndex());
            if (isVisited) return;
            UpdateVisitedEdges(startVertex.GetIndex(), vertex.GetIndex());
        }
        startVertex = vertex;
        int vertexIndex = vertex.GetIndex();
        Debug.Log("vertexIndex" + vertexIndex);
        List<int> _availableEdgeVertexList = new List<int>();
        // Find all edges and add list
        int tempCount = allPossibleEdgeList.Count;
        for (int i = 0; i < tempCount; i++)
        {
            Edge edge = allPossibleEdgeList[i];
            if (edge.FromVertex == vertexIndex)
            {
                _availableEdgeVertexList.Add(edge.ToVertex);
            }
        }
        availableEdgeIndexList = _availableEdgeVertexList;
        CreateLine(vertex);
    }
    public bool IsAllEdgesVisited()
    {
        bool isAllEdgesVisited = false;
        for (int i = 0; i < visitedEdge.Length; i++)
        {
            if (!visitedEdge[i])
            {
                isAllEdgesVisited = false;
                break;
            }
            else isAllEdgesVisited = true;
        }
        return isAllEdgesVisited;
    }
    private void CreateLine(Dot vertex)
    {
        Vector2 tempPos = vertex.GetPos();
        FixPreviousLinePos(tempPos);
        GameObject currGameplayLineRendererObject = ObjectPooler.Generate("lineRenderer");
        currGameplayLineRenderer = currGameplayLineRendererObject.GetComponent<LineRenderer>();
        currGameplayLineRenderer.positionCount = LINE_RENDERER_POS_COUNT;
        //Set pos
        currGameplayLineRenderer.SetPosition(LINE_RENDERER_POS_COUNT - 2, tempPos);
        currGameplayLineRenderer.SetPosition(LINE_RENDERER_POS_COUNT - 1, tempPos);
    }
    public bool isAvailableEdge(int _index)
    {
        return availableEdgeIndexList.Contains(_index);
    }
    public bool isCurrentLineNull()
    {
        if (currGameplayLineRenderer == null) return true;
        else return false;
    }
    private void UpdateVisitedEdges(int from, int to)
    {
        for (int i = 0; i < allPossibleEdgeList.Count; i++)
        {
            if (!visitedEdge[i])
            {
                Edge edge = allPossibleEdgeList[i];
                if (edge.FromVertex == from && edge.ToVertex == to || edge.FromVertex == to && edge.ToVertex == from)
                {
                    visitedEdge[i] = true;
                    Debug.Log(i + " " + visitedEdge[i]);
                }
            }
        }
    }
    private bool isVisitedEdge(int from, int to)
    {
        bool isVisited = false;
        for (int i = 0; i < allPossibleEdgeList.Count; i++)
        {
            Edge edge = allPossibleEdgeList[i];
            if (edge.FromVertex == from && edge.ToVertex == to || edge.FromVertex == to && edge.ToVertex == from)
            {
                if (visitedEdge[i])
                {
                    isVisited = true;
                }
                else isVisited = false;
            }
        }
        return isVisited;
    }
    // UpdateLine
    private void FixPreviousLinePos(Vector2 pos)
    {
        if (currGameplayLineRenderer != null)
        {
            Vector2 tempPos = pos;
            currGameplayLineRenderer.SetPosition(LINE_RENDERER_POS_COUNT - 1, tempPos);
            currGameplayLineRenderer = null;
        }
        else return;
    }
    private void UpdateLine(Vector2 mousePos)
    {
        Vector2 tempPos = mousePos;
        if (currGameplayLineRenderer == null) return;
        currGameplayLineRenderer.SetPosition(LINE_RENDERER_POS_COUNT - 1, tempPos);
    }
}
public class Graph
{
    private int V; // edge count
    private List<int>[] adj; //adjacency list for edges

    public Graph(int V)
    {
        this.V = V;
        adj = new List<int>[V];
        for (int i = 0; i < V; ++i)
            adj[i] = new List<int>();
    }

    public void AddEdge(int v, int w)
    {
        adj[v].Add(w); // add egdes between v and w edges
        adj[w].Add(v);
    }

    public List<int> FindEulerPath(int start)
    {
        List<int> path = new List<int>();
        Dictionary<Tuple<int, int>, bool> visitedEdges = new Dictionary<Tuple<int, int>, bool>(); // visited edge holder Dictionary
        DFS(start, visitedEdges, path); // Depth-first search
        return path;
    }

    private void DFS(int v, Dictionary<Tuple<int, int>, bool> visitedEdges, List<int> path)
    {
        foreach (int w in adj[v])
        {
            Tuple<int, int> edge = new Tuple<int, int>(Math.Min(v, w), Math.Max(v, w)); // Sort edges from the smaller vertex number to the larger vertex number
            if (!visitedEdges.ContainsKey(edge))
            {
                visitedEdges[edge] = true;
                DFS(w, visitedEdges, path); // visit adjencey
            }
        }
        path.Add(v); // Add path current edge
    }
}
[Serializable]
public class Edge
{
    public int FromVertex;
    public int ToVertex;
    public Edge(int fromVertex, int toVertex)
    {
        FromVertex = fromVertex;
        ToVertex = toVertex;
    }
}