using System;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;

namespace Necromancy.Server.Logging
{
    public class NecLogPacket : NecPacket
    {
        public NecLogPacket(string clientIdentity, NecPacket packet, NecLogType logType, ServerType serverType)
            : base(packet.Id, packet.Data.Clone(), serverType)
        {
            Header = packet.Header;
            LogType = logType;
            TimeStamp = DateTime.Now;
            ClientIdentity = clientIdentity;
        }

        public string ClientIdentity { get; }
        public NecLogType LogType { get; }
        public DateTime TimeStamp { get; }
        public string Hex => Data.ToHexString('-');
        public string Ascii => Data.ToAsciiString(true);
        public string HeaderHex => Util.ToHexString(Header, '-');

        public string ToLogText()
        {
            String log = $"{ClientIdentity} Packet Log";
            log += Environment.NewLine;
            log += "----------";
            log += Environment.NewLine;
            log += $"[{TimeStamp:HH:mm:ss}][Typ:{LogType}]";
            log += $"[{ServerType}]";
            log += Environment.NewLine;
            log += $"[Id:0x{Id:X2}|{Id}][Len(Data/Total):{Data.Size}/{Data.Size + Header.Length}][Header:{HeaderHex}]";
            log += $"[{PacketIdName}]";
            log += Environment.NewLine;
            log += "ASCII:";
            log += Environment.NewLine;
            log += Ascii;
            log += Environment.NewLine;
            log += "HEX:";
            log += Environment.NewLine;
            log += Hex;
            log += Environment.NewLine;
            log += "----------";
            return log;
        }
    }
}
