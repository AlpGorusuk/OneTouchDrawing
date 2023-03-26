using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGraph : MonoBehaviour
{
    public int startIndexForCheckEdgesVisited = 0;
    public List<Dot> DotList;
    public List<Edge> EdgeList;
    Dictionary<int, Vector2> Vertices = new Dictionary<int, Vector2>();
    List<int> eulerPath = new List<int>();
    [ContextMenu("Reset Graph")]
    public void ResetGraph()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
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
        Graph g = new Graph(EdgeList.Count);
        for (int i = 0; i < EdgeList.Count; i++)
        {
            int fv = EdgeList[i].FromVertex;
            int tv = EdgeList[i].ToVertex;
            g.AddEdge(fv, tv);
        }
        eulerPath = g.FindEulerPath(startIndexForCheckEdgesVisited);
        eulerPath.Reverse();
    }

    private void SetVertices()
    {
        for (int i = 0; i < DotList.Count; i++)
        {
            int key = i;
            Vector2 value = DotList[key].GetPos();
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
        // LineRenderer bileşenini alıyoruz.
        LineRenderer lineRenderer = GetComponent<LineRenderer>();

        // Euler yolu boyunca geçilecek noktaları belirlemek için yeni bir Vector3 listesi oluşturuyoruz.
        List<Vector3> pathPoints = new List<Vector3>();

        // Euler yolu boyunca gezinerek, pathPoints listesine noktalar ekliyoruz.
        foreach (int vertex in eulerPath)
        {
            Vector2 position = Vertices[vertex];
            pathPoints.Add(new Vector3(position.x, position.y, 0f));
        }

        // LineRenderer bileşeninin pozisyonlarını belirliyoruz.
        lineRenderer.positionCount = pathPoints.Count;
        lineRenderer.SetPositions(pathPoints.ToArray());
    }
    private bool CheckAllEdgesVisited(List<Edge> path, int startVertex)
    {
        // Başlangıç düğümünden başlanarak, gezilen düğümlerin tutulduğu bir HashSet oluştur
        HashSet<int> visitedVertices = new HashSet<int>();
        visitedVertices.Add(startVertex);

        // Tüm kenarlar gezilene kadar döngüye gir
        for (int i = 0; i < path.Count; i++)
        {
            Edge edge = path[i];

            // Geçilen kenarların başlangıç ve bitiş düğümleri kontrol edilir
            if (!visitedVertices.Contains(edge.FromVertex) || !visitedVertices.Contains(edge.ToVertex))
            {
                return false;
            }

            // Kenarın bitiş düğümü, sonraki başlangıç düğümü olarak belirlenir
            int nextVertex = edge.ToVertex;
            if (visitedVertices.Contains(nextVertex))
            {
                nextVertex = edge.FromVertex;
            }

            visitedVertices.Add(nextVertex);
        }

        // Tüm kenarlar gezildiyse, true döndür
        return true;
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
        adj[w].Add(v); // w köşesi ile v köşesi arasında bir kenar ekleniyor (undirected graph)
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