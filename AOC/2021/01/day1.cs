using AOC;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace year2021.day1
{
    class day1 : IDay
    {
        int[] inputs;
        public void UseInput()
        {
            inputs = Utility.ParseToInt_StringArr(Utility.input.ReadByLines());
        }
        public void UseSample()
        {
            inputs = Utility.ParseToInt_StringArr(Utility.sample.ReadByLines());
        }

        public string Part1()
        {
            var step = 0;
            for (int i = 1; i < inputs.Length; i++)
            {
                if (inputs[i] > inputs[i - 1])
                {
                    step++;
                }
            }
            return step.ToString();
        }

        public string Part2()
        {
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
            return step2.ToString();
        }
    }
}
