// See https://aka.ms/new-console-template for more information

using System.Collections.Immutable;
using System.Runtime.CompilerServices;

Console.WriteLine("Hello World on December 5th 2023!");
string inputPath = @"..\..\..\AOC2023-05-Input.txt";
using (StreamReader inputFile = new StreamReader(inputPath))
{
    UInt64 answer = 0, answer2 = 0;

    List<Map> maps = new List<Map>();
    Stack<SeedRange> seeds = new Stack<SeedRange>();
    Stack<SeedRange> newseeds = new Stack<SeedRange>();

    int mapsindex = 0, fromindex = 0, toindex = 0;
    string[] currentmap;

    string line = inputFile.ReadLine();

    if (line.Length < 8 || line.Substring(0, 7) != "seeds: ")
    {
        Console.WriteLine("Error! The first line of input must start with 'seeds:'!");
    }
    else
    {
        UInt64[] SeedValues = line.Substring(7, line.Length - 7).Split().Select(UInt64.Parse).ToArray();
        for (int i = 0; i < SeedValues.Length; i += 2) // For Part 1: i += 1
        {
            // seeds.Push(new SeedRange { start = SeedValues[i], length = 1 }); // Part 1
            seeds.Push(new SeedRange { start = SeedValues[i], length = SeedValues[i+1] });  // Part 2
        }
    }
    while ((line = inputFile.ReadLine()) != null)
    {
        if (line.Length > 5 && line.Substring(line.Length - 5, 5) == " map:")
        {
            currentmap = line.Substring(0, line.Length - 5).Split("-to-");
            maps.Add(new Map { from = currentmap[0], to = currentmap[1] });
            mapsindex = maps.Count - 1;
        }
        else if (line.Length > 5 ) // Line with three map-numbers (minimum one digit)
        {
            UInt64[] mapnumbers = line.Split().Select(UInt64.Parse).ToArray();
            maps[mapsindex].mappings.Add(new Mapping { destination = mapnumbers[0], source = mapnumbers[1], length = mapnumbers[2] });
        }
    }

    // Part 1 / Part 2
    foreach (Map map in maps)
    {
        while (seeds.Count > 0)
        {
            SeedRange s = seeds.Pop();
            bool mapped = false;

            foreach (Mapping m in map.mappings)
            {
                UInt64 os = Math.Max(s.start, m.source);
                UInt64 oe = Math.Min(s.start + s.length - 1, m.source + m.length - 1);
                if (os <= oe)
                {
                    newseeds.Push(new SeedRange { start = os - m.source + m.destination, length = oe - os + 1 });

                    Console.WriteLine("Seed mapping: {0}+{1} => {2}+{3}", s.start, s.length, os - m.source + m.destination, oe - os + 1);

                    if (os > s.start)
                        seeds.Push(new SeedRange { start = s.start, length = os - s.start });
                    if (oe < s.start + s.length - 1)
                        seeds.Push(new SeedRange { start = oe + 1, length = s.start + s.length - oe - 1 });

                    mapped = true;
                    break;
                }
            }
            if (!mapped)
            {
                newseeds.Push(new SeedRange { start = s.start, length = s.length });
                Console.WriteLine("Seed mapping: {0}+{1} => no change", s.start, s.length);
            }
        }
        seeds = newseeds;
        newseeds = new Stack<SeedRange>();
    }
    while (seeds.Count > 0)
    {
        SeedRange loopsr = seeds.Pop();
        if (answer == 0 || answer > loopsr.start)
            answer = loopsr.start;
    }
    Console.WriteLine("The answer to part one/two is: " + answer.ToString());
    inputFile.Close();
}
Console.WriteLine("Hit any key to exit!");
Console.ReadKey();

public class SeedRange
{
    public UInt64 start { get; set; }
    public UInt64 length { get; set; }
}

public class Mapping
{
    public UInt64 destination { get; set; }
    public UInt64 source { get; set; }
    public UInt64 length { get; set; }
}

public class Map
{
    public string from { get; set; }
    public string to { get; set; }
    public List<Mapping> mappings { get; set; }

    public Map()
    {
        mappings = new List<Mapping>();
    }
}