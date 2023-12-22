// See https://aka.ms/new-console-template for more information
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Xml.Serialization;

public static class Program
{
    static SortedList<int, Brick> bricks = new SortedList<int, Brick>();
    public static HashSet<int> hasfallen = new HashSet<int>();

    static SortedList<int, Brick> fallenbricks = new SortedList<int, Brick>();

    public static int lowcount = 0, highcount = 0;
    static int push;

    static void Main()
    {
        Stopwatch sw = Stopwatch.StartNew();
        Console.WriteLine("Hello World on December 22th 2023!");
        
        string inputPath = @"..\..\..\AOC2023-22-Input.txt";
        using (StreamReader inputFile = new StreamReader(inputPath))
        {
            string input;
            // Read modules
            while ((input = inputFile.ReadLine()) != null)
            {
                Match match = Regex.Match(input, @"^(?<x1>\d+),(?<y1>\d+),(?<z1>\d+)~(?<x2>\d+),(?<y2>\d+),(?<z2>\d+)$");
                if (match.Success)
                {
                    Brick nb = new Brick(int.Parse(match.Groups["x1"].Value), int.Parse(match.Groups["y1"].Value), int.Parse(match.Groups["z1"].Value),
                                         int.Parse(match.Groups["x2"].Value), int.Parse(match.Groups["y2"].Value), int.Parse(match.Groups["z2"].Value));
                    bricks.Add(nb.SortKey(), nb);
                }
            }
            inputFile.Close();
            Console.WriteLine("Done reading bricks!");
            Console.WriteLine("The bricks in sorted order from lowest and up are:");

            // For all bricks
            foreach (Brick b in bricks.Values)
            {
                Console.Write($"{b.x1},{b.y1},{b.z1} ~ {b.x2},{b.y2},{b.z2} => ");
                b.Drop(bricks);
                Console.WriteLine($"{b.x1},{b.y1},{b.z1} ~ {b.x2},{b.y2},{b.z2}");
            }

            Console.WriteLine("After dropping to rest the bricks are:");
            int answer = 0, answer2 = 0, tmpchainreaction = 0;
            foreach (Brick b in bricks.Values)
            {
                Console.Write($"Brick {b.x1},{b.y1},{b.z1} ~ {b.x2},{b.y2},{b.z2} => supports {b.supports.Count()} and is supported by {b.supportedby.Count()}. Disintegrateble? ");
                if (b.Disintegrateable(bricks))
                {
                    answer++;
                    Console.WriteLine("Yes!");
                }
                else
                {
                    Console.WriteLine("No!");

                    hasfallen.Clear();
                    fallenbricks.Clear();
                    tmpchainreaction = b.ChainReaction2(bricks, fallenbricks);
                    Console.WriteLine($"=> Will cause a chain reaction disintegrating {tmpchainreaction} bricks.");
                    answer2 += tmpchainreaction;
                }
            }


            Console.WriteLine($"Answer to part two is {answer2}!");
            Console.WriteLine($"Hit any key to exit!");
            Console.ReadKey();
        }
    }
}

class Brick
{
    public int sortkey;
    public int x1;
    public int y1;
    public int z1;
    public int x2;
    public int y2;
    public int z2;
    public List<int> supports;
    public List<int> supportedby;

    public Brick(int x1, int y1, int z1, int x2, int y2, int z2)
    {
        this.x1 = x1;
        this.y1 = y1;
        this.z1 = z1;
        this.x2 = x2;
        this.y2 = y2;
        this.z2 = z2;
        this.supports = new List<int>();
        this.supportedby = new List<int>();
    }

    public Brick(Brick sb)
    {
        this.sortkey = sb.sortkey;
        this.x1 = sb.x1;
        this.y1 = sb.y1;
        this.z1 = sb.z1;
        this.x2 = sb.x2;
        this.y2 = sb.y2;
        this.z2 = sb.z2;
        this.supports = new List<int>();
        this.supportedby = new List<int>();
    }

    public int SortKey()
    {
        sortkey = Math.Min(z1, z2) * 100000 + Math.Min(y1, y2) * 100 + Math.Min(x1, x2);
        return sortkey;
    }

    public int SortKeyTop()
    {
        return Math.Max(z1, z2) * 100000 + Math.Max(y1, y2) * 100 + Math.Max(x1, x2); ;
    }

    public void Drop(SortedList<int, Brick> bsl)
    {
        int minz = 1, tempminz = 1;
        List<int> tempsupport = new List<int>();

        foreach (Brick b in bsl.Values)
        {
            if (this.sortkey == b.sortkey)
                break;

            if (Math.Max(Math.Min(x1, x2), Math.Min(b.x1, b.x2)) <= Math.Min(Math.Max(x1, x2), Math.Max(b.x1, b.x2)) &&
                Math.Max(Math.Min(y1, y2), Math.Min(b.y1, b.y2)) <= Math.Min(Math.Max(y1, y2), Math.Max(b.y1, b.y2)))
            {
                tempminz = Math.Max(b.z1, b.z2) + 1;
                if (tempminz > minz)
                {
                    tempsupport.Clear();
                }
                if (tempminz >= minz)
                {
                    tempsupport.Add(b.sortkey);
                    minz = tempminz;
                }
            }
        }

        int drop = Math.Min(z1, z2) - minz;
        if (drop > 0)
        {
            z1 -= drop;
            z2 -= drop;
        }

        foreach (int bsk in tempsupport)
        {
            this.supportedby.Add(bsk);
            bsl[bsk].supports.Add(this.sortkey);
        }
    }

    public bool Disintegrateable(SortedList<int, Brick> bsl)
    {
        foreach (int s in supports)
        {
            if (bsl[s].supportedby.Count() < 2) // The brick supports a brick with no other support
                return false;
        }
        return true;
    }

    public int ChainReaction(SortedList<int, Brick> bsl, SortedList<int, Brick> fb)
    {
        fb.Add(this.SortKeyTop(), new Brick(this));
        Program.hasfallen.Add(this.sortkey);

        while (fb.Count() > 0)
        {
            int key = fb.First().Value.sortkey;
            fb.Remove(fb.First().Key);

            foreach (int s in bsl[key].supports)
            {
                int remainingsupport = 0;
                foreach (int rs in bsl[s].supportedby)
                    if (!Program.hasfallen.Contains(rs))
                        remainingsupport++;

                if (remainingsupport == 0) // The brick supports a brick with no other support
                {
                    if (Program.hasfallen.Add(bsl[s].sortkey)) // Add if not already fallen
                        fb.Add(bsl[s].SortKeyTop(), new Brick(bsl[s]));
                }
            }
        }
        return Program.hasfallen.Count() - 1;
    }

    public int ChainReaction2(SortedList<int, Brick> bsl, SortedList<int, Brick> fb)
    {
        Program.hasfallen.Add(this.sortkey);

        int fell = 0;
        do
        {
            fell = 0;
            foreach (Brick b in bsl.Values)
            {
                if (Program.hasfallen.Contains(b.sortkey)) // No need to check fallen bricks
                    continue;

                if (Math.Min(b.z1, b.z2) == 1) // On ground level
                    continue;

                int remainingsupport = 0;
                foreach (int s in b.supportedby)
                    if (!Program.hasfallen.Contains(s))
                        remainingsupport++;

                if (remainingsupport == 0) // The brick supports a brick with no other support
                {
                    Program.hasfallen.Add(b.sortkey); // Add if not already fallen
                    fell++;
                }
            }
            // Console.WriteLine($"{fell} bricks fell in this pass!");
        } while (fell > 9);

        return Program.hasfallen.Count() - 1;
    }
}