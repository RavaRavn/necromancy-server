using System.Collections.Generic;
using System.Text.RegularExpressions;
using Arrowgene.Services.Logging;

namespace Necromancy.Server.Data.Setting
{
    public class StrTableSettingLookup
    {
        private const string RegexStringIdPattern = @"<str ([0-9]+) ([0-9]+) ([0-9]+)>";
        private readonly ILogger _logger;
        private Dictionary<string, StrTableSetting> _table;

        public StrTableSettingLookup()
        {
            _logger = LogProvider.Logger(this);
            _table = new Dictionary<string, StrTableSetting>();
        }

        public void Add(StrTableSetting setting)
        {
            string key = GenerateKey(setting);
            if (_table.ContainsKey(key))
            {
                _logger.Error($"Key '{key}' already exists");
                return;
            }

            _table.Add(key, setting);
        }

        /// <summary>
        ///  Gets the StrTableSetting by reference string.
        /// </summary>
        /// <param name="reference">String reference in format: &gt;str 11 1 3&lt;</param>
        /// <returns></returns>
        public StrTableSetting Get(string reference)
        {
            Match match = Regex.Match(reference, RegexStringIdPattern);
            if (!int.TryParse(match.Groups[1].Value, out int id))
            {
                return null;
            }

            if (!int.TryParse(match.Groups[2].Value, out int subId))
            {
                return null;
            }

            if (!int.TryParse(match.Groups[3].Value, out int stringId))
            {
                return null;
            }

            return Get(id, subId, stringId);
        }

        public StrTableSetting Get(int id, int subId, int stringId)
        {
            string key = GenerateKey(id, subId, stringId);
            if (_table.TryGetValue(key, out StrTableSetting setting))
            {
                return setting;
            }

            return null;
        }

        public void Clear()
        {
            _table.Clear();
        }

        private string GenerateKey(StrTableSetting setting)
        {
            return GenerateKey(setting.Id, setting.SubId, setting.StringId);
        }

        private string GenerateKey(int id, int subId, int stringId)
        {
            return $"{id}-{subId}-{stringId}";
        }
    }
}
