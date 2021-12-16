namespace Day16.Packets;

internal class MinimumPacket : OperatorPacket
{
    public override ulong ComputeResult()
    {
        ulong min = ulong.MaxValue;

        foreach( PacketBase inner in InnerPackets)
        {
            ulong res = inner.ComputeResult();
            if(min > res)
            {
                min = res;
            }
        }

        return min;
    }
}
