// See https://aka.ms/new-console-template for more information

using System.Collections.Immutable;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

Console.WriteLine("Hello World on December 9th 2023!");
string inputPath = @"..\..\..\AOC2023-09-Input.txt";
using (StreamReader inputFile = new StreamReader(inputPath))
{
    int answer = 0, answer2 = 0;
    string line;

    int linenumber = 0;
    while ((line = inputFile.ReadLine()) != null)
    {
        linenumber++;
        List<int[]> s = new List<int[]>();
        int[] numbers = line.Split(' ').Select(int.Parse).ToArray();
        s.Add(numbers);

        bool notallzero = true;
        while (notallzero)
        {
            int level = s.Count;
            s.Add(new int[s[level - 1].Length - 1]);
            
            notallzero = false;
            for (int i = 0; i < s[level].Length; i++)
            {
                s[level][i] = s[level - 1][i + 1] - s[level - 1][i];
                if (s[level][i] != 0)
                    notallzero = true;
            }
        }

        int nextvalue = 0, prevvalue = 0;
        for (int i = s.Count - 2; i >= 0; i--)
        {
            nextvalue += s[i][s[i].Length - 1];
            prevvalue = s[i][0] - prevvalue;
        }
        Console.WriteLine("Line {0} with length {1} has next value of {2} and previous value of {3}", linenumber, s[0].Length, nextvalue, prevvalue);
        answer += nextvalue;
        answer2 += prevvalue;
    }
    Console.WriteLine("The answer to part one is: {0}", answer);
    Console.WriteLine("The answer to part two is: {0}", answer2);
    inputFile.Close();
}
Console.WriteLine("Hit any key to exit!");
Console.ReadKey();