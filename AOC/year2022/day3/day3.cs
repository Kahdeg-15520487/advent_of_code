using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC.year2022.day3
{
    public class day3 : Day
    {
        private List<(int[] c1, int[] c2)> rucksacks;
        public override void UseInput()
        {
            base.UseInput();
            //base.UseSample();
            rucksacks = inputs.Select(s => s.Select(c => c >= 97 ? c - 96 : c - 38).ToArray()).Select(i => (i.Take(i.Length / 2).ToArray(), i.TakeLast(i.Length / 2).ToArray())).ToList();
        }

        public override void UseSample()
        {
            base.UseSample();
        }

        public override string Part1()
        {
            return rucksacks.Select(r => r.c1.Intersect(r.c2).First()).Sum().ToString();
        }

        public override string Part2()
        {
            int i = 0;
            var query = from s in rucksacks.Select(c => c.c1.Concat(c.c2).ToArray())
                        let num = i++
                        group s by num / 3 into g
                        select g.ToArray();
            var groups = query.ToArray();
            var totalBadge = 0;
            foreach (var group in groups)
            {
                var r1 = group[0];
                var r2 = group[1];
                var r3 = group[2];

                var r12 = r1.Intersect(r2);
                var r123 = r12.Intersect(r3);
                totalBadge += r123.First();
            }

            return totalBadge.ToString();
        }
    }
}
