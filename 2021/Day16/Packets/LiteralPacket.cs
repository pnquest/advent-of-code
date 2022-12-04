namespace Day16.Packets;

internal class LiteralPacket : PacketBase
{
    public ulong LiteralValue { get; private set; }

    public override ulong ComputeResult() => LiteralValue;

    protected override PacketBase ParsePacketInternal(ReadOnlyMemory<char> bits, out int bitsUsed)
    {
        int index = 0;

        List<char> numberBits = new();
        bool isLastBlock = false;

        while (!isLastBlock)
        {
            if (bits.Span[index] == '0')
            {
                isLastBlock = true;
            }
            for (int i = index + 1; i < index + 5; i++)
            {
                numberBits.Add(bits.Span[i]);
            }

            index += 5;
        }

        LiteralValue = Convert.ToUInt64(new string(numberBits.ToArray()), 2);
        bitsUsed = index;

        return this;
    }
}
