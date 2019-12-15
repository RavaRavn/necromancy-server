namespace Necromancy.Server.Packet
{
    public interface IPacketSerializer<T>
    {
        NecPacket Serialize(T obj);
    }
}
