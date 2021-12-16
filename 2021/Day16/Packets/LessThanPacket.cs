namespace Day16.Packets;

internal class LessThanPacket : OperatorPacket
{
    public override ulong ComputeResult()
    {
        ulong left = InnerPackets[0].ComputeResult();
        ulong right = InnerPackets[1].ComputeResult();

        return left < right ? 1UL : 0UL;
    }
}
