using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;

// Part 1
//836040384
// Part 2
// Find the range or lower than for each mapping. 
// Apply to seeds 
// 10834440

string[] readText = File.ReadAllLines("./AocD5/ConsoleApp1/input.txt");
// string[] readText = File.ReadAllLines("./AocD5/ConsoleApp1/ex.txt");
List<(Int64,Int64,Int64)> soil = [];
List<(Int64,Int64,Int64)> fertilizer = [];
List<(Int64,Int64,Int64)> water =[];
List<(Int64,Int64,Int64)> light=[];
List<(Int64,Int64,Int64)> temp=[];
List<(Int64,Int64,Int64)> humidity=[];
List<(Int64,Int64,Int64)> location=[];

List<List<(Int64,Int64,Int64)>> filters = [];
// Read and fill out the maps.
filters.Add(buildMap("seed-to-soil",soil));
filters.Add(buildMap("soil-to-fertilizer",fertilizer));
filters.Add(buildMap("fertilizer-to-water",water));
filters.Add(buildMap("water-to-light",light));
filters.Add(buildMap("light-to-temperature",temp));
filters.Add(buildMap("temperature-to-humidity",humidity));
filters.Add(buildMap("humidity-to-location",location));

// seeds: 79 14 55 13
string[] top = readText[0].Split(":");
string[] seeds = top[1].Trim().Split(" ");

// Part 1.
// For each seed
// lookup the location by chaining to the humidity
// humidity to temp
// temp to light
// light to water 
// water to fertilizer
// fertilizer to soil
// soil to seed
// return the lowest location

Int64 min = Int64.MaxValue;
// for (Int64 s = 0; s<seeds.Length; s++)
// {
//     Int64 seed=Int64.Parse(seeds[s]);
//     // Int64 max = seed + Int64.Parse(seeds[s+1]);
//     // for(; seed <max; seed++)
//     // {
//         // Console.WriteLine($"Seed {seed}...");
//         // int loc = location[humidity[temp[light[water[fertilizer[soil[seed]]]]]]];
//     // Int64 loc = getM(getM(getM(getM(getM(getM(getM(seed, soil), fertilizer), water), light), temp), humidity),location);
//     Int64 next = seed;
//     foreach (var filter in filters)
//     {
//         next = getM(next,filter);
//     }
//     Int64 loc = next;
//     // Console.WriteLine($"seed {seed} loc is {loc}");
//     if (loc < min) min = loc;
//     Console.WriteLine("min loc is {0}", min);
// }
// Console.WriteLine("Done Part 1.");


// Build a seed map. 
// Iterate locations until min seed found.

// Part2.
// 10834440
min = Int64.MaxValue;
List<(Int64,Int64)> sources = [];
for (Int64 s = 0; s<seeds.Length; s+=2)
{
    Int64 start = Int64.Parse(seeds[s]);
    Int64 amt = Int64.Parse(seeds[s+1]);
    (Int64,Int64) range = (start,start+amt-1);
    Console.WriteLine($"Range {range}");
    sources.Add(range);
}

int fcount =0;
foreach (var f in filters)
{
    foreach (var item in f)
    {
        fcount++;
    }
}
Console.WriteLine($"There are {fcount} filters.");
// 18 filters in total in example.
// 198 in input.

// Apply filters.

// for each source map and pass to next round of mappings.
foreach (var source in sources)
{
    Console.WriteLine($"Processing {source}");
    List<(Int64,Int64)> next = [source];
    foreach (var filter in filters)
    {
        next = remap(next,filter);
        Console.WriteLine("next list now size: {0}", next.Count);
    }

    foreach (var item in next)
    {
        if (item.Item1 < min)
        {
            min = item.Item1;
        }
        Console.WriteLine("min loc is {0}", min);
    }
}

Console.WriteLine($"Final min loc is: {min}");

List<(Int64,Int64,Int64)> buildMap(string map, List<(Int64,Int64,Int64)> d)
{
    int l = 0;
    while(++l < readText.Length && !readText[l].StartsWith(map)){
        continue;
    }
    // Found it. map out until line break.
    while(true){
        if (++l >= readText.Length) break;

        string line = readText[l];
        if (line.Trim().Equals("")) {
            break;
        }
        string[] tuple = line.Split(" ");
        Int64 source = Int64.Parse(tuple[0].Trim());
        Int64 destination = Int64.Parse(tuple[1].Trim());
        Int64 range = Int64.Parse(tuple[2].Trim());

        d.Add((source,destination,range));
    }
    return d;
}

Int64 getM(Int64 v, List<(Int64,Int64,Int64)> d)
{
    // Console.WriteLine($"Looking up:{v}...");
    foreach (var t in d)
    {
        if (v >= t.Item2 && v<= t.Item2+t.Item3) {

            // in range. get offset and apply to destination.
            Int64 offset = t.Item1 - t.Item2;
            // Console.WriteLine($"{v} in {t} offset {offset}");
            return v + offset;
        }
    }
    return v;
}

List<(Int64,Int64)> remap(List<(Int64,Int64)> ranges, List<(Int64,Int64,Int64)> maps)
{
    List<(Int64,Int64)> remapped = [];
    // ranges.Sort(sortByItem1());

    foreach (var range in ranges)
    {
        // Console.WriteLine($"Mapping: {range}");
        // Find maps that apply if any.
        List<Int64> splits = [range.Item1];

        foreach (var map in maps)
        {
            Int64 mapEnd = map.Item2 + map.Item3 -1; // inclusive end.
            if(mapEnd < range.Item1 || map.Item2 > range.Item2) {
                continue; // map doesnt apply.
            }
            if (map.Item2 > range.Item1)
            {
                splits.Add(map.Item2);
            }
            if (mapEnd < range.Item2)
            {
                splits.Add(mapEnd);
            }
            if (map.Item2 < range.Item1 && mapEnd > range.Item2)
            {
                // Console.WriteLine($"Map ({map.Item2}-{mapEnd}) covers range, no split required.");
            }
        }
        splits.Add(range.Item2);
        splits.Sort();
        foreach (var split in splits)
        {
            // Console.WriteLine("Split: {0}", split);
        }

        List<(Int64,Int64)> unmapped = [];
        for (int s = 0; s<splits.Count-1; s++)
        {
            unmapped.Add((splits[s],splits[s+1]));
        }

        // Iterate new splits and map what matches.
        foreach (var u in unmapped) 
        {
            // Console.WriteLine($"Remapping unmapped:{u}");
            bool mapped = false;
            foreach (var map in maps)
            {
                // Console.WriteLine($"Map: {map}");
                Int64 offset = map.Item1 - map.Item2;
                Int64 mapEnd = map.Item2 + map.Item3 -1; // inclusive end.

                // Map what can be, else append to unmapped for next filter.
                // If cand mapping end before the start or cand starts after end rane is unmapped.
                if(map.Item2 <= u.Item1 && mapEnd >= u.Item2)
                {
                    remapped.Add((u.Item1+offset,u.Item2+offset));
                    mapped=true;
                    // Console.WriteLine($"Match - remapped:{u} with map: {map.Item2}");
                    break;  // Done with this one.
                }
            }

            if (!mapped) 
            {
                remapped.Add(u);
            }
        }
    }

    return remapped;
}
