// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Immutable;
using System.Data;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;



class Program
{
    const int inputdimensions = 100;
    static HashSet<Board> savedboards = new HashSet<Board>();

    static void Main()
    {
        string line;

        List<PosIDPair>[] rrNS = new List<PosIDPair>[inputdimensions];
        List<PosIDPair>[] rrWE = new List<PosIDPair>[inputdimensions];

        List<char[]> rows = new List<char[]>();
        List<char[]> savedrows = new List<char[]>();
        int i = 0, answer = 0, answer2 = 0;
        int maxrepeats = 1000, rrockID = 0, srockID = 0, row = 0;
        Console.WriteLine("Hello World on December 14th 2023!");

        // Creating list objects for each element in the arrays
        for (i = 0; i < inputdimensions; i++)
        {
            rrNS[i] = new List<PosIDPair>();
            rrWE[i] = new List<PosIDPair>();
        }
        string inputPath = @"..\..\..\AOC2023-14-Input.txt";
        using (StreamReader inputFile = new StreamReader(inputPath))
        {
            // Read input and expand lines (y-axis)
            while ((line = inputFile.ReadLine()) != null)
            {
                for (i = 0; i < line.Length; i++)
                {
                    switch (line[line.Length - i - 1])
                    {
                        case '.': // space
                            break;
                        case 'O': // roundrock populate NS only
                            rrNS[i].Add(new PosIDPair(row, ++rrockID));
                            break;
                        case '#': // squarerock
                            rrNS[i].Add(new PosIDPair(row, --srockID));
                            break;
                    }
                }
                row++;
            }
            PrintPlatform(rrNS);
            savedboards.Add(new Board(0, rrNS));

            Console.WriteLine("Initial rockcount is {0} round and {1} square", rrockID, -srockID);
            Console.ReadKey();
        }
        // Perform initial tilts
        for (i = 1; i <= maxrepeats; i++)
        {
            TiltAndTurn(rrNS, rrWE);
            TiltAndTurn(rrWE, rrNS);
            TiltAndTurn(rrNS, rrWE);
            TiltAndTurn(rrWE, rrNS);
            Console.Write($"After cycle {i}: ");
            PrintPlatform(rrNS);
            Board tmpBoard = new Board(i, rrNS);
            if (!savedboards.Add(tmpBoard))
            {
                savedboards.TryGetValue(tmpBoard, out tmpBoard);
                Console.WriteLine("Found a repeat after {0} cylcles back to cycle {1}.", i, tmpBoard.Number);
                answer2 = ((1000000000 - tmpBoard.Number) % (i - tmpBoard.Number)) + tmpBoard.Number;
                break;
            }                
        }        
        Console.WriteLine("The ending cycle after 1 000 000 000 will be the same as after cycle {0}.", answer2);
        // Console.WriteLine("The answer to part one is: {0}", answer);
        // Console.WriteLine("The answer to part two is: {0}", answer2);
        // Console.WriteLine("{0} => {1}", new string(kvp.Key._key), new string(kvp.Value));
        Console.WriteLine("Hit any key to exit!");
        Console.ReadKey();
    }

    static void TiltAndTurn(List<PosIDPair>[] inlist, List<PosIDPair>[] outlist)
    {
        // Clear all output lists
        for (int i = 0; i < inputdimensions; i++)
        {
            outlist[i].Clear();
        }

        // Start tilt and turn
        for (int i = inputdimensions - 1; i >= 0; i--)
        {
            int cantiltto = 0;
            for (int j = 0; j < inlist[i].Count(); j++)
            {
                if (inlist[i][j].ID < 0) // Square rock
                {
                    cantiltto = inlist[i][j].Pos;
                }
                outlist[cantiltto++].Add(new PosIDPair(inputdimensions - i - 1, inlist[i][j].ID));
            }
        }
    }

    static void PrintPlatform(List<PosIDPair>[] roundin)
    {
        int load = 0;
        for (int i = 0; i < inputdimensions; i++)
        {
            int j = 0;
            foreach (PosIDPair pip in roundin[i])
            {
                load += (inputdimensions - pip.Pos) * ((pip.ID > 0) ? 1 : 0);
/*              while (j++ < pip.Pos)
                    Console.Write('.');

                Console.Write((pip.ID > 0) ? 'O' : '#');
*/
            }
/*          while (j++ < inputdimensions)
                Console.Write('.');
            
            Console.WriteLine();
*/
        }

        Console.WriteLine($"Total load on the north support for this board is: {load}");
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
    public int Pos { get; set; }
    public int ID { get; }

    public PosIDPair(int pos, int id)
    {
        Pos = pos;
        ID = id;
    }
}

public class Board
{
    public int Number { get; set; }
    public List<int>[] poslist;

    public Board(int num, List<PosIDPair>[] pip)
    {
        Number = num;
        poslist = new List<int>[pip.Length];
        for (int i = 0; i < pip.Length; i++)
        {
            poslist[i] = new List<int>();
            foreach (PosIDPair p in pip[i])
                poslist[i].Add(p.Pos);
        }
    }

    public override bool Equals(Object? o)
    {
        // Check that object is not null or different type
        if (o == null || GetType() != o.GetType())
            return false;

        // Check that the poslist arrays are the same length
        if (poslist.Length != ((Board)o).poslist.Length)
            return false;
        
        // Check if all the lists are the same length
        for (int i = 0; i < poslist.Length; i++)
            if (poslist[i].Count() != ((Board)o).poslist[i].Count())
                return false;

        // Check if all the lists contain the same elements in the same order
        for (int i = 0; i < poslist.Length; i++)
        {
            for (int j = 0; j < poslist[i].Count(); j++)
            {
                if (poslist[i][j] != ((Board)o).poslist[i][j])
                    return false;
            }
        }
        return true;
    }

    public override int GetHashCode()
    {
        int hash = 0;
        for (int i = 0; i < poslist.Length; i++)
        {
            foreach (int pos in poslist[i])
            {
                hash = hash ^ pos.GetHashCode();
            }
        }
        return hash;
    }
}