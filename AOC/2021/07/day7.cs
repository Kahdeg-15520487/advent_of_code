using AOC;

using Konsole;

using System;
using System.IO;
using System.Linq;

namespace year2021.day7
{
    class day7 : Day
    {
        int[] inputs;
        static void d(string[] args)
        {
            //var inputs = File.ReadAllText("sample").Split(",", StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToArray();
            var inputs = File.ReadAllText("input").Split(",", StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToArray();

            var minfuel = Solve(inputs);
            Console.WriteLine(minfuel);
        }

        private static int Solve(int[] inputs, int compoundFuel = 0)
        {
            var minfuel = int.MaxValue;
            int min = inputs.Min();
            int max = inputs.Max();
            ProgressBar progressBar = new ProgressBar(max - min);
            for (int i = min; i <= max; i++)
            {
                var fuel = inputs.Sum(c =>
                {
                    var step = Math.Abs(c - i);
                    var fuelTaken = compoundFuel == 0 ? step : Enumerable.Range(1, step).Sum(f => f * compoundFuel);
                    return fuelTaken;
                });
                progressBar.Refresh(i - min, i.ToString());

                if (fuel < minfuel)
                {
                    minfuel = fuel;
                }
            }

            return minfuel;
        }

        public override void UseInput()
        {
            inputs = Utility.input.ReadInts();
        }

        public override void UseSample()
        {
            inputs = Utility.sample.ReadInts();
        }

        public override string Part1()
        {
            sb.AppendFormat("minFuel: {0}", Solve(inputs, 0));
            return base.Part1();
        }

        public override string Part2()
        {
            sb.AppendFormat("minFuel: {0}", Solve(inputs, 1));
            return base.Part2();
        }
    }
}
