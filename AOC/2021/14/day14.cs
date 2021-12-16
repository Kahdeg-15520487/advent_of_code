using AOC;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace year2021.day14
{
    public class day14 : Day
    {
        private string template;
        private Dictionary<string, char> rules;
        private Dictionary<(char c1, char c2), char> rules2;

        public override void UseInput()
        {
            var raw = Utility.Get_StringList_DoubleLineBreak(Utility.sample.Read());
            template = raw[0];
            rules = Utility.Get_StringList_LineBreak(raw[1]).Select(l =>
            {
                var t = l.Split(" -> ");
                return (pair: t[0], next: t[1][0]);
            }).ToDictionary(p => p.pair, p => p.next);
            rules2 = rules.ToDictionary(r => (r.Key[0], r.Key[1]), r => r.Value);
        }

        public override void UseSample()
        {
            throw new NotImplementedException();
        }
        public override string Part1()
        {
            //sb.AppendLine($"Template\t: {template}");
            //var cur = template;
            //for (int i = 0; i < 10; i++)
            //{
            //    cur = Substitute(cur);
            //    //Console.WriteLine($"after day {i + 1}\t: {cur.LongCount()}");
            //    //Console.WriteLine($"after day {i + 1}\t: {string.Join(string.Empty, cur.OrderBy(c => c))}");

            //    var ttt = cur.GroupBy(c => c).Select(c => (c: c.Key, count: c.LongCount())).OrderByDescending(c => c.count);

            //    Console.WriteLine($"after day {i + 1}\t:");
            //    foreach (var item in ttt)
            //    {
            //        Console.WriteLine("\t{0}:{1}", item.c, item.count);
            //    }
            //}

            //var t = cur.GroupBy(c => c).Select(c => (c: c.Key, count: c.LongCount())).OrderByDescending(c => c.count);

            //sb.Append(t.First().count - t.Last().count);

            //foreach (var item in t)
            //{
            //    Console.WriteLine("{0}:{1}", item.c, item.count);
            //}

            return base.Part1();
        }

        public override string Part2()
        {
            var counter = new Dictionary<char, long>();
            foreach (var c in template)
            {
                if (counter.ContainsKey(c))
                {
                    counter[c] += 1;
                }
                else
                {
                    counter.Add(c, 1);
                }
            }

            Console.WriteLine($"begin\t:");
            foreach (var item in counter)
            {
                Console.WriteLine("\t{0}:{1}", item.Key, item.Value);
            }

            var cur = template.SelectTwo((c1, c2) => (c1, c2)).GroupBy(p => p).ToDictionary(p => p.Key, p => p.LongCount());

            for (int i = 0; i < 10; i++)
            {
                Dictionary<(char c1, char c2), long> next = new Dictionary<(char c1, char c2), long>();
                foreach (var item in cur)
                {
                    if (rules2.ContainsKey(item.Key))
                    {
                        var sub = rules2[item.Key];
                        var left = (item.Key.c1, sub);
                        var right = (sub, item.Key.c2);

                        if (next.ContainsKey(left))
                        {
                            next[left] += item.Value;
                        }
                        else
                        {
                            next.Add(left, cur.GetValueOrDefault(left, 0) + item.Value);
                        }

                        if (next.ContainsKey(right))
                        {
                            next[right] += item.Value;
                        }
                        else
                        {
                            next.Add(right, cur.GetValueOrDefault(right, 0) + item.Value);
                        }

                        if (counter.ContainsKey(sub))
                        {
                            counter[sub] += item.Value;
                        }
                        else
                        {
                            counter.Add(sub, item.Value);
                        }
                    }
                }
                cur = next;

                Console.WriteLine($"after day {i + 1}\t:");
                foreach (var item in counter)
                {
                    Console.WriteLine("\t{0}:{1}", item.Key, item.Value);
                }
            }

            var max = counter.Max(c => c.Value);
            var min = counter.Min(c => c.Value);
            sb.Append(max - min);

            return base.Part2();
        }

        private IEnumerable<(char c1, char c2)> Produce((char c1, char c2) p)
        {
            if (rules2.ContainsKey(p))
            {
                yield return (p.c1, rules2[p]);
                yield return (rules2[p], p.c2);
            }
            else
            {
                yield return p;
            }
        }

        private string Substitute(string cur)
        {
            var s1 = cur.SelectTwo((c1, c2) =>
            {
                var p = $"{c1}{c2}";
                if (rules.ContainsKey(p))
                {
                    return (c1, c2, c3: rules[p]);
                }
                else
                {
                    return (c1, c2, c3: '\0');
                }
            }).Aggregate(new StringBuilder(), (s, r) => s.Append($"{r.c3}{r.c2}")).ToString();
            s1 = cur[0] + s1;
            cur = s1;
            return cur;
        }
    }
}
