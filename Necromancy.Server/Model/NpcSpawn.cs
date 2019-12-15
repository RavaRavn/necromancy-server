using System;
using Necromancy.Server.Common.Instance;

namespace Necromancy.Server.Model
{
    public class NpcSpawn : IInstance
    {
        public uint InstanceId { get; set; }
        public int Id { get; set; }
        public int NpcId { get; set; }
        public int ModelId { get; set; }
        public byte Level { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public int MapId { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public bool Active { get; set; }
        public byte Heading { get; set; }
        public short Size { get; set; }
        public int Visibility { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public int Icon { get; set; }
        public int Status { get; set; }
        public int Status_X { get; set; }
        public int Status_Y { get; set; }
        public int Status_Z { get; set; }
        public int Radius { get; set; }



        public NpcSpawn()
        {
            Created = DateTime.Now;
            Updated = DateTime.Now;
            Radius = 100;
        }
    }
}
