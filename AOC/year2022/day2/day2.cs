using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC.year2022.day2
{
    public class day2 : Day
    {
        private (hand o, hand m)[] rounds;
        enum hand
        {
            rock = 1,
            paper = 2,
            scissor = 3,
        }

        public override void UseInput()
        {
            base.UseInput();
            rounds = inputs.Select(i => i.Split()).Select(r =>
            {
                var o = r[0] switch
                {
                    "A" => hand.rock,
                    "B" => hand.paper,
                    "C" => hand.scissor,
                };
                var m = r[1] switch
                {
                    "X" => hand.rock,
                    "Y" => hand.paper,
                    "Z" => hand.scissor,
                };
                return (o, m);
            }).ToArray();
        }

        public override void UseSample()
        {
            base.UseSample();

        }

        public override string Part1()
        {
            long score = 0;
            foreach (var round in rounds)
            {
                var opponent = round.o;
                var me = round.m;
                score += (int)me;
                if (opponent == me)
                {
                    score += 3;
                }
                else
                {
                    score += round switch
                    {
                        (hand.rock, hand.scissor) => 0,
                        (hand.rock, hand.paper) => 6,
                        (hand.paper, hand.rock) => 0,
                        (hand.paper, hand.scissor) => 6,
                        (hand.scissor, hand.paper) => 0,
                        (hand.scissor, hand.rock) => 6,
                    };
                }
            }
            return score.ToString();
        }

        private hand lose(hand opponent) => opponent switch
        {
            hand.rock => hand.scissor,
            hand.paper => hand.rock,
            hand.scissor => hand.paper,
        };

        private hand win(hand opponent) => opponent switch
        {
            hand.rock => hand.paper,
            hand.paper => hand.scissor,
            hand.scissor => hand.rock,
        };

        public override string Part2()
        {
            long score = 0;
            foreach (var round in rounds)
            {
                var opponent = round.o;
                var me = round.m switch
                {
                    hand.rock => lose(opponent),
                    hand.paper => opponent,
                    hand.scissor => win(opponent),
                };
                score += (int)me;
                if (opponent == me)
                {
                    score += 3;
                }
                else
                {
                    score += (opponent, me) switch
                    {
                        (hand.rock, hand.scissor) => 0,
                        (hand.rock, hand.paper) => 6,
                        (hand.paper, hand.rock) => 0,
                        (hand.paper, hand.scissor) => 6,
                        (hand.scissor, hand.paper) => 0,
                        (hand.scissor, hand.rock) => 6,
                    };
                }
            }
            return score.ToString();
        }
    }
}
