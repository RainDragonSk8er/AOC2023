// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Text.RegularExpressions;

public class Program
{
    public static Dictionary<string, int> compName2ID = new Dictionary<string, int>();
    public static Dictionary<int, string> compID2Name = new Dictionary<int, string>();
    public static List<List<int>> wiring = new List<List<int>>();

    public static Dictionary<Edge, int> edgeuse = new Dictionary<Edge, int>();

    public static Random rnd = new Random();


    static void Main()
    {
        Stopwatch sw = Stopwatch.StartNew();
        Console.WriteLine("Hello World on December 25th 2023!");

        string inputPath = @"..\..\..\AOC2023-25-Input.txt";
        using (StreamReader inputFile = new StreamReader(inputPath))
        {
            string input;

            // Read input and build contraption map
            while ((input = inputFile.ReadLine()) != null)
            {
                Match match = Regex.Match(input, @"^(?<s>\w+): (?<d>(\w+\s*)+)$");
                if (match.Success)
                {
                    if (!compName2ID.ContainsKey(match.Groups["s"].Value))
                    {
                        wiring.Add(new List<int>());
                        compName2ID.Add(match.Groups["s"].Value, wiring.Count - 1);
                    }
                        
                    string[] values = match.Groups["d"].Value.Split(' ');
                    for (int i=0; i<values.Length; i++)
                    {
                        if (!compName2ID.ContainsKey(values[i]))
                        {
                            wiring.Add(new List<int>());
                            compName2ID.Add(values[i], wiring.Count - 1);
                        }

                        wiring[compName2ID[match.Groups["s"].Value]].Add(compName2ID[values[i]]); // Add bi-directional wiring connections
                        wiring[compName2ID[values[i]]].Add(compName2ID[match.Groups["s"].Value]);
                        Console.WriteLine($"Added wiring between '{match.Groups["s"].Value}' and '{values[i]}'.");
                    }
                }
            }
            inputFile.Close();
        }
        Console.WriteLine("Done reading wire diagram!\n");
        Console.WriteLine($"Found {compName2ID.Count} components.");

        foreach (KeyValuePair<string, int> NameID in compName2ID) // Create reverse lookup from component ID back to name.
            compID2Name.Add(NameID.Value, NameID.Key);

        String[] top3edges = ScanForKeyEdges();

        for (int i = 0; i < top3edges.Length; i++)
        {
            Cut(top3edges[i]);
            for (int j = 0; j < top3edges.Length; j++)
            {
                AlternateConnections(top3edges[j]);
            }            
        }

        string first = top3edges[0].Substring(0, 3);
        int firstcount = WiredComponentCount(first);
        string second = top3edges[0].Substring(4, 3);
        int secondcount = WiredComponentCount(second);
        int answer = firstcount * secondcount;

        Console.WriteLine($"Component group containg '{first}' consists of {firstcount} components.");
        Console.WriteLine($"Component group containg '{second}' consists of {secondcount} components.");
        Console.WriteLine("The answer is: {0}", answer);

        sw.Stop();
        Console.WriteLine("Time elapsed: {0}\n", sw.Elapsed);

        Console.WriteLine("Hit any key to exit!");
        Console.ReadKey();      
    }

    static int WiredComponentCount(string start)
    {
        HashSet<int> visited = new HashSet<int>();
        Stack<int> stack = new Stack<int>();

        stack.Push(compName2ID[start]);

        while (stack.Count > 0)
        {
            int comp = stack.Pop();
            visited.Add(comp);

            foreach  (int next in wiring[comp])
            {
                if (!visited.Contains(next))
                {
                    stack.Push(next);
                }
            }
        } 
        return visited.Count;
    }

    static bool Cut(string cutinput)
    {
        string[] comps = cutinput.Split('/');
        if (comps.Length != 2)
        {
            Console.WriteLine($"Illegal cut input: {cutinput}");
            return false;
        }

        int first = compName2ID[comps[0]];
        int second = compName2ID[comps[1]];

        if (wiring[first].Contains(second) && wiring[second].Contains(first))
        {
            wiring[first].Remove(second);
            wiring[second].Remove(first);
            Console.WriteLine($"wiring between compName2ID '{comps[0]}' and '{comps[1]}' was cut in both directions.");
        }        
        else
        {
            Console.WriteLine($"Coundn't frind bi-directional wiring between compName2ID '{comps[0]}' and '{comps[1]}'");
            return false;
        }
        return true;
    }

    static int AlternateConnections(string acinput)
    {
        string[] comps = acinput.Split('/');
        if (comps.Length != 2)
        {
            Console.WriteLine($"Illegal cut input: {acinput}");
            return -1;
        }

        HashSet<int> visited = new HashSet<int>();
        Stack<int> stack = new Stack<int>();

        int start = compName2ID[comps[0]];
        int end = compName2ID[comps[1]];

        stack.Push(start);

        int account = 0;

        while (stack.Count > 0)
        {
            int comp = stack.Pop();
            visited.Add(comp);

            foreach (int next in wiring[comp])
            {
                if (comp == start && next == end) // Ignore direct connection (simulate cut)
                    continue;

                if (next == end) // Reached destination via alternate route
                {
                    account++;
                }                    

                if (!visited.Contains(next))
                {
                    stack.Push(next);
                }
            }
        }
        Console.WriteLine($"wiring between compName2ID '{comps[0]}' and '{comps[1]}' has {account} alternate connection(s).");
        return account;
    }

    static string[] ScanForKeyEdges()
    {
        HashSet<int> visited = new HashSet<int>();
        string[] top3edges = new string[3];

        for (int i = 1; i < 1000; i++)
        {
            int start = rnd.Next(1, compName2ID.Count) - 1;
            int end = rnd.Next(1, compName2ID.Count) - 1;

            visited.Clear();
            FindFirstPathUtil(start, end, visited);
        }

        var edgeuselist = edgeuse.ToList();
        edgeuselist.Sort((p1, p2) => p2.Value.CompareTo(p1.Value)); // Sort decending

        for (int i = 0; i < 3; i++)
        {
            top3edges[i] = compID2Name[edgeuselist[i].Key.n0] + '/' + compID2Name[edgeuselist[i].Key.n1];
        }
        return top3edges;
    }

    static bool FindFirstPathUtil(int current, int end, HashSet<int> visited)
    {
        visited.Add(current);
        SortedList<int, int> shuffledNext = new SortedList<int, int>();

        foreach (int next in wiring[current])
            shuffledNext.Add(rnd.Next(), next);

        foreach (int next in shuffledNext.Values)
        {
            if (visited.Contains(next))
                continue;

            Edge nextedge = new Edge(current, next);
            if (!edgeuse.ContainsKey(nextedge))
                edgeuse.Add(nextedge, 0);

            edgeuse[nextedge]++; // Add use count

            if (next == end) // Reached destination 
                return true;

            if (FindFirstPathUtil(next, end, visited)) // Recurse and roll-back if destination found
                return true;

            edgeuse[nextedge]--; // subtract use count since destination not reached
        }
        return false;
    }
}

public class Edge
{
    public int n0;
    public int n1;

    public Edge(int n0, int n1)
    {
        this.n0 = (n0 < n1) ? n0 : n1;
        this.n1 = (n0 < n1) ? n1 : n0;
    }

    public override bool Equals(Object? obj)
    {
        if (obj == null || !(obj is Edge))
            return false;
        
        Edge other = (Edge)obj;

        return (this.n0 == other.n0 && this.n1 == other.n1);
    }

    public override int GetHashCode()
    {
        return Tuple.Create(n0, n1).GetHashCode();
    }
}