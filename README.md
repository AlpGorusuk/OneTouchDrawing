# One Touch Drawing Game Clone

 **One Touch Drawing** is a simple yet engaging puzzle game where players are challenged to complete a drawing using only a single continuous line. The objective of the game is to connect all the designated points on the screen by drawing a line without lifting the finger.

The game typically features a grid or a series of dots arranged in a specific pattern or shape. Each dot represents a point that needs to be connected. Players start by touching any point on the screen and then dragging their finger or stylus across the grid, following a path that connects the dots.

However, there are certain rules that players must adhere to while drawing the line. Firstly, the line must not intersect itself or any other lines already drawn. Secondly, the line cannot go outside the boundaries of the grid or touch any obstacles or barriers placed within the puzzle.

 ## [One Touch Drawing Gameplay](https://www.youtube.com/watch?v=I8WBNI5eEDk)

### I create **Graph** class for creating shape with using search algorithm DFS
```
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
```
### For the drawing using only a single continuous line I use EulerPath Algorithm

```
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

```
