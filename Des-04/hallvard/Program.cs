// See https://aka.ms/new-console-template for more information

using System.Collections.Immutable;
using System.Runtime.CompilerServices;

Console.WriteLine("Hello World on December 4th 2023!");
string inputPath = @"..\..\..\AOC2023-04-Input.txt";
using (StreamReader inputFile = new StreamReader(inputPath))
{
    int answer = 0, answer2 = 0;
    string line;

    int[] wincopies = new int[11];

    while ((line = inputFile.ReadLine()) != null)
    {
        string[] lineparts = line.Split(": ");
        int CardID = int.Parse(lineparts[0].Split(new char[0], StringSplitOptions.RemoveEmptyEntries)[1]);
        string[] sets = lineparts[1].Split(" | ");
        int[] winNumbers = sets[0].Split(new char[0], StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        int[] cardNumbers = sets[1].Split(new char[0], StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        Array.Sort(winNumbers);
        Array.Sort(cardNumbers);

        int winIndex = 0;
        int cardIndex = 0;
        int cardMatches = 0;

        while (winIndex < winNumbers.Length && cardIndex < cardNumbers.Length)
        {
            if (winNumbers[winIndex] == cardNumbers[cardIndex])
            {
                cardMatches++;
                winIndex++;
                cardIndex++;
            }
            else if (winNumbers[winIndex] < cardNumbers[cardIndex])
            {
                winIndex++;
            }
            else if (winNumbers[winIndex] > cardNumbers[cardIndex])
            {
                cardIndex++;
            }
        }
        answer2 += ++wincopies[0]; // Add the original card and number of cards to the total pile
        if (cardMatches > 0)
        {
            answer += (int)Math.Pow(2, cardMatches - 1);

            for (int i = 1; i <= cardMatches; i++) { wincopies[i] += wincopies[0]; }
        }
        // Scroll the card-copies array
        // (int i = 1; i <= winNumbers.Length; i++) { wincopies[i-1] = wincopies[i]; } -- Replaced with Array.Copy
        Array.Copy(wincopies, 1, wincopies, 0, winNumbers.Length);
        wincopies[winNumbers.Length] = 0;
    }
    Console.WriteLine("The answer to part one is: " + answer.ToString());
    Console.WriteLine("The answer to part two is: " + answer2.ToString());
    inputFile.Close();
}
Console.WriteLine("Hit any key to exit!");
Console.ReadKey();
