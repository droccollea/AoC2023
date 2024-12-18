using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;

// Part 1
// 55843 - too high!
// 51687 - too high
// 21568 - counted spaces, needed a trim.
// Part 2
// 11827296



string[] readText = File.ReadAllLines("./AocD4/input.txt");
// string[] readText = File.ReadAllLines("./AocD4/ex.txt");
Dictionary<int,int> matches = [];
Dictionary<int,int> collection = [];

int total = 0;

// Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
// Split on |
// on 0, split on : then split on ' '
// foreach number check if exists in [1] of original split 
// if so points *= 2 (or 1 if first)
// add points to total
for (int g=1; g<=readText.Length; g++)
{
    // Build win dictionary to calc.
    matches.Add(g, getWinsForGame(g));
    // Add initial card to collection.
    collection.Add(g,1);
}

// foreach (var k in dictionary)
// {
//     Console.WriteLine($"Game {k.Key} and matches {k.Value}");
// }

// For each game get its winners.
foreach (int k in matches.Keys)
{
    Console.WriteLine($"Initial card: {k}");
    addWinners(k);
}

// Tally up the cards.
foreach (var g in collection)
{
    Console.WriteLine($"adding {g}");
    total += g.Value;
}

Console.WriteLine($"Total:{total}");

void addWinners(int game)
{
    // collection[game]++;
    // int cards = 1; // This card.
    int wins = matches[game];
    // cards += wins;

    for (int c = 1; c<=collection[game]; c++) 
    {
        for (int i = game+1; i<=(game+wins); i++)
        {
            // Console.WriteLine($"Incrementing for: {i}");
            // Console.WriteLine($"  Looping: {i} cards:{cards}");
            collection[i]++;
        }
    }
    // Console.WriteLine($" After checking new cards: game:{game} cards:{cards}");
    return;
}

int getWinsForGame(int game)
{
    string card = readText[game-1];
    string[] cardSplit = card.Split('|');
    string[] left = cardSplit[0].Trim().Split(':');
    string[] drawn = left[1].Trim().Split(' ');
    int score = 0;
    foreach (var num in drawn)
    {
        if (num.Trim() == "")
        { continue; }

        // Console.WriteLine("num: {0}", num);
        // Console.WriteLine("hand: {0}", cardSplit[1].Trim());

        // Console.WriteLine("num: {0}", num);
        if ($"{cardSplit[1]} ".Contains($" {num} "))
        {
            // Console.WriteLine($"hand {cardSplit[1]} contains {num} ");
            // if (score == 0)
            // {
            //     score = 1;
            // }
            // else
            // {
            //     score *= 2;
            // }
            score++;
        }
    }
    // Console.WriteLine($"Score for {left[0]} is {score}");
    return score;
}