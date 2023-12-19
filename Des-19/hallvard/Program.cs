// See https://aka.ms/new-console-template for more information
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Xml.Serialization;

class Program
{
    static Dictionary<string, Workflow> workflows = new Dictionary<string, Workflow>();
    static List<Part> acceptedParts = new List<Part>();
    static Stack<PartRange> prStack = new Stack<PartRange>();
    static Stack<PartRange> acceptedPartRanges = new Stack<PartRange>();

    static void Main()
    {
        Stopwatch sw = Stopwatch.StartNew();
        Console.WriteLine("Hello World on December 19th 2023!");
        
        List<Part> parts = new List<Part>();

        string inputPath = @"..\..\..\AOC2023-19-Input.txt";
        using (StreamReader inputFile = new StreamReader(inputPath))
        {
            string input;
            // Read workflows
            while ((input = inputFile.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(input)) // Blank line => Move to next section
                    break;

                Match match = Regex.Match(input, @"^(?<name>[^\{]+)\{(?<rules>[^\}]+)\}$");
                if (match.Success)
                {
                    Workflow wf = new Workflow(match.Groups["name"].Value);
                    workflows.Add(wf.Name, wf);

                    string[] rules = match.Groups["rules"].Value.Split(',');

                    for (int i = 0; i < rules.Length - 1; i++)
                    {
                        Match match2 = Regex.Match(rules[i], @"^(.)([<>])(\d+)\:(.+)$");
                        if (match2.Success)
                        {
                            Rule r = new Rule(match2.Groups[1].Value[0], match2.Groups[2].Value[0],
                                int.Parse(match2.Groups[3].Value), match2.Groups[4].Value);
                            wf.Rules.Add(r);
                        }
                    }
                    wf.Finally = rules[rules.Length - 1];
                }
            }

            // Read Parts
            while ((input = inputFile.ReadLine()) != null)
            {
                Match match = Regex.Match(input, @"^\{x=(\d+),m=(\d+),a=(\d+),s=(\d+)\}$");
                if (match.Success)
                {
                    Part p = new Part(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value));
                    parts.Add(p);
                }
            }

            // Check parts against workflows
            foreach (Part p in parts)
            {
                Console.Write($" - {{x={p.x},m={p.m},a={p.a},s={p.s}}}: in");
                CheckPart("in", p);
            }                

            // Calculate answer as sum of acceptes parts parameters
            int answer = 0;
            foreach (Part p in acceptedParts)
                answer += p.x + p.m + p.a + p.s;

            Console.WriteLine($"The answer to part one is: {answer}");

            // Part 2
            prStack.Push(new PartRange(1, 4000));
            FindAcceptedPartRanges("in");
            UInt64 answer2 = 0;
            while (acceptedPartRanges.Count > 0)
            {
                PartRange cpr = acceptedPartRanges.Pop();
                // Add new range
                answer2 += ((UInt64)(cpr.xmax - cpr.xmin + 1)) * ((UInt64)(cpr.mmax - cpr.mmin + 1)) *
                            ((UInt64)(cpr.amax - cpr.amin + 1)) * ((UInt64)(cpr.smax - cpr.smin + 1));
                
                // Subtract all intersections with the rest
                foreach (PartRange opr in acceptedPartRanges)
                {
                    int xint = Math.Min(cpr.xmax, opr.xmax) - Math.Max(cpr.xmin, opr.xmin);
                    int mint = Math.Min(cpr.mmax, opr.mmax) - Math.Max(cpr.mmin, opr.mmin);
                    int aint = Math.Min(cpr.amax, opr.amax) - Math.Max(cpr.amin, opr.amin);
                    int sint = Math.Min(cpr.smax, opr.smax) - Math.Max(cpr.smin, opr.smin);

                    if (xint > 0 && mint > 0 && aint > 0 && sint > 0)
                    {
                        answer2 -= ((UInt64)xint) * ((UInt64)mint) * ((UInt64)aint) * ((UInt64)sint);
                    }                     
                }
            }

            Console.WriteLine("The answer to part two is: {0}", answer2);
            sw.Stop();
            Console.WriteLine("Time elapsed: {0}\n", sw.Elapsed);

            Console.WriteLine("Hit any key to exit!");
            Console.ReadKey();
            inputFile.Close();
        }
    }

    static void CheckPart(string wfname, Part part)
    {
        Workflow wf;
        if (!workflows.TryGetValue(wfname, out wf))
        {
            Console.WriteLine($"Failed to find the {wfname} workflow.");
            return;
        }

        int partparameter = 0;
        foreach (Rule r in wf.Rules)
        {
            switch (r.Category)
            {
                case 'x':
                    partparameter = part.x; break;

                case 'm':
                    partparameter = part.m; break;

                case 'a':
                    partparameter = part.a; break;

                case 's':
                    partparameter = part.s; break;
            }

            if ((partparameter - r.Parameter) * ((r.Operation == '>') ? 1 : -1) > 0)
            {
                if (r.Action == "A")
                {
                    acceptedParts.Add(part);
                    Console.WriteLine(" -> A");
                    return;
                }

                if (r.Action == "R")
                {
                    Console.WriteLine(" -> R");
                    return;
                }

                Console.Write($" -> {r.Action}");
                CheckPart(r.Action, part);
                return;
            }
        }

        if (wf.Finally == "A")
        {
            acceptedParts.Add(part);
            Console.WriteLine(" -> A");
            return;
        }

        if (wf.Finally == "R")
        {
            Console.WriteLine(" -> R");
            return;
        }

        Console.Write($" -> {wf.Finally}");
        CheckPart(wf.Finally, part);
    }

    static void FindAcceptedPartRanges(string wfname)
    {
        int falsepushcount = 0;
        Workflow wf;
        if (!workflows.TryGetValue(wfname, out wf))
        {
            Console.WriteLine($"Failed to find the {wfname} workflow.");
            return;
        }

        foreach (Rule r in wf.Rules)
        {
            PartRange trueRange = new PartRange(prStack.Peek());
            PartRange falseRange = new PartRange(prStack.Peek());

            switch (r.Category)
            {
                case 'x':
                    if (r.Operation == '>' && r.Parameter > trueRange.xmin )
                        trueRange.xmin = r.Parameter + 1;
                    else if (r.Operation == '<' && r.Parameter < trueRange.xmax)
                        trueRange.xmax = r.Parameter - 1;
                    if (r.Operation == '>' && r.Parameter < falseRange.xmax)
                        falseRange.xmax = r.Parameter;
                    else if (r.Operation == '<' && r.Parameter > falseRange.xmin)
                        falseRange.xmin = r.Parameter;
                    break;

                case 'm':
                    if (r.Operation == '>' && r.Parameter > trueRange.mmin)
                        trueRange.mmin = r.Parameter + 1;
                    else if (r.Operation == '<' && r.Parameter < trueRange.mmax)
                        trueRange.mmax = r.Parameter - 1;
                    if (r.Operation == '>' && r.Parameter < falseRange.mmax)
                        falseRange.mmax = r.Parameter;
                    else if (r.Operation == '<' && r.Parameter > falseRange.mmin)
                        falseRange.mmin = r.Parameter;
                    break;

                case 'a':
                    if (r.Operation == '>' && r.Parameter > trueRange.amin)
                        trueRange.amin = r.Parameter + 1;
                    else if (r.Operation == '<' && r.Parameter < trueRange.amax)
                        trueRange.amax = r.Parameter - 1;
                    if (r.Operation == '>' && r.Parameter < falseRange.amax)
                        falseRange.amax = r.Parameter;
                    else if (r.Operation == '<' && r.Parameter > falseRange.amin)
                        falseRange.amin = r.Parameter;
                    break;

                case 's':
                    if (r.Operation == '>' && r.Parameter > trueRange.smin)
                        trueRange.smin = r.Parameter + 1;
                    else if (r.Operation == '<' && r.Parameter < trueRange.smax)
                        trueRange.smax = r.Parameter - 1;
                    if (r.Operation == '>' && r.Parameter < falseRange.smax)
                        falseRange.smax = r.Parameter;
                    else if (r.Operation == '<' && r.Parameter > falseRange.smin)
                        falseRange.smin = r.Parameter;
                    break;
            }

            if (!trueRange.IsEmpty())
            {
                prStack.Push(trueRange);

                if (r.Action == "A") // Accepted
                {
                    // Calculate accepted ranges and add to total range set
                    PartRange pr = prStack.Peek();
                    acceptedPartRanges.Push(new PartRange(pr));
                    Console.WriteLine($"Accepted part ranges x=[{pr.xmin}, {pr.xmax}] m=[{pr.mmin}, {pr.mmax}] a=[{pr.amin}, {pr.amax}] s=[{pr.smin}, {pr.smax}]");
                }
                else if (r.Action != "R") // Not rejected => New worflow
                {
                    // Recurse new workflow
                    FindAcceptedPartRanges(r.Action);
                }
                prStack.Pop();
            }

            if (!falseRange.IsEmpty())
            {
                prStack.Push(falseRange);
                falsepushcount++;
            }
        }
        if (wf.Finally == "A") // Accepted
        {
            // Calculate accepted ranges and add to total range set
            PartRange pr = prStack.Peek();
            acceptedPartRanges.Push(new PartRange(pr));
            Console.WriteLine($"Accepted part ranges x=[{pr.xmin}, {pr.xmax}] m=[{pr.mmin}, {pr.mmax}] a=[{pr.amin}, {pr.amax}] s=[{pr.smin}, {pr.smax}]");
        }
        else if (wf.Finally != "R") // Not rejected => New worflow
        {
            // Recurse new workflow
            FindAcceptedPartRanges(wf.Finally);
        }
        // Pop off falseRanges
        while (falsepushcount-- > 0)
            prStack.Pop();
    }
}

class Workflow
{
    public string Name;
    public List<Rule> Rules;
    public string Finally;

    public Workflow(string name)
    {
        Name = name;
        Rules = new List<Rule>();
    }
}

class Rule
{
    public char Category;
    public char Operation;
    public int Parameter;
    public string Action;

    public Rule(char category, char operation, int parameter, string action)
    {
        Category = category;
        Operation = operation;
        Parameter = parameter;
        Action = action;
    }
}

class Part
{
    public int x;
    public int m;
    public int a;
    public int s;

    public Part(int x, int m, int a, int s)
    {
        this.x = x;
        this.m = m;
        this.a = a;
        this.s = s;
    }
}

class PartRange
{
    public int xmin, xmax;
    public int mmin, mmax;
    public int amin, amax;
    public int smin, smax;

    public PartRange(PartRange pr)
    {
        this.xmin = pr.xmin; this.xmax = pr.xmax;
        this.mmin = pr.mmin; this.mmax = pr.mmax;
        this.amin = pr.amin; this.amax = pr.amax;
        this.smin = pr.smin; this.smax = pr.smax;
    }

    public PartRange(int min, int max)
    {
        this.xmin = min; this.xmax = max;
        this.mmin = min; this.mmax = max;
        this.amin = min; this.amax = max;
        this.smin = min; this.smax = max;
    }

    public PartRange(int xmin, int xmax, int mmin, int mmax, int amin, int amax, int smin, int smax)
    {
        this.xmin = xmin; this.xmax = xmax;
        this.mmin = mmin; this.mmax = mmax;
        this.amin = amin; this.amax = amax;
        this.smin = smin; this.smax = smax;
    }

    public bool IsEmpty()
    {
        return ((xmin > xmax) || (mmin > mmax) || (amin > amax) || (smin > smax));
    }
}
