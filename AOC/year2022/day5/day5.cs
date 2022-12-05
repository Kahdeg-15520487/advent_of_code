using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC.year2022.day5
{
    public class day5 : Day
    {
        private List<string>[] columns;
        private List<string[]> image;
        private int columnCount;
        private List<(int amount, int src, int dest)> commands;

        public override void UseInput()
        {
            base.UseInput();
            //base.UseSample();
            var commandstart = 0;
            for (int i = 0; i < inputs.Length; i++)
            {
                if (inputs[i].StartsWith("move"))
                {
                    commandstart = i;
                    break;
                }
            }

            commands = new();
            for (int i = commandstart; i < inputs.Length; i++)
            {
                var l = inputs[i].Split(new string[] { "move", "from", "to" }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                commands.Add((int.Parse(l[0]), int.Parse(l[1]) - 1, int.Parse(l[2]) - 1));
            }

            image = new List<string[]>();
            columnCount = GroupBy4(inputs[commandstart - 2]).Length;
            for (int i = commandstart - 3; i >= 0; i--)
            {
                image.Add(GroupBy4(inputs[i]).Select(g => g.Trim(' ', '[', ']')).ToArray());
            }
            columns = new List<string>[columnCount];
            for (int i = 0; i < image.Count; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    if (!string.IsNullOrEmpty(image[i][j]))
                    {
                        if (columns[j] == null)
                        {
                            columns[j] = new List<string>();
                        }
                        columns[j].Add(image[i][j]);
                    }
                }
            }
        }

        public override string Part1()
        {
            var cs = columns.ToList().ToArray();
            Render(cs, (0, -1, -1));
            foreach (var c in commands)
            {
                var tobemoved = cs[c.src].TakeLast(c.amount).ToList();
                tobemoved.ForEach(t => cs[c.src].Remove(t));
                cs[c.dest].AddRange(tobemoved.AsEnumerable().Reverse());
                Console.WriteLine("move {0} from {1} to {2}", c.amount, c.src + 1, c.dest + 1);
                Render(cs, c, tobemoved);
                Console.ReadLine();
            }

            return string.Join("", cs.Select(c => c.Last()));
        }

        private void Render(List<string>[] cols, (int amount, int src, int dest) command, List<string> moved = null)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                if (i == command.src)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                if (i == command.dest)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                Console.Write(" {0}  ", i + 1);
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine();
            for (int i = 0; i < cols.Max(l => l.Count) + (moved == null ? 0 : moved.Count); i++)
            {
                if (cols.All(c => i > cols.Max(l => l.Count) + moved?.Count))
                {
                    continue;
                }
                for (int j = 0; j < cols.Length; j++)
                {
                    if (i < cols[j].Count)
                    {
                        if (j == command.dest && i >= cols[j].Count - command.amount)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        Console.Write(" {0}  ", cols[j][i]);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        if (j == command.src && (i - cols[j].Count) < moved.Count)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(" {0}  ", moved[i - cols[j].Count]);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            Console.Write("    ");
                        }
                    }
                }
                Console.WriteLine();
            }
        }

        private string[] GroupBy4(string inp)
        {
            int i = 0;
            var query = from s in inp
                        let num = i++
                        group s by num / 4 into g
                        select new string(g.ToArray());
            return query.ToArray();
        }
    }
}
