namespace Day16.Packets;

internal class ProductPacket : OperatorPacket
{
    public override ulong ComputeResult()
    {
        ulong result = 1;

        foreach(PacketBase inner in InnerPackets)
        {
            result *= inner.ComputeResult();
        }

        return result;
    }
}
