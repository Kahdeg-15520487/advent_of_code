using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AOC.year2022.day7
{
    public class day7 : Day
    {
        class File
        {
            public string name;
            public bool IsDir => childFiles.Count != 0;
            private int _size;
            public int size
            {
                get { return childFiles.Count == 0 ? _size : childFiles.Sum(f => f.size); }
                set { _size = value; }
            }
            public Dictionary<string, File> childs;
            public List<File> childFiles => childs.Select(kvp => kvp.Value).ToList();
            public File parent;

            public File(string name, int size, File parent, List<File> childs)
            {
                this.name = name;
                this._size = size;
                this.parent = parent;
                this.childs = childs.ToDictionary(c => c.name, c => c);
            }

            public int CalcSize(Func<File, bool> predicate)
            {
                if (!IsDir)
                {
                    return predicate(this) ? _size : 0;
                }
                else
                {
                    return childFiles.Where(predicate).Sum(f => f.CalcSize(predicate));
                }
            }

            public IEnumerable<File> Query(Func<File, bool> predicate)
            {
                if (IsDir)
                {
                    foreach (var cf in childFiles.SelectMany(f => f.Query(predicate)))
                    {
                        yield return cf;
                    }
                }

                if (predicate(this))
                {
                    yield return this;
                }
            }

            public override string ToString()
            {
                if (IsDir)
                {
                    return $"dir {size} {name}";
                }
                else
                {
                    return $"{size} {name}";
                }
            }

            public void PrintPretty(string indent, bool last)
            {
                Console.Write(indent);
                if (last)
                {
                    Console.Write("\\-");
                    indent += "  ";
                }
                else
                {
                    Console.Write("|-");
                    indent += "| ";
                }
                Console.WriteLine(this);

                for (int i = 0; i < childFiles.Count; i++)
                    childFiles[i].PrintPretty(indent, i == childFiles.Count - 1);
            }
        }

        File root;

        public override void UseInput()
        {
            base.UseInput();
            //base.UseSample();

            root = new File("root", 0, null, new List<File>());
            File current = root;

            int currLine = 0;
            do
            {
                var c = inputs[currLine];
                if (c.StartsWith("$"))
                {
                    var cmd = c.Split(" ");
                    var verb = cmd[1];
                    var path = verb == "cd" ? cmd[2] : string.Empty;

                    switch (verb)
                    {
                        case "cd":
                            {
                                if (path == "..")
                                {
                                    current = current.parent;
                                }
                                else if (path == "/")
                                {
                                    current = root;
                                }
                                else
                                {
                                    current = current.childs[path];
                                }
                            }
                            currLine++;
                            break;
                        case "ls":
                            {
                                currLine++;
                                while (currLine < inputs.Length && !inputs[currLine].StartsWith("$"))
                                {
                                    var l = inputs[currLine].Split();
                                    if (l[0] == "dir")
                                    {
                                        if (!current.childs.ContainsKey(l[1]))
                                        {
                                            current.childs.Add(l[1], new File(l[1], 0, current, new List<File>()));
                                        }
                                    }
                                    else
                                    {
                                        current.childs.Add(l[1], new File(l[1], int.Parse(l[0]), current, new List<File>()));
                                    }
                                    currLine++;
                                }
                            }
                            break;
                    }
                }
            } while (currLine < inputs.Length);

            root.PrintPretty("", false);
        }

        public override string Part1()
        {
            var t = root.Query(f => f.IsDir && f.size <= 100000).ToList();
            return "dir with total size under 100000:" + Environment.NewLine + string.Join(Environment.NewLine, t) + Environment.NewLine + t.Sum(f => f.size).ToString();
        }

        public override string Part2()
        {
            var totalSpace = 70000000;
            var required = 30000000 - (totalSpace - root.size);
            return root.Query(f => f.IsDir && f.size > required).OrderBy(f => f.size).First().ToString();
        }
    }
}
