

// Part 1
// 6786 - first time
// Part2
// 487 too low. Try +24 unknowns??
// 511 too high. Some unknowns can be classified as L or R...
// 495 - correct missed corners!

using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Transactions;

bool test =
// true;
false;

string input = test ? "./AocD10/ex.txt" : "./AocD10/input.txt";

string[] readText = File.ReadAllLines(input);
int maxX = readText[0].Length - 1;
int maxY = readText.Length - 1;
List<(int, int)> visited = [];

// Find start.
(int, int) start = (0, 0);
for (int x = 0; x < maxX; x++)
{
    for (int y = 0; y < maxY; y++)
    {
        if (readText[y][x] == 'S')
        {
            start = (x, y);
        }
    }
}

Console.WriteLine($"Starting at {start}");

List<(int, int)> directions = [(start.Item1, start.Item2 + 1), (start.Item1 + 1, start.Item2), (start.Item1, start.Item2 - 1), (start.Item1 - 1, start.Item2)];

(int, int) first = start;
foreach (var d in directions)
{
    if (isValidNext(start, d))
    {
        first = d;
        break;  // Only need first valid next step (2 out of 4). 
    }
}
Int64 steps = followPath(start, first);
Console.WriteLine($"From {first} took {steps} steps");
// Part 1
// Split sucessful length as furthest point.
// Console.WriteLine($"Farthest is therefore {steps/2}");

// Part2
// Iterate every cell and in not in visited, follow N,S,E,W and ensure it has evnetually a visited on all four sides.
// Draw loop in X's and .'s then count manually?
// Area of irregular shape. Take each corner and get area (average height x width). Subtract the visited cells.
// Solution was to follow path and add left/right depending on the direction from prev to curr/next.
// Also needed to add the L/R of the previous point where a change in direction meant a point missed. 

// Pointless!
List<(int, int)> points = [];
foreach (var item in visited)
{
    // List the corners. Include S but might need to work out its char?
    if ("S7FLJ".Contains(readText[item.Item2][item.Item1]))
    {
        points.Add(item);
    }
}

// Didnt work, way too high.
int area = 0;
for (int i = 0; i < points.Count; i++)
{
    (int, int) from = points[i];
    (int, int) to;

    // Back to start if at end.
    if (i + 1 == points.Count)
    {
        to = points[0];
    }
    else
    {
        to = points[i + 1];
    }

    // Area equals avg height x width.
    int h = (from.Item2 + to.Item2) / 2;
    int w = to.Item1 - from.Item1;
    area += h * w;
}

Console.WriteLine($"Area is: {area}");
Console.WriteLine($"Loop length is: {visited.Count}");

List<(int, int)> left = [];
List<(int, int)> right = [];

buildLandRLists();

// Check overlaps of L in R - none in either
foreach (var l in left)
{
    if (right.Contains(l))
    {
        Console.WriteLine($"L {l} in right list");
    }
    if (visited.Contains(l))
    {
        Console.WriteLine($"L {l} in visited list");
    }
}
foreach (var r in right)
{
    if (left.Contains(r))
    {
        Console.WriteLine($"R {r} in left list");
    }
}

Console.WriteLine($"Left count before neighbours added {left.Count}");
Console.WriteLine($"Right count before neighbours added {right.Count}");

// Iterate through L and unvisited until no changes.

finishLandRLists();

Console.WriteLine($"Left count {left.Count}");
Console.WriteLine($"Right count {right.Count}");


// Sanity map...
for (int y = 0; y < readText.Length; y++)
{
    for (int x = 0; x < readText[y].Length; x++)
    {
        if (visited.Contains((x, y)))
        {
            Console.Write(readText[y][x]);
        }
        else if (left.Contains((x, y)))
        {
            Console.Write('I');
        }
        else if (right.Contains((x, y)))
        {
            Console.Write('O');
        }
        else
        {
            Console.Write('.');
        }
    }
    Console.WriteLine();

}

// Create a L and R list following the route. 
// Add neighbours to the list until no more found. Count them.
void buildLandRLists()
{
    (int, int) curr = visited.Last();
    foreach (var next in visited)
    {
        // Moving N
        if (next.Item2 < curr.Item2)
        {
            (int, int) east = (next.Item1 + 1, next.Item2);
            (int, int) west = (next.Item1 - 1, next.Item2);
            if ((!visited.Contains(east)) && inbounds(east) && !right.Contains(east)) right.Add(east);
            if ((!visited.Contains(west)) && inbounds(west) && !left.Contains(west))
            {
                left.Add(west);
                if (visited.Contains(west))
                {
                    Console.WriteLine("Added l and its in visited.");
                }
            }
            // Change dir apply to curr too.
            east = (curr.Item1 + 1, curr.Item2);
            west = (curr.Item1 - 1, curr.Item2);
            if ((!visited.Contains(east)) && inbounds(east) && !right.Contains(east)) right.Add(east);
            if ((!visited.Contains(west)) && inbounds(west) && !left.Contains(west))
            {
                left.Add(west);
                if (visited.Contains(west))
                {
                    Console.WriteLine("Added l and its in visited.");
                }
            }
        }
        // Moving E
        else if (next.Item1 > curr.Item1)
        {
            (int, int) north = (next.Item1, next.Item2 - 1);
            (int, int) south = (next.Item1, next.Item2 + 1);
            if ((!visited.Contains(north)) && inbounds(north) && !left.Contains(north))
            {
                left.Add(north);
            }
            if (!visited.Contains(south) && inbounds(south) && !right.Contains(south)) right.Add(south);

            north = (curr.Item1, curr.Item2 - 1);
            south = (curr.Item1, curr.Item2 + 1);
            if ((!visited.Contains(north)) && inbounds(north) && !left.Contains(north))
            { 
                left.Add(north);
            }
            if (!visited.Contains(south) && inbounds(south) && !right.Contains(south)) right.Add(south);
        }
        // Moving S
        else if (next.Item2 > curr.Item2)
        {
            (int, int) west = (next.Item1 - 1, next.Item2);
            (int, int) east = (next.Item1 + 1, next.Item2);
            if ((!visited.Contains(east)) && inbounds(east) && !left.Contains(east)) left.Add(east);
            if (!visited.Contains(west) && inbounds(west) && !right.Contains(west)) right.Add(west);

            west = (curr.Item1 - 1, curr.Item2);
            east = (curr.Item1 + 1, curr.Item2);
            if ((!visited.Contains(east)) && inbounds(east) && !left.Contains(east)) left.Add(east);
            if (!visited.Contains(west) && inbounds(west) && !right.Contains(west)) right.Add(west);
        }
        // Moving W
        else if (next.Item1 < curr.Item1)
        {
            (int, int) north = (next.Item1, next.Item2 - 1);
            (int, int) south = (next.Item1, next.Item2 + 1);
            if (!visited.Contains(north) && inbounds(north) && !right.Contains(north)) right.Add(north);
            if (!visited.Contains(south) && inbounds(south) && !left.Contains(south)) left.Add(south);

            north = (curr.Item1, curr.Item2 - 1);
            south = (curr.Item1, curr.Item2 + 1);
            if (!visited.Contains(north) && inbounds(north) && !right.Contains(north)) right.Add(north);
            if (!visited.Contains(south) && inbounds(south) && !left.Contains(south)) left.Add(south);
        }
        curr = next;
    }
}

void finishLandRLists()
{
    // Everything is either in or out. 
    // For anything not in a list - loop, L or R, add to list until done.
    List<(int, int)> unknowns = [];
    for (int y = 0; y < readText.Length; y++)
    {
        for (int x = 0; x < readText[y].Length; x++)
        {
            if (!visited.Contains((x, y)) && !right.Contains((x, y)) && !left.Contains((x, y)))
            {
                unknowns.Add((x, y));
            }
        }
    }

    bool done = false;
    while (!done)
    {
        List<(int, int)> remaining = [];
        int toFind = unknowns.Count;
        Console.WriteLine($"Still to find: {toFind}");

        foreach (var item in unknowns)
        {
            // N
            (int, int) n = (item.Item1, item.Item2 - 1);
            if (left.Contains(n))
            {
                left.Add(item);
                continue;
            }
            else if (right.Contains(n))
            {
                right.Add(item);
                continue;
            }
            // E
            (int, int) e = (item.Item1 + 1, item.Item2);
            if (left.Contains(e))
            {
                left.Add(item);
                continue;
            }
            else if (right.Contains(e))
            {
                right.Add(item);
                continue;
            }
            // S
            (int, int) s = (item.Item1, item.Item2 + 1);
            if (left.Contains(s))
            {
                left.Add(item);
                continue;
            }
            else if (right.Contains(s))
            {
                right.Add(item);
                continue;
            }
            // W
            (int, int) w = (item.Item1 - 1, item.Item2);
            if (left.Contains(w))
            {
                left.Add(item);
                continue;
            }
            else if (right.Contains(w))
            {
                right.Add(item);
                continue;
            }
            remaining.Add(item);
        }

        // Prep next iteration.
        unknowns = remaining;

        // As fas as can go. Done.
        if (unknowns.Count == 0 || toFind == remaining.Count)
        {
            done = true;
        }
    }
    Console.WriteLine($"Unknowns not found: {unknowns.Count}");
}

// Determine directions S can go next with the 4 next points.
bool isValidNext((int, int) curr, (int, int) next)
{
    // if next is over an edge, invalid.
    if (!inbounds(next))
    {
        return false;
    }

    // if next cant get reached from curr, invalid.
    char from = readText[curr.Item2][curr.Item1];
    char to = readText[next.Item2][next.Item1];

    // Done!
    if (to == 'S')
    {
        return true;
    }

    if (to == '.')
    {
        return false;
    }

    // Moving N
    if (next.Item2 < curr.Item2)
    {
        // Valid is 7,|, F
        if ("7|F".IndexOf(to) == -1)
        {
            return false;
        }
    }
    // Moving E
    else if (next.Item1 > curr.Item1)
    {
        // Valid is 7,-, J
        if ("J-7".IndexOf(to) == -1)
        {
            return false;
        }
    }
    // Moving S
    else if (next.Item2 > curr.Item2)
    {
        // Valid is J,|, L
        if ("J|L".IndexOf(to) == -1)
        {
            return false;
        }
    }
    // Moving W
    else if (next.Item1 < curr.Item1)
    {
        // Valid is L,-, F
        if ("L-F".IndexOf(to) == -1)
        {
            return false;
        }
    }

    return true;
}

// Recursion solution? Pass the list and visit each node appending to the list until end case.
// Recusion blows stack (unless buggy?)
// Return the path length at end. For non-loops, return max.
Int64 followPath((int, int) curr, (int, int) next)
{
    // Console.WriteLine($"Path length: {visited.Count}");

    bool done = false;
    while (!done)
    {
        // If dead-end or in a loop but not back to start bail here with a max. 
        if (!isValidNext(curr, next) || visited.Contains(next))
        {
            Console.WriteLine($"Dead end moving from {curr} to {next} ");
            return Int64.MaxValue;
        }

        visited.Add(next);

        // if done return the step count.
        if (readText[next.Item2][next.Item1] == 'S')
        {
            Console.WriteLine($"Found end at {next}");
            done = true;
            return visited.Count;
        }

        // Follow to next hop.
        (int, int) to = next;

        // Moving N
        if (next.Item2 < curr.Item2)
        {
            switch (readText[next.Item2][next.Item1])
            {
                case '7':
                    to = (next.Item1 - 1, next.Item2);
                    break;
                case '|':
                    to = (next.Item1, next.Item2 - 1);
                    break;
                case 'F':
                    to = (next.Item1 + 1, next.Item2);
                    break;
            }
        }
        // Moving E
        else if (next.Item1 > curr.Item1)
        {
            // Valid is 7,-, J
            switch (readText[next.Item2][next.Item1])
            {
                case '7':
                    to = (next.Item1, next.Item2 + 1);
                    break;
                case '-':
                    to = (next.Item1 + 1, next.Item2);
                    break;
                case 'J':
                    to = (next.Item1, next.Item2 - 1);
                    break;
            }
        }
        // Moving S
        else if (next.Item2 > curr.Item2)
        {
            // Valid is J,|, L
            switch (readText[next.Item2][next.Item1])
            {
                case 'J':
                    to = (next.Item1 - 1, next.Item2);
                    break;
                case '|':
                    to = (next.Item1, next.Item2 + 1);
                    break;
                case 'L':
                    to = (next.Item1 + 1, next.Item2);
                    break;
            }
        }
        // Moving W
        else if (next.Item1 < curr.Item1)
        {
            // Valid is L,-, F
            switch (readText[next.Item2][next.Item1])
            {
                case 'L':
                    to = (next.Item1, next.Item2 - 1);
                    break;
                case '-':
                    to = (next.Item1 - 1, next.Item2);
                    break;
                case 'F':
                    to = (next.Item1, next.Item2 + 1);
                    break;
            }
        }
        // Prep for next iteration.
        curr = next;
        next = to;
    }

    // return followPath(visited,next,to);
    return visited.Count;
}

bool inbounds((int, int) next)
{
    if (next.Item1 > maxX || next.Item1 < 0 || next.Item2 < 0 || next.Item2 > maxY)
    {
        return false;
    }
    return true;
}