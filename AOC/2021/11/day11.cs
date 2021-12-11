using AOC;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace year2021.day11
{
    public class day11 : Day
    {
        private World world;
        private int step2;

        public override void UseInput()
        {
            var raw = new Input("input").ReadByLines();
            var w = raw.First().Length;
            var h = raw.Length;
            var inputs = string.Join(string.Empty, raw).Select(c => c - 48).ToArray();
            world = new World(inputs, w, h);
        }

        public override void UseSample()
        {
            throw new NotImplementedException();
        }
        public override string Part1()
        {
            var w = world.Clone();

            //sb.AppendLine(w.ToString());
            //sb.AppendLine(w.Flash().ToString());
            //sb.AppendLine(w.ToString());
            sb.AppendLine("before any step:");
            sb.AppendLine(w.ToString());

            long sum = 0;
            for (int i = 0; i < 100; i++)
            {
                var f = w.Flash();
                sum += f;
                sb.AppendLine($"after step {i + 1}:");
                sb.AppendLine(w.ToString());
            }
            sb.Append(sum);

            return base.Part1();
        }

        public override string Part2()
        {
            var w = world.Clone();

            //sb.AppendLine("before any step:");
            //sb.AppendLine(w.ToString());

            for (int i = 0; i < 10000; i++)
            {
                var f = w.Flash();
                //sum += f;
                //sb.AppendLine($"after step {i + 1}:");
                //sb.AppendLine(w.ToString());

                if (f == w.width * w.height && step2 == 0)
                {
                    step2 = i + 1;
                    break;
                }
            }
            sb.Append(step2);
            return base.Part2();
        }
    }

    class World
    {
        public int[] otc;
        public int width;
        public int height;
        public int this[int x, int y] {
            get {
                if (x >= 0 && x < width
                 && y >= 0 && y < height)
                {
                    return otc[y * width + x];
                }
                return int.MaxValue;
            }
            set { otc[y * width + x] = value; }
        }

        public int this[(int x, int y) p] {
            get => this[p.x, p.y];
            set => this[p.x, p.y] = value;
        }

        public World(int[] raw, int w, int h)
        {
            this.width = w;
            this.height = h;
            otc = new int[width * height];
            Array.Copy(raw, otc, raw.Length);
        }

        public World Clone()
        {
            return new World(otc, width, height);
        }

        internal int Flash()
        {
            List<(int x, int y)> init = new List<(int x, int y)>();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    this[x, y]++;
                    if (this[x, y] > 9)
                    {
                        init.Add((x, y));
                    }
                }
            }

            HashSet<(int x, int y)> visited = new HashSet<(int x, int y)>(init);
            Stack<(int x, int y)> s = new Stack<(int x, int y)>(init);
            while (s.Count > 0)
            {
                var cur = s.Pop();
                this[cur] = 0;
                foreach (var a in adjacent(cur.x, cur.y))
                {
                    if (visited.Contains(a))
                    {
                        continue;
                    }
                    this[a]++;
                    if (this[a] > 9)
                    {
                        s.Push(a);
                        visited.Add(a);
                    }
                }
            }

            return visited.Count;
        }

        private IEnumerable<(int x, int y)> adjacent(int x, int y)
        {
            if (x > 0) yield return (x - 1, y);
            if (x < width - 1) yield return (x + 1, y);
            if (y > 0) yield return (x, y - 1);
            if (y < height - 1) yield return (x, y + 1);

            if (x > 0 && y > 0)
            {
                yield return (x - 1, y - 1);
            }
            if (x < width - 1 && y > 0)
            {
                yield return (x + 1, y - 1);
            }
            if (x > 0 && y < height - 1)
            {
                yield return (x - 1, y + 1);
            }
            if (x < width - 1 && y < height - 1)
            {
                yield return (x + 1, y + 1);
            }
        }

        private void Flash(int x, int y)
        {

            foreach (var a in adjacent(x, y))
            {
                this[a.x, a.y]++;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (this[x, y] < 0)
                    {
                        sb.Append(9);
                    }
                    else
                    {
                        sb.Append(this[x, y]);
                    }
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
