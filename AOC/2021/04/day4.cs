﻿using AOC;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace year2021.day4
{
    class day4 : Day
    {
        string[] inputs;
        private (Board first, Board last) result;

        void d(string[] args)
        {
            var raw = File.ReadAllText("input").Split(new[] { $"{Environment.NewLine}{Environment.NewLine}", "\n\n" }, StringSplitOptions.RemoveEmptyEntries).ToArray();

        }

        public override void UseInput()
        {
            inputs = Utility.Get_StringList_Custom(Utility.input.Read(), $"{Environment.NewLine}{Environment.NewLine}", "\n\n");
        }

        public override void UseSample()
        {
            inputs = Utility.Get_StringList_Custom(Utility.sample.Read(), $"{Environment.NewLine}{Environment.NewLine}", "\n\n");
        }

        public override string Part1()
        {
            this.result = Solve();

            sb.AppendFormat("first bingo: score: {0}", this.result.first.score);
            sb.AppendLine();
            sb.AppendLine(this.result.first.ToString());

            return base.Part1();
        }

        public override string Part2()
        {
            sb.AppendFormat("last bingo: score: {0}", this.result.last.score);
            sb.AppendLine();
            sb.Append(this.result.last);
            return base.Part2();
        }

        private (Board first, Board last) Solve()
        {
            var drawn = Utility.ParseToInt_StringArr(Utility.Get_StringList_Comma(inputs[0]));// inputs[0].Split(",", StringSplitOptions.RemoveEmptyEntries).Select(c => int.Parse(c)).ToArray();
            var boards = inputs.Skip(1).Select(rb => new Board(rb.Split(new[] { " ", "\n", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Select(c => int.Parse(c)).ToArray())).ToList();
            var bingoed = new List<int>();
            foreach (var d in drawn)
            {
                //Console.WriteLine("drawn: {0}", d);
                int bi = 0;
                foreach (var b in boards)
                {
                    //Console.WriteLine("board {0}", bi);
                    if (b.isBingoed)
                    {
                        //Console.WriteLine("bingoed, skipping");
                        bi++;
                        continue;
                    }
                    var score = b.GetScore(d);
                    if (score > 0)
                    {
                        //Console.WriteLine("bingo {0}", score);
                        bingoed.Add(bi);
                    }
                    //Console.WriteLine(b);
                    //Console.WriteLine("=====");
                    bi++;
                }
            }
            return (boards[bingoed.First()], boards[bingoed.Last()]);
        }
    }

    class Board
    {
        private int[] board;
        private int[] check;
        public int size;
        public int score;
        public bool isBingoed = false;

        public (int n, int c) this[int x, int y]
        {
            get { return (board[y * size + x], check[y * size + x]); }
            set
            {
                board[y * size + x] = value.n;
                check[y * size + x] = value.c;
            }
        }

        public Board(int[] flatBoard)
        {
            board = new int[flatBoard.Length];
            check = new int[flatBoard.Length];
            for (int i = 0; i < check.Length; i++)
            {
                check[i] = 0;
            }
            Array.Copy(flatBoard, board, flatBoard.Length);
            size = (int)Math.Sqrt(flatBoard.Length);
        }

        private void Check(int number)
        {
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    if (this[x, y].n == number)
                    {
                        check[y * size + x] = 1;
                    }
                }
            }
        }
        private bool IsBingo()
        {
            //check row
            for (int y = 0; y < size; y++)
            {
                int sum = 0;
                for (int x = 0; x < size; x++)
                {
                    sum += this[x, y].c;
                }
                if (sum == 5)
                {
                    return true;
                }
            }

            //check collumn
            for (int x = 0; x < size; x++)
            {
                int sum = 0;
                for (int y = 0; y < size; y++)
                {
                    sum += this[x, y].c;
                }
                if (sum == 5)
                {
                    return true;
                }
            }

            return false;
        }

        public int GetScore(int number)
        {
            this.Check(number);
            if (this.IsBingo())
            {
                //calc score
                score = 0;
                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size; y++)
                    {
                        if (this[x, y].c == 0)
                        {
                            score += this[x, y].n;
                        }
                    }
                }
                isBingoed = true;
                score *= number;
                return score;
            }
            else
            {
                return -1;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    sb.AppendFormat("[{0},{1}]", this[x, y].n.ToString().PadLeft(2, ' '), this[x, y].c == 1 ? "X" : " ");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
