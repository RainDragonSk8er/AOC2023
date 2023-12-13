// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Immutable;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;

class Program
{
    public static Dictionary<string, UInt64> MyCache = new Dictionary<string, UInt64>();

    static void Main()
    {
        Console.WriteLine("Hello World on December 12th 2023!");

        string inputPath = @"..\..\..\AOC2023-12-Input.txt";
        using (StreamReader inputFile = new StreamReader(inputPath))
        {
            string line;
            UInt64 answer = 0, answer2 = 0;
            UInt64 arrangements;

            // Read input and expand lines (y-axis)
            while ((line = inputFile.ReadLine()) != null)
            {
                MyCache.Clear();

                string[] segments = line.Split(" ");
                int[] counts = segments[1].Split(",").Select(int.Parse).ToArray();

                // Part 1
                Console.WriteLine("New line: '{0}'.", line);
                arrangements = Count(segments[0], counts);
                Console.WriteLine(" => {0} arrangements", arrangements);
                answer += arrangements;

                // Part 2
                int[] counts5 = counts.Concat(counts).Concat(counts).Concat(counts).Concat(counts).ToArray();
                string mapconcatinated = string.Join("?", Enumerable.Repeat(segments[0], 5));
                Console.Write("New line: '{0}'.", mapconcatinated);
                arrangements = Count(mapconcatinated, counts5);
                answer2 += arrangements;
                Console.WriteLine(" - {0} arrangements", arrangements);
            }
            Console.WriteLine("The answer to part one is: {0}", answer);
            Console.WriteLine("The answer to part two is: {0}", answer2);
            Console.WriteLine("Hit any key to exit!");
            Console.ReadKey();
        }
    }

    static UInt64 Count(string cfg, int[] nums)
    {
        
        // Console.WriteLine("Trying input: {0} remaining streaks {1}", cfg, nums.Length);

        if (cfg.Length == 0) // At end of string
            return (UInt64)((nums.Length == 0) ? 1 : 0); // Return one if also at end of Streaks

        if (nums.Length == 0) // At end of Streaks
            return (UInt64)((cfg.IndexOf('#') == -1) ? 1 : 0); // Return one if only '.' og '?' left in input

        // Check if result in cache and return
        string key = cfg + nums.Length.ToString();
        if (MyCache.ContainsKey(key))
            return MyCache[key];

        UInt64 result = 0;

        if (cfg[0] == '.' || cfg[0] == '?')
            result += Count(cfg.Substring(1), nums); // Assume '?' to be '.' and recurse with ramaining string

        if (cfg[0] == '#' || cfg[0] == '?')
            if (nums[0] <= cfg.Length && cfg.Substring(0, nums[0]).IndexOf('.') == -1 &&
                    (nums[0] == cfg.Length || cfg[nums[0]] != '#')) // Assume '?' to be '#' check if streak is possible
                result += Count((nums[0] < cfg.Length ? cfg.Substring(nums[0] + 1) : string.Empty), nums.Skip(1).ToArray()); // Recurse with remaining string and numbers

        // Add new result to cache
        MyCache.Add(key, result);
        return result;
    }
}