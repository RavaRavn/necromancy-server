using System;
using Necromancy.Server.Common.Instance;

namespace Necromancy.Server.Model
{
    public class MonsterSpawn : IInstance
    {
        public uint InstanceId { get; set; }
        public int Id { get; set; }
        public int MonsterId { get; set; }
        public int ModelId { get; set; }
        public byte Level { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public int MapId { get; set; }
        public bool Active { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public byte Heading { get; set; }
        public short Size { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public MonsterSpawn()
        {
            Created = DateTime.Now;
            Updated = DateTime.Now;
        }
    }
}
