namespace Day16.Packets;

internal class MaximumPacket : OperatorPacket
{
    public override ulong ComputeResult()
    {
        ulong max = ulong.MinValue;

        foreach (PacketBase inner in InnerPackets)
        {
            ulong res = inner.ComputeResult();
            if (max < res)
            {
                max = res;
            }
        }

        return max;
    }
}
