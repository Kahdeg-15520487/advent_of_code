using AOC;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace year2021.day12
{
    public class day12 : Day
    {
        private List<string[]> inputs;

        public override void UseInput()
        {
            inputs = Utility.sample.ReadByLines().Select(l => Utility.Get_StringList_Comma(l)).ToList();
        }

        public override void UseSample()
        {
            throw new NotImplementedException();
        }
        public override string Part1()
        {


            return base.Part1();
        }

        public override string Part2()
        {
            return base.Part2();
        }
    }

    class Cave
    {
        public bool IsBig { get; private set; }
        //public List<Cave> 
    }
}
