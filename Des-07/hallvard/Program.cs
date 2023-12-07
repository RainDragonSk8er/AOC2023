// See https://aka.ms/new-console-template for more information

using System.Collections.Immutable;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

char[] sortorder = { 'A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2' };
char[] sortorder2 = { 'A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J' };
// hand types: 1. Five of a kind, 2. Four of a kind, 3. Full house, 4. Three of a kind, 5. Two pair, 6. One pair and 7. High card

Console.WriteLine("Hello World on December 7th 2023!");
string inputPath = @"..\..\..\AOC2023-07-Input.txt";
using (StreamReader inputFile = new StreamReader(inputPath))
{
    int answer = 0, answer2 = 0;
    List<Hand> hands = new List<Hand>();
    string line;

    while ((line = inputFile.ReadLine()) != null)
    {
        string[] splitline = line.Split(" ");
        hands.Add(new Hand { original = splitline[0].ToCharArray(), sorted = splitline[0].ToCharArray(),
                                sorted2 = splitline[0].ToCharArray(), bid = int.Parse(splitline[1]) });
        
        // Sort the hands
        Array.Sort(hands.Last().sorted, (x, y) => Array.IndexOf(sortorder, x) - Array.IndexOf(sortorder, y));
        Array.Sort(hands.Last().sorted2, (x, y) => Array.IndexOf(sortorder2, x) - Array.IndexOf(sortorder2, y));

        // Calculate the 'of a kind' numbers
        int[] ofakind = { 1, 1 };
        int[] ofakind2 = { 1, 1 };
        int ofakindindex = 0;
        int ofakindindex2 = 0;
        char[] sortedhand = hands.Last().sorted;
        char[] sortedhand2 = hands.Last().sorted;
        for (int i = 0; i < 4; i++)
        {
            if (sortedhand[i] == sortedhand[i + 1])
                ofakind[ofakindindex]++;
            else if (ofakind[ofakindindex] > 1)
                ofakindindex++;

            if (sortedhand2[i] == sortedhand2[i + 1] && sortedhand2[i] != 'J') // Ignore jokers
                ofakind2[ofakindindex2]++;
            else if (ofakind2[ofakindindex2] > 1)
                ofakindindex2++;
        }
        Array.Sort(ofakind);
        hands.Last().score = ofakind[1] * 10 + ofakind[0];

        if (ofakind2[0] == 5)
            ofakind2[1] = 0;

        Array.Sort(ofakind2);

        // Joker-logic
        int jokers = hands.Last().sorted2.Count(c => c == 'J');
        if (jokers > 0)
        {
            ofakind2[1] += jokers;

            if (ofakind2[1] > 5)
                ofakind2[1] = 5;

            if (ofakind2[0] + ofakind2[1] > 5)
                ofakind2[0] = 5 - (ofakind2[1]);
        }
        hands.Last().score2 = ofakind2[1] * 10 + ofakind2[0];

        Console.WriteLine("Hand: {0} -> {1} -> {2} with bid {3} has 'of a kind' score of {4} and {5}", new string(hands.Last().original),
                            new string(hands.Last().sorted), new string(hands.Last().sorted2),
                            hands.Last().bid, hands.Last().score, hands.Last().score2);
    }

    // Part 1
    hands.Sort((x, y) =>
    {
        // Primary sort by descending by score
        int cmp = y.score - x.score;
        if (cmp != 0)
        {
            return cmp;
        }

        // Secondary sort by original (card by card)
        for (int i = 0; i <= 4; i++)
        {
            cmp = Array.IndexOf(sortorder, x.original[i]) - Array.IndexOf(sortorder, y.original[i]);
            if (cmp != 0)
            {
                return cmp;
            }
        }
        return 0;
    });

    Console.WriteLine("\nSorted by score:");
    int rank = hands.Count;
    foreach (Hand hand in hands)
    {
        answer += rank-- * hand.bid;
        Console.WriteLine("Rank {4}: Hand: {0} -> {1} with bid {2} has 'of a kind' score of {3}", new string(hand.original),
                            new string(hand.sorted), hand.bid, hand.score, rank);
    }

    // Part 2
    hands.Sort((x, y) =>
    {
        // Primary sort by descending by score
        int cmp = y.score2 - x.score2;
        if (cmp != 0)
        {
            return cmp;
        }

        // Secondary sort by original (card by card) with sortorder2
        for (int i = 0; i <= 4; i++)
        {
            cmp = Array.IndexOf(sortorder2, x.original[i]) - Array.IndexOf(sortorder2, y.original[i]);
            if (cmp != 0)
            {
                return cmp;
            }
        }
        return 0;
    });

    Console.WriteLine("\nSorted by score2:");
    rank = hands.Count;
    foreach (Hand hand in hands)
    {
        answer2 += rank-- * hand.bid;
        Console.WriteLine("Rank {4}: Hand: {0} -> {1} with bid {2} has 'of a kind' score of {3}", new string(hand.original),
                            new string(hand.sorted2), hand.bid, hand.score2, rank);
    }

    Console.WriteLine("The answer to part one is: {0}", answer);
    Console.WriteLine("The answer to part two is: {0}", answer2);
    inputFile.Close();
}
Console.WriteLine("Hit any key to exit!");
Console.ReadKey();

public class Hand
{
    public char[] original = new char[5];
    public char[] sorted = new char[5];
    public char[] sorted2 = new char[5];
    public int bid;
    public int score;
    public int score2;
}