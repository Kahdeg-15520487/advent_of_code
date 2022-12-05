using System.Text;

namespace AOC
{
    public interface IDay
    {
        void UseInput();
        void UseSample();
        string Part1();
        string Part2();
    }

    public abstract class Day : IDay
    {
        protected StringBuilder sb = new StringBuilder();
        protected string[] inputs;

        public virtual string Part1()
        {
            var t = sb.ToString();
            sb.Clear();
            return t;
        }

        public virtual string Part2()
        {
            var t = sb.ToString();
            sb.Clear();
            return t;
        }

        public virtual void UseInput()
        {
            inputs = Utility.input.ReadByLines();
        }

        public virtual void UseSample()
        {
            inputs = Utility.sample.ReadByLines();
        }
    }
}