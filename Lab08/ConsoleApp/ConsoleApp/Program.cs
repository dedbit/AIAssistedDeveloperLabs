using System;
using System.Collections.Generic;

public class Example
{
    public static void Main(string[] args)
    {
        int value = Int32.Parse(args[0]);
        List<String> names = null;
        if (value > 0)
            names = new List<String>();

        foreach (var item in args)
        {
            names.Add("Name: " + item);
        }
    }
}