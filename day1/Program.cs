using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day1
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputs = File.ReadAllLines("input").Select(l => int.Parse(l)).ToArray();
            var step = 0;
            for (int i = 1; i < inputs.Length; i++)
            {
                if (inputs[i] > inputs[i - 1])
                {
                    step++;
                }
            }
            Console.WriteLine(step);

            var step2 = 0;
            List<int> slides = new List<int>();
            for (int i = 0; i < inputs.Length - 2; i++)
            {
                slides.Add(inputs[i] + inputs[i + 1] + inputs[i + 2]);
            }
            for (int i = 1; i < slides.Count; i++)
            {
                if (slides[i] > slides[i - 1])
                {
                    step2++;
                }
            }
            Console.WriteLine(step2);
            
        }
    }
}
