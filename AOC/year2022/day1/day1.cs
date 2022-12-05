using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC.year2022.day1
{
    public class day1 : Day
    {
        private new int[] inputs;
        private List<long> elfs;
        public override void UseInput()
        {
            base.UseInput();
            inputs = Utility.ParseToInt_StringArr_EmptyStringZero(base.inputs);
        }

        public override void UseSample()
        {
            base.UseSample();
            inputs = Utility.ParseToInt_StringArr_EmptyStringZero(base.inputs);
        }

        public override string Part1()
        {
            long max = 0;
            long current = 0;
            elfs = new();
            for (int i = 0; i < inputs.Length; i++)
            {
                if (inputs[i] != 0)
                {
                    current += inputs[i];
                }
                else
                {
                    if (max < current)
                    {
                        max = current;
                    }
                    elfs.Add(current);
                    current = 0;
                }
            }

            return max.ToString();
        }

        public override string Part2()
        {
            return $"{(this.elfs.OrderByDescending(e => e).Take(3).Sum())}";
        }
    }
}
