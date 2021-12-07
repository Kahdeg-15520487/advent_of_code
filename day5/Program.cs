using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace day5
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input").Select(l => Line.Parse(l)).ToArray();
            //var lines = File.ReadAllLines("sample").Select(l => Line.Parse(l)).ToArray();
            World world = new World(lines, true);
            Console.WriteLine("hor/ver only: {0}", world.GetOverlapPoint().Count());
            world = new World(lines, false);
            Console.WriteLine("hor/ver/diag: {0}", world.GetOverlapPoint().Count());
            
        }
    }

    class World
    {
        const int size = 1000;
        int[] matrix;
        public int this[int x, int y] {
            get { return matrix[y * size + x]; }
            set { matrix[y * size + x] = value; }
        }

        public int this[Point p] {
            get { return matrix[p.Y * size + p.X]; }
            set { matrix[p.Y * size + p.X] = value; }
        }

        public World(IEnumerable<Line> lines, bool countDiagonal = false)
        {
            matrix = new int[size * size];
            foreach (var l in lines)
            {
                PlaceLine(l, countDiagonal);
            }
        }

        public IEnumerable<Point> GetOverlapPoint()
        {
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    if (this[x, y] > 1)
                    {
                        yield return new Point(x, y);
                    }
                }
            }
        }

        private void PlaceLine(Line l, bool countDiagonal)
        {
            var t = l.Iterate(countDiagonal).ToArray();
            foreach (var p in t)
            {
                this[p] += 1;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    var t = this[x, y];
                    if (t == 0)
                    {
                        sb.Append(".");
                    }
                    else if (t == 1)
                    {
                        sb.Append(t);
                    }
                    else
                    {
                        sb.Append(t);
                    }
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }

    class Line
    {
        public Point A { get; set; }
        public Point B { get; set; }
        public bool IsHorizontal => A.Y == B.Y;
        public bool IsVertical => A.X == B.X;
        public bool IsHorVer => IsHorizontal || IsVertical;
        public Line(Point a, Point b)
        {
            this.A = a;
            this.B = b;
        }
        public Line(int x1, int y1, int x2, int y2)
        {
            this.A = new Point(x1, y1);
            this.B = new Point(x2, y2);
        }

        public static Line Parse(string raw)
        {
            var points = raw.Split("->").Select(p => Point.Parse(p.Trim())).ToArray();
            return new Line(points[0], points[1]);
        }

        public IEnumerable<Point> Iterate(bool countDiagonal)
        {
            if (IsHorizontal)
            {
                if (A.X < B.X)
                {
                    for (int x = A.X; x <= B.X; x++)
                    {
                        yield return new Point(x, A.Y);
                    }
                }
                else
                {

                    for (int x = B.X; x <= A.X; x++)
                    {
                        yield return new Point(x, A.Y);
                    }
                }
            }
            else if (IsVertical)
            {
                if (A.Y < B.Y)
                {
                    for (int y = A.Y; y <= B.Y; y++)
                    {
                        yield return new Point(A.X, y);
                    }
                }
                else
                {
                    for (int y = B.Y; y <= A.Y; y++)
                    {
                        yield return new Point(A.X, y);
                    }
                }
            }
            else if (countDiagonal) // diagonal
            {
                if (A.X < B.X)
                {
                    if (A.Y < B.Y) //xmax = b.x-a.x; (a.x,a.y) -> (a.x+xmax,a.y+xmax)
                    {
                        int xmax = B.X - A.X;
                        for (int x = 0; x <= xmax; x++)
                        {
                            yield return new Point(A.X + x, A.Y + x);
                        }
                    }
                    else // xmax = b.x-a.x; ymax = a.x;(a.x,a.y) -> (a.x+xmax,a.y-ymax)
                    {
                        int xmax = B.X - A.X;
                        for (int x = 0; x <= xmax; x++)
                        {
                            yield return new Point(A.X + x, A.Y - x);
                        }
                    }
                }
                else
                {
                    if (A.Y < B.Y) //xmax = a.x-b.x; ymax = b.y-a.y; (a.x,a.y) -> (a.x-xmax,a.y+ymax)
                    {
                        int ymax = B.Y - A.Y;
                        for (int y = 0; y <= ymax; y++)
                        {
                            yield return new Point(A.X - y, A.Y + y);
                        }
                    }
                    else //xmax = a.x-b.x; ymax = a.y-b.y; (b.x,b.y) -> (b.x+xmax,b.y+ymax)
                    {
                        int ymax = A.Y - B.Y;
                        for (int y = 0; y <= ymax; y++)
                        {
                            yield return new Point(B.X + y, B.Y + y);
                        }
                    }
                }
            }
        }

        public override string ToString()
        {
            return $"{A} -> {B}";
        }
    }

    class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public static Point Parse(string raw)
        {
            var coord = raw.Split(",").Select(n => int.Parse(n)).ToArray();
            return new Point(coord[0], coord[1]);
        }

        public override string ToString()
        {
            return $"{X},{Y}";
        }
    }
}
