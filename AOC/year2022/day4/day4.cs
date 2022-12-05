using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC.year2022.day4
{
    public class day4 : Day
    {
        private class range
        {
            public int begin;
            public int end;

            public range(int begin, int end)
            {
                this.begin = begin;
                this.end = end;
            }

            public bool Contains(range other)
            {
                return begin <= other.begin && end >= other.end;
            }

            public bool Overlap(range other)
            {
                return this.Contains(other)
                 || (begin <= other.begin && end >= other.begin)
                 || (begin <= other.end && end >= other.end)
                ;
            }

            public override string ToString()
            {
                return $"{begin}-{end}";
            }
        }
        private (range p1, range p2)[] pairs;
        public override void UseInput()
        {
            base.UseInput();
            //base.UseSample();
            pairs = inputs.Select(i => i.Split(",")).Select(i =>
            {
                var p1 = i[0].Split("-");
                var p2 = i[1].Split("-");
                return (new range(int.Parse(p1[0]), int.Parse(p1[1])), new range(int.Parse(p2[0]), int.Parse(p2[1])));
            }).ToArray();
        }

        public override string Part1()
        {
            return pairs.Count(p => p.p1.Contains(p.p2) || p.p2.Contains(p.p1)).ToString();
        }

        public override string Part2()
        {
            return pairs.Count(p => p.p1.Overlap(p.p2) || p.p2.Overlap(p.p1)).ToString();
        }
    }
}
