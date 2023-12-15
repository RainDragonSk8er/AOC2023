// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Immutable;
using System.Data;
using System.Net.NetworkInformation;



class Program
{
    const int inputdimensions = 10;

    static void Main()
    {
        string line;

        // rr = roundrocks (O), sr = squarerocks (#)
        List<PosIDPair>[] rrNS = new List<PosIDPair>[inputdimensions];
        List<PosIDPair>[] rrWE = new List<PosIDPair>[inputdimensions];
        List<int>[] srNS = new List<int>[inputdimensions];
        List<int>[] srWE = new List<int>[inputdimensions];

        List<char[]> rows = new List<char[]>();
        List<char[]> savedrows = new List<char[]>();
        int i = 0, answer = 0, answer2 = 0;
        int repeats = 1, rockID = 0, row = 0;
        Console.WriteLine("Hello World on December 14th 2023!");

        // Creating list objects for each element in the arrays
        for (i = 0; i < inputdimensions; i++)
        {
            rrNS[i] = new List<PosIDPair>();
            rrWE[i] = new List<PosIDPair>();
            srNS[i] = new List<int>();
            srWE[i] = new List<int>();
        }
        string inputPath = @"..\..\..\AOC2023-14-TestInput.txt";
        using (StreamReader inputFile = new StreamReader(inputPath))
        {
            // Read input and expand lines (y-axis)
            while ((line = inputFile.ReadLine()) != null)
            {
                for (i = 0; i < line.Length; i++)
                {
                    switch (line[i])
                    {
                        case '.': // space
                            break;
                        case 'O': // roundrock populate NS only
                            rrNS[i].Add(new PosIDPair(row, ++rockID));
                            break;
                        case '#': // squarerock
                            srNS[i].Add(row);
                            srWE[row].Add(i);
                            break;
                    }
                }
                row++;
            }
            Console.WriteLine("Initial round rockcount is {0}", rockID);
            Console.ReadKey();
        }
        // Perform initial tilts
        for (i = 0; i < repeats; i++)
        {
            PrintPlatform(rrNS);
            TiltAndTurn(rrNS, rrWE, srNS, 1);
            PrintPlatform(rrWE);
            TiltAndTurn(rrWE, rrNS, srWE, 1);
            PrintPlatform(rrNS);
            TiltAndTurn(rrNS, rrWE, srNS, -1);
            PrintPlatform(rrWE);
            TiltAndTurn(rrWE, rrNS, srWE, -1);
            PrintPlatform(rrNS);
            // 
        }
        Console.WriteLine("Completed {0} spin cylcles:", repeats);
        // Console.WriteLine("The answer to part one is: {0}", answer);
        // Console.WriteLine("The answer to part two is: {0}", answer2);
        // Console.WriteLine("{0} => {1}", new string(kvp.Key._key), new string(kvp.Value));
        Console.WriteLine("Hit any key to exit!");
        Console.ReadKey();
    }

    static void TiltAndTurn(List<PosIDPair>[] roundin, List<PosIDPair>[] roundout, List<int>[] squares, int direction)
    {
        // Clear all output lists
        for (int i = 0; i < inputdimensions; i++)
        {
            roundout[i].Clear();
        }
        
        // Start tilt and turn
        for (int i = 0; i < inputdimensions; i++)
        {
            int cantiltto = 0, squareIndex = (direction > 0) ? -1 : squares[i].Count, squarepos = -1;

            foreach (PosIDPair pip in roundin[i])
            {
                while (pip.Pos > squarepos)
                {
                    cantiltto = squarepos + 1;
                    if ((direction > 0) ? (squareIndex < squares[i].Count - 1) : (squareIndex > 0))
                        squarepos = (direction > 0) ? (squares[i][++squareIndex]) : (inputdimensions - squares[i][--squareIndex]);
                    else
                        squarepos = inputdimensions;
                }
                if (pip.Pos >=  cantiltto)
                {
                    roundout[cantiltto].Add(new PosIDPair(i, pip.ID));
                    cantiltto++;
                }
            }
        }
    }

    static void PrintPlatform(List<PosIDPair>[] roundin)
    {
        for (int i = 0; i < inputdimensions; i++)
        {
            int j = 0;
            foreach (PosIDPair pip in roundin[i])
            {
                while (j++ < pip.Pos)
                    Console.Write('.');

                Console.Write('O');
            }
            while (j++ < inputdimensions)
                Console.Write('.');
            
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    static char[] TiltCharArray(char[] ca)
    {
        char[] tiltedca = new char[ca.Length];

        int cantiltto = ca.Length - 1;
        for (int i = 0; i < ca.Length; i++)
        {
            tiltedca[i] = ca[i];
            switch (ca[i])
            {
                case '.':
                    if (cantiltto > i)
                        cantiltto = i;
                    break;

                case '#':
                    cantiltto = i + 1;
                    break;

                case 'O':
                    if (cantiltto < i)
                    {
                        tiltedca[i] = '.';
                        tiltedca[cantiltto] = 'O';
                    }
                    cantiltto++;
                    break;
            }
        }
        return tiltedca;
    }

    static void TiltN(List<char[]> rows)
    {
        // For each column (i)
        for (int i = 0; i < rows[0].Length; i++)
        {
            // Cheate character array of row
            char[] colca = new char[rows.Count];
            for (int j = 0; j < rows.Count; j++)
                colca[j] = rows[j][i];

            // Tilt the char array
            char[] tiltedcolca = TiltCharArray(colca);

            // Return the tilted array to the row
            for (int j = 0; j < rows.Count; j++)
                rows[j][i] = tiltedcolca[j];
        }
    }

    static void TiltS(List<char[]> rows)
    {
        // For each column (i)
        for (int i = 0; i < rows[0].Length; i++)
        {
            // Cheate character array of row
            char[] colca = new char[rows.Count];
            for (int j = 0; j < rows.Count; j++)
                colca[j] = rows[rows.Count -  1 - j][i];

            // Tilt the char array
            char[] tiltedcolca = TiltCharArray(colca);

            // Return the tilted array to the row
            for (int j = 0; j < rows.Count; j++)
                rows[j][i] = tiltedcolca[rows.Count - 1 - j];
        }
    }

    static void TiltW(List<char[]> rows)
    {
        for (int j = 0; j < rows.Count; j++)
        {
            rows[j] = TiltCharArray(rows[j]);
        }
    }

    static void TiltE(List<char[]> rows)
    {
        for (int j = 0; j < rows.Count; j++)
        {
            char[] rowca = new char[rows[j].Length];
            for (int i = 0; i < rowca.Length; i++)
                rowca[i] = rows[j][rowca.Length - 1 -i];

            rowca = TiltCharArray(rowca);

            for (int i = 0; i < rowca.Length; i++)
                rows[j][i] = rowca[rowca.Length - 1 - i];
        }
    }
}

class RoundRock
{
    public int ID { get; set; }
    public int cyclestart { get; set; }
    public int cyclelength { get; set; }
    public int cyclecount { get; set; }
    public List<int> path;
}

public struct PosIDPair
{
    public int Pos { get; }
    public int ID { get; }

    public PosIDPair(int pos, int id)
    {
        Pos = pos;
        ID = id;
    }
}