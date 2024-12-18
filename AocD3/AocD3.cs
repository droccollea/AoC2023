using System.Collections;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualBasic;


string[] readText = File.ReadAllLines("../../../input.txt");
// string[] readText = File.ReadAllLines("../../../ex.txt");

int total = 0;
int maxX = readText[0].Length;
int maxY = readText.Length;

// Nest loops Y and X.
// If number x10 and add to current part number with xy coords.
// If at end, symbol or period, part number complete. Add to a list.
// with xy coords.

// Match stars with a parts list. Any with exactly 2 is a gear.
Dictionary<string, List<int>> stars = [];

string nothing = "null";

for (int y = 0; y < maxY; y++)
{
    int part = 0;
    string coord = nothing;

    for (int x = 0; x < maxX; x++)
    {
        if (Char.IsDigit(readText[y][x]))
        {
            if (part == 0)
            {
                coord = $"{x},{y}";
            }
            part *= 10;
            part += (int)Char.GetNumericValue(readText[y][x]);
        }

        // If next is not a digit or end of line, we're done.
        if (part > 0 && (x + 1 >= maxX || !Char.IsDigit(readText[y][x + 1])))
        {
            // Console.WriteLine("Checking part:{0}", part);
            // if (validPart(part, coord))
            // {
            //     // parts.Add(part, coord);
            //     total += part;
            // }
            checkForStars(part,coord);

            part = 0;
            coord = nothing;
        }

    }
}

// Phase 2 check each number for a neigbouring symbol. If so sum it.

// foreach (var p in parts)
// {
//     // Console.WriteLine("Part: {0}", p);
//     total += p.Key;
// }

// With the star list, iterate values and get the product of the "gears" where there are two or more parts.

foreach (var list in stars)
{
    Console.WriteLine("Star:{0} has {1} nearby parts",list.Key, list.Value.Count);
    if (list.Value.Count == 2 ) 
    {
        total += list.Value[0] * list.Value[1];            
    }
}

Console.WriteLine("Total: " + total);
// Part 1
//298728 - didnt conider dups.
//320131 - too low. Consider the dups.
//528369 - still low - missing right edge at end of x.
//530849
// Part 2
// 84900879 - correct.

bool validPart(int p, string v)
{
    // Check perimiter of the part for any symbol - non digit, not period.
    // If good return part num else 0.
    string[] xy = v.Split(",");
    int x = int.Parse(xy[0]);
    int y = int.Parse(xy[1]);

    for (int i = x - 1; i <= x + p.ToString().Length && i < maxX; i++)
    {
        if (i < 0)
        {
            continue;
        }
        for (int j = y - 1; j <= y + 1 && j < maxY; j++)
        {
            if (j < 0)
            {
                continue;
            }
            // Console.WriteLine("Checking xy:{0},{1}",i,j);
            // Not a numeric or period, it's a part.
            if (!Char.IsDigit(readText[j][i]) && readText[j][i] != '.')
            {
                // Console.WriteLine("Found:{0}",readText[j][i]);
                return true;
            }
        }
    }
    return false;
}

void checkForStars(int p, string v)
{
    // Check perimiter of the part for a start.
    // If good return part num else 0.
    string[] xy = v.Split(",");
    int x = int.Parse(xy[0]);
    int y = int.Parse(xy[1]);

    for (int i = x - 1; i <= x + p.ToString().Length && i < maxX; i++)
    {
        if (i < 0)
        {
            continue;
        }
        for (int j = y - 1; j <= y + 1 && j < maxY; j++)
        {
            if (j < 0)
            {
                continue;
            }
            // If star in the perimiter, add part to the stars map entry.
            if (readText[j][i] == '*')
            {
                // Console.WriteLine("Star at:{0}", $"{i},{j}");
                if(!stars.ContainsKey($"{i},{j}")) 
                {
                    stars.Add($"{i},{j}", [p]);
                }
                else 
                {
                    //append
                    List<int> parts = stars[$"{i},{j}"];
                    parts.Add(p);
                    stars[$"{i},{j}"] = parts;
                }
            }
        }
    }
    return;
}
