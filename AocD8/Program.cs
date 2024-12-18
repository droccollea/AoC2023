

// Part 1
// Complete.

// Part2
// 21083806112641

using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;

string[] readText = File.ReadAllLines("./AocD8/input.txt");
// string[] readText = File.ReadAllLines("./AocD8/ex.txt");

Dictionary<string,(string,string)> map = [];

string pattern = @"^(...) = \((...), (...)\)$";

foreach (var line in readText)
{
    MatchCollection mc = Regex.Matches(line, pattern);
    if(mc.Count > 0)
    {
        map.Add(mc[0].Groups[1].Value,(mc[0].Groups[2].Value,mc[0].Groups[3].Value));
        // Console.WriteLine($"Added...");
    }
}

// foreach (var item in map.Keys)
// {
//     Console.WriteLine($"map: {item} - {map[item].Item1} {map[item].Item2}");
// }

string path = readText[0];

List<string> locations = [];

// Build the *A starting list.
foreach (var item in map.Keys)
{
    if(item.EndsWith("A"))
    {
        locations.Add(item);
    }
}

// Find each individual minimum Z
SortedList<Int64,string> lcd = [];
foreach (var loc in locations)
{
    Console.WriteLine($"Starting locations:{loc}");
    lcd.Add(stepsToZ([loc]),loc);
}

// Print the LCDs
Int64 inc = lcd.GetKeyAtIndex(lcd.Count-1);
Int64 minlcd = 0;
bool found = false;
while (!found && minlcd < 999099999999999)
{
    minlcd += inc;
    int matches = 0;
    foreach (var k in lcd.Keys)
    {
        if(minlcd%k != 0){
            continue;
        }
        else 
        {
            matches++;
        }
    }
    if (matches == lcd.Count)
    {
        found = true;
    }
}

Console.WriteLine($"Min steps is {minlcd}");

Int64 stepsToZ(List<string> nodes)
{
    Int64 steps = 0;
    int inst = 0;
    bool finished = false;
    List<string> nextSet = [];

    while ((!finished) && steps < 999999)
    {        
        if(inst>=path.Length)
        {
            // Console.WriteLine($"{inst} past end. restarting at {inst%path.Length}");
            inst = inst%path.Length;
        }
        
        nextSet.Clear();
        
        foreach (var loc in nodes)
        {
            var (l,r) = map[loc];
            nextSet.Add(path[inst] == 'L' ?  l : r); 
        }

        // Clone to locations. Assignment is by ref. 
        nodes.Clear();
        foreach (var item in nextSet)
        {
            nodes.Add(item);
        }

        finished = allEndWithZ(nodes);
        inst++;
        steps++;
    }

    foreach (var n in nodes)
    {
        Console.WriteLine($"End locations:{n}");
    }
    return steps;
}

bool allEndWithZ(List<string> list)
{
    foreach (var l in list)
    {
        // Console.WriteLine($"Now at locations:{l}");
        if(!l.EndsWith('Z'))
        {
            return false;
        }
    }
    // Console.WriteLine("All end with Z!)");
    return true;
}

