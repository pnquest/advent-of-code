using Pidgin;

namespace Day132022;

internal class Program
{
    static void Main(string[] args)
    {
        Parser<char, Packet> listParser = null!;
        Parser<char, Packet> numberParser = Parser.Num.Map(n => new Packet(PacketType.Number, n, null));

        listParser = Parser.Char('[')
            .Then(numberParser.Or(Parser.Rec(() => listParser)).SeparatedAndOptionallyTerminated(Parser.Char(',')))
            .Before(Parser.Char(']'))
            .Map(p => new Packet(PacketType.List, null, p.ToList()));

        List<Packet> packets = File.ReadAllLines("./input.txt")
                    .Where(l => l != string.Empty)
                    .Select(l => listParser.ParseOrThrow(l)).ToList();

        Packet[][] chunked = packets
            .Chunk(2)
            .ToArray();

        Part1(chunked);
        Part2(listParser, packets);
    }

    private static void Part2(Parser<char, Packet> listParser, List<Packet> packets)
    {
        Packet firstDivider = listParser.ParseOrThrow("[[2]]");
        Packet secondDivider = listParser.ParseOrThrow("[[6]]");
        List<Packet> sorted = packets.Concat(new[] { firstDivider, secondDivider })
            .Order()
            .ToList();

        int result = (sorted.IndexOf(firstDivider) + 1) * (sorted.IndexOf(secondDivider) + 1);

        Console.WriteLine($"Part 2: {result}");
    }

    private static void Part1(Packet[][] packets)
    {
        int result = packets.Select((p, idx) => (p, idx))
                    .Where(v => v.p[0] < v.p[1])
                    .Sum(v => v.idx + 1);

        Console.WriteLine($"Part 1: {result}");
    }

    internal record Packet(PacketType Type, int? NumberValue, List<Packet>? ListValue) : IComparable<Packet>
    {
        public int CompareTo(Packet? other)
        {
            if(other == null)
            {
                ArgumentNullException.ThrowIfNull(other);
            }

            if(Type == PacketType.Number && other.Type == PacketType.Number && NumberValue.HasValue && other.NumberValue.HasValue)
            {
                return NumberValue.Value.CompareTo(other.NumberValue.Value);
            }
            if(Type == PacketType.List && other.Type == PacketType.List && ListValue != null && other.ListValue != null)
            {
                int index = 0;
                while(index < ListValue.Count && index < other.ListValue.Count)
                {
                    int comp = ListValue[index].CompareTo(other.ListValue[index]);
                    if(comp != 0)
                    {
                        return comp;
                    }

                    index++;
                }

                return ListValue.Count.CompareTo(other.ListValue.Count);
            }
            if(Type == PacketType.Number && other.Type == PacketType.List && NumberValue != null && other.ListValue != null)
            {
                var listPacket = new Packet(PacketType.List, null, [this]);
                return listPacket.CompareTo(other);
            }
            if(Type == PacketType.List && other.Type == PacketType.Number && ListValue != null && other.NumberValue != null)
            {
                var listpacket = new Packet(PacketType.List, null, [other]);
                return CompareTo(listpacket);
            }

            throw new InvalidOperationException("Packet with value not matching type found");
        }

        public static bool operator <(Packet first, Packet other)
        {
            return first.CompareTo(other) < 0;
        }

        public static bool operator >(Packet first, Packet other)
        {
            return first.CompareTo(other) > 0;
        }
    }
    internal enum PacketType
    {
        Number,
        List
    }
}
