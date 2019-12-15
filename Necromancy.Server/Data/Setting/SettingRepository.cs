using System.Collections.Generic;
using System.IO;
using Arrowgene.Services.Logging;

namespace Necromancy.Server.Data.Setting
{
    public class SettingRepository
    {
        private DirectoryInfo _directory;
        private readonly ILogger _logger;

        public SettingRepository(string folder)
        {
            _logger = LogProvider.Logger(this);
            _directory = new DirectoryInfo(folder);
            if (!_directory.Exists)
            {
                _logger.Error($"Could not initialize repository, '{folder}' does not exist");
                return;
            }

            Items = new Dictionary<int, ItemSetting>();
            Maps = new Dictionary<int, MapSetting>();
            MapSymbols = new Dictionary<int, MapSymbolSetting>();
            Strings = new StrTableSettingLookup();
            Monster = new Dictionary<int, MonsterSetting>();
            Npc = new Dictionary<int, NpcSetting>();
            ModelAtr = new Dictionary<int, ModelAtrSetting>();
            ModelCommon = new Dictionary<int, ModelCommonSetting>();
        }

        public Dictionary<int, ItemSetting> Items { get; }
        public Dictionary<int, MapSetting> Maps { get; }
        public Dictionary<int, MapSymbolSetting> MapSymbols { get; }
        public Dictionary<int, MonsterSetting> Monster { get; }
        public StrTableSettingLookup Strings { get; }
        public Dictionary<int, NpcSetting> Npc { get; }
        public Dictionary<int, ModelAtrSetting> ModelAtr { get; }
        public Dictionary<int, ModelCommonSetting> ModelCommon { get; }

        public SettingRepository Initialize()
        {
            Items.Clear();
            Maps.Clear();
            MapSymbols.Clear();
            Strings.Clear();
            Monster.Clear();
            Npc.Clear();
            ModelAtr.Clear();
            ModelCommon.Clear();
            Load(Strings, "str_table.csv", new StrTableCsvReader());
            Load(Items, "iteminfo.csv", new ItemInfoCsvReader());
            Load(Monster, "monster.csv", new MonsterCsvReader());
            Load(Npc, "npc.csv", new NpcCsvReader());
            Load(ModelAtr, "model_atr.csv", new ModelAtrCsvReader());
            Load(Maps, "map.csv", new MapCsvReader(Strings));
            //Load(MapSymbols, "map_symbol.csv", new MapSymbolCsvReader());
            Load(ModelCommon, "model_common.csv", new ModelCommonCsvReader(Monster, ModelAtr));
            return this;
        }

        private void Load<T>(List<T> list, string fileName, CsvReader<T> reader)
        {
            string path = Path.Combine(_directory.FullName, fileName);
            FileInfo file = new FileInfo(path);
            if (!file.Exists)
            {
                _logger.Error($"Could not load '{fileName}', file does not exist");
            }

            list.AddRange(reader.Read(file.FullName));
        }

        private void Load<T>(Dictionary<int, T> dictionary, string fileName, CsvReader<T> reader)
        {
            List<T> items = new List<T>();
            Load(items, fileName, reader);
            foreach (T item in items)
            {
                if (item is ISettingRepositoryItem repositoryItem)
                {
                    dictionary.Add(repositoryItem.Id, item);
                }
            }
        }

        private void Load(StrTableSettingLookup lookup, string fileName, CsvReader<StrTableSetting> reader)
        {
            List<StrTableSetting> items = new List<StrTableSetting>();
            Load(items, fileName, reader);
            foreach (StrTableSetting item in items)
            {
                lookup.Add(item);
            }
        }
    }
}
