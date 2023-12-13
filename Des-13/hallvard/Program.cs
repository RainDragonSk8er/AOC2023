// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Immutable;
using System.Net.NetworkInformation;

class Program
{
    public static UInt64 arrangements;

    static void Main()
    {
        Console.WriteLine("Hello World on December 13th 2023!");

        string inputPath = @"..\..\..\AOC2023-13-Input.txt";
        using (StreamReader inputFile = new StreamReader(inputPath))
        {
            string line;
            List<string> rows = new List<string>();
            int answer = 0, answer2 = 0;

            // Read input and expand lines (y-axis)
            while ((line = inputFile.ReadLine()) != null)
            {
                if (line != string.Empty)
                {
                    rows.Add(line);
                }
                else
                {
                    answer += 100 * FindMirror(rows);
                    answer2 += 100 * FindSmudgedMirror(rows);
                    List<string> cols = Transpose(rows);
                    answer += FindMirror(cols);
                    answer2 += FindSmudgedMirror(cols);
                    rows.Clear();
                }
            }
            if (rows.Count > 0)
            {
                answer += 100 * FindMirror(rows);
                answer2 += 100 * FindSmudgedMirror(rows);
                List<string> cols = Transpose(rows);
                answer += FindMirror(cols);
                answer2 += FindSmudgedMirror(cols);
                rows.Clear();
            }
            Console.WriteLine("The answer to part one is: {0}", answer);
            Console.WriteLine("The answer to part two is: {0}", answer2);
            Console.WriteLine("Hit any key to exit!");
            Console.ReadKey();
        }
    }

    static int FindMirror(List<string> rows)
    {
        bool mirror = false;
        Console.WriteLine("Looking for a mirror in:");
        for (int i = 0; i < rows.Count; i++)
        {
            Console.WriteLine(rows[i]);
        }
        for (int i = 1; i < rows.Count; i++)
        {
            if (rows[i] == rows[i - 1])
            {
                mirror = true;
                for (int j = 1; j < i && j + i < rows.Count; j++)
                {
                    if (rows[i + j] != rows[i - j - 1])
                    {
                        mirror = false;
                        break;
                    }
                }
            }
            if (mirror)
            {
                Console.WriteLine("Found mirror between rows {0} and {1}!", i, i + 1);
                return i;
            }
        }
        Console.WriteLine("Found no mirrors!");
        return 0;
    }

    static int FindSmudgedMirror(List<string> rows)
    {
        Console.WriteLine("Looking for a smudged mirror in:");
        for (int i = 0; i < rows.Count; i++)
        {
            Console.WriteLine(rows[i]);
        }

        int smudgecount = 0;
        for (int i = 1; i < rows.Count; i++)
        {
            smudgecount = SmudgeCompare(rows[i], rows[i - 1]);
            if (smudgecount <= 1)
            {
                for (int j = 1; j < i && j + i < rows.Count; j++)
                {
                    smudgecount += SmudgeCompare(rows[i + j], rows[i - j - 1]);
                    if (smudgecount > 1)
                        break;
                }
            }
            if (smudgecount == 1) // Excatly one
            {
                Console.WriteLine("Found mirror between rows {0} and {1}!", i, i + 1);
                return i;
            }
        }
        Console.WriteLine("Found no mirrors!");
        return 0;
    }

    static int SmudgeCompare(string row1, string row2)
    {
        int smudgecount = 0;
        for (int i = 0; i < row1.Length; i++)
        {
            if ((row1[i] != row2[i])) { smudgecount++; }
            if (smudgecount > 1) break;
        }
        return smudgecount;
    }

    static List<string> Transpose(List<string> rows)
    {
        List<string> transposed = new List<string>();

        for (int j = 0; j < rows[0].Length; j++)
        {
            char[] column = new char[rows.Count];

            for (int i = 0; i < rows.Count; i++)
            {
                column[i] = rows[i][j];
            }

            transposed.Add(new string(column));            
        }
        return transposed;
    }
}