using System;
using Necromancy.Server.Common.Instance;

namespace Necromancy.Server.Model
{
    public class SkillTreeItem
    {
        public int Id { get; set; }
        public int SkillId { get; set; }
        public int CharId { get; set; }
        public int Level { get; set; }

        public SkillTreeItem()
        {
            Id = -1;
            SkillId = -1;
            CharId = -1;
            Level = -1;
        }

    }
}
