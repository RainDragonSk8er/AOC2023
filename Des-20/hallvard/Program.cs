// See https://aka.ms/new-console-template for more information
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Xml.Serialization;

public static class Program
{
    static Dictionary<string, Module> modules = new Dictionary<string, Module>();
    static Queue<Pulse> pulseQueue = new Queue<Pulse>();

    public static int lowcount = 0, highcount = 0;
    static int push;

    static void Main()
    {
        Stopwatch sw = Stopwatch.StartNew();
        Console.WriteLine("Hello World on December 20th 2023!");
        
        string inputPath = @"..\..\..\AOC2023-20-Input.txt";
        using (StreamReader inputFile = new StreamReader(inputPath))
        {
            string input;
            // Read modules
            while ((input = inputFile.ReadLine()) != null)
            {
                Match match = Regex.Match(input, @"^(?<type>%|&)?(?<name>\w+)\s->\s(?<destinations>.+)$");
                if (match.Success)
                {
                    Module newModule = new Module(match.Groups["type"].Value, match.Groups["name"].Value, match.Groups["destinations"].Value);
                    modules.Add(newModule.Name, newModule);
                }
            }
            inputFile.Close();
            Console.WriteLine("Done reading modules!");

            // For all modules
            foreach (Module m in modules.Values)
            {
                // And all their destinations
                foreach (string d in m.Destinations)
                {
                    // Get destination module and if exists
                    if (modules.TryGetValue(d, out Module dm))
                    {
                        // and is of type conjunction (&)
                        if (dm.Type == '&')
                        {
                            dm.InputMemory.Add(m.Name, false); // Default to low-pulse
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Couldn't find module '{d}' which is a destination of '{m.Name}' .");
                    }
                }
            }

            for (push = 1; push <= 10000; push++)
            {
                // Inital puch of button module send a low-pulse to the boradcaster module
                // Console.WriteLine($"Push number {push} on the button!");
                pulseQueue.Enqueue(new Pulse("button", "broadcaster", false));

                HandlePulseQueue(pulseQueue);

            }

            Console.WriteLine($"Counted {lowcount} low pulses and {highcount} high pulses for an answer to part one of: {lowcount * highcount}");
            Console.ReadKey();

            // Reset all modules to false and clear imputs memory
            foreach (Module m in modules.Values)
            {
                m.FlipFlopState = false;
                foreach (var key in m.InputMemory.Keys.ToList())
                    m.InputMemory[key] = false;
            }

            // Part two

            // Increased the push-count above to 5 000 and found that module 'dd' which has 'rx' as its sole destination
            // gets a high-pulse on nx after push 3851, jq after push 3911, cc after push 4001 and qp after push 4013.
            // Increased to 10 000 and found repeats after double number.
            //
            // Assuming there are no common factors in these numbers gives a least common multipla of 241823802412393
            // which was the correct answer for part 2.
        }
    }

    static void HandlePulseQueue(Queue<Pulse> pq)
    {
        while (pq.Count() > 0)
        {
            Pulse p = pq.Dequeue();
            if (modules.TryGetValue(p.Destination, out Module m))
            {
                switch (m.Type)
                {
                    case '%': // FLip-flop
                        if (!p.HiPulse) // Ignore high-pulse
                        {
                            m.FlipFlopState = !m.FlipFlopState; // Flip the state
                            foreach (string d in m.Destinations)
                            {
                                pq.Enqueue(new Pulse(m.Name, d, m.FlipFlopState));
                            }
                        }
                        break;

                    case '&': // Conjuction
                        m.InputMemory[p.Source] = p.HiPulse;
                        bool sendpulse = false; // Send low-pulse only if all input-pulses are high
                        foreach (bool hipulse in m.InputMemory.Values)
                        {
                            if (!hipulse) // If any input not high then send high-pulse
                            {
                                sendpulse = true;
                                break;
                            }
                        }
                        foreach (string d in m.Destinations)
                        {
                            pq.Enqueue(new Pulse(m.Name, d, sendpulse));
                        }
                        break;

                    case '_': // Broadcast
                        foreach (string d in m.Destinations)
                        {
                            pq.Enqueue(new Pulse(m.Name, d, p.HiPulse));
                        }
                        break;
                }
            }
            else
            {
                // Console.Write($"Destination '{p.Destination}' from module '{p.Source}' doesn't exist! Have a {(p.HiPulse ? "high" : "low")} pulse to send.\nInput memory is:");
                Module sm = modules[p.Source];
                bool foundhigh = false;
                foreach (var key in sm.InputMemory.Keys.ToList())
                    if (sm.InputMemory[key] == true)
                    {
                        Console.Write($" '{key}'={(sm.InputMemory[key] ? "high" : "low")}");
                        foundhigh = true;
                    }
                if (foundhigh)
                    Console.WriteLine($" Push count is {Program.push} and queue size is {pq.Count}.");
            }
        }
    }
}

class Pulse
{
    public string Source;
    public string Destination;
    public bool HiPulse; // false = low, true = high

    public Pulse(string source, string destination, bool hipulse)
    {
        Source = source;
        Destination = destination;
        HiPulse = hipulse;

        // Console.WriteLine($"{source} -{(hipulse ? "high" : "low")}-> {destination}");

        if (hipulse)
            Program.highcount++;
        else
            Program.lowcount++;
    }
}

class Module
{
    public char Type;
    public string Name;
    public bool FlipFlopState; // false = off, true = on
    public string[] Destinations;
    public Dictionary<string, bool> InputMemory;

    public Module(string type, string name, string destinations)
    {
        Type = (type.Length == 1) ? type[0] : '_';
        Name = name;
        FlipFlopState = false;
        Destinations = destinations.Split(", ");
        InputMemory = new Dictionary<string, bool>();
    }
}