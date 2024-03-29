﻿using AOC;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace year2021.day8
{
    class day8 : Day
    {
        Line[] inputs;

        public override void UseInput()
        {
            inputs = Utility.input.ReadByLines().Select(l => new Line(l)).ToArray();
        }

        public override void UseSample()
        {
            inputs = Utility.sample.ReadByLines().Select(l => new Line(l)).ToArray();
        }

        public override string Part1()
        {
            sb.AppendFormat("{0}", inputs.Sum(l => l.Output.Where(o => o == "1" || o == "4" || o == "7" || o == "8").Count()));
            return base.Part1();
        }

        public override string Part2()
        {
            sb.AppendFormat("{0}", inputs.Sum(l => l.Number));
            return base.Part2();
        }
    }

    class Line
    {
        public string[] Encoded { get; set; }
        public string[] Input { get; set; }

        public Dictionary<string, int> Decoded { get; set; }
        public string[] Output { get; set; }
        public int Number { get; set; }

        public Line(string raw)
        {
            var t = raw.Split("|", StringSplitOptions.RemoveEmptyEntries);
            Encoded = t[0].Split(" ", options: StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(e => new string(e.OrderBy(c => c).ToArray())).ToArray();
            Input = t[1].Split(" ", options: StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(e => new string(e.OrderBy(c => c).ToArray())).ToArray();

            Decode(Encoded);
        }

        private void Decode(string[] encoded)
        {
            Decoded = new Dictionary<string, int>();
            Decoded.Add(encoded.First(e => e.Length == 2), 1);
            Decoded.Add(encoded.First(e => e.Length == 4), 4);
            Decoded.Add(encoded.First(e => e.Length == 3), 7);
            Decoded.Add(encoded.First(e => e.Length == 7), 8);

            var one = Decoded.First(k => k.Value == 1).Key;
            var four = Decoded.First(k => k.Value == 4).Key;
            var seven = Decoded.First(k => k.Value == 7).Key;
            var eight = Decoded.First(k => k.Value == 8).Key;
            var top = new string(seven.Except(one).ToArray());

            var nine = encoded.Where(e => e.Length == 6 && e.Except(four).Except(seven).Count() == 1).First();
            var six = encoded.Where(e => e.Length == 6 && e != nine && one.Except(e).Count() == 1).First();
            var zero = encoded.Where(e => e.Length == 6 && e != six && e != nine).First();

            Decoded.Add(nine, 9);
            Decoded.Add(six, 6);
            Decoded.Add(zero, 0);

            var upperRight = new string(one.Except(six).ToArray());
            var bottomRight = new string(one.Except(upperRight).ToArray());
            var bottomLeft = new string(eight.Except(nine).ToArray());
            var middle = new string(eight.Except(zero).ToArray());

            var five = encoded.Where(e => e.Length == 5 && !e.Contains(upperRight) && !e.Contains(bottomLeft)).First();
            var two = encoded.Where(e => e.Length == 5 && e.Contains(upperRight) && e.Contains(bottomLeft)).First();
            var three = encoded.Where(e => e.Length == 5 && e.Contains(upperRight) && e.Contains(bottomRight)).First();

            Decoded.Add(five, 5);
            Decoded.Add(two, 2);
            Decoded.Add(three, 3);

            Output = Input.Select(i => Decoded.ContainsKey(i) ? Decoded[i].ToString() : "-").ToArray();
            Number = int.Parse(string.Join("", Output));
        }

        public override string ToString()
        {
            return $"{string.Join(" ", Encoded)} | {string.Join(" ", Input)}" + Environment.NewLine +
                    $"{string.Join(" ", Input.Select(i => Decoded.ContainsKey(i) ? $"{Decoded[i]},{i}" : $"*,{i}"))} | {string.Join(" ", Output)}";
        }
    }
}
