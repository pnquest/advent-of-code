namespace Day16.Packets;

internal abstract class PacketBase
{
    public List<PacketBase> InnerPackets { get; set; } = new();
    public uint Version { get; private set; }
    public uint TypeId { get; private set; }

    protected abstract PacketBase ParsePacketInternal(ReadOnlyMemory<char> bits, out int bitsUsed);
    public abstract ulong ComputeResult();

    public static PacketBase ParsePacket(ReadOnlyMemory<char> bits, bool isTopLevel, out int bitsRead)
    {
        uint version = Convert.ToUInt32(new string(bits[..3].Span), 2);
        uint type = Convert.ToUInt32(new string(bits[3..6].Span), 2);

        PacketBase packet = type switch {
            4 => new LiteralPacket { TypeId = type, Version = version },
            0 => new SumPacket { TypeId = type, Version = version },
            1 => new ProductPacket { TypeId = type, Version = version },
            2 => new MinimumPacket { TypeId = type, Version = version },
            3 => new MaximumPacket { TypeId = type, Version = version },
            5 => new GreaterThanPacket { TypeId = type, Version = version },
            6 => new LessThanPacket { TypeId = type, Version = version },
            7 => new EqualToPacket { TypeId = type, Version = version },
            _ => throw new InvalidOperationException("Unknown packet type")
        };

        int used = 6;

        packet = packet.ParsePacketInternal(bits[6..], out int read);
        bitsRead = used + read;
        if(isTopLevel)
        {
            while (bitsRead % 4 != 0)
            {
                bitsRead++;
            }
        }
        return packet;
    }

    public int SumVersions()
    {
        return (int)Version + InnerPackets.Sum(v => v.SumVersions());
    }
}
