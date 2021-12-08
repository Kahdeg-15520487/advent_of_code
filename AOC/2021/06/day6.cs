using AOC;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace year2021.day6
{
    class day6 : Day
    {
        int[] input;
        static void d(string[] args)
        {
            var fishes = File.ReadAllText("input").Split(",", StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToArray();
            World world = new World(fishes);

            for (int d = 0; d < 256; d++)
            {
                world.Tick();
                Console.WriteLine("day {0}: {1}", d.ToString().PadLeft(2, ' '), world.Count);
            }
        }

        public override void UseInput()
        {
            input = Utility.ParseToInt_StringArr(Utility.Get_StringList_Comma(Utility.input.Read()));
        }

        public override void UseSample()
        {
            input = Utility.ParseToInt_StringArr(Utility.Get_StringList_Comma(Utility.sample.Read()));
        }

        public override string Part1()
        {
            World world = new World(input);

            for (int d = 0; d < 80; d++)
            {
                world.Tick();
            }

            sb.AppendLine(world.Count.ToString());

            return base.Part1();
        }

        public override string Part2()
        {
            World world = new World(input);

            for (int d = 0; d < 256; d++)
            {
                world.Tick();
            }

            sb.AppendLine(world.Count.ToString());

            return base.Part2();
        }
    }

    class World
    {
        public double[] FishGens { get; set; }
        public double Count
        {
            get
            {
                double sum = 0;
                for (int g = 0; g < 9; g++)
                {
                    sum = sum + (double)FishGens[g];
                }
                return sum;
            }
        }
        public World(IEnumerable<int> fishes)
        {
            FishGens = new double[9];
            foreach (var f in fishes)
            {
                FishGens[f]++;
            }
        }

        public void Tick()
        {
            var newFish = FishGens[0];
            var reproducedFish = FishGens[0];

            LeftShiftArray(FishGens, 1);

            FishGens[8] = newFish;
            FishGens[6] += reproducedFish;
        }

        public static void LeftShiftArray(double[] arr, int shift)
        {
            shift = shift % arr.Length;
            double[] buffer = new double[shift];
            Array.Copy(arr, buffer, shift);
            Array.Copy(arr, shift, arr, 0, arr.Length - shift);
            Array.Copy(buffer, 0, arr, arr.Length - shift, shift);
        }
    }
}
