using System;
using System.IO;

namespace AOC
{
    class Program
    {
        static void Main(string[] args)
        {
            int year = 2021;
            int d = 11;
            string currentWorkDir = Directory.GetCurrentDirectory();
            for (int i = 1; i <= 25; i++)
            {
                if (i != d)
                {
                    continue;
                }
                try
                {
                    Type dayType = Type.GetType($"year{year}.day{i}.day{i}");

                    if (dayType == null)
                    {
                        goto skip;
                    }

                    var obj = Activator.CreateInstance(dayType);
                    IDay day = obj as IDay;
                    if (day == null)
                    {
                        goto skip;
                    }

                    Directory.SetCurrentDirectory($"{year}/{i.ToString().PadLeft(2, '0')}");

                    day.UseInput();
                    Console.WriteLine("Day {0}:", i);
                    Console.WriteLine(day.Part1());
                    Console.WriteLine("=====");
                    Console.WriteLine(day.Part2());
                    Console.WriteLine("=====");
                    Console.WriteLine();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error running day {0}:", i);
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine("=====");
                    Console.WriteLine();
                }

                skip:
                Directory.SetCurrentDirectory(currentWorkDir);
            }
        }
    }
}
