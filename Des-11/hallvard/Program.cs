// See https://aka.ms/new-console-template for more information

using System.Collections.Immutable;
using System.ComponentModel.Design;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;


Console.WriteLine("Hello World on December 11th 2023!");
Console.Write("Enter expansion factor [2]: ");

string FactorInput = Console.ReadLine();
UInt64 ExpFactor = string.IsNullOrEmpty(FactorInput) ? 2 : UInt64.Parse(FactorInput);

string inputPath = @"..\..\..\AOC2023-11-Input.txt";
using (StreamReader inputFile = new StreamReader(inputPath))
{
    UInt64 answer = 0, answer2 = 0;
    string line;
    Dictionary<UInt64, Dictionary<UInt64, int>> Universe = new Dictionary<UInt64, Dictionary<UInt64, int>>();
    int galaxies = 0;

    // Read input and expand lines (y-axis)
    UInt64 row = 0, expansion = 0;
    while ((line = inputFile.ReadLine()) != null)
    {
        bool empty = true;
        for (UInt64 column = 0; column < (UInt64)line.Length; column++)
        {
            if (line[(int)column] == '#')
            {
                empty = false;
                galaxies++;
                if (Universe.ContainsKey(column))
                {
                    Universe[column][row + expansion] = galaxies;
                }
                else
                {
                    Universe.Add(column, new Dictionary<UInt64, int> { { row + expansion, galaxies } });
                }
            }
        }
        if (empty) expansion += ExpFactor - 1;
        row++;
    }

    List<Galaxy> ExpUniverse = new List<Galaxy>();
    // Loop list column by column and expand
    expansion = 0;
    for (UInt64 column = 0; column <= Universe.Keys.Max(); column++)
    {
        if (Universe.ContainsKey(column))
        {
            foreach (KeyValuePair<UInt64, int> loopRow in Universe[column])
                ExpUniverse.Add(new Galaxy { number = loopRow.Value, column = column + expansion, row = loopRow.Key });
        }
        else
        {
            expansion += ExpFactor - 1;
        }
    }

    // Loop to calculate and add distances
    for (int i = 0 ; i < ExpUniverse.Count - 1; i++)
    {
        for (int j = i + 1; j < ExpUniverse.Count; j++)
        {
            answer += (ExpUniverse[j].column > ExpUniverse[i].column ? ExpUniverse[j].column - ExpUniverse[i].column : ExpUniverse[i].column - ExpUniverse[j].column)
                + (ExpUniverse[j].row > ExpUniverse[i].row ? ExpUniverse[j].row - ExpUniverse[i].row : ExpUniverse[i].row - ExpUniverse[j].row);
        }
    }
    Console.WriteLine("The answer is: {0}", answer);
    Console.WriteLine("Hit any key to exit!");
    Console.ReadKey();
}

public class Galaxy
{
    public int number { get; set; }
    public UInt64 column { get; set; }
    public UInt64 row { get; set; }
}