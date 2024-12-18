using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualBasic;

// Part 1
// Override sorted list sort by with custom sort by hand then by card
// Iterate and calc the total.
// 250254244

// Part 2
// Re-rank joker.
// Change score sorting.
// 249721373 - too low
// 250087440 - correct - needed to not add joker to a joker and ignore when all jokers!

string[] readText = File.ReadAllLines("./AocD7/input.txt");
// string[] readText = File.ReadAllLines("./AocD7/ex.txt");

// List<string> hands =[];
// List<Int64> bids = [];
SortedList<string,Int64> ranked = new SortedList<string, long>(new HandComp());
// Read the hands and bids.
foreach (var item in readText)
{
    string[] parts = item.Split(" ");
    // hands.Add(parts[0]);
    // bids.Add(Int64.Parse(parts[1]));
    // Console.WriteLine(parts[0]);
    ranked.Add(parts[0],Int64.Parse(parts[1]));
}

Int64 total = 0;
for(int i = 0; i<ranked.Count(); i++)
{
    Console.WriteLine($"Rank {i+1}: {ranked.GetKeyAtIndex(i)}");
    total += (ranked.GetValueAtIndex(i) * (i+1));
}

Console.WriteLine($"Total: {total}");

// Sort by hand score then cardscore within.
class HandComp : IComparer<string>
{
    Int64 scorehand(string hand)
    {
        // Count cards.
        Dictionary<Char,int> cards = [];
        foreach (var c in hand)
        {
            if(cards.ContainsKey(c))
            {
                cards[c]++;
            }
            else{
                cards[c] = 1;
            }
        }

        // Handle Jokers.
        if(cards.ContainsKey('J') && cards['J'] < 5)
        {
            // Remove Jokers - dont want to add to them.
            int jokers = cards['J'];
            cards.Remove('J');

            // Add joker(s) to most frequent and highest ranked card.
            // Exception: 2 of a Kind is better as Full House than 3 of a Kind.
            int most = 0;
            foreach (var v in cards.Values)
            {
                if(v>most){
                    most=v;
                }
            }

            // Rank shouldnt matter.
            SortedList<Int64,char> topCards = [];
            foreach (var c in cards)
            {
                if (c.Value == most)
                {
                    topCards.Add(cardscore[c.Key],c.Key);
                }
            }

            // Add Joker(s)

            // Handle 2OAK -> FH.
            // 3 cards, 2 pair, 1 Joker, add to third ranked.
            // 1J - 2Q 2T (already handled)
            // 1J - 1Q 1T 2
            // if(cards.Count == 3 && topCards.Count == 2 && jokers == 1) {
            //     // Add to card with one count.
            //     Console.WriteLine($"Hand: {hand} adding {jokers} to {topCards.GetValueAtIndex(topCards.Count-1)}");
            //     cards[topCards.GetValueAtIndex(0)] += jokers;
            // }
            // else {
            //     cards[topCards.GetValueAtIndex(topCards.Count-1)] += jokers;
            // }
            cards[topCards.GetValueAtIndex(topCards.Count-1)] += jokers;

        }

        // Check score.
        // 7 - Five of a kind, where all five cards have the same label: AAAAA
        if (cards.Count == 1) 
        {
            return 700; 
        }

        if (cards.Count == 2) 
        {
            // 6 - Four of a kind, where four cards have the same label and one card has a different label: AA8AA
            foreach (var v in cards.Values)
            {
                if (v == 4)
                {
                    return 600;
                } 
            }
            
            // 5 - Full house, where three cards have the same label, and the remaining two cards share a different label: 23332
            // Only 2 cards, must be full house.
            return 500;
        }

        // 4 - Three of a kind, where three cards have the same label, and the remaining two cards are each different from any other card in the hand: TTT98
        if (cards.Count == 3) 
        {
            foreach (var v in cards.Values)
            {
                if (v == 3)
                {
                    return 400;
                } 
            }
            
            // 3 - Two pair, where two cards share one label, two other cards share a second label, and the remaining card has a third label: 23432
            // Only 3 cards, must be two pair.
            return 300;
        }

        // 2 - One pair, where two cards share one label, and the other three cards have a different label from the pair and each other: A23A4
        if (cards.Count == 4) 
        {
            return 200;
        }    
        else {
        // 1 -  High card
            return 100;
        }
    }

    public int Compare(string h1, string h2)
    {
        if(scorehand(h1) < scorehand(h2))
        {
            return -1;
        }
        else if (scorehand(h1) > scorehand(h2))
        {
            return 1;
        }
        else
        {
            return sortByCard(h1,h2);
        }
    }


    Dictionary<Char,Int64> cardscore = new Dictionary<Char,Int64>()
    {
        {'A',14},
        {'K',13},
        {'Q',12},
        {'J',1}, // Rescored as joker with lowest rank.
        {'T',10},
        {'9',9},
        {'8',8},
        {'7',7},
        {'6',6},
        {'5',5},
        {'4',4},
        {'3',3},
        {'2',2}
    };

    int sortByCard(string h1, string h2)
    {
        for (int i =0; i < 5; i++)
        {
            if (cardscore[h1[i]] < cardscore[h2[i]]) 
            {
                return -1;
            }
            if (cardscore[h1[i]] > cardscore[h2[i]]) 
            {
                return 1;
            }
        }
        return 0;
    }
}