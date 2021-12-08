using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC
{
    public class Input
    {
        public string fileName { get; set; }
        public Input(string f)
        {
            fileName = f;
        }

        public string Read() => File.ReadAllText(fileName);
        public string[] ReadByLines() => File.ReadAllLines(fileName);
        public byte[] ReadBytes() => File.ReadAllBytes(fileName);
    }

    public static class Utility
    {
        public static readonly Input input = new Input("input");
        public static readonly Input sample = new Input("sample");

        public static string[] Get_StringList_Comma(string raw)
        {
            return raw.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        }

        public static string[] Get_StringList_Space(string raw)
        {
            return raw.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        }

        public static int[] ParseToInt_StringArr(string[] raw)
        {
            return raw.Select(r => int.Parse(r)).ToArray();
        }
    }
}
