// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Net.NetworkInformation;

class Program
{
    public const int dimensions = 110;
    public static char[,] contraption = new char[dimensions, dimensions];
    public static int[,] beams = new int [dimensions, dimensions];
    public static int[,] bestbeams = new int[dimensions, dimensions]; 
    public static int[] xdirection2bits = { 1, 0, 2 };
    public static int[] ydirection2bits = { 4, 0, 8 };

    static void Main()
    {
        Stopwatch sw = Stopwatch.StartNew();
        Console.WriteLine("Hello World on December 16th 2023!");

        string inputPath = @"..\..\..\AOC2023-16-Input.txt";
        using (StreamReader inputFile = new StreamReader(inputPath))
        {
            // Read input and build contraption map
            string line;
            for (int y = 0; y < dimensions; y++)
            {
                line = inputFile.ReadLine();
                for (int x = 0; x < dimensions; x++)
                {
                    contraption[x, y] = line[x];
                }                    
            }

            // Part 1
            BeamTrace(0, 0, 1, 0); // Start beam trace in upper left corner goint right
            int answer = CountEnergized(beams);
            Console.WriteLine("The answer to part one is: {0}", answer);

            // Part 2
            int answer2 = 0, tmpcount;
            for (int i = 0; i < dimensions; i++)
            {
                ClearBeams(beams);
                BeamTrace(i, 0, 0, 1); // Top rom and down
                tmpcount = CountEnergized(beams);
                if (tmpcount > answer2)
                {
                    answer2 = tmpcount;
                    Console.WriteLine("Current max energy {0} found going down from position {1} in top row", answer2, i);
                    CopyBeams(beams, bestbeams);
                }
                ClearBeams(beams);
                BeamTrace(i, dimensions - 1, 0, -1); // Bottom rom and up
                tmpcount = CountEnergized(beams);
                if (tmpcount > answer2)
                {
                    answer2 = tmpcount;
                    Console.WriteLine("Current max energy {0} found going up from position {1} in bottom row", answer2, i);
                    CopyBeams(beams, bestbeams);
                }
                ClearBeams(beams);
                BeamTrace(0, i, 1, 0); // Leftmost column going right
                tmpcount = CountEnergized(beams);
                if (tmpcount > answer2)
                {
                    answer2 = tmpcount;
                    Console.WriteLine("Current max energy {0} found going right from position {1} in leftmost column", answer2, i);
                    CopyBeams(beams, bestbeams);
                }
                ClearBeams(beams);
                BeamTrace(dimensions - 1, i, -1, 0); // Rightmost column going left
                tmpcount = CountEnergized(beams);
                if (tmpcount > answer2)
                {
                    answer2 = tmpcount;
                    Console.WriteLine("Current max energy {0} found going left from position {1} in rigthmost column", answer2, i);
                    CopyBeams(beams, bestbeams);
                }
            }

            Console.WriteLine("The answer to part two is: {0}", answer2);
            sw.Stop();
            Console.WriteLine("Time elapsed: {0}\n", sw.Elapsed);
            Console.WriteLine("Hit any key to print mirror and energy maps!");
            Console.ReadKey();

            PrintContraptionMap();
            PrintBeamsMap(bestbeams);
            Console.WriteLine("Hit any key to exit!");
            Console.ReadKey();
        }
    }

    static void BeamTrace(int x, int y, int xdirection, int ydirection)
    {
        while (x >= 0 && x < dimensions && y >= 0 && y < dimensions)
        {
            if ((beams[x, y] & (xdirection2bits[xdirection + 1] | ydirection2bits[ydirection + 1])) != 0) // Beam there before
                return;

            beams[x, y] |= (xdirection2bits[xdirection + 1] | ydirection2bits[ydirection + 1]); // Mark beam direction on beammap

            int tmpdirection;
            switch (contraption[x, y]) // Check mirror and splitter
            {
                case '.':
                    break;

                case '\\':
                    tmpdirection = xdirection;
                    xdirection = ydirection;
                    ydirection = tmpdirection;
                    break;

                case '/':
                    tmpdirection = xdirection;
                    xdirection = -ydirection;
                    ydirection = -tmpdirection;
                    break;

                case '-':
                    if (ydirection != 0)
                    {
                        xdirection = ydirection;
                        ydirection = 0;
                        BeamTrace(x, y, -xdirection, ydirection); // Recurse new beam in oposite direction
                    }
                    break;

                case '|':
                    if (xdirection != 0)
                    {
                        ydirection = xdirection;
                        xdirection = 0;
                        BeamTrace(x, y, xdirection, -ydirection); // Recurse new beam in oposite direction
                    }
                    break;
            }
            x += xdirection;
            y += ydirection;
        }
    }

    static void ClearBeams(int[,] beamsmap)
    {
        for (int j = 0; j < dimensions; j++)
            for (int i = 0; i < dimensions; i++)
                beamsmap[i, j] = 0;
    }

    static void CopyBeams(int[,] source, int[,] destination)
    {
        for (int j = 0; j < dimensions; j++)
            for (int i = 0; i < dimensions; i++)
                destination[i, j] = source[i, j];
    }

    static int CountEnergized(int[,] beamsmap)
    {
        int count = 0;
        for (int j = 0; j < dimensions; j++)
            for (int i = 0; i < dimensions; i++)
                count += (beamsmap[i, j] != 0) ? 1 : 0;
        return count;
    }

    static void PrintContraptionMap()
    {
        for (int j = 0; j < dimensions; j++)
        {
            for (int i = 0; i < dimensions; i++)
                Console.Write(contraption[i, j]);

            Console.WriteLine();
        }
    }

    static void PrintBeamsMap(int[,] beamsmap)
    {
        for (int j = 0; j < dimensions; j++)
        {
            for (int i = 0; i < dimensions; i++)
            {
                if (beamsmap[i, j] != 0)
                {
                    Console.Write('#');
                }
                else
                    Console.Write('.');
            }
            Console.WriteLine();
        }
    }
}