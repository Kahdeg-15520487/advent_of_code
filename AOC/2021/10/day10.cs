using AOC;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace year2021.day10
{
    public class day10 : Day
    {
        private string[] inputs;
        private (string, char[])[] incompeletes;

        public override void UseInput()
        {
            inputs = Utility.input.ReadByLines();
        }

        public override void UseSample()
        {
            throw new NotImplementedException();
        }
        public override string Part1()
        {
            List<(string, char[])> ic = new List<(string, char[])>();
            var points = 0;
            foreach (var line in inputs)
            {
                Stack<char> awaitClose = new Stack<char>();
                var chunkCounter = 0;
                var corruptedClose = '\0';
                var expectedClose = '\0';
                foreach (var c in line)
                {
                    if (c == '('
                     || c == '['
                     || c == '{'
                     || c == '<')
                    {
                        chunkCounter++;
                        sb.AppendLine($"{new string('\t', chunkCounter)}{c}");
                        switch (c)
                        {
                            case '(':
                                awaitClose.Push(')');
                                break;
                            case '[':
                                awaitClose.Push(']');
                                break;
                            case '{':
                                awaitClose.Push('}');
                                break;
                            case '<':
                                awaitClose.Push('>');
                                break;
                        }
                    }
                    else if (c == ')'
                          || c == ']'
                          || c == '}'
                          || c == '>')
                    {
                        if (c == awaitClose.Peek())
                        {
                            sb.AppendLine($"{new string('\t', chunkCounter)}{c}");
                            chunkCounter--;
                            awaitClose.Pop();
                        }
                        else
                        {
                            corruptedClose = c;
                            expectedClose = awaitClose.Peek();
                            break;
                        }
                    }
                }
                if (chunkCounter != 0)
                {
                    if (corruptedClose == '\0')
                    {
                        sb.AppendLine($"{line} is incomplete");
                        ic.Add((line, awaitClose.ToArray()));
                    }
                    else
                    {
                        sb.AppendLine($"{line} is corrupted, expected {expectedClose}, found {corruptedClose}");
                        switch (corruptedClose)
                        {
                            case ')':
                                points += 3;
                                break;
                            case ']':
                                points += 57;
                                break;
                            case '}':
                                points += 1197;
                                break;
                            case '>':
                                points += 25137;
                                break;
                        }
                    }
                }
                else
                {
                    sb.AppendLine($"{line} is complete");
                }
            }
            sb.Append($"total point: {points}");

            incompeletes = ic.ToArray();

            return base.Part1();
        }

        public override string Part2()
        {
            List<long> totalPoints = new List<long>();
            foreach ((string line, char[] expectedClose) in incompeletes)
            {
                long points = 0;
                foreach (var c in expectedClose)
                {
                    points *= 5;
                    switch (c)
                    {
                        case ')':
                            points += 1;
                            break;
                        case ']':
                            points += 2;
                            break;
                        case '}':
                            points += 3;
                            break;
                        case '>':
                            points += 4;
                            break;
                    }
                }
                sb.AppendLine($"{new string(expectedClose)} - {points}");
                totalPoints.Add(points);
            }
            totalPoints.Sort();
            sb.Append(totalPoints[totalPoints.Count / 2]);

            return base.Part2();
        }
    }
}
