using System;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet
{
    public class NecPacket
    {
        public static string GetPacketIdName(ushort id, ServerType serverType)
        {
            switch (serverType)
            {
                case ServerType.Auth:
                    if (Enum.IsDefined(typeof(AuthPacketId), id))
                    {
                        AuthPacketId authPacketId = (AuthPacketId) id;
                        return authPacketId.ToString();
                    }

                    break;
                case ServerType.Msg:
                    if (Enum.IsDefined(typeof(MsgPacketId), id))
                    {
                        MsgPacketId msgPacketId = (MsgPacketId) id;
                        return msgPacketId.ToString();
                    }

                    break;
                case ServerType.Area:
                    if (Enum.IsDefined(typeof(AreaPacketId), id))
                    {
                        AreaPacketId areaPacketId = (AreaPacketId) id;
                        return areaPacketId.ToString();
                    }

                    break;
            }

            if (Enum.IsDefined(typeof(CustomPacketId), id))
            {
                CustomPacketId customPacketId = (CustomPacketId) id;
                return customPacketId.ToString();
            }
            
            return null;
        }

        private string _packetIdName;

        public NecPacket(ushort id, IBuffer buffer, ServerType serverType)
        {
            Data = buffer;
            Id = id;
            ServerType = serverType;
            PacketType = null;
        }

        public NecPacket(ushort id, IBuffer buffer, ServerType serverType, PacketType packetType)
        {
            Data = buffer;
            Id = id;
            ServerType = serverType;
            PacketType = null;
            PacketType = packetType;
        }

        public IBuffer Data { get; }
        public ushort Id { get; }
        public byte[] Header { get; set; }
        public ServerType ServerType { get; }
        public PacketType? PacketType { get; }

        public string PacketIdName
        {
            get
            {
                if (_packetIdName != null)
                {
                    return _packetIdName;
                }

                _packetIdName = GetPacketIdName(Id, ServerType);
                if (_packetIdName == null)
                {
                    _packetIdName = $"ID_NOT_DEFINED_{ServerType}_{Id}";
                }

                return _packetIdName;
            }
        }
    }
}
