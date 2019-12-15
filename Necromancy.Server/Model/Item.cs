using Necromancy.Server.Data.Setting;

namespace Necromancy.Server.Model
{
    public class Item
    {
        public Item(ItemSetting setting)
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
