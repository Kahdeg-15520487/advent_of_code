using AOC;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace year2021.day3
{
    class day3 : Day
    {
        int[][] inputs;

        public override void UseInput()
        {
            inputs = Utility.input.ReadByLines().Select(l => l.ToCharArray().Select(c => (int)c - 48).ToArray()).ToArray();
        }

        public override void UseSample()
        {
            inputs = Utility.sample.ReadByLines().Select(l => l.ToCharArray().Select(c => (int)c - 48).ToArray()).ToArray();
        }

        public override string Part1()
        {
            var digitLength = inputs.First().Length;
            var count = inputs.Length;
            char[] gammaDigit = new char[digitLength];
            char[] epsilonDigit = new char[digitLength];

            for (int digit = 0; digit < digitLength; digit++)
            {
                int d = 0;
                for (int i = 0; i < count; i++)
                {
                    d += inputs[i][digit];
                }
                gammaDigit[digit] = d > count / 2 ? '1' : '0';
                epsilonDigit[digit] = d < count / 2 ? '1' : '0';
            }

            uint gamma = Convert.ToUInt32(new string(gammaDigit), 2);
            uint epsilon = Convert.ToUInt32(new string(epsilonDigit), 2);
            sb.AppendFormat("gamma = {0}", gamma);
            sb.AppendLine();
            sb.AppendFormat("epsilon = {0}", epsilon);
            sb.AppendLine();
            sb.AppendFormat("power consumption = {0}", gamma * epsilon);
            return base.Part1();
        }

        public override string Part2()
        {
            uint o2 = FindRating(inputs, true);
            uint co2 = FindRating(inputs, false);
            sb.AppendFormat("o2 = {0}", o2);
            sb.AppendLine();
            sb.AppendFormat("co2 = {0}", co2);
            sb.AppendLine();
            sb.AppendFormat("life support = {0}", o2 * co2);
            return base.Part2();
        }

        private static uint FindRating(int[][] inputs, bool criteria)
        {
            List<int[]> temp = inputs.ToList();
            int currentDigit = 0;
            do
            {
                int bit1count = temp.Sum(l => l[currentDigit]);
                int currentBit = 0;
                if (temp.Count % 2 == 0 && bit1count == temp.Count / 2)
                {
                    currentBit = criteria ? 1 : 0;
                }
                else if ((bit1count > temp.Count / 2))//&& criteria)
                {
                    currentBit = criteria ? 1 : 0;
                }
                else currentBit = criteria ? 0 : 1;

                temp = temp.Where(l => l[currentDigit] == currentBit).ToList();

                currentDigit++;
            } while (temp.Count > 1);
            return Convert.ToUInt32(new string(temp.First().Select(d => (char)(d + 48)).ToArray()), 2);
        }
    }
}
