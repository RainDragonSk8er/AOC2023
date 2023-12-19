// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;

class Program
{
    public const int dimensions = 13;
    public static int[,] cityblocks = new int[dimensions, dimensions];

    static void Main()
    {
        Stopwatch sw = Stopwatch.StartNew();
        Console.WriteLine("Hello World on December 17th 2023!");

        string inputPath = @"..\..\..\AOC2023-17-TestInput.txt";
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

            // Part 1
            Dijkstra dijkstra = new Dijkstra(cityblocks);
            Node start = new Node(0, 0, 0, 0, 0, 0);
            Node end = new Node(cityblocks.GetLength(0), cityblocks.GetLength(0));

            int answer = dijkstra.ShortestPath(start, end);

            Console.WriteLine("The answer to part one is: {0}", answer);

            // Part 2
            // Console.WriteLine("The answer to part two is: {0}", answer2);
            sw.Stop();
            Console.WriteLine("Time elapsed: {0}\n", sw.Elapsed);

            Console.WriteLine("Hit any key to exit!");
            Console.ReadKey();
        }
    }
}

public class Dijkstra
{
    private int[,] _graph;
    private int _x;
    private int _y;

    private static int[] xdir = new int[4] { 1, 0, -1, 0 };
    private static int[] ydir = new int[4] { 0, 1, 0, -1 };

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
                return current.Cost;

            Console.WriteLine("Position {0}, {1} pulled from the queue with direction {2}, {3}, steps {4} and accumulated cost {5}. Visited count is {6}.",
                current.X, current.Y, current.Dx, current.Dy, current.Steps, current.Cost, visited.Count());

            foreach (Node neighbor in GetNeighbors(current))
            {
                Node oldNeighbor;
                if (visited.TryGetValue(neighbor, out oldNeighbor))
                {
                    if (neighbor.Cost >= oldNeighbor.Cost && neighbor.Steps >= oldNeighbor.Steps)
                    {
                        Console.WriteLine("Neighbor at {0}, {1} has been visited before in direction {2}, {3} with both steps {4} ({5}) and cost {6} ({7}) lower or equal.",
                                            neighbor.X, neighbor.Y, neighbor.Dx, neighbor.Dy, oldNeighbor.Steps, neighbor.Steps, oldNeighbor.Cost, neighbor.Cost);
                        continue;
                    }
                    else
                    {
                        visited.Remove(neighbor);
                    }
                }

                queue.Add(neighbor);
                visited.Add(new Node(neighbor));

                Console.WriteLine("Route to {0}, {1} with direction {2}, {3} steps {4} and accumulated cost {5} added to queue (Size is {6}).",
                                        neighbor.X, neighbor.Y, neighbor.Dx, neighbor.Dy, neighbor.Steps, neighbor.Cost, queue.Count);
            }
        }
        return -1;
    }

    private List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (int i = 0; i < 4; i++)
        {
            if (node.X + xdir[i] >= 0 && node.Y + ydir[i] >= 0 && node.X + xdir[i] < _x && node.Y + ydir[i] < _y // Out of bounds
                && !(xdir[i] != 0 && xdir[i] == -node.Dx) && !(ydir[i] != 0 && ydir[i] == -node.Dy) // Backwards violation
                && !(xdir[i] != 0 && xdir[i] == node.Dx && node.Steps >= 3) && !(ydir[i] != 0 && ydir[i] == node.Dy && node.Steps >= 3)) // Reach max three steps
            {
                neighbors.Add(new Node(node.X + xdir[i], node.Y + ydir[i], xdir[i], ydir[i],
                                        ((xdir[i] != 0 && xdir[i] == node.Dx) || (ydir[i] != 0 && ydir[i] == node.Dy)) ? node.Steps + 1 : 1,
                                        node.Cost + _graph[node.X + xdir[i], node.Y + ydir[i]]));
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

    public Node(int x, int y, int dx, int dy, int steps, int cost)
    {
        X = x;
        Y = y;
        Dx = dx;
        Dy = dy;
        Steps = steps;
        Cost = cost;
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
            return X == n.X && Y == n.Y && Dx == n.Dx && Dy == n.Dy && Steps == n.Steps && Cost == n.Cost;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Dx, Dy, Steps, Cost);
    }
}