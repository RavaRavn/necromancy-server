namespace Necromancy.Server.Packet
{
    public interface IPacketDeserializer<T>
    {
        T Deserialize(NecPacket packet);
    }
}
