using AOC;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using XIL.Assembler;
using XIL.LangDef;
using XIL.VM;

namespace year2021.day2
{
    class day2 : Day
    {
        private string p1;
        private string p2;
        private IInstructionImplementation[] libs;
        private Assembler assembler;

        public day2()
        {
            this.libs = GetDirectoryPlugins<IInstructionImplementation>(Directory.GetCurrentDirectory()).ToArray();
            this.assembler = new Assembler(libs);
        }

        public override void UseInput()
        {
            p1 = Utility.input.Read() + Environment.NewLine + @"get_hor
get_dep
mul
print
exit
";

            p2 = "use_aim" + Environment.NewLine + Utility.input.Read() + Environment.NewLine + @"get_hor
get_dep
mul
print
exit
";
        }

        public override void UseSample()
        {
            p1 = Utility.sample.Read() + Environment.NewLine + @"get_hor
get_dep
mul
print
exit
";

            p2 = "use_aim" + Environment.NewLine + Utility.sample.Read() + Environment.NewLine + @"get_hor
get_dep
mul
print
exit
";
        }

        public override string Part1()
        {
            var t = Console.Out;
            Console.SetOut(TextWriter.Null);
            var compiled = assembler.Compile(p1);
            Console.SetOut(t);
            Solve(compiled);

            return base.Part1();
        }

        public override string Part2()
        {
            var t = Console.Out;
            Console.SetOut(TextWriter.Null);
            var compiled = assembler.Compile(p2);
            Console.SetOut(t);
            Solve(compiled);

            return base.Part2();
        }

        private void Solve(CompileResult compiled)
        {
            var vm = new VirtualMachine(VirtualMachineVerboseLevel.None, libs);
            var program = compiled.CodeGenerator.Emit();
            Instruction[] instr;
            string[] strTable;

            byte[] buffer;
            using (var stream = new MemoryStream())
            {
                XIL.VM.Program.Serialize(stream, program);
                buffer = stream.ToArray();
            }

            using (var stream = new MemoryStream(buffer))
            {
                XIL.VM.Program.Deserialize(stream, out instr, out strTable);
            }
            vm.LoadProgram(instr, strTable);
            vm.Run();
        }

        static void HOST_print(XIL.VM.Thread thread)
        {
            var value = thread.Pop();
            Console.WriteLine(value);
        }

        public static List<T> GetDirectoryPlugins<T>(string dirname)
        {
            List<T> ret = new List<T>();
            string[] dlls = Directory.EnumerateFiles(dirname).Where(x => x.EndsWith(".dll") || x.EndsWith(".exe")).ToArray();
            foreach (string dll in dlls)
            {
                List<T> dll_plugins = GetFilePlugins<T>(Path.GetFullPath(dll));
                ret.AddRange(dll_plugins);
            }
            return ret;
        }

        public static List<T> GetFilePlugins<T>(string filename)
        {
            List<T> ret = new List<T>();
            if (File.Exists(filename))
            {
                Type typeT = typeof(T);
                Assembly assembly;
                try
                {
                    assembly = Assembly.LoadFrom(filename);
                }
                catch
                {
                    return ret;
                }
                foreach (Type type in assembly.GetTypes())
                {
                    if (!type.IsClass || type.IsNotPublic)
                        continue;
                    if (typeT.IsAssignableFrom(type))
                    {
                        T plugin = (T)Activator.CreateInstance(type);
                        ret.Add(plugin);
                    }
                }
            }
            return ret;
        }
    }
}
