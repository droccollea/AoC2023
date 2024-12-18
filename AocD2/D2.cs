using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;

string[] readText = File.ReadAllLines("../../../d2.txt");
// string[] readText = File.ReadAllLines("../../../d2-ex.txt");

int total = 0;
foreach (var item in readText)
{
    total += validGame(item);
}

Console.WriteLine("Total: " + total);
// Part 1
// 2102 - typo too high
// 2101
// Part 2
// 58269

int validGame(string game)
{
    //Game 8: 7 red, 12 green; 9 blue, 15 red, 8 green; 3 blue, 11 green, 6 red; 8 blue, 12 red, 5 green
    string pattern = @"Game (\d+):(.*$)";
    System.Text.RegularExpressions.MatchCollection m = System.Text.RegularExpressions.Regex.Matches(game, pattern);
    int gameNum = Int32.Parse(m[0].Groups[1].Value);
    // Console.WriteLine("game is: {0}", gameNum);

    int red = 0;
    int green = 0;
    int blue = 0;

    // Split by semi
    foreach (var hands in m[0].Groups[2].Value.Split(';'))
    {
        // if (!validHand(hands)) 
        // {
        //     // Console.WriteLine("Game {0} invalid", gameNum);
        //     return 0;
        // }
        // Pattern match cubes one colour.
        // Iterate matches and return max r.g.b
        string colourPattern = @"(\d+) ([a-z]+)";
        foreach (System.Text.RegularExpressions.Match m2 in System.Text.RegularExpressions.Regex.Matches(hands, colourPattern))
        {
            int cubes = Int32.Parse(m2.Groups[1].Value);
            string colour = m2.Groups[2].Value;

            Console.WriteLine("colour {0}, cubes {1}", colour, cubes);

            switch (colour)
            {
                case "red":
                    // Console.WriteLine("Checking red");
                    if (cubes > red)
                    {
                        red = cubes;
                    }
                    break;

                case "green":
                    // Console.WriteLine("Checking green");
                    if (cubes > green)
                    {
                        green = cubes;
                    }
                    break;

                case "blue":
                    // Console.WriteLine("Checking blue");
                    if (cubes > blue)
                    {
                        blue = cubes;
                    }
                    break;
            }

        }

    } 
    return red * green * blue;
}

static bool validHand(string hands)
{
    string cubePattern = @"[ ]*(\d+) (.*)";

    // Console.WriteLine("Hands {0}", hands);

    foreach (var hand in hands.Split(','))
    {
        foreach (System.Text.RegularExpressions.Match m2 in System.Text.RegularExpressions.Regex.Matches(hand, cubePattern))
        {
            int cubes = Int32.Parse(m2.Groups[1].Value);
            string colour = m2.Groups[2].Value;

            // Console.WriteLine("colour {0}, cubes {1}", colour, cubes);

            // 12 red cubes, 13 green cubes, and 14 blue cubes
            switch (colour)
            {
                case "red":
                    // Console.WriteLine("Checking red");
                    if (cubes > 12)
                    {
                        return false;
                    }
                    break;

                case "green":
                    // Console.WriteLine("Checking green");
                    if (cubes > 13)
                    {
                        return false;
                    }
                    break;

                case "blue":
                    // Console.WriteLine("Checking blue");
                    if (cubes > 14)
                    {
                        return false;
                    }
                    break;
            }
        }
    }
    return true;
}