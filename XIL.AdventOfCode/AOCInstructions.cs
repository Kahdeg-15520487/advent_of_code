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
    public class AOCInstructions : IInstructionImplementation
    {
        const string lib = "aoc";

        static string filePath = null;
        static string fileContent = null;
        static int cursorPos = 0;

        /// <summary>
		/// open &lt;path&gt; <para/>
		/// open the file at path to read and write
		/// </summary>
		[Instruction(0xB0, "aoc_read", lib)]
        public void OpenFile(Thread thread, int operand1, int operand2)
        {
            //todo implement file open
            //open a file if it exist, load the file content into fileContent
            var path = thread.GetString(operand1);
            if (!File.Exists(path))
            {
                thread.RuntimeError(string.Format("{0} not found", path));
                return;
            }

            filePath = path;
            fileContent = File.ReadAllText(filePath);
            cursorPos = 0;
            var t = fileContent.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(l => int.Parse(l)).ToArray();
            thread.PushArray(t);
            thread.Push(t.Length);
        }

        [Instruction(0xB1, "aoc_cd", lib)]
        public void ChangeWorkDir(Thread thread, int op1, int op2)
        {
            var path = thread.GetString(op1);
            Directory.SetCurrentDirectory(path);
        }

        [Instruction(0xB2, "aoc_tsi", lib)]
        public void GetStackTopIndex(Thread thread, int op1, int op2)
        {
            Console.WriteLine("current stackIndex: {0}", thread.StackTopIndex);
        }

        [Instruction(0xB3, "aoc_sg", lib)]
        public void GrowStack(Thread thread, int op1, int op2)
        {
            thread.Grow(op1);
        }

        [Instruction(0xB5, "aoc_gets", lib)]
        public void GetAt(Thread thread, int op1, int op2)
        {
            int stackIndex = op1;
            int value = thread.Get(stackIndex);
            thread.Push(value);
        }

        [Instruction(0xB6, "aoc_sets", lib)]
        public void SetAt(Thread thread, int op1, int op2)
        {
            int value = thread.Pop();
            int stackIndex = op1;
            thread.Set(stackIndex, value);
        }
    }
}
