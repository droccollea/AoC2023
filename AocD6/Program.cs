using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualBasic;

// Part 1
//500346
// Part 2
// 42515755

string[] readText = File.ReadAllLines("./AocD6/input.txt");
// string[] readText = File.ReadAllLines("./AocD6/ex.txt");


List<Int64> times = [];
List<Int64> dists = [];

StringBuilder raceTimeSb = new StringBuilder(8);
StringBuilder raceDistSb = new StringBuilder(8);

string pattern = @"(\d+)";
foreach (System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(readText[0], pattern))
{
    Int64 time = Int64.Parse(m.Groups[1].Value);
    times.Add(time);
    raceTimeSb.Append(time);
    // Console.WriteLine($"time: {time}");
}

foreach (System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(readText[1], pattern))
{
    Int64 dist = Int64.Parse(m.Groups[1].Value);
    dists.Add(dist);
    raceDistSb.Append(dist);
    // Console.WriteLine($"dist: {dist}");
}

long total = 1;

// Each race.
// for (int i=0; i<dists.Count; i++) 
// {
// Single Race
Int64 raceTime = Int64.Parse(raceTimeSb.ToString());
Int64 minDist = Int64.Parse(raceDistSb.ToString());
Int64 wins = 0;
for (long press=0; press<raceTime; press++)
{
    Int64 travelled = press * (raceTime-press);
    // Console.WriteLine($"travelled {travelled} = press {press} * remaining time {raceTime-press}");
    if (travelled>minDist){
        wins++;
    }
}
Console.WriteLine($"Wins: {wins}");
total *= wins;

Console.WriteLine("Total {0}", total);

// Part2

