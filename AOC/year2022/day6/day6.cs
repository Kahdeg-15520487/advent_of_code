using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC.year2022.day6
{
    public class day6 : Day
    {
        string raw;
        public override void UseInput()
        {
            raw = Utility.input.Read();
            //raw = Utility.sample.Read();
            //raw = "bvwbjplbgvbhsrlpgdmjqwftvncz";
        }

        public override string Part1()
        {
            var rawSpan = new Span<char>(raw.ToCharArray());
            for (int i = 4; i < raw.Length; i++)
            {
                if (IsSpanDistinct(rawSpan.Slice(i - 4, 4), 4))
                {
                    return i.ToString();
                }
            }

            return raw;
        }

        public override string Part2()
        {
            var rawSpan = new Span<char>(raw.ToCharArray());
            for (int i = 14; i < raw.Length; i++)
            {
                if (IsSpanDistinct(rawSpan.Slice(i - 14, 14), 14))
                {
                    return i.ToString();
                }
            }

            return base.Part2();
        }

        private bool IsSpanDistinct(ReadOnlySpan<char> span, int count)
        {
            return span.ToArray().Distinct().Count() == count;
        }
    }
}
