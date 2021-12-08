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

        public virtual string Part1()
        {
            return sb.ToString();
        }

        public virtual string Part2()
        {
            return sb.ToString();
        }

        public abstract void UseInput();

        public abstract void UseSample();
    }
}