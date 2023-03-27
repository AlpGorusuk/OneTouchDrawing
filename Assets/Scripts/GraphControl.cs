using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GraphControl : MonoBehaviour
{
    public int startIndexForCheckEdgesVisited = 0;
    public List<Dot> dotList;
    public List<Edge> edgeList;
    Dictionary<int, Vector2> Vertices = new Dictionary<int, Vector2>();
    List<int> eulerPath = new List<int>();
    public LineRenderer BGLineRenderer;
    //OnGameplay variables
    public LineRenderer currGameplayLineRenderer;
    private const int LINE_RENDERER_POS_COUNT = 2;
    private bool[] visitedVertices;
    private List<int> visitedEdgeIndices;
    //Gameplay Callbacks
    public Action<Dot> DotClickCallback;
    public Action<Vector2> UpdateSelectLineRendererCallback;
    public Action ResetCurrentLineRendererCallback;
    private Vector2? startVertex, endVertex;
    private void OnEnable()
    {
        DotClickCallback += VisitVertex;
        UpdateSelectLineRendererCallback += UpdateGamePlayLineRenderer;
        // ResetCurrentLineRendererCallback += 
    }
    private void OnDisable()
    {
        DotClickCallback -= VisitVertex;
        UpdateSelectLineRendererCallback -= UpdateGamePlayLineRenderer;
    }
    private void Awake()
    {
        BGLineRenderer = GetComponent<LineRenderer>();
        visitedVertices = new bool[dotList.Count];
        visitedEdgeIndices = new List<int>();
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

    // Depth-first search algoritması ile belirlenen Euler yolu ya da döngüsü için linerenderer ile çizim.
    public void DrawEulerPath(List<int> eulerPath)
    {
        // Euler yolu boyunca geçilecek noktaları belirlemek için yeni bir Vector3 listesi oluşturuyoruz.
        List<Vector3> pathPoints = new List<Vector3>();

        // Euler yolu boyunca gezinerek, pathPoints listesine noktalar ekliyoruz.
        foreach (int vertex in eulerPath)
        {
            Vector2 position = Vertices[vertex];
            pathPoints.Add(new Vector3(position.x, position.y, 0f));
        }

        // LineRenderer bileşeninin pozisyonlarını belirliyoruz.
        BGLineRenderer.positionCount = pathPoints.Count;
        BGLineRenderer.SetPositions(pathPoints.ToArray());
    }
    // Onplay Control Graph
    // Köşe ziyareti gerçekleştirir
    private void SetStartVertex(Dot vertex)
    {
        // Vector2 temp = vertex.GetPos();
        // startVertex = temp;
        // SetLineInitPos(startVertex.Value);
    }
    private void VisitVertices(Vector2 mousePosition)
    {
        // if (startVertex != null)
        // {
        //     Vector2? clickedVertex = GetClosestVertex(mousePosition, 0.2f);
        //     Debug.Log(clickedVertex);
        //     if (clickedVertex == null) { DrawLine(mousePosition); }
        //     if (clickedVertex != null && clickedVertex != gameplayLineRenderer.GetPosition(gameplayLineRenderer.positionCount - 2))
        //     {
        //         gameplayLineRenderer.positionCount++;
        //         DrawLine(mousePosition);
        //         clickedVertex = null;
        //     }
        // }
    }
    private Vector2? GetClosestVertex(Vector2 point, float maxDistance)
    {
        foreach (Dot _dot in dotList)
        {
            Vector2 tempPos = _dot.GetPos();
            if (Vector2.Distance(point, tempPos) <= maxDistance)
            {
                return tempPos;
            }
        }

        return null;
    }
    // private void SetLineInitPos(Vector2 toVertex)
    // {
    //     gameplayLineRenderer.SetPosition(0, toVertex);
    // }
    // private void DrawLine(Vector2 toVertex)
    // {
    //     gameplayLineRenderer.SetPosition(gameplayLineRenderer.positionCount - 1, toVertex);
    // }


    private void Update()
    {
        // Sol tıklama ile seçilen köşe index'i alınır
        // if (Input.GetMouseButtonDown(0))
        // {
        //     Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //     RaycastHit2D raycastHit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity);
        //     if (raycastHit.collider != null)
        //     {
        //         Dot selectedDot = raycastHit.collider.GetComponent<Dot>();

        //         if (selectedDot != null)
        //         {
        //             int vertexIndex = selectedDot.GetIndex();
        //             Action _dotCallback = selectedDot.ClickedCallback;
        //             _dotCallback?.Invoke();
        //             if (vertexIndex >= 0)
        //             {
        //                 VisitVertex(vertexIndex);
        //             }
        //         }
        //     }
        // }

        // Çizgiyi güncelle
        // UpdateLineRenderer();
    }

    // Köşe ziyareti gerçekleştirir
    private void VisitVertex(Dot vertex)
    {
        int vertexIndex = vertex.GetIndex();
        Vector2 tempPos = vertex.GetPos();
        visitedVertices[vertexIndex] = true;
        List<int> availableEdgeIndexList = new List<int>();
        // Ziyaret edilen tüm kenarları bul ve listeye ekle
        for (int i = 0; i < edgeList.Count; i++)
        {
            Edge edge = edgeList[i];
            if (edge.FromVertex == vertexIndex && !visitedEdgeIndices.Contains(i))
            {
                availableEdgeIndexList.Add(i);
            }
            else if (edge.ToVertex == vertexIndex && !visitedEdgeIndices.Contains(i))
            {
                availableEdgeIndexList.Add(i);
            }
        }
        CreateGameplayLineRenderer(tempPos);
        Debug.Log(availableEdgeIndexList.Count);
    }
    private void CreateGameplayLineRenderer(Vector2 vertexPos)
    {
        GameObject currGameplayLineRendererObject = ObjectPooler.Generate("lineRenderer");
        currGameplayLineRenderer = currGameplayLineRendererObject.GetComponent<LineRenderer>();
        currGameplayLineRenderer.positionCount = LINE_RENDERER_POS_COUNT;
        //Set pos
        currGameplayLineRenderer.SetPosition(LINE_RENDERER_POS_COUNT - 2, vertexPos);
        currGameplayLineRenderer.SetPosition(LINE_RENDERER_POS_COUNT - 1, vertexPos);
    }
    // Çizgiyi güncelle
    private void UpdateGamePlayLineRenderer(Vector2 mousePos)
    {
        if (currGameplayLineRenderer != null)
        {
            currGameplayLineRenderer.SetPosition(LINE_RENDERER_POS_COUNT - 1, mousePos);
        }
        // int vertexIndex = vertex.GetIndex();
        // if (!visitedEdgeIndices.Contains(vertexIndex))
        // {

        // }
        // gameplayLineRenderer.positionCount = visitedEdgeIndices.Count * 2;
        // for (int i = 0; i < visitedEdgeIndices.Count; i++)
        // {
        //     Edge edge = edgeList[visitedEdgeIndices[i]];
        //     gameplayLineRenderer.SetPosition(i * 2, dotList[edge.FromVertex].transform.position);
        //     gameplayLineRenderer.SetPosition(i * 2 + 1, dotList[edge.ToVertex].transform.position);
        // }
    }
}
public class Graph
{
    private int V; // Grafın köşe sayısı
    private List<int>[] adj; // Köşelerin birbirine olan komşulukları için kullanılacak adjacency list

    public Graph(int V)
    {
        this.V = V;
        adj = new List<int>[V];
        for (int i = 0; i < V; ++i)
            adj[i] = new List<int>();
    }

    public void AddEdge(int v, int w)
    {
        adj[v].Add(w); // v köşesi ile w köşesi arasında bir kenar ekleniyor
        adj[w].Add(v); // w köşesi ile v köşesi arasında bir kenar ekleniyor
    }

    public List<int> FindEulerPath(int start)
    {
        List<int> path = new List<int>(); // Oluşturulan yolun tutulacağı liste
        Dictionary<Tuple<int, int>, bool> visitedEdges = new Dictionary<Tuple<int, int>, bool>(); // Ziyaret edilen kenarların tutulacağı dictionary
        DFS(start, visitedEdges, path); // Depth-first search algoritmasının kullanılacağı yardımcı fonksiyon
        return path;
    }

    private void DFS(int v, Dictionary<Tuple<int, int>, bool> visitedEdges, List<int> path)
    {
        foreach (int w in adj[v])
        {
            Tuple<int, int> edge = new Tuple<int, int>(Math.Min(v, w), Math.Max(v, w)); // Kenarı küçük köşe numarasından büyük köşe numarasına doğru sırala
            if (!visitedEdges.ContainsKey(edge)) // Kenar daha önce ziyaret edilmediyse
            {
                visitedEdges[edge] = true; // Kenar işaretleniyor
                DFS(w, visitedEdges, path); // Komşu köşe ziyaret ediliyor
            }
        }
        path.Add(v); // Ziyaret edilen köşe yola ekleniyor
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