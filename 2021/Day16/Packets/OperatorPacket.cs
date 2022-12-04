namespace Day16.Packets;

internal abstract class OperatorPacket : PacketBase
{
    protected override PacketBase ParsePacketInternal(ReadOnlyMemory<char> bits, out int bitsUsed)
    {
        int index = 1;

        bool isLength = bits.Span[0] == '0';

        if (isLength)
        {
            uint length = Convert.ToUInt32(new string(bits[1..16].Span), 2);
            index += 15;

            ReadOnlyMemory<char> remainingSub = bits[index..(index + (int)length)];

            while (remainingSub.Length > 0)
            {
                InnerPackets.Add(ParsePacket(remainingSub, false, out int read));
                remainingSub = remainingSub[read..];
            }

            bitsUsed = index + (int)length;
        }
        else
        {
            uint count = Convert.ToUInt32(new string(bits[1..12].Span), 2);
            index += 11;
            ReadOnlyMemory<char> remainingSub = bits[index..];
            int subUsed = 0;
            for (int i = 0; i < count; i++)
            {
                InnerPackets.Add(ParsePacket(remainingSub, false, out int used));
                subUsed += used;
                remainingSub = remainingSub[used..];
            }
            bitsUsed = index + subUsed;
        }

        return this;
    }
}
