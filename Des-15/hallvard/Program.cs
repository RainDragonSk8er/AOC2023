// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Immutable;
using System.Net.NetworkInformation;

class Program
{
    public static UInt64 arrangements;

    static void Main()
    {
        Console.WriteLine("Hello World on December 15th 2023!");

        List<Lens>[] boxes = new List<Lens>[256];

        // Creating list objects for each element in the array
        for (int i = 0; i < 256; i++)
        {
            boxes[i] = new List<Lens>();
        }

        string inputPath = @"..\..\..\AOC2023-15-Input.txt";
        using (StreamReader inputFile = new StreamReader(inputPath))
        {
            string line;
            List<string> rows = new List<string>();
            int answer = 0, answer2 = 0;

            // Read input and expand lines (y-axis)
            while ((line = inputFile.ReadLine()) != null)
            {
                int pos = 0, hash = 0;
                while (pos < line.Length)
                {
                    if (line[pos] == ',')
                    {
                        answer += hash;
                        hash = 0;
                    }
                    else if (line[pos] != '\n')
                    {
                        hash += (int)line[pos];
                        hash *= 17;
                        hash %= 256;
                    }
                    pos++;
                }
                answer += hash;
            }
            Console.WriteLine("The answer to part one is: {0}", answer);

            inputFile.BaseStream.Seek(0, SeekOrigin.Begin);
            inputFile.DiscardBufferedData();
            while ((line = inputFile.ReadLine()) != null)
            {
                int pos = 0, hash = 0, start = 0;
                while (pos < line.Length)
                {
                    if (line[pos] == '=')
                    {
                        string label = line.Substring(start, pos - start);
                        pos++;
                        bool replaced = false;
                        for (int i = 0; i < boxes[hash].Count(); i++)
                        {
                            if (boxes[hash][i].Label.Equals(label))
                            {
                                boxes[hash][i].FocalLength = line[pos] - '0';
                                replaced = true;
                                break;
                            }
                        }
                        if (!replaced)
                        {
                            boxes[hash].Add(new Lens(label, line[pos] - '0'));
                        }                        
                    }
                    else if (line[pos] == '-')
                    {
                        string label = line.Substring(start, pos - start);
                        for (int i = 0; i < boxes[hash].Count(); i++)
                        {
                            if (boxes[hash][i].Label.Equals(label))
                                boxes[hash].RemoveAt(i);
                        }
                    }
                    else if (line[pos] == ',')
                    {
                        hash = 0;
                        start = pos + 1;
                    }
                    else if (line[pos] != '\n')
                    {
                        hash += (int)line[pos];
                        hash *= 17;
                        hash %= 256;
                    }
                    pos++;
                }
            }
            for (int i = 0; i < boxes.Length; i++)
            {
                int slot = 1;
                foreach (Lens lens in boxes[i])
                {
                    Console.WriteLine("Box + 1: {0} * slot {1} * focal length {2}", i + 1, slot, lens.FocalLength);
                    answer2 += (i + 1) * slot * lens.FocalLength;
                    slot++;
                }
            }
            Console.WriteLine("The answer to part two is: {0}", answer2);
            Console.WriteLine("Hit any key to exit!");
            Console.ReadKey();
        }
    }
}
class Lens
{
    public string Label { get; }
    public int FocalLength { get; set; }

    public Lens(string label, int focallength)
    {
        Label = label;
        FocalLength = focallength;
    }
}