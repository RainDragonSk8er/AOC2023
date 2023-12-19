// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        List<Instruction> instructions = new List<Instruction>();
        
        Stopwatch sw = Stopwatch.StartNew();
        Console.WriteLine("Hello World on December 18th 2023!");

        string inputPath = @"..\..\..\AOC2023-18-Input.txt";
        using (StreamReader inputFile = new StreamReader(inputPath))
        {
            // Read input and build contraption map
            string input;
            int xpos = 0, ypos = 0;
            int totallength = 0;
            int dx = 0, dy = 0;
            while ((input = inputFile.ReadLine()) != null)
            {
                Match match = Regex.Match(input, @"^(.) (\d+) \(#([\da-fA-F]{5})([\da-fA-F])\)$");
                if (match.Success)
                {
                    int length = int.Parse(match.Groups[3].Value, System.Globalization.NumberStyles.HexNumber);
                    int direction = int.Parse(match.Groups[4].Value, System.Globalization.NumberStyles.HexNumber);

                    Console.Write($"D:{direction} ");
                    Console.WriteLine($"L:{length} ");

                    instructions.Add(new Instruction(xpos, ypos, direction, length));

                    switch (direction)
                    {
                        case 0: // Right / East
                            xpos += length; break;
                        case 1: // Down / South
                            ypos += length; break;
                        case 2: // Left / West
                            xpos -= length; break;
                        case 3: // Up / North
                            ypos -= length; break;
                    }
                    totallength += length;
                }
                else
                {
                    Console.WriteLine("Unable to parse '{0}'.", input);
                    break;
                }
            }

            int count = instructions.Count();
            Console.WriteLine($"Count is {count} and totallength is {totallength}.");

            Int64 runningarea = 0;
            for (int i = 0; i < count; i++)
            {
                runningarea  += (Int64)instructions[i].x * (Int64)(instructions[(i + 1) % count].y - instructions[(i - 1 + count) % count].y);
            }
            UInt64 answer2 = ((UInt64)((runningarea > 0) ? runningarea : -runningarea) + (UInt64)totallength) / 2 + 1;

            Console.WriteLine("The answer to part two is: {0}", answer2);
            sw.Stop();
            Console.WriteLine("Time elapsed: {0}\n", sw.Elapsed);

            Console.WriteLine("Hit any key to exit!");
            Console.ReadKey();
        }
    }
}

class Instruction
{
    public int x;
    public int y;
    public int direction;
    public int length;
    
    public Instruction(int x, int y, int direction, int length)
    {
        this.x = x;
        this.y = y;
        this.length = length;
        this.direction = direction;
    }
}