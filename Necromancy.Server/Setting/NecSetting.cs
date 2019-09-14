using System.Net;
using System.Runtime.Serialization;

namespace Necromancy.Server.Setting
{
    [DataContract]
    public class NecSetting
    {
        /// <summary>
        /// Warning:
        /// Changing while having existing accounts requires to rehash all passwords.
        /// The number is log2, so adding +1 doubles the time it takes.
        /// https://wildlyinaccurate.com/bcrypt-choosing-a-work-factor/
        /// </summary>
        public const int BCryptWorkFactor = 10;
        
        [IgnoreDataMember] 
        public IPAddress ListenIpAddress { get; set; }

        [DataMember(Name = "ListenIpAddress", Order = 0)]
        public string DataListenIpAddress
        {
            get => ListenIpAddress.ToString();
            set => ListenIpAddress = string.IsNullOrEmpty(value) ? null : IPAddress.Parse(value);
        }

        [IgnoreDataMember] 
        public IPAddress AuthIpAddress { get; set; }

        [DataMember(Name = "AuthIpAddress", Order = 1)]
        public string DataAuthIpAddress
        {
            get => AuthIpAddress.ToString();
            set => AuthIpAddress = string.IsNullOrEmpty(value) ? null : IPAddress.Parse(value);
        }

        [DataMember(Order = 2)] 
        public ushort AuthPort { get; set; }

        [IgnoreDataMember] 
        public IPAddress MsgIpAddress { get; set; }

        [DataMember(Name = "MsgIpAddress", Order = 3)]
        public string DataMsgIpAddress
        {
            get => MsgIpAddress.ToString();
            set => MsgIpAddress = string.IsNullOrEmpty(value) ? null : IPAddress.Parse(value);
        }

        [DataMember(Order = 4)] 
        public ushort MsgPort { get; set; }

        [IgnoreDataMember] 
        public IPAddress AreaIpAddress { get; set; }

        [DataMember(Name = "AreaIpAddress", Order = 5)]
        public string DataAreaIpAddress
        {
            get => AreaIpAddress.ToString();
            set => AreaIpAddress = string.IsNullOrEmpty(value) ? null : IPAddress.Parse(value);
        }

        [DataMember(Order = 6)] 
        public ushort AreaPort { get; set; }
        
        [DataMember(Order = 10)]
        public bool NeedRegistration { get; set; }

        [DataMember(Order = 20)] 
        public int LogLevel { get; set; }

        [DataMember(Order = 21)] 
        public bool LogUnknownIncomingPackets { get; set; }

        [DataMember(Order = 22)] 
        public bool LogOutgoingPackets { get; set; }

        [DataMember(Order = 23)] 
        public bool LogIncomingPackets { get; set; }

        [DataMember(Order = 70)]
        public DatabaseSettings DatabaseSettings { get; set; }

        public NecSetting()
        {
            ListenIpAddress = IPAddress.Any;
            AuthIpAddress = IPAddress.Loopback;
            AuthPort = 60000;
            MsgIpAddress = IPAddress.Loopback;
            MsgPort = 60001;
            AreaIpAddress = IPAddress.Loopback;
            AreaPort = 60002;
            NeedRegistration = false;
            LogLevel = 0;
            LogUnknownIncomingPackets = true;
            LogOutgoingPackets = true;
            LogIncomingPackets = true;
            DatabaseSettings = new DatabaseSettings();
        }

        public NecSetting(NecSetting setting)
        {
            ListenIpAddress = setting.ListenIpAddress;
            AuthIpAddress = setting.AuthIpAddress;
            AuthPort = setting.AuthPort;
            MsgIpAddress = setting.MsgIpAddress;
            MsgPort = setting.MsgPort;
            AreaIpAddress = setting.AreaIpAddress;
            AreaPort = setting.AreaPort;
            NeedRegistration = setting.NeedRegistration;
            LogLevel = setting.LogLevel;
            LogUnknownIncomingPackets = setting.LogUnknownIncomingPackets;
            LogOutgoingPackets = setting.LogOutgoingPackets;
            LogIncomingPackets = setting.LogIncomingPackets;
            DatabaseSettings = new DatabaseSettings(setting.DatabaseSettings);
        }
    }
}