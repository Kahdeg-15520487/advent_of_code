using AOC;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tesseract;

namespace year2021.day13
{
    public class day13 : Day
    {
        private (int x, int y)[] rawPaper;
        private (string instr, int param)[] rawInstr;
        private int width;
        private int height;

        public override void UseInput()
        {
            var raw = Utility.input.Read().Split(new string[] { $"{Environment.NewLine}{Environment.NewLine}", "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
            rawPaper = raw[0].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(l =>
            {
                var tt = l.Split(",");
                var x = int.Parse(tt[0]);
                var y = int.Parse(tt[1]);
                return (x, y);
            }).ToArray();
            rawInstr = raw[1].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(l =>
            {
                var tt = l.Replace("fold along ", string.Empty);
                var ttt = tt.Split("=");
                var instr = ttt[0];
                var param = int.Parse(ttt[1]);
                return (instr, param);
            }).ToArray();

            width = rawPaper.Max(c => c.x) + 1;
            height = rawPaper.Max(c => c.y) + 1;
            //Console.WriteLine(Render(rawPaper, width, height));
        }

        public override void UseSample()
        {
            throw new NotImplementedException();
        }
        public override string Part1()
        {
            var instr = rawInstr.First();
            (int x, int y)[] folded;
            if (instr.instr == "x")
            {
                folded = Fold(rawPaper, false, instr.param);
            }
            else
            {
                folded = Fold(rawPaper, true, instr.param);
            }
            sb.Append(folded.Length);

            return base.Part1();
        }

        public override string Part2()
        {

            //var instr = rawInstr.First();
            (int x, int y)[] folded = new (int x, int y)[rawPaper.Length];
            Array.Copy(rawPaper, folded, rawPaper.Length);
            var w = width;
            var h = height;
            //sb.AppendLine(Render(folded, w, h));
            foreach (var instr in rawInstr)
            {
                if (instr.instr == "x")
                {
                    folded = Fold(folded, false, instr.param);
                    w = instr.param;
                }
                else
                {
                    folded = Fold(folded, true, instr.param);
                    h = instr.param;
                }
                //sb.AppendLine(Render(folded, w, h));
            }

            //File.WriteAllText("d13.txt", sb.ToString());

            using (Bitmap bmp = new Bitmap(w * 2, h * 2))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.White);
                }

                foreach (var c in folded)
                {
                    bmp.SetPixel(c.x + w / 2, c.y + h / 2, Color.Black);
                }

                using (var org = new Bitmap(w * 4, h * 4))
                {
                    float scale = 4;
                    using (Graphics g = Graphics.FromImage(org))
                    {
                        g.Clear(Color.White);
                        g.DrawImage(bmp, 0, 0, w * 4, h * 4);
                    }
                    org.Save("d13.4x.png", System.Drawing.Imaging.ImageFormat.Png);
                }
            }

            sb.Append(OCR());

            return base.Part2();
        }

        private string OCR()
        {
            try
            {
                using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                {
                    using (var img = Pix.LoadFromFile("d13.4x.png"))
                    {
                        using (var page = engine.Process(img))
                        {
                            return page.GetText();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                Console.WriteLine("Unexpected Error: " + e.Message);
                Console.WriteLine("Details: ");
                Console.WriteLine(e.ToString());
                return "<err>";
            }
        }

        private (int x, int y)[] Fold((int x, int y)[] rawPaper, bool isFoldHorizontal, int foldCoord)
        {
            if (isFoldHorizontal)
            {
                var upper = rawPaper.Where(m => m.y < foldCoord);
                var lower = rawPaper.Where(m => m.y >= foldCoord).Select(m => (m.x, (int)Math.Abs(m.y - 2 * foldCoord)));
                return upper.Concat(lower).Distinct().ToArray();

                // 0,14 -> 0,0 : 
                // 0,13 -> 0,1 : newY = abs(oldY - 2*foldCoord)
            }
            else
            {
                var left = rawPaper.Where(m => m.x < foldCoord);
                var right = rawPaper.Where(m => m.x >= foldCoord).Select(m => ((int)Math.Abs(m.x - 2 * foldCoord), m.y));
                return left.Concat(right).Distinct().ToArray();
            }
        }

        private string Render((int x, int y)[] paper, int width, int height)
        {
            StringBuilder sb = new StringBuilder();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (paper.Any(c => c.x == x && c.y == y))
                    {
                        sb.Append("#");
                    }
                    else
                    {
                        sb.Append(".");
                    }
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
