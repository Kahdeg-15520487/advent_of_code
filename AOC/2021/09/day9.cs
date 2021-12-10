using AOC;

using FloodSpill;
using FloodSpill.Utilities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace year2021.day9
{
    public class day9 : Day
    {
        World world;

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
            sb.AppendFormat("{0}", world.GetLowPoints().Sum(lp => lp.height + 1));
            return base.Part1();
        }

        public override string Part2()
        {
            //var basins = world.GetLowPoints().Select(lp =>
            //{
            //    return world.GetBasin(lp).ToList();
            //}).ToList();
            //world.RenderBasins(basins.OrderByDescending(b => b.Count).Take(3).SelectMany(b => b), world.GetLowPoints());
            //sb.Append(basins.OrderByDescending(b => b.Count).Take(3).Aggregate(1, (a, b) => a * (b.Count - 1)));

            var visited = new HashSet<(int y, int x)>();
            IEnumerable<(int y, int x)> adjacent(int y, int x)
            {
                if (x > 0) yield return (y, x - 1);
                if (x < world.width - 1) yield return (y, x + 1);
                if (y > 0) yield return (y - 1, x);
                if (y < world.height - 1) yield return (y + 1, x);
            }
            IEnumerable<(int y, int x)> basin((int y, int x) low)
            {
                if (!visited.Contains(low) && world[low.x, low.y] != 9)
                {
                    visited.Add(low);
                    yield return low;

                    foreach (var a in adjacent(low.y, low.x))
                        foreach (var c in basin(a))
                            yield return c;
                }
            }

            var basins = world.GetLowPoints().Select(lp => (lp.y, lp.x)).Select(l => basin(l).ToArray()).OrderByDescending(b => b.Length).ToArray();

            var part2 = basins.Take(3).Aggregate(1L, (s, b) => s * b.Count());

            sb.Append(part2);

            return base.Part2();
        }
    }

    class World
    {
        public int[] heightMap;
        public int width;
        public int height;
        public int this[int x, int y] {
            get {
                if (x >= 0 && x < width
                 && y >= 0 && y < height)
                {
                    return heightMap[y * width + x];
                }
                return int.MaxValue;
            }
            set { heightMap[y * width + x] = value; }
        }

        public int this[Point p] {
            get => this[p.x, p.y];
            set => this[p.x, p.y] = value;
        }

        public World(int[] raw, int w, int h)
        {
            this.width = w;
            this.height = h;
            heightMap = new int[width * height];
            Array.Copy(raw, heightMap, raw.Length);
        }

        public IEnumerable<Point> GetLowPoints()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var top = this[x, y - 1];
                    var down = this[x, y + 1];
                    var left = this[x - 1, y];
                    var right = this[x + 1, y];
                    if (this[x, y] < top
                     && this[x, y] < down
                     && this[x, y] < left
                     && this[x, y] < right)
                    {
                        yield return new Point(x, y, this[x, y]);
                    }
                }
            }
        }

        public IEnumerable<Point> GetBasin(Point p)
        {
            HashSet<Point> visited = new HashSet<Point>();

            IEnumerable<Point> basin(Point p)
            {
                if (!visited.Contains(p) && p.height != 9)
                {
                    visited.Add(p);
                    yield return p;

                    foreach (var a in GetAdjacent(p))
                    {
                        foreach (var c in basin(a))
                        {
                            yield return c;
                        }
                    }
                }
            }
            return basin(p);

            //Predicate<int, int> notWall = (x, y) => this[x, y] != 9;
            //var floodParam = new FloodParameters(startX: p.x, startY: p.y)
            //{
            //    NeighbourStopCondition = (x, y) => this[x, y] == 9,
            //};
            //var matrix = new int[width, height];
            //for (int y = 0; y < height; y++)
            //{
            //    for (int x = 0; x < width; x++)
            //    {
            //        matrix[x, y] = this[x, y];
            //    }
            //}

            //var tt = new FloodSpiller().SpillFlood(floodParam, matrix);
            //string representation = MarkMatrixVisualiser.Visualise(matrix);
            //Console.WriteLine(representation);

            //for (int y = 0; y < height; y++)
            //{
            //    for (int x = 0; x < width; x++)
            //    {
            //        if (matrix[x, y] < 9)
            //        {
            //            yield return new Point(x, y, this[x, y]);
            //        }
            //    }
            //}

            //Queue<Point> points = new Queue<Point>();
            //HashSet<Point> visited = new HashSet<Point>();
            //points.Enqueue(p);
            //while (points.Count > 0)
            //{
            //    Point cur = points.Dequeue();

            //    if (!visited.Contains(cur) && cur.height != 9)
            //    {
            //        visited.Add(cur);
            //        yield return cur;

            //        foreach (Point a in GetAdjacent(cur))
            //        {
            //            points.Enqueue(a);
            //        }
            //    }
            //}

            //if (p.height != 9)
            //{
            //    Point left = GetPoint(p.left);
            //    Point right = GetPoint(p.right);
            //    Point top = GetPoint(p.top);
            //    Point down = GetPoint(p.down);

            //    if (left != null)
            //    {
            //        foreach (var bp in GetBasin(left))
            //        {
            //            yield return bp;
            //        }
            //    }

            //    if (right != null)
            //    {
            //        foreach (var bp in GetBasin(right))
            //        {
            //            yield return bp;
            //        }
            //    }

            //    if (top != null)
            //    {
            //        foreach (var bp in GetBasin(top))
            //        {
            //            yield return bp;
            //        }
            //    }

            //    if (down != null)
            //    {
            //        foreach (var bp in GetBasin(down))
            //        {
            //            yield return bp;
            //        }
            //    }

            //    yield return p;
            //}
            //else
            //{
            //    yield break;
            //}
        }

        public IEnumerable<Point> GetAdjacent(Point p)
        {
            if (p.x > 0) yield return new Point(p.x - 1, p.y, this[p.x - 1, p.y]);
            if (p.x < width - 1) yield return new Point(p.x + 1, p.y, this[p.x + 1, p.y]);
            if (p.y > 0) yield return new Point(p.x, p.y - 1, this[p.x, p.y - 1]);
            if (p.y < height - 1) yield return new Point(p.x, p.y + 1, this[p.x, p.y + 1]);

        }

        private Point GetPoint(Point p)
        {
            if (p.x < 0 || p.x >= width
             || p.y < 0 || p.y >= height)
            {
                return null;
            }

            p.height = this[p];
            return p;
        }

        public void RenderBasins(IEnumerable<Point> basin, IEnumerable<Point> lowPoints = null)
        {
            basin = basin.ToList();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (basin.FirstOrDefault(p => p.x == x && p.y == y) == null)
                    {
                        if (this[x, y] == 9)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    if (lowPoints != null && lowPoints.FirstOrDefault(p => p.x == x && p.y == y) != null)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                    }
                    Console.Write(this[x, y]);
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }
    }

    class Point
    {
        public int x;
        public int y;
        public int height;

        public Point left => new Point(x - 1, y, -1);
        public Point right => new Point(x + 1, y, -1);
        public Point top => new Point(x, y - 1, -1);
        public Point down => new Point(x, y + 1, -1);

        public Point(int x, int y, int h)
        {
            this.x = x;
            this.y = y;
            this.height = h;
        }

        public override bool Equals(object obj)
        {
            return obj.GetType() == typeof(Point) && obj.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 13;
                hash = (hash * 7) + x.GetHashCode();
                hash = (hash * 7) + y.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return $"[{x},{y},{height}]";
        }
    }
}
