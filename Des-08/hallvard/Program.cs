// See https://aka.ms/new-console-template for more information

using System.Net.NetworkInformation;
using System.Numerics;
using System.Text.RegularExpressions;

string mappattern = @"(\w+)\s*=\s*\((\w+),\s*(\w+)\)";

Console.WriteLine("Hello World on December 8th 2023!");
string inputPath = @"..\..\..\AOC2023-08-Input.txt";
using (StreamReader inputFile = new StreamReader(inputPath))
{
    int answer = 0;
    Dictionary<String, String> desertMap = new Dictionary<String, String>();

    char[] leftright = inputFile.ReadLine().ToCharArray();
    string line;

    while ((line = inputFile.ReadLine()) != null)
    {
        Match match = Regex.Match(line, mappattern);

        if (match.Success)
        {
            desertMap.Add(match.Groups[1].Value, match.Groups[2].Value + match.Groups[3].Value);
        }
    }
    inputFile.Close();

    int leftrightindex = 0;
    
    string position = "AAA";
    while (position != "ZZZ")
    {
        position = (leftright[leftrightindex++] == 'L') ? desertMap[position].Substring(0, 3) : desertMap[position].Substring(3, 3);       
        answer++;
        if (leftrightindex >= leftright.Length)
            leftrightindex = 0;
    }
    Console.WriteLine("The answer to part one is: {0}", answer);

    List<NodePath> nodePaths = new List<NodePath>();
    foreach (KeyValuePair<string, string> pair in desertMap)
    {
        if (pair.Key.Substring(2, 1) == "A")
        {
            nodePaths.Add(new NodePath(pair.Key));
        }
    }

    //
    // Brute force took forever. Rewrote to find the number of steps for each start-node to
    // reach a z-node and then to reach its start position at the 0-index of the leftright path.
    //
    // The solution should then be reachable via the least common multiplum of the getting to z-node
    //
    // Wrote my own methods for CalculatePrimeFactors and FindCommonFactors, but could probably be simplified
    // by calculating LCM via GCD where the latter can be done by Euclid's Algorithm.
    //

    Console.WriteLine("There are {0} nodes to be tracked for part two", nodePaths.Count);
    int pathcounter = 0, zcount;
    for (int i = 0; i < nodePaths.Count; i++)
    {
        leftrightindex = 0; zcount = 0;
        while (true)
        {
            pathcounter++;
            nodePaths[i].Current = desertMap[nodePaths[i].Current].Substring((leftright[leftrightindex] == 'L') ? 0 : 3, 3);
            if (nodePaths[i].Current.Substring(2, 1) == "Z")
            {
                nodePaths[i].StepsToZ = pathcounter;
                pathcounter = 0; zcount++;
                Console.WriteLine("Found the Z-period of node {0} to be {1} with the '{2}' Z-node",
                    i, nodePaths[i].StepsToZ, nodePaths[i].Current);

                if (zcount == 10)                
                {
                    nodePaths[i].CalculatePrimeFactors();
                    Console.Write("Factors are: ");

                    foreach (int factor in nodePaths[i].PrimeFactors)
                        Console.Write(" {0}", factor);

                    Console.WriteLine();
                    break;
                }
            }
            if (++leftrightindex >= leftright.Length)
                leftrightindex = 0;
        }
    }
    HashSet<int> commonFactors = FindCommonFactors(nodePaths);

    UInt128 answer2 = 1;
    Console.Write("\nAnswer2 = {0}", answer2);
    foreach (int factor in commonFactors)
    {
        answer2 *= (UInt128)factor;
        Console.Write(" * {0}", factor);
    }

    foreach (NodePath np in nodePaths)
    {
        foreach (int factor in np.PrimeFactors)
        {
            if (!commonFactors.Contains(factor))
            {
                answer2 *= (UInt128)factor;
                Console.Write(" * {0}", factor);
            }

        }
    }

    Console.WriteLine("\n\nThe answer to part two is: {0}", answer2);
}
Console.WriteLine("\nHit any key to exit!");
Console.ReadKey();

static HashSet<int> FindCommonFactors(List<NodePath> listOfNP)
{
    // Convert the first list to a HashSet to start with.
    HashSet<int> commonFactors = new HashSet<int>(listOfNP[0].PrimeFactors);

    // Perform intersection with the other lists.
    for (int i = 1; i < listOfNP.Count; i++) // Start at 1 to skip first
        commonFactors.IntersectWith(listOfNP[i].PrimeFactors);

    return commonFactors;
}

public class NodePath
{
    public string Start { get; set; }
    public string Current { get; set; }
    public int StepsToZ { get; set; }
    public List<int> PrimeFactors { get; set; }

    public NodePath(string StartParam)
    {
        Current = Start = StartParam;
        StepsToZ = 0;
        PrimeFactors = new List<int>();
    }
    public void CalculatePrimeFactors()
    {
        PrimeFactors.Clear();
        int number = StepsToZ;
        while (number % 2 == 0)
        {
            PrimeFactors.Add(2);
            number /= 2;
        }

        // Loop from 3 to the square root of the number.
        for (int i = 3; i <= Math.Sqrt(number); i += 2)
        {
            while (number % i == 0) // If i is a factor
            {
                PrimeFactors.Add(i);
                number /= i;
            }
        }

        if (number > 2)
        {
            PrimeFactors.Add(number);
        }
    }
}

