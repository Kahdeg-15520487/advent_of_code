using System;
using System.IO;
using System.Linq;

namespace day7
{
    class Program
    {
        static void Main(string[] args)
        {
            //var inputs = File.ReadAllText("sample").Split(",", StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToArray();
            var inputs = File.ReadAllText("input").Split(",", StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToArray();

            //var minpos = Math.Sqrt(inputs.Average());
            //Console.WriteLine(inputs.Sum(i => Math.Abs(i - minpos)));

            var minfuel = int.MaxValue;
            var compoundFuel = 1;
            for (int i = inputs.Min(); i <= inputs.Max(); i++)
            {
                var fuel = inputs.Sum(c =>
                {
                    var step = Math.Abs(c - i);
                    var fuelTaken = compoundFuel == 0 ? step : Enumerable.Range(1, step).Sum(f => f * compoundFuel);
                    return fuelTaken;
                });
                Console.WriteLine("{0} : {1}", i, fuel);
                if (fuel < minfuel)
                {
                    minfuel = fuel;
                }
            }
            Console.WriteLine(minfuel);

            
        }
    }
}
