namespace Necromancy.Server.Packet.Id
{
    /// <summary>
    /// Necromancy Custom OP Codes
    /// </summary>
    public enum CustomPacketId : ushort
    {
        // Recv OP Codes - ordered by op code
        RecvHeartbeat = 0xFFFF,
        RecvUnknown1 = 0xFFFD,

        // Send OP Codes - ordered by op code
        SendHeartbeat = 0xFFFE,
        SendUnknown1 = 0xFFFC,
    }
}
