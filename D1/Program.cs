using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;

string[] readText = File.ReadAllLines("../../../d1.txt");
// string[] readText = File.ReadAllLines("../../../d1-ex2.txt");

//Console.WriteLine(readText);
string[] digits =
[
    "zero",
    "one",
    "two",
    "three",
    "four",
    "five",
    "six",
    "seven",
    "eight",
    "nine"
];

int total = 0;
foreach (var item in readText)
{
    // Console.WriteLine("digits: " + getFirstDigit(item) + getlastDigit(item));

    total += int.Parse("" + getFirstDigit(item) + getlastDigit(item));
}
Console.WriteLine("Total: " + total);
// Part 1
// 55259 - too low?
// 55712 - correct.
// Part 2 - Look for numeric strings.
// 54768 - too low
// 54753 - +lastindexof would be wrong!
// 55413 - + break after first found

char getFirstDigit(string item)
{
    char d = '0';
    var pos = item.Length;
    // Look for word.
    for (int w=1; w<=9; w++)
    {
        var x = item.IndexOf(digits[w]);
        if (x > -1 && x < pos)
        {
            pos = x;
            d = Convert.ToChar(w+48);
            // Console.WriteLine("In {0} found word {1} as char {2}", item, digits[w], d);
        }
    }

    for (int i=0; i < pos; i++)
    {
        if (Char.IsDigit(item[i])) 
        {
            d = item[i];
            break;
        }      
    }
    Console.WriteLine("For item: " + item + " first is: " + d);

    return d;
}


char getlastDigit(string item)
{
    char d = '0';
    var pos = 0;
    // Look for word.
    for (int w=1; w<=9; w++)
    {
        var x = item.LastIndexOf(digits[w]);
        if (x > -1 && x > pos)
        {
            pos = x;
            d = Convert.ToChar(w+48);
        }
    }

    for (int i=pos; i<item.Length; i++)
    {
        if (Char.IsDigit(item[i])) 
        {
            d = item[i];
        }      
    }

    Console.WriteLine("For item: " + item + " last is: " + d);

    return d;
}


