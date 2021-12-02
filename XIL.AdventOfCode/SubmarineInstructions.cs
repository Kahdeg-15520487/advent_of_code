using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using XIL.LangDef;
using XIL.VM;
using XIL.StandardLibrary;
using System.Linq;

namespace XIL.AdventOfCode
{
    public class SubmarineInstructions : IInstructionImplementation
    {
        const string lib = "submarine";
        int horizontal = 0;
        int depth = 0;
        int aim = 0;
        bool useAim = false;

        [Instruction(0xC0, "forward", lib)]
        public void Forward(Thread thread, int op1, int op2)
        {
            horizontal += op1;
            if (useAim)
            {
                depth += aim * op1;
            }
        }

        [Instruction(0xC1, "down", lib)]
        public void Down(Thread thread, int op1, int op2)
        {
            if (useAim)
            {
                aim += op1;
            }
            else
            {
                depth += op1;
            }
        }

        [Instruction(0xC2, "up", lib)]
        public void Up(Thread thread, int op1, int op2)
        {
            if (useAim)
            {
                aim -= op1;
            }
            else
            {
                depth -= op1;
            }
        }

        [Instruction(0xC3, "get_hor", lib)]
        public void GetHorizontal(Thread thread, int op1, int op2)
        {
            thread.Push(horizontal);
        }

        [Instruction(0xC4, "get_dep", lib)]
        public void GetDepth(Thread thread, int op1, int op2)
        {
            thread.Push(depth);
        }

        [Instruction(0xC5, "use_aim", lib)]
        public void UseAim(Thread thread, int op1, int op2)
        {
            useAim = true;
        }
    }
}
