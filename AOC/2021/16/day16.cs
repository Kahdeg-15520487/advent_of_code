using AOC;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace year2021.day16
{
    public class day16 : Day
    {
        private Packet packet;

        public override void UseInput()
        {
            (packet, _) = PacketParser.Parse(Utility.input.Read());
        }

        public override void UseSample()
        {
            throw new NotImplementedException();
        }
        public override string Part1()
        {
            PacketVersionSumCalculator packetVersionSumTraverser = new PacketVersionSumCalculator();
            packet.Accept(packetVersionSumTraverser);
            sb.Append(packetVersionSumTraverser.Sum);

            return base.Part1();
        }

        public override string Part2()
        {
            PacketPrettyPrint ppp = new PacketPrettyPrint();
            packet.Accept(ppp);
            sb.AppendLine(ppp.Result);

            PacketProcessor packetProcessor = new PacketProcessor();
            packet.Accept(packetProcessor);
            sb.AppendLine(packetProcessor.Result.ToString());
            return base.Part2();
        }
    }

    enum PacketType
    {
        sum = 0,
        product = 1,
        minimum = 2,
        maximum = 3,
        literal = 4,
        greater = 5,
        less = 6,
        equal = 7
    }

    class PacketPrettyPrint : PacketVisitor
    {
        StringBuilder sb = new StringBuilder();

        public string Result => sb.ToString();

        public override void Visit(LiteralPacket literalPacket)
        {
            sb.AppendFormat("{0} ", literalPacket.literal);
        }

        public override void Visit(OperatorPacket operatorPacket)
        {
            foreach (var subPacket in operatorPacket.subPackets)
            {
                subPacket.Accept(this);
            }
            var opc = operatorPacket.subPackets.Length;
            switch (operatorPacket.type)
            {
                case PacketType.sum:
                    sb.Append("+ ");
                    break;
                case PacketType.product:
                    sb.Append("* ");
                    break;
                case PacketType.minimum:
                    sb.Append("min ");
                    break;
                case PacketType.maximum:
                    sb.Append("max ");
                    break;
                case PacketType.greater:
                    sb.Append("> ");
                    break;
                case PacketType.less:
                    sb.Append("< ");
                    break;
                case PacketType.equal:
                    sb.Append("== ");
                    break;
            }
        }
    }

    class PacketProcessor : PacketVisitor
    {
        Stack<long> number = new Stack<long>();
        Stack<(PacketType op, int opc)> operators = new Stack<(PacketType op, int opc)>();

        public long Result => number.Peek();

        public override void Visit(LiteralPacket literalPacket)
        {
            number.Push(literalPacket.literal);
        }

        public override void Visit(OperatorPacket operatorPacket)
        {
            foreach (var subPacket in operatorPacket.subPackets)
            {
                subPacket.Accept(this);
            }
            var opc = operatorPacket.subPackets.Length;
            long acc = 0;
            switch (operatorPacket.type)
            {
                case PacketType.sum:
                    acc = 0;
                    for (int i = 0; i < opc; i++)
                    {
                        var n = number.Pop();
                        acc += n;
                    }
                    number.Push(acc);
                    break;
                case PacketType.product:
                    acc = 1;
                    for (int i = 0; i < opc; i++)
                    {
                        var n = number.Pop();
                        acc *= n;
                    }
                    number.Push(acc);
                    break;
                case PacketType.minimum:
                    acc = int.MaxValue;
                    for (int i = 0; i < opc; i++)
                    {
                        var n = number.Pop();
                        if (n < acc)
                        {
                            acc = n;
                        }
                    }
                    number.Push(acc);
                    break;
                case PacketType.maximum:
                    acc = int.MinValue;
                    for (int i = 0; i < opc; i++)
                    {
                        var n = number.Pop();
                        if (n > acc)
                        {
                            acc = n;
                        }
                    }
                    number.Push(acc);
                    break;
                case PacketType.greater:
                    {
                        var op2 = number.Pop();
                        var op1 = number.Pop();
                        if (op1 > op2)
                        {
                            number.Push(1);
                        }
                        else
                        {
                            number.Push(0);
                        }
                    }
                    break;
                case PacketType.less:
                    {
                        var op2 = number.Pop();
                        var op1 = number.Pop();
                        if (op1 < op2)
                        {
                            number.Push(1);
                        }
                        else
                        {
                            number.Push(0);
                        }
                    }
                    break;
                case PacketType.equal:
                    {
                        var op2 = number.Pop();
                        var op1 = number.Pop();
                        if (op1 == op2)
                        {
                            number.Push(1);
                        }
                        else
                        {
                            number.Push(0);
                        }
                    }
                    break;
            }
        }
    }

    class PacketVersionSumCalculator : PacketVisitor
    {
        public int Sum { get; private set; }
        public override void Visit(LiteralPacket literalPacket)
        {
            Sum += literalPacket.version;
        }

        public override void Visit(OperatorPacket operatorPacket)
        {
            Sum += operatorPacket.version;
            foreach (var subPacket in operatorPacket.subPackets)
            {
                Visit(subPacket);
            }
        }
    }

    abstract class PacketVisitor : IVisitor
    {
        public void Visit(Packet packet)
        {
            switch (packet)
            {
                case LiteralPacket lit:
                    Visit(lit);
                    break;
                case OperatorPacket op:
                    Visit(op);
                    break;
                default:
                    break;
            }
        }

        public abstract void Visit(LiteralPacket literalPacket);

        public abstract void Visit(OperatorPacket operatorPacket);
    }

    interface IVisitable
    {
        void Accept(IVisitor visitor);
    }

    interface IVisitor
    {
        void Visit(Packet packet);
        void Visit(LiteralPacket literalPacket);
        void Visit(OperatorPacket operatorPacket);
    }

    abstract class Packet : IVisitable
    {
        protected Packet(int version, PacketType type)
        {
            this.version = version;
            this.type = type;
        }

        public int version { get; set; }
        public PacketType type { get; set; }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    class LiteralPacket : Packet
    {
        public LiteralPacket(int version, PacketType type, long literal) : base(version, type)
        {
            this.literal = literal;
        }

        public long literal { get; private set; }
    }

    class OperatorPacket : Packet
    {
        public OperatorPacket(int version, PacketType type, Packet[] subpackets) : base(version, type)
        {
            this.length = subpackets.Length;
            this.subPackets = subpackets;
        }

        public int length { get; private set; }
        public Packet[] subPackets { get; private set; }
    }

    static class PacketParser
    {
        public static (Packet packet, int parsedBit) Parse(string hexString)
        {
            var bits = hexString.HexStringToBitArray();
            return Parse(bits);
        }

        public static (Packet packet, int parsedBit) Parse(BitArray bits)
        {
            var indx = 0;
            var ver = bits.GetByte(indx, 3);
            indx += 3;
            var type = (PacketType)bits.GetByte(indx, 3);
            indx += 3;

            if (type == PacketType.literal)
            {
                //literal packet
                bool isEnd = false;
                string temp = string.Empty;
                do
                {
                    isEnd = !bits.Get(indx);
                    indx++;
                    temp += bits.Stringify(indx, 4);
                    indx += 4;
                } while (!isEnd);
                var lit = temp.BinaryStringToBitArray().GetLong();
                return (new LiteralPacket(ver, type, lit), indx);
            }
            else
            {
                //operator packet
                bool lengthType = bits.Get(indx);
                indx++;
                var length = 0;
                if (lengthType)
                {
                    length = bits.GetInt(indx, 11);
                    indx += 11;
                }
                else
                {
                    length = bits.GetInt(indx, 15);
                    indx += 15;
                }

                List<Packet> subpackets = new List<Packet>();
                var subPacketStart = indx;
                do
                {
                    var sub = bits.GetSubBitArray(indx);
                    var (packet, parsedBit) = Parse(sub);
                    indx += parsedBit;
                    subpackets.Add(packet);
                } while (lengthType ? subpackets.Count < length : indx - subPacketStart < length);

                // do calculation in place
                if (subpackets.All(sp => sp is LiteralPacket))
                {
                    PacketProcessor pp = new PacketProcessor();
                    var temp = new OperatorPacket(ver, type, subpackets.ToArray());
                    temp.Accept(pp);
                    var calculated = pp.Result;
                    //return (new LiteralPacket(ver, type, calculated), indx);
                }

                return (new OperatorPacket(ver, type, subpackets.ToArray()), indx);
            }
        }
    }

    static class ExtensionMethod
    {
        public static BitArray GetSubBitArray(this BitArray barr, int start = 0, int length = -1)
        {
            if (length == -1)
            {
                length = barr.Length - start;
            }
            var subarr = new BitArray(length);
            for (int i = 0; i < length; i++)
            {
                subarr[i] = barr[start + i];
            }
            return subarr;
        }

        public static byte GetByte(this BitArray barr, int start = 0, int length = -1)
        {
            byte result = 0;
            if (length == -1)
            {
                length = barr.Length - start;
            }
            for (int i = 0; i < length; i++)
            {
                result = (byte)((result << 1) | (barr[start + i] ? 1 : 0));
            }
            return result;
        }

        public static int GetInt(this BitArray barr, int start = 0, int length = -1)
        {
            int result = 0;
            if (length == -1)
            {
                length = barr.Length - start;
            }
            for (int i = 0; i < length; i++)
            {
                result = (int)((result << 1) | (barr[start + i] ? 1 : 0));
            }
            return result;
        }

        public static long GetLong(this BitArray barr, int start = 0, int length = -1)
        {
            long result = 0;
            if (length == -1)
            {
                length = barr.Length - start;
            }
            for (int i = 0; i < length; i++)
            {
                result = (long)((result << 1) | (barr[start + i] ? 1 : 0));
            }
            return result;
        }

        public static BitArray HexStringToBitArray(this string hexString)
        {
            return new BitArray(hexString.SelectMany(c => ParseHexChar(c).Select(hc => hc == '1')).ToArray());
        }

        public static BitArray BinaryStringToBitArray(this string binString)
        {
            return new BitArray(binString.Select(hc => hc == '1').ToArray());
        }

        public static string Stringify(this BitArray barr)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < barr.Length; i++)
            {
                sb.Append(barr[i] ? 1 : 0);
            }
            return sb.ToString();
        }

        public static string Stringify(this BitArray barr, int start, int length)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sb.Append(barr[i + start] ? 1 : 0);
            }
            return sb.ToString();
        }

        public static string ParseHexChar(this char c)
        {
            if (c == '0') return "0000";
            if (c == '1') return "0001";
            if (c == '2') return "0010";
            if (c == '3') return "0011";
            if (c == '4') return "0100";
            if (c == '5') return "0101";
            if (c == '6') return "0110";
            if (c == '7') return "0111";
            if (c == '8') return "1000";
            if (c == '9') return "1001";
            if (c == 'A') return "1010";
            if (c == 'B') return "1011";
            if (c == 'C') return "1100";
            if (c == 'D') return "1101";
            if (c == 'E') return "1110";
            if (c == 'F') return "1111";
            throw new Exception("wtf");
        }
    }
}
