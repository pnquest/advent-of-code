namespace Day16.Packets;

internal class SumPacket : OperatorPacket
{
    public override ulong ComputeResult()
    {
        ulong result = 0;

        foreach (PacketBase inner in InnerPackets)
        {
            result += inner.ComputeResult();
        }

        return result;
    }
}
