// See https://aka.ms/new-console-template for more information

using System.Runtime.CompilerServices;

Console.WriteLine("Hello World on December 3nd 2023!");
string inputPath = @"..\..\..\AOC2023-03-Input.txt";
string[] lines = new string[3];

using (StreamReader inputFile = new StreamReader(inputPath))
{
    int answer = 0, answer2 = 0;
    int lineindex = -1, searchline = 0;
    string line;
    while ((line = inputFile.ReadLine()) != null)
    {
        if (lineindex > 1)
        {
            // Scroll
            lines[0] = lines[1];
            lines[1] = lines[2];
            lines[2] = line;
        }
        else
        {
            lines[++lineindex] = line;
        }
        if (lineindex < 1)
            continue; // Need two lines to start looking for part numbers

        answer += FoundParts(searchline, lineindex);
        answer2 += FoundGears(searchline, lineindex);

        if (searchline == 0)
            searchline++;
    }
    if (lineindex > searchline) // Search last line
    {
        answer += FoundParts(++searchline, lineindex);
        answer2 += FoundGears(searchline, lineindex);
    }

    Console.WriteLine("The answer to part one is: " + answer.ToString());
    Console.WriteLine("The answer to part two is: " + answer2.ToString());
    inputFile.Close();
}
Console.WriteLine("Hit any key to exit!");
Console.ReadKey();

int FoundParts(int searchline, int maxline)
{
    int sumoffoundparts = 0;
    int partnumber = 0;
    int ioFirstDigit = -1, ioLastDigit = -1;
    bool validPart;

    for (int i = 0; i < lines[searchline].Length; i++)
    {
        if (char.IsDigit(lines[searchline][i]))
        {
            if (ioFirstDigit == -1)
            {
                ioLastDigit = ioFirstDigit = i;
                partnumber = int.Parse(lines[searchline][i].ToString());
            }
            else
            {
                ioLastDigit = i;
                partnumber = partnumber * 10 + int.Parse(lines[searchline][i].ToString());
            }
        }
        if (ioFirstDigit != -1 && (i == lines[searchline].Length - 1 || !char.IsDigit(lines[searchline][i]))) // End of number / line
        {
            validPart = false;
            for (int j = Math.Max(0, ioFirstDigit - 1); !validPart && j <= Math.Min(ioLastDigit + 1, lines[searchline].Length - 1); j++)
            {
                if (searchline > 0) // Search line above
                {
                    if (lines[searchline - 1][j].ToString() != ".") { validPart = true; break; }
                }
                if (j < ioFirstDigit || j > ioLastDigit) // Search before / after
                {
                    if (lines[searchline][j].ToString() != ".") { validPart = true; break; }
                }
                if (searchline < maxline) // Search line below
                {
                    if (lines[searchline + 1][j].ToString() != ".") { validPart = true; break; }
                }
            }
            ioFirstDigit = ioLastDigit = -1;
            if (validPart)
                sumoffoundparts += partnumber;
        }
    }

    return sumoffoundparts;
}
int FoundGears(int searchline, int maxline)
{
    int sumoffoundgears = 0;
    int GearLine1, GearLine2;
    int GearDigitPos1, GearDigitPos2;

    for (int i = 0; i < lines[searchline].Length; i++)
    {
        if (lines[searchline][i].ToString() == "*")
        {
            GearLine1 = GearLine2 = -1;
            GearDigitPos1 = GearDigitPos2 = -1;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Red;

            for (int l = Math.Max(0, searchline - 1); GearLine1 == -1 && l <= maxline; l++)
            {
                for (int j = Math.Max(0, i - 1); j <= Math.Min(i + 1, lines[l].Length - 1); j++)
                {
                    if (!(l == searchline && j == i)) // Skipp star position
                        if (char.IsDigit(lines[l][j])) { GearLine1 = l; GearDigitPos1 = j; break; }
                }
            }
            for (int l = maxline; GearLine2 == -1 && l >= Math.Max(0, searchline - 1); l--)
            {
                for (int j = Math.Min(i + 1, lines[l].Length - 1); j >= Math.Max(0, i - 1); j--)
                {
                    if (!(l == searchline && j == i)) // Skipp star position
                        if (char.IsDigit(lines[l][j])) { GearLine2 = l; GearDigitPos2 = j; break; }
                }
            }
            if (GearLine1 != -1 && GearLine2 != -1)
            {
                if (!(GearLine1 == GearLine2 && GearDigitPos2 - GearDigitPos1 <= 1)) // Ikke samme tall/siffer
                {
                    if (!(GearLine1 == GearLine2 && GearDigitPos2 - GearDigitPos1 == 2 &&
                          char.IsDigit(lines[GearLine1][GearDigitPos1+1]))) // Ikke samme tall/siffer
                    {
                        int Gear1 = int.Parse(lines[GearLine1][GearDigitPos1].ToString());
                        int Gear2 = int.Parse(lines[GearLine2][GearDigitPos2].ToString());

                        for (int j = GearDigitPos1 - 1; j >= 0 && char.IsDigit(lines[GearLine1][j]); j--) // Digits to the left?
                        {
                            Gear1 = Gear1 + (int)Math.Pow(10, GearDigitPos1 - j) * int.Parse(lines[GearLine1][j].ToString());
                        }
                        for (int j = GearDigitPos1 + 1; j < lines[GearLine1].Length && char.IsDigit(lines[GearLine1][j]); j++) // Digits to the left?
                        {
                            Gear1 = Gear1 * 10 + int.Parse(lines[GearLine1][j].ToString());
                        }

                        for (int j = GearDigitPos2 - 1; j >= 0 && char.IsDigit(lines[GearLine2][j]); j--) // Digits to the left?
                        {
                            Gear2 = Gear2 + (int)Math.Pow(10, GearDigitPos2 - j) * int.Parse(lines[GearLine2][j].ToString());
                        }
                        for (int j = GearDigitPos2 + 1; j < lines[GearLine2].Length && char.IsDigit(lines[GearLine2][j]); j++) // Digits to the left?
                        {
                            Gear2 = Gear2 * 10 + int.Parse(lines[GearLine2][j].ToString());
                        }

                        sumoffoundgears += Gear1 * Gear2;
                        Console.BackgroundColor = ConsoleColor.Green;
                    }
                }
            }
        }
        Console.Write(lines[searchline][i].ToString());
        Console.ResetColor();
    }
    Console.WriteLine();
    return sumoffoundgears;
}