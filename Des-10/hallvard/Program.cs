// See https://aka.ms/new-console-template for more information

using System.Collections.Immutable;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

string oldpipes = "|-LJ7F", newpipes = "│─└┘┐┌";
string[] validPipes = { "|7F", "-J7", "|LJ", "-LF" };
string[] nextDirection = { "NWE", "ENS", "SEW", "WNS" };
string directions = "NESW";
int[] lookSN = { -1, 0, 1, 0 };
int[] lookEW = { 0, 1, 0, -1 };
int[] upordownvalues = { 2, 0, 1, -1, 1, -1};

Console.WriteLine("Hello World on December 10th 2023!");
string inputPath = @"..\..\..\AOC2023-10-Input.txt";
using (StreamReader inputFile = new StreamReader(inputPath))
{
    int answer = 0, answer2 = 0;
    string line;
    List<char[]> ground = new List<char[]>();
    List<char[]> pipeline = new List<char[]>();

    int startEW = -1, startSN = 0;
    while ((line = inputFile.ReadLine()) != null)
    {
        char[] groundline = line.ToCharArray();
        ground.Add(groundline);
        if (startEW == -1)
            startEW = line.IndexOf('S');
        if (startEW == -1)
            startSN++;

        pipeline.Add(new string('.', line.Length).ToCharArray());
    }
    Console.WriteLine("Found S at {0} east and {1} south (0-indexed from NW corner)", startEW, startSN);
    inputFile.Close();

    int traceEW = startEW, traceSN = startSN, stepcount = 0;
    pipeline[traceSN][traceEW] = newpipes[oldpipes.IndexOf('7')]; ;
    int looknext = 0;
    bool foundFirst = false;
    do
    {
        stepcount++;
        Console.Write("Step {0} @ (E{1},S{2}) on '{3}:", stepcount, traceEW, traceSN, ground[traceSN][traceEW]);
        int pipeHit;

        do
        {
            if (traceEW + lookEW[looknext] >= 0 && traceEW + lookEW[looknext] < ground[traceSN].Length &&
                traceSN + lookSN[looknext] >= 0 && traceSN + lookSN[looknext] < ground.Count)
            {
                char groundChar = ground[traceSN + lookSN[looknext]][traceEW + lookEW[looknext]];
                pipeHit = validPipes[looknext].IndexOf(groundChar);

                if (pipeHit != -1)
                {
                    // Console.WriteLine("\nFound {0} looking '{1}': Full loop completed in {2} steps.", groundChar, looking[i], stepcount);
                    Console.Write("Found {0} looking '{1}' Going E{2}, S{3}", groundChar, directions[looknext], lookEW[looknext], lookSN[looknext]);
                    traceEW += lookEW[looknext];
                    traceSN += lookSN[looknext];
                    pipeline[traceSN][traceEW] = newpipes[oldpipes.IndexOf(groundChar)];
                    looknext = directions.IndexOf(nextDirection[looknext][pipeHit]);
                    Console.WriteLine(" and looking next to '{0}'.", directions[looknext]);
                    foundFirst = true;
                    break;
                }

                if (groundChar == 'S')
                {
                    Console.WriteLine("\nFull loop completed after {0} steps.", stepcount);
                    traceEW += lookEW[looknext];
                    traceSN += lookSN[looknext];
                    break;
                }
            }
            looknext++; // Loop until fornd first pipe
            if (looknext > 3)
            {
                Console.WriteLine("Failed to fint a starting pipe!");
                break;
            }
        } while (!foundFirst);
    } while ((traceEW != startEW || traceSN != startSN));
    answer = stepcount / 2;
    Console.WriteLine("The answer to part one is: {0}", answer);
    Console.ReadKey();

    bool inside;
    int upordown, pipeIndex;
    Console.WriteLine("PIPELINE MAP WITH ENCLOSED TILES ('0'):");
    foreach (char[] groundline in pipeline)
    {
        inside = false;
        upordown = 0;
        foreach (char c in groundline)
        {
            if (inside && c == '.')
            {
                Console.Write('0');
                answer2++;
            }
            else
            {
                pipeIndex = newpipes.IndexOf(c);
                if (pipeIndex >= 0)
                {
                    upordown += upordownvalues[pipeIndex];
                    if (Math.Abs(upordown) == 2)
                    {
                        upordown = 0;
                        inside = !inside;
                    }
                }
                Console.Write(c);
            }
        }
        Console.WriteLine();
    }
    Console.WriteLine("The answer to part two is: {0}", answer2);
    Console.WriteLine("Hit any key to exit!");
    Console.ReadKey();
}