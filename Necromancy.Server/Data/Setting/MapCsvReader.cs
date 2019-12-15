using System.Text.RegularExpressions;

namespace Necromancy.Server.Data.Setting
{
    public class MapCsvReader : CsvReader<MapSetting>
    {
        private const string RegexNamePattern =
            @"(<str [0-9]+ [0-9]+ [0-9]+>)\\(<str [0-9]+ [0-9]+ [0-9]+>)\\(<str [0-9]+ [0-9]+ [0-9]+>)";

        private readonly StrTableSettingLookup _stringLookup;

        public MapCsvReader()
        {
            _stringLookup = null;
        }

        protected override int NumExpectedItems => 7;

        public MapCsvReader(StrTableSettingLookup stringLookup)
        {
            _stringLookup = stringLookup;
        }

        protected override MapSetting CreateInstance(string[] properties)
        {
            if (!int.TryParse(properties[0], out int id))
            {
                return null;
            }

            string country = null;
            string area = null;
            string place = null;
            string nameReference = properties[3];
            if (!string.IsNullOrEmpty(nameReference))
            {
                Match match = Regex.Match(nameReference, RegexNamePattern);
                if (match.Success && match.Groups.Count == 4)
                {
                    StrTableSetting countryStr = _stringLookup.Get(match.Groups[1].Value);
                    if (countryStr != null)
                    {
                        country = countryStr.Text;
                    }

                    StrTableSetting areaStr = _stringLookup.Get(match.Groups[2].Value);
                    if (areaStr != null)
                    {
                        area = areaStr.Text;
                    }

                    StrTableSetting placeStr = _stringLookup.Get(match.Groups[3].Value);
                    if (placeStr != null)
                    {
                        place = placeStr.Text;
                    }
                }
                else
                {
                    country = nameReference;
                }
            }

            if (!int.TryParse(properties[4], out int x))
            {
                return null;
            }

            if (!int.TryParse(properties[5], out int y))
            {
                return null;
            }

            if (!int.TryParse(properties[6], out int z))
            {
                return null;
            }

            if (!int.TryParse(properties[7], out int orientation))
            {
                return null;
            }

            return new MapSetting
            {
                Id = id,
                Country = country,
                Area = area,
                Place = place,
                X = x,
                Y = y,
                Z = z,
                Orientation = orientation
            };
        }
    }
}
