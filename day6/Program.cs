using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day6
{
    class Program
    {
        static void Main(string[] args)
        {
            //var fishes = File.ReadAllText("sample").Split(",", StringSplitOptions.RemoveEmptyEntries).Select(n => new Fish(int.Parse(n)));
            //var fishes = File.ReadAllText("input").Split(",", StringSplitOptions.RemoveEmptyEntries).Select(n => new Fish(int.Parse(n)));
            //var world = new World(fishes);
            //for (int i = 0; i < 256; i++)
            //{
            //    world.Tick();
            //    //Console.WriteLine("After {0} day(s): {1} fish(es):{2}", i.ToString().PadLeft(2, ' '), world.Count.ToString().PadLeft(2, ' '), world);
            //}
            //Console.WriteLine("After 80 days: {0} fishes", world.Count.ToString().PadLeft(2, ' '));
            //Console.ReadLine();

            var fishes = File.ReadAllText("input").Split(",", StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToArray();
            World world = new World(fishes);

            for (int d = 0; d < 256; d++)
            {
                world.Tick();
                Console.WriteLine("day {0}: {1}", d.ToString().PadLeft(2, ' '), world.Count);
            }

            
        }
    }

    class World
    {
        public double[] FishGens { get; set; }
        public double Count {
            get {
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
        {/*
          * gen 0 will be the next gen 8 and gen 6
          * gen 1 will be the next gen 0
          * 
          * 
          * 
          */
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
