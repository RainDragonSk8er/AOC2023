// See https://aka.ms/new-console-template for more information

using System.Collections.Immutable;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

Console.WriteLine("Hello World on December 6th 2023!");
string inputPath = @"..\..\..\AOC2023-06-Input.txt";
using (StreamReader inputFile = new StreamReader(inputPath))
{
    UInt64 answer = 1;
    BigInteger j2, answer2;
    double sw, bw, j;

    string line = inputFile.ReadLine();
    UInt64[] t = line.Substring(10, line.Length - 10).Split(new char[0], StringSplitOptions.RemoveEmptyEntries).Select(UInt64.Parse).ToArray();
    BigInteger t2 = BigInteger.Parse(Regex.Replace(line, @"[^\d]+", ""));
    line = inputFile.ReadLine();
    UInt64[] d = line.Substring(10, line.Length - 10).Split(new char[0], StringSplitOptions.RemoveEmptyEntries).Select(UInt64.Parse).ToArray();
    BigInteger d2 = BigInteger.Parse(Regex.Replace(line, @"[^\d]+", ""));

    for (int i = 0; i < t.Length; i++)
    {
        j = Math.Sqrt(Math.Pow(t[i], 2) - 4 * d[i]);
        sw = Math.Floor((t[i] - j) / 2) + 1;
        bw = Math.Ceiling((t[i] + j) / 2) - 1;
        Console.WriteLine("Time {0}, Record distance {1} => Margin of error: {2}", t[i], d[i], bw - sw + 1);
        answer *= (UInt64)(bw - sw + 1);
    }

    // Part 2 - Find solution with binary search
    BigInteger t0 = t2 / 2;
    BigInteger s = t0 * (t2 - t0);
    BigInteger tj = t0;
    int bitpos = 0;
    while (tj != 0)
    {
        bitpos++;
        tj = tj >> 1;
    }
    tj = BigInteger.Pow(2, bitpos);

    Console.WriteLine("Start:\n t0 {0}, tj {1}, s {2}", t0, tj, s);
    while (!(s > d2 && (t0 - 1) * (t2 - t0 + 1) < d2) && tj > 1)
    {
        tj /= 2;
        
        if (s > d2)
            t0 -= tj;
        else
            t0 += tj;

        s = t0 * (t2 - t0);
        Console.WriteLine("t0 {0}, tj {1}, s {2}", t0, tj, s);
    }

    answer2 = (t2 - t0 * 2 + 1);
    Console.WriteLine("Time {0}, Record distance {1} => Margin of error: {2}", t2, d2, answer2);

    Console.WriteLine("The answer to part one is: {0}", answer);
    Console.WriteLine("The answer to part two is: {0}", answer2);
    inputFile.Close();
}
Console.WriteLine("Hit any key to exit!");
Console.ReadKey();