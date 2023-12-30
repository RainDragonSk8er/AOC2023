// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Security.Cryptography;

class Program
{
    public const int dimensions = 141;
    public static int[,] hikemap = new int[dimensions, dimensions]; // '.' = Path = 0, '>', 'v' and '<', '^¨ = Slopes = 1, 2, 3, 4, '#' = Forest = -1
    public static string mapledgend = "#.>v<^"; // Index minus 1

    public static bool PartTwo = true;

    static void Main()
    {
        Stopwatch sw = Stopwatch.StartNew();
        Console.WriteLine("Hello World on December 23th 2023!");

        string inputPath = @"..\..\..\AOC2023-23-Input.txt";
        using (StreamReader inputFile = new StreamReader(inputPath))
        {
            // Read input and build contraption map
            string line;
            for (int y = 0; y < dimensions; y++)
            {
                line = inputFile.ReadLine();
                for (int x = 0; x < dimensions; x++)
                {
                    hikemap[x, y] = mapledgend.IndexOf(line[x]) - 1;
                }
            }
            inputFile.Close();
        }        

        // Part 1 and 2
        LP_graph lpg = new LP_graph(hikemap);
        Node start = new Node(1, 0);
        Node end = new Node(hikemap.GetLength(0) - 2, hikemap.GetLength(0) - 1);

        lpg.FindJunctionNodes(start, end);
        lpg.FindPathsBetweenNodes(start, end);
        //lpg.LongestPath(0); // Fails with bi-directional paths and there fore not suitable for part two
        lpg.DFSLongestPath(0);

        // Console.WriteLine("The answer is: {0}", answer);

        sw.Stop();
        Console.WriteLine("Time elapsed: {0}\n", sw.Elapsed);

        Console.WriteLine("Hit any key to exit!");
        Console.ReadKey();
      
    }
}

public class LP_graph
{
    private int[,] _graph;
    private int _x;
    private int _y;
    private int nodecount;

    private int lpmaxcount;

    private bool[,] visited;
    private Dictionary<Node, int> nodeIDs = new Dictionary<Node, int>();
    private List<AdjListNode>[] adjnodes;

    private static int[] xdir = new int[4] { 1, 0, -1, 0 };
    private static int[] ydir = new int[4] { 0, 1, 0, -1 };
    private static char[] dirchar = new char[4] { '>', 'v', '<', '^' };

    public LP_graph(int[,] graph)
    {
        _graph = graph;
        _x = _graph.GetLength(0);
        _y = _graph.GetLength(1);

        visited = new bool[_x, _y];
    }

    public void FindJunctionNodes(Node start, Node end)
    {
        int nodeID = 0;
        nodeIDs.Add(start, nodeID++);
        for (int x = 1; x < _x - 1; x++)
        {
            for (int y = 1; y < _y - 1; y++)
            {
                if (_graph[x, y] >= 0) // Not in a forest
                {
                    int pathcount = 0;
                    for (int i = 0; i < 4; i++) // Look around
                    {
                        if (_graph[x + xdir[i], y + ydir[i]] >= 0) // Count not forest
                            pathcount++;
                    }
                    if (pathcount > 2) // x, y is a junction
                    {
                        Console.WriteLine($"Adding junction at {x}, {y} with nodeID {nodeID} to dictionary nodeIDs.");
                        nodeIDs.Add(new Node(x, y), nodeID++);                        
                    }
                }
            }
        }
        nodeIDs.Add(end, nodeID);
        nodecount = nodeIDs.Count;
    }

    public void FindPathsBetweenNodes(Node start, Node end)
    {
        adjnodes = new List<AdjListNode>[nodeIDs.Count()];
        for (int i = 0; i < nodeIDs.Count(); i++) {
            adjnodes[i] = new List<AdjListNode>();
        }

        List<Node> queue = new List<Node>();
        List<Node> queuenext = new List<Node>();

        queue.Add(new Node(start)); // Add start ...

        while (queue.Count > 0)
        {
            Node pathstart = new Node(queue[0]);
            Node current;

            if (queuenext.Count > 0)
            {
                current = queuenext[0];
                queuenext.RemoveAt(0);
            }
            else
                current = queue[0];

            queue.RemoveAt(0);

            do 
            {
                visited[current.X, current.Y] = true;

                List<Node> neighbors = GetNeighbors(current);

                if (nodeIDs.ContainsKey(current) && !pathstart.Equals(current)) // Reached a new juction
                {
                    adjnodes[nodeIDs[pathstart]].Add(new AdjListNode(nodeIDs[current], current.Steps));
                    Console.WriteLine($"Added path between nodeID {nodeIDs[pathstart]} and {nodeIDs[current]} with stepcount {current.Steps} steps.");

                    if (!current.Equals(end))
                    {
                        if (Program.PartTwo && !current.Equals(start)) // Add reverse path (for part two)
                        {
                            adjnodes[nodeIDs[current]].Add(new AdjListNode(nodeIDs[pathstart], current.Steps));
                        }

                        foreach (Node neighbor in neighbors)
                        {
                            queue.Add(current);
                            neighbor.Steps -= current.Steps;
                            queuenext.Add(neighbor);
                        }
                        break;
                    }
                    else
                    {
                        Console.WriteLine($"Reached destination at {current.X}, {current.Y} from {pathstart.X}, {pathstart.Y} in {current.Steps} steps.");
                        break;
                    }                        
                }
                else if (neighbors.Count >= 1)
                {
                    if (neighbors[0].Equals(start))
                    {
                        current.X = neighbors[1].X;
                        current.Y = neighbors[1].Y;
                        current.Steps = neighbors[1].Steps;
                    }
                    else
                    {
                        current.X = neighbors[0].X;
                        current.Y = neighbors[0].Y;
                        current.Steps = neighbors[0].Steps;
                    }                        
                }
                else
                    break;
            } while (true);
        }
    }

    private void TopologicalSortUtil(int nodeID, bool[] visited, Stack<int> stack)
    {
        visited[nodeID] = true;

        for (int i = 0; i < adjnodes[nodeID].Count; i++)
        {
            AdjListNode node = adjnodes[nodeID][i];
            if (!visited[node.getID()])
                TopologicalSortUtil(node.getID(), visited, stack);
        }
        stack.Push(nodeID);
    }

    public void LongestPath(int s)
    {
        Stack<int> stack = new Stack<int>();
        int[] dist = new int[nodecount];

        bool[] visitednodes = new bool[nodecount];
        for (int i = 0; i < nodecount; i++)
            visitednodes[i] = false;

        for (int i = 0; i < nodecount; i++)
        {
            if (visitednodes[i] == false)
                TopologicalSortUtil(i, visitednodes, stack);
        }

        for (int i = 0; i < nodecount; i++)
            dist[i] = Int32.MinValue;

        dist[s] = 0;

        while (stack.Count > 0)
        {
            int u = stack.Pop();

            if (dist[u] != Int32.MinValue)
            {
                for (int i = 0; i < adjnodes[u].Count; i++)
                {
                    AdjListNode node = adjnodes[u][i];
                    if (dist[node.getID()] < dist[u] + node.getSteps())
                        dist[node.getID()] = dist[u] + node.getSteps();
                }
            }
        }
        for (int i = 0; i < nodecount; i++)
        {
            if (dist[i] == int.MinValue)
                Console.Write("INF ");
            else
                Console.Write($"{dist[i]} ");
        }
        Console.WriteLine();
    }

    public void DFSLongestPath(int s)
    {
        lpmaxcount = 0;

        bool[] visitednodes = new bool[nodecount];
        for (int i = 0; i < nodecount; i++)
            visitednodes[i] = false;

        int lpsc = 0;
        DFSLongestPathUtil(s, visitednodes, lpsc);

        Console.WriteLine($"Longest path step count is {lpmaxcount}");
    }

    public void DFSLongestPathUtil(int n, bool[] visitednodes, int lpsc)
    {
        visitednodes[n] = true;

        // Console.WriteLine($"@Node {n} with path step count of {lpsc}");

        for (int i = 0; i < adjnodes[n].Count; i++)
        {
            AdjListNode aln = adjnodes[n][i];

            if (!visitednodes[aln.getID()])
            {
                lpsc += aln.getSteps();
                if (aln.getID() == nodecount - 1) // Found the end-node
                {
                    if (lpsc > lpmaxcount)
                    {
                        lpmaxcount = lpsc;
                        Console.WriteLine($"New longest path with step count {lpmaxcount}");
                    }

                    lpsc -= aln.getSteps();
                    break;
                }
                else
                {
                    DFSLongestPathUtil(aln.getID(), visitednodes, lpsc);
                    lpsc -= aln.getSteps();
                }
            }
        }
        visitednodes[n] = false;
    }

    private List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int i = 0; i < 4; i++)
        {
            if (nodeIDs.ContainsKey(new Node(node.X + xdir[i], node.Y + ydir[i])) // A junction or
                || (node.X + xdir[i] >= 0 && node.Y + ydir[i] >= 0 && node.X + xdir[i] < _x && node.Y + ydir[i] < _y // (Inside bounds
                    && Program.hikemap[node.X + xdir[i], node.Y + ydir[i]] >= 0  // and not a forest 
                    && !visited[node.X + xdir[i], node.Y + ydir[i]])) // and not visited before)
            {
                int type = Program.hikemap[node.X + xdir[i], node.Y + ydir[i]];

                if (type > 0) // Hit a slope
                {
                    if (xdir[i] == -xdir[type - 1] && ydir[i] == -ydir[type - 1]) // Uphill
                    {
                        // Console.WriteLine($"Stopped by uphill at {node.X + xdir[i]}, {node.Y + ydir[i]} direction {xdir[i]},{ydir[i]}. Stepcount {node.Steps}.");
                    }
                    else if (Program.hikemap[node.X + xdir[i] + xdir[type - 1], node.Y + ydir[i] + ydir[type - 1]] == 0) // path after slope = ok
                    {
                        neighbors.Add(new Node(node.X + xdir[i] + xdir[type - 1], node.Y + ydir[i] + ydir[type - 1], node.Steps + 2));
                    }
                    else
                    {
                        Console.WriteLine($"Found a slope without a following path at {node.X + xdir[i]}, {node.Y + ydir[i]}.");
                    }
                }
                else
                {
                    neighbors.Add(new Node(node.X + xdir[i], node.Y + ydir[i], node.Steps + 1));
                }
            }
        }
        return neighbors;
    }
}

public class Node
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Steps { get; set; }

    public Node(int x, int y)
    {
        X = x;
        Y = y;
        Steps = 0;
    }

    public Node(int x, int y, int steps)
    {
        X = x;
        Y = y;
        Steps = steps;
    }

    public Node(Node node)
    {
        X = node.X;
        Y = node.Y;
        Steps = node.Steps;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || !(obj is Node))
            return false;

        Node node = (Node)obj;
        return this.X == node.X && this.Y == node.Y;
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 + X.GetHashCode();
        hash = hash * 23 + Y.GetHashCode();
        return hash;
    }
}

public class AdjListNode
{
    private int ID;
    private int Steps;

    public AdjListNode(int id, int steps)
    {
        ID = id;
        Steps = steps;
    }

    public int getID() { return ID; }
    public int getSteps() { return Steps; }
}