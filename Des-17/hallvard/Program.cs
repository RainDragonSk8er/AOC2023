// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;

class Program
{
    public const int dimensions = 141;
    public static int[,] cityblocks = new int[dimensions, dimensions];
    public static int firststep = 4;    // Input 1 for Part one and 4 for Part two
    public static int maxdirstep = 10;  // Input 3 for Part one and 10 for Part two

    static void Main()
    {
        Stopwatch sw = Stopwatch.StartNew();
        Console.WriteLine("Hello World on December 17th 2023!");

        string inputPath = @"..\..\..\AOC2023-17-Input.txt";
        using (StreamReader inputFile = new StreamReader(inputPath))
        {
            // Read input and build contraption map
            string line;
            for (int y = 0; y < dimensions; y++)
            {
                line = inputFile.ReadLine();
                for (int x = 0; x < dimensions; x++)
                {
                    cityblocks[x, y] = (int)(line[x] - '0');
                }
            }
            inputFile.Close();
        }        

        // Part 1 and 2
        Dijkstra dijkstra = new Dijkstra(cityblocks);
        Node start = new Node(0, 0, 0, 0, 0, 0, "");
        Node end = new Node(cityblocks.GetLength(0) - 1, cityblocks.GetLength(0) - 1);

        int answer = dijkstra.ShortestPath(start, end);

        Console.WriteLine("The answer is: {0}", answer);

        sw.Stop();
        Console.WriteLine("Time elapsed: {0}\n", sw.Elapsed);

        Console.WriteLine("Hit any key to exit!");
        Console.ReadKey();
      
    }
}

public class Dijkstra
{
    private int[,] _graph;
    private int _x;
    private int _y;

    private static int[] xdir = new int[4] { 1, 0, -1, 0 };
    private static int[] ydir = new int[4] { 0, 1, 0, -1 };
    private static char[] dirchar = new char[4] { '>', 'v', '<', '^' };

    public Dijkstra(int[,] graph)
    {
        _graph = graph;
        _x = _graph.GetLength(0);
        _y = _graph.GetLength(1);

        for (int y = 0; y < _y; y++)
        {
            for (int x = 0; x < _x; x++)
            {
                Console.Write(_graph[x, y]);
            }
            Console.WriteLine();
        }
        Console.WriteLine("Hit any key to exit!");
        Console.ReadKey();
    }

    public int ShortestPath(Node start, Node end)
    {
        HashSet<Node> visited = new HashSet<Node>();
        List<Node> queue = new List<Node>();

        start.Cost = 0;
        queue.Add(start);

        while (queue.Count > 0)
        {
            queue.Sort();
            Node current = queue[0];
            queue.RemoveAt(0);

            if (current.X == end.X && current.Y == end.Y)
            {
                Console.WriteLine("Reached destination at {0}, {1} from direction {2}, {3}, with steps streak of {4} and accumulated cost {5} via path:\n{6}",
                    current.X, current.Y, current.Dx, current.Dy, current.Steps, current.Cost, current.StepHistory);
                return current.Cost;
            }

            //Console.WriteLine("Position {0}, {1} pulled from the queue with direction {2}, {3}, steps {4} and accumulated cost {5}. Visited count is {6}.",
            //    current.X, current.Y, current.Dx, current.Dy, current.Steps, current.Cost, visited.Count());

            foreach (Node neighbor in GetNeighbors(current))
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    queue.Add(neighbor);
                }                    
            }
        }
        return -1;
    }

    private List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        int xf = (node.Dx == 0) ? Program.firststep : 1;
        int yf = (node.Dy == 0) ? Program.firststep : 1;

        for (int i = 0; i < 4; i++)
        {
            if (node.X + xdir[i] * xf >= 0 && node.Y + ydir[i] * yf >= 0 && node.X + xdir[i] * xf < _x && node.Y + ydir[i] * yf < _y // Out of bounds
                && !(xdir[i] != 0 && xdir[i] == -node.Dx) && !(ydir[i] != 0 && ydir[i] == -node.Dy) // Backwards violation
                && !(xdir[i] != 0 && xdir[i] == node.Dx && node.Steps >= Program.maxdirstep) && !(ydir[i] != 0 && ydir[i] == node.Dy && node.Steps >= Program.maxdirstep)) // Reach max steps
            {
                int newcost = 0, j;
                for (j = 1; j <= Math.Abs(xdir[i] * xf + ydir[i] * yf); j++)
                {
                    newcost += _graph[node.X + xdir[i] * j, node.Y + ydir[i] * j];
                }
                neighbors.Add(new Node(node.X + xdir[i] * xf, node.Y + ydir[i] * yf, xdir[i], ydir[i],
                                        ((xdir[i] != 0 && xdir[i] == node.Dx) || (ydir[i] != 0 && ydir[i] == node.Dy)) ? node.Steps + 1 : Program.firststep,
                                        node.Cost + newcost, node.StepHistory + new String(dirchar[i], Math.Abs(xdir[i] * xf + ydir[i] * yf))));
            }
        }
        return neighbors;
    }
}

public class Node : IComparable<Node>
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Dx { get; set; }
    public int Dy { get; set; }
    public int Steps { get; set; }
    public int Cost { get; set; }
    public string StepHistory { get; set; }
    

    public Node(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Node(int x, int y, int dx, int dy, int steps, int cost, string history)
    {
        X = x;
        Y = y;
        Dx = dx;
        Dy = dy;
        Steps = steps;
        Cost = cost;
        StepHistory = history;
    }

    public Node(Node node)
    {
        X = node.X;
        Y = node.Y;
        Dx = node.Dx;
        Dy = node.Dy;
        Steps = node.Steps;
        Cost = node.Cost;
    }

    public int CompareTo(Node node)
    {
        return this.Cost - node.Cost;
    }

    public override bool Equals(object obj)
    {
        if (obj is Node n)
        {
            return X == n.X && Y == n.Y && Dx == n.Dx && Dy == n.Dy && Steps == n.Steps;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Dx, Dy, Steps);
    }
}