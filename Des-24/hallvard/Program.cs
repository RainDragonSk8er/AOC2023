// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

class Program
{
    public static List<Hailstone> hailstones = new List<Hailstone>();

    static void Main()
    {
        Stopwatch sw = Stopwatch.StartNew();
        Console.WriteLine("Hello World on December 24th 2023!");

        // Int128 lower_bound = 7, upper_bound = 27; // Test-bounds
        Int128 lower_bound = 200000000000000, upper_bound = 400000000000000;

        string inputPath = @"..\..\..\AOC2023-24-Input.txt";
        using (StreamReader inputFile = new StreamReader(inputPath))
        {
            string input;

            // Read input and build contraption map
            while ((input = inputFile.ReadLine()) != null)
            {
                Match match = Regex.Match(input, @"^(?<px>\d+),\s*(?<py>\d+),\s*(?<pz>\d+)\s*@\s*(?<vx>-?\d+),\s*(?<vy>-?\d+),\s*(?<vz>-?\d+)$");
                if (match.Success)
                {
                    Hailstone hs = new Hailstone(UInt64.Parse(match.Groups["px"].Value), UInt64.Parse(match.Groups["py"].Value), UInt64.Parse(match.Groups["pz"].Value),
                                         int.Parse(match.Groups["vx"].Value), int.Parse(match.Groups["vy"].Value), int.Parse(match.Groups["vz"].Value));
                    hailstones.Add(hs);
                    Console.WriteLine($"Read hailstone at {hs.px}, {hs.py} with velocity vector [{hs.vx}, {hs.vy}].");
                }
            }
            inputFile.Close();
        }
        Console.WriteLine("Done reading hailstones!\n");
        Console.WriteLine("Starting crosscheck");

        int answer = 0;
        for (int i = 0; i < hailstones.Count() - 1; i++)
        {
            Console.WriteLine();

            for (int j = i + 1; j < hailstones.Count(); j++)
            {

                if (hailstones[i].FutureIntercetsInRange(hailstones[j], lower_bound, upper_bound))                
                {
                    Console.Write('#');
                    answer++;
                }
                else
                    Console.Write('.');
            }            
        }
        Console.WriteLine();

        Console.WriteLine("The answer to part one is: {0}", answer);

        sw.Stop();
        Console.WriteLine("Time elapsed: {0}\n", sw.Elapsed);

        // Part two created equations for the first four hailstones (n = 1..4) on the form
        //
        // SX + VX * Tn = Xn + VXn * Tn
        // SY + VY * Tn = Yn + VYn * Tn
        // SZ + VZ * Tn = Zn + VZn * Tn
        //
        // For a total of 12 equations with 10 unknowns SX,VX,SY,VY,SZ,VZ,T1,T2,T3,T4
        // and put them into GeoGebra giving the following solution:
        //
        // SX = 461522278379729, VX = -336
        // SY = 278970483473640, VY = 29
        // SZ = 243127954482382, VZ = 38
        //
        // T1 = 631956804666, T2 = 447067527849, T3 = 98915323508, T4 = 172376561422

        Console.WriteLine("The answer to part two is: {0}", 461522278379729 + 278970483473640 + 243127954482382);

        Console.WriteLine("Hit any key to exit!");
        Console.ReadKey();      
    }
}


public class Hailstone
{
    public Int128 px;
    public Int128 py;
    public Int128 pz;
    public int vx;
    public int vy;
    public int vz;

    public Hailstone(Int128 px, Int128 py, Int128 pz, int vx, int vy, int vz)
    {
        this.px = px;
        this.py = py;
        this.pz = pz;
        this.vx = vx;
        this.vy = vy;
        this.vz = vz;
    }

    public bool FutureIntercetsInRange(Hailstone other, Int128 low, Int128 high)
    {
        bool retval = true;

        // Console.WriteLine($"Hailstone A: {this.px}, {this.py} @ {this.vx}, {this.vy}");
        // Console.WriteLine($"Hailstone B: {other.px}, {other.py} @ {other.vx}, {other.vy}");

        Int128 hnf = (Int128)(this.vx * other.vy - this.vy * other.vx); // Whole numer faktor (to avoid rounding and divide by zero)

        if (hnf == 0) // Check for parallelity
        {
            // Console.WriteLine("Hailstones are parallel and will never intersect.");
            retval = false;
        }
        else
        {
            Int128 xcross = (Int128)(this.vx * other.vx) * (this.py - other.py) - (Int128)(this.vy * other.vx) * this.px + (Int128)(other.vy * this.vx) * other.px;
            Int128 ycross = (Int128)(this.vy * other.vy) * (other.px - this.px) + (Int128)(this.vx * other.vy) * this.py - (Int128)(other.vx * this.vy) * other.py;

            if (hnf < 0)
            {
                hnf = -hnf;
                xcross = -xcross;
                ycross = -ycross;
            }

            if ((this.vx < 0 && xcross > this.px * hnf) || (this.vx > 0 && xcross < this.px * hnf) ||
                (this.vy < 0 && ycross > this.py * hnf) || (this.vy > 0 && ycross < this.py * hnf)) // In the past for this (A)
            {
                // Console.WriteLine("Hailstones will cross in the past for hailstone A.");
                retval = false;
            }

            if ((other.vx < 0 && xcross > other.px * hnf) || (other.vx > 0 && xcross < other.px * hnf) ||
                (other.vy < 0 && ycross > other.py * hnf) || (other.vy > 0 && ycross < other.py * hnf)) // In the past for other (B)
            {
                // Console.WriteLine("Hailstones will cross in the past for hailstone B.");
                retval = false;
            }

            if (xcross < low * hnf || xcross > high * hnf || ycross < low * hnf || ycross > high * hnf) // Outside X-range or Y-range
            {
                // Console.WriteLine($"Hailstones will cross outside the test area at {(double)xcross/(double)hnf}, {(double)ycross / (double)hnf}.");
                retval = false;
            }

            // if (retval)
                // Console.WriteLine($"({this.px},{this.py})[{this.vx},{this.vy}] intersects ({other.px},{other.py})[{other.vx},{other.vy}]@({xcross}/{hnf},{ycross}/{hnf}).");
        }

        return retval;
    }
}