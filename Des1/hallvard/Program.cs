using System;
using System.IO;

namespace Dec01a
{
    internal class Program
    {
        static string[] digitWords = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World on December 1st 2023!");
            string inputPath = @"..\..\..\AOC2023-01-Input.txt";
            string outputPath = @"..\..\..\AOC2023-01-Output.txt";
            using (StreamReader inputFile = new StreamReader(inputPath))
            {
                using (StreamWriter outputFile = new StreamWriter(outputPath))
                {
                    int answer = 0;
                    string line;
                    while ((line = inputFile.ReadLine()) != null)
                    {
                        int firstDigit = -1, lastDigit = -1;
                        for (int i = 0; i < line.Length && (firstDigit == -1 || lastDigit == -1); i++)
                        {
                            if (firstDigit == -1) // Still searching for first digit
                            {
                                if (char.IsDigit(line[i]))
                                {
                                    firstDigit = int.Parse(line[i].ToString());
                                }
                                else // check for digitword
                                {
                                    for (int d = 0; d <= 9; d++)
                                    {
                                        int dwlen = digitWords[d].Length;
                                        if (i + dwlen <= line.Length)
                                        {
                                            if (line.Substring(i, dwlen).Equals(digitWords[d]))
                                            {
                                                firstDigit = d;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            if (lastDigit == -1) // Still searching for last digit
                            {
                                if (char.IsDigit(line[line.Length - i - 1]))
                                {
                                    lastDigit = int.Parse(line[line.Length - i - 1].ToString());
                                }
                                else // check for digitword
                                {
                                    for (int d = 0; d <= 9; d++)
                                    {
                                        int dwlen = digitWords[d].Length;
                                        if (i + dwlen <= line.Length)
                                        {
                                            if (line.Substring(line.Length - i - dwlen, dwlen).Equals(digitWords[d]))
                                            {
                                                lastDigit = d;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (firstDigit == -1 || lastDigit == -1)
                        {
                            Console.WriteLine("Error! Found no digits in line: " + line);
                        }
                        else
                        {
                            outputFile.WriteLine(line + ";" + (firstDigit * 10 + lastDigit).ToString());
                            answer += firstDigit * 10 + lastDigit;
                        }
                    }

                    Console.WriteLine("The answer is: " + answer.ToString());
                    outputFile.WriteLine("The sum is:;" + answer.ToString());
                    outputFile.Close();
                }
                inputFile.Close();
            }
            Console.WriteLine("Hit any key to exit!");
            Console.ReadKey();
        }
    }
}
