

// Part 1
// 1975710229 - too high.
// 1973284947 - high
// 1969958228 - X
// 1969958987 - summing row made a false zero. Needed to check all values.
// 717718844 - ignoring sign for diff - too low

// Part2
// 1068

using System.Text.RegularExpressions;

bool test = 
// true;
false;

string input = test? "./AocD9/ex.txt" : "./AocD9/input.txt";

string[] readText = File.ReadAllLines(input);

string pattern = @"([\-]?[\d]+)\b";

List<List<int>> histories = [];
foreach (var line in readText)
{
    MatchCollection mc = Regex.Matches(line, pattern);
    if(mc.Count > 0)
    {
        List<int> history =[];
        foreach (Match m in mc)
        {
            history.Add(int.Parse(m.Groups[1].Value));   
        }
        histories.Add(history);
    }
}

// Sanity
// int nums = 0;
// foreach (var item in histories)
// {
//     nums += item.Count();
//     // Console.WriteLine($"Elements {item.Count}");
//     // Console.WriteLine($"Next...");
//     // foreach (var h in item)
//     // {
//     //     Console.Write($"{h} ");
//     // }   
// }
// Console.WriteLine($"Total nums before {nums}");
// Console.WriteLine($"Total rows before {histories.Count}");


Int64 total = 0;

foreach (var h in histories)
{    
    // total += findNext(h);
    total += findFirst(h);   
}

Console.WriteLine($"Total: {total}");


Int64 findNext(List<int> history)
{
    // Build the histories to zero then work back up for next in original series.
    List<List<int>> thisHistory = [history];
    List<int> current = history;
    bool done = false;
    while (!done)
    {
        current = buildNextHist(current);
        thisHistory.Add(current);

        // Assume done unless a non-zero lurks.
        done = true;
        foreach (int i in current)
        {
            if(i!=0) {
                done =false; 
                break;
            }
        }
        // if (done)
        // {
        //     Console.WriteLine();
        //     foreach (var item in current)
        //     {
        //         Console.Write($"{item} ");
        //     }
        // }
    }

    // Add a zero. Now reverse.
    current.Add(0);
    int added = 0;
    for(int h = thisHistory.Count-2; h>=0; h--)
    {
        added = thisHistory[h][thisHistory[h].Count-1]
                    + thisHistory[h+1][thisHistory[h+1].Count-1];
        thisHistory[h].Add(added); 
    }

    // Final sanity check 
    // foreach (var item in thisHistory)
    // {
    //     // Console.WriteLine($"Elements {item.Count}");
    //     foreach (var h in item)
    //     {
    //         Console.Write($" {h}");
    //     }   
    //     Console.Write("\n");

    // }

    // Return last element.
    return added;
}

Int64 findFirst(List<int> history)
{
    // Build the histories to zero then work back up for next in original series.
    List<List<int>> thisHistory = [history];
    List<int> current = history;
    bool done = false;
    while (!done)
    {
        current = buildNextHist(current);
        thisHistory.Add(current);

        // Assume done unless a non-zero lurks.
        done = true;
        foreach (int i in current)
        {
            if(i!=0) {
                done =false; 
                break;
            }
        }
    }

    // Insert a zero. Now reverse.
    current.Insert(0,0);
    int added = 0;
    for(int h = thisHistory.Count-2; h>=0; h--)
    {
        added = thisHistory[h][0] - thisHistory[h+1][0];
        thisHistory[h].Insert(0,added); 
    }

    // Final sanity check 
    // foreach (var item in thisHistory)
    // {
    //     // Console.WriteLine($"Elements {item.Count}");
    //     foreach (var h in item)
    //     {
    //         Console.Write($" {h}");
    //     }   
    //     Console.Write("\n");
    // }

    // Return last element.
    return added;
}

List<int> buildNextHist(List<int> prev)
{
    List<int> next=[];
    for (int i = 0; i < prev.Count-1; i++)
    {
        // int diff = (prev[i]>prev[i+1]) ? prev[i]-prev[i+1] : prev[i+1]-prev[i];
        int diff = prev[i+1]-prev[i];
        next.Add(diff);
    }
    return next;
}