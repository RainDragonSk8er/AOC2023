using System;
using System.IO;

namespace Dec02
{
    internal class CubeSet
    {
        public int red = 0;
        public int green = 0;
        public int blue = 0;
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World on December 2nd 2023!");
            string inputPath = @"..\..\..\AOC2023-02-Input.txt";
            using (StreamReader inputFile = new StreamReader(inputPath))
            {
                int answer = 0, answer2 = 0;
                bool impossible;
                string line;
                while ((line = inputFile.ReadLine()) != null)
                {
                    impossible = false;
                    CubeSet minCubeSet = new CubeSet();
                    string[] lineparts = line.Split(": ");
                    int GameID = int.Parse(lineparts[0].Split(" ")[1]);
                    string[] sets = lineparts[1].Split("; ");
                    foreach (string set in sets)
                    {
                        string[] cubes = set.Split(", ");
                        CubeSet cubeSet= new CubeSet();
                        foreach (string cube in cubes)
                        {
                            string[] cubeattrib = cube.Split(" ");
                            switch (cubeattrib[1])
                            {
                                case "red":
                                    cubeSet.red = int.Parse(cubeattrib[0]);
                                    break;

                                case "green":
                                    cubeSet.green = int.Parse(cubeattrib[0]);
                                    break;

                                case "blue":
                                    cubeSet.blue = int.Parse(cubeattrib[0]);
                                    break;

                                default:
                                    Console.WriteLine("Unknown cube color {0]", cubeattrib[1]);
                                    break;
                            }
                        }
                        // PART ONE - CHECK IF OBSERVATION IS POSSIBLE WITH CUBE SET OF 12R, 13G and 14B
                        if (cubeSet.red > 12 || cubeSet.green > 13 || cubeSet.blue > 14)
                        {
                            impossible = true;
                            // break; MUST RUN THROUGH ALL FOR PART TWO
                        }

                        // PART TWO - UPDATE MINIMUM REQUIRED CUBE SET
                        if (cubeSet.red > minCubeSet.red) minCubeSet.red = cubeSet.red;
                        if (cubeSet.green > minCubeSet.green) minCubeSet.green = cubeSet.green;
                        if (cubeSet.blue > minCubeSet.blue) minCubeSet.blue = cubeSet.blue;

                    }
                    if (!impossible)
                    {
                        answer += GameID;
                    }
                    answer2 += minCubeSet.red * minCubeSet.green * minCubeSet.blue;
                }
                Console.WriteLine("The answer to part one is: " + answer.ToString());
                Console.WriteLine("The answer to part two is: " + answer2.ToString());
                inputFile.Close();
            }
            Console.WriteLine("Hit any key to exit!");
            Console.ReadKey();
        }
    }
}
