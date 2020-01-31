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
        RecvDisconnect = 0xFFFB,

        // Send OP Codes - ordered by op code
        SendHeartbeat = 0xFFFE,
        SendUnknown1 = 0xFFFC,
        SendDisconnect = 0xFFFA
    }
}
