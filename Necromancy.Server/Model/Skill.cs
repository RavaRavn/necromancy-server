using System;
using Necromancy.Server.Common.Instance;

namespace Necromancy.Server.Model
{
    public class Skill : IInstance
    {
        public uint InstanceId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }

        public Skill()
        {
        }

    }
}
