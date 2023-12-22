// See https://aka.ms/new-console-template for more information
using System;
using System.Drawing;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Net.NetworkInformation;

public static class Program
{
    static HashSet<Plot> rocks = new HashSet<Plot>();
    static HashSet<Plot>[] steps = new HashSet<Plot>[101];

    static int[] dx = {1, 0, -1, 0};
    static int[] dy = {0, 1, 0, -1};

    static int[] startx = [0, 65, 130, 130, 130, 65, 0, 0];
    static int[] starty = [0, 0, 0, 65, 130, 130, 130, 65];
    static int[] count64 = [202300, 0, 202300, 0, 202300, 0, 202300, 0];
    static int[] count130 = [0, 1, 0, 1, 0, 1, 0, 1];
    static int[] count195 = [202299, 0, 202299, 0, 202299, 0, 202299, 0];

    public static int xlen, ylen;

    static void Main()
    {
        Stopwatch sw = Stopwatch.StartNew();
        Console.WriteLine("Hello World on December 21th 2023!");

        
        string inputPath = @"..\..\..\AOC2023-21-Input.txt";
        using (StreamReader inputFile = new StreamReader(inputPath))
        {
            string input;
            int y = 0;
            steps[0] = new HashSet<Plot>();

            while ((input = inputFile.ReadLine()) != null)
            {
                xlen = Math.Max(xlen, input.Length);

                for (int x = 0; x < input.Length; x++)
                {
                    if (input[x] == '#')
                        rocks.Add(new Plot(x, y));
                    else if (input[x] == 'S')
                    {
                        steps[0].Add(new Plot(x, y));
                        Console.WriteLine($"Starting point S is at {x}, {y}.");
                    }

                }
                y++;
            }
            ylen = y;
            inputFile.Close();
            Console.WriteLine("Done reading modules!");

            for (int s = 0; s < 64; s++)
            {
                steps[s + 1] = new HashSet<Plot>();

                foreach (Plot p in steps[s])
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Plot nextstep = new Plot(p.x + dx[i], p.y + dy[i]);
                        if (nextstep.IsValid(rocks, false))
                            steps[s + 1].Add(nextstep);
                    }
                }
            }

            int answer = steps[64].Count();

            Console.WriteLine($"The answer to part one is {answer} possible plot destinations after 64 steps.");
            Console.WriteLine($"Box size is {xlen}, {ylen}.");
            Console.ReadKey();

            // Part 2
            UInt64 answer2 = 0;
            for (int j = 0; j < startx.Length; j++)
            {
                steps[0].Clear();
                steps[0].Add(new Plot(startx[j], starty[j]));
                Console.WriteLine($"Starting at {startx[j]}, {starty[j]}:");

                int maxcount = 0;
                int countdown = 500;
                for (int s = 0; s < 300 && countdown > 0; s++)
                {
                    steps[(s + 1) % 2].Clear();

                    foreach (Plot p in steps[s % 2])
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            Plot nextstep = new Plot(p.x + dx[i], p.y + dy[i]);
                            if (nextstep.IsValid(rocks, false))
                                steps[(s + 1) % 2].Add(nextstep);
                        }
                    }

                    if (s + 1 == 64 && count64[j] > 0)
                    {
                        Console.WriteLine($"Type {startx[j]}, {starty[j]} with {s + 1} steps has a count of {steps[(s + 1) % 2].Count()} plots and {count64[j]} occurences contributing to the grand total with: {steps[(s + 1) % 2].Count() * count64[j]}");
                        answer2 += (UInt64)(steps[(s + 1) % 2].Count() * count64[j]);
                    }
                    if (s + 1 == 130 && count130[j] > 0)
                    {
                        Console.WriteLine($"Type {startx[j]}, {starty[j]} with {s + 1} steps has a count of {steps[(s + 1) % 2].Count()} plots and {count130[j]} occurences contributing to the grand total with: {steps[(s + 1) % 2].Count() * count130[j]}");
                        answer2 += (UInt64)(steps[(s + 1) % 2].Count() * count130[j]);
                    }
                    if (s + 1 == 195 && count195[j] > 0)
                    {
                        Console.WriteLine($"Type {startx[j]}, {starty[j]} with {s + 1} steps has a count of {steps[(s + 1) % 2].Count()} plots and {count195[j]} occurences contributing to the grand total with: {steps[(s + 1) % 2].Count() * count195[j]}");
                        answer2 += (UInt64)(steps[(s + 1) % 2].Count() * count195[j]);
                    }
                    if (maxcount < steps[(s + 1) % 2].Count() || maxcount < 7560)
                    {
                        maxcount = steps[(s + 1) % 2].Count();
                    }
                    else
                    {
                        if (countdown > 4)
                        {
                            Console.WriteLine($"Reached maxcount of {maxcount} reachable garden plots after {s} steps.");
                            countdown = 4;
                        }
                    }

                    countdown--;
                    if (countdown < 4)
                        Console.WriteLine($"After {s + 1} steps the count of reachable garden plots is {steps[(s + 1) % 2].Count()}.");
                }
            }
            Console.WriteLine($"Count of filled maps in plot state 7568 is {(UInt64)202300 * (UInt64)202300} contributing to the grand total with: {(UInt64)202300 * (UInt64)202300 * (UInt64)7568}.");
            answer2 += (UInt64)202300 * (UInt64)202300 * (UInt64)7568;
            Console.WriteLine($"Count of filled maps in plot state 7567 is {(UInt64)202299 * (UInt64)202299} contributing to the grand total with: {(UInt64)202299 * (UInt64)202299 * (UInt64)7567}.");
            answer2 += (UInt64)202299 * (UInt64)202299 * (UInt64)7567;
            Console.WriteLine($"The answer to part two is {answer2} possible plot destinations after 26 501 365 steps.");

            Console.WriteLine("Hit any key to exit!");
            Console.ReadKey();
        }
    }
}

class Plot
{
    public int x;
    public int y;

    public Plot(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public bool IsValid(HashSet<Plot> rocks, bool infinite)
    {
        if (infinite)
        {
            if (rocks.Contains(new Plot(this.x % Program.xlen, this.y % Program.ylen))) // Hit a rock
                return false;
        }
        else
        {
            if (x < 0 || x >= Program.xlen || y < 0 || y >= Program.ylen) // Out of bounds
                return false;

            if (rocks.Contains(this)) // Hit a rock
            return false;
        }

        return true;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || !(obj is Plot))
            return false;

        Plot plot = (Plot)obj;
        return this.x == plot.x && this.y == plot.y;
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 + x.GetHashCode();
        hash = hash * 23 + y.GetHashCode();
        return hash;
    }
}